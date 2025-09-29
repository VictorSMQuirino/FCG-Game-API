using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using FCG_Games.Application.Converters;
using FCG_Games.Application.Validators.Game;
using FCG_Games.Domain.DTO;
using FCG_Games.Domain.DTO.Elasticsearch;
using FCG_Games.Domain.DTO.Elasticsearch.ElasticsearchDocuments;
using FCG_Games.Domain.Entities;
using FCG_Games.Domain.Enums;
using FCG_Games.Domain.Exceptions;
using FCG_Games.Domain.Extensions;
using FCG_Games.Domain.Interfaces.Repositories;
using FCG_Games.Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace FCG_Games.Application.Services;

public class GameService : IGameService
{
	private readonly IGameRepository _gameRepository;
	private readonly ElasticsearchClient _elasticClient;
	private readonly IConfiguration _configuration;
	private readonly IApplicationUserService _applicationUserService;
	private readonly IUserGameRepository _userGameRepository;

	public GameService(IGameRepository gameRepository, ElasticsearchClient elasticClient, IConfiguration configuration, IApplicationUserService applicationUserService, IUserGameRepository userGameRepository)
	{
		_gameRepository = gameRepository;
		_elasticClient = elasticClient;
		_configuration = configuration;
		_applicationUserService = applicationUserService;
		_userGameRepository = userGameRepository;
	}

	public async Task<Guid> CreateAsync(CreateGameDto dto)
	{
		var validationResult = await new CreateGameValidator().ValidateAsync(dto);

		if (!validationResult.IsValid)
			throw new ValidationErrorsException(validationResult.Errors.ToErrorsPropertyDictionary());

		if (await _gameRepository.ExistsBy(game => game.Title.ToUpper().Equals(dto.Title.ToUpper())))
			throw new DuplicatedEntityException(nameof(Game), nameof(Game.Title), dto.Title);

		var newGame = dto.ToEntity();

		using var transaction = await _gameRepository.BeginTransaction();

		try
		{
			await _gameRepository.CreateAsync(newGame);

			var gameDocument = newGame.ToElasticsearchDocument();

			var response = await _elasticClient.IndexAsync(gameDocument, idx => idx.Index(_configuration["Elasticsearch:Index"]!).Id(gameDocument.Id));

			if (!response.IsSuccess()) throw new ElasticsearchException(ElasticsearchOperation.Index, $"{_configuration["Elasticsearch:Index"]}", $"{gameDocument.Id}");

			await transaction.CommitAsync();

			return newGame.Id;
		}
		catch(Exception)
		{
			await transaction.RollbackAsync();
			throw;
		}
	}

	public async Task UpdateAsync(Guid id, UpdateGameDto dto)
	{
		var validationResult = await new UpdateGameValidator().ValidateAsync(dto);

		if (!validationResult.IsValid)
			throw new ValidationErrorsException(validationResult.Errors.ToErrorsPropertyDictionary());

		var game = await _gameRepository.GetByIdAsync(id) ?? throw new NotFoundException(nameof(Game), id);

		if (await _gameRepository.ExistsBy(gameInDb => gameInDb.Title.ToUpper().Equals(dto.Title.ToUpper()) && gameInDb.Id != game.Id))
			throw new DuplicatedEntityException(nameof(Game), nameof(Game.Title), dto.Title);

		var transaction = await _gameRepository.BeginTransaction();

		try
		{
			game = dto.ToEntity(game);
			await _gameRepository.UpdateAsync(game);

			var partialDocument = new
			{
				game.Title,
				game.Description,
				game.Developer,
				game.Publisher,
				game.ReleaseDate,
				game.Genres
			};

			var elasticResponse = await _elasticClient.UpdateAsync<GameDocument, object>(
				_configuration["Elasticsearch:Index"]!,
				game.Id,
				u => u.Doc(partialDocument)
				);

			if (!elasticResponse.IsSuccess()) throw new ElasticsearchException(ElasticsearchOperation.Update, $"{_configuration["Elasticsearch:Index"]}", $"{game.Id}");

			await transaction.CommitAsync();
		}
		catch (Exception)
		{
			await transaction.RollbackAsync();
			throw;
		}
	}

	public async Task DeleteAsync(Guid id)
	{
		var game = await _gameRepository.GetByIdAsync(id) ?? throw new NotFoundException(nameof(Game), id);

		await _gameRepository.DeleteAsync(game);
	}

	public async Task<GameDto?> GetByIdAsync(Guid id)
	{
		var game = await _gameRepository.GetByIdAsync(id) ?? throw new NotFoundException(nameof(Game), id);

		return game.ToDto();
	}

	public async Task<ICollection<GameDto>> GetAllAsync()
	{
		var games = await _gameRepository.GetAllAsync();

		return games.ToDtoList();
	}

	public async Task<ICollection<GameDocument>> Search(ElasticsearchQueryParameters elasticsearchQueryParameters)
	{
		var index = _configuration["Elasticsearch:Index"];

		var response = await _elasticClient.SearchAsync<GameDocument>(s => s
			.Indices(index!)
			.From(elasticsearchQueryParameters.StartDocumentPosition)
			.Size(elasticsearchQueryParameters.Size)
			.Query(q => 
				q.MultiMatch(mm => mm
					.Query(elasticsearchQueryParameters.Term)
					.Type(TextQueryType.BoolPrefix)
					.Fields(nameof(GameDocument.Title).ToLower())
					)
				)
			);

		if (!response.IsValidResponse) throw new ElasticsearchException(ElasticsearchOperation.Search, index);

		return [.. response.Documents];
	}

	public async Task<ICollection<GameDocument>> GetRecomendationsForUser(int topGenredCount = 2, int recommendationSize = 5)
	{
		var userId = _applicationUserService.GetUserId();

		var index = _configuration["Elasticsearch:Index"];

		var aggregationResponse = await _elasticClient
			.SearchAsync<GameDocument>(s => s
				.Indices(index!)
				.Size(0)
				.Query(q => q
					.Term(t => t.Field(f => f.OwnerUserIds)
					.Value(userId.ToString()))
				)
				.Aggregations(aggs => aggs
					.Add("top_genres_agg", aggregation => aggregation
						.Terms(t => t
							.Field(f => f.Genres.Suffix("keyword"))
							.Size(topGenredCount)
						)
					)
				)
			);

		if (!aggregationResponse.IsValidResponse)
			throw new ElasticsearchException(ElasticsearchOperation.Search);

		var topGenres = aggregationResponse.Aggregations?
			.GetStringTerms("top_genres_agg")?
			.Buckets
			.Select(b => b.Key)
			.ToList() ?? [];

		if (topGenres.Count == 0) return [];

		var recommendationResponse = await _elasticClient.SearchAsync<GameDocument>(s => s
			.Indices(index!)
			.Size(recommendationSize)
			.Query(q => q
				.Bool(b => b
					.Should(sh => sh
						.Terms(t => t
							.Field(f => f.Genres.Suffix("keyword"))
							.Terms(new TermsQueryField(topGenres))
						)
					)
					.MinimumShouldMatch(1)
					.MustNot(mn => mn
						.Term(t => t
							.Field(f => f.OwnerUserIds)
							.Value(userId.ToString())
						)
					)
				)
			)
		);

		if (!recommendationResponse.IsValidResponse)
			throw new ElasticsearchException(ElasticsearchOperation.Search);

		return [.. recommendationResponse.Documents];
	}

	public async Task AddGameToUserLibrary(Guid gameId)
	{
		var loggedUserId = _applicationUserService.GetUserId();

		var gameExists = await _gameRepository.ExistsBy(g => g.Id == gameId);

		if (!gameExists)
			throw new NotFoundException(nameof(Game), gameId);

		var userGameAlreadyExists = await _userGameRepository.ExistsBy(ug => ug.UserId == loggedUserId && ug.GameId == gameId);

		if (userGameAlreadyExists)
			throw new DomainException("The logged user already has the game in own library.");

		var elasticUpdateResponse = await _elasticClient.UpdateAsync<GameDocument, object>(
			_configuration["Elasticsearch:Index"]!,
			gameId,
			u => u.Script(s => s
				.Source("ctx._source.ownerUserIds.add(params.userId)")
				.Params(p => p.Add("userId", loggedUserId))
				)
			);

		if (!elasticUpdateResponse.IsValidResponse)
			throw new ElasticsearchException(ElasticsearchOperation.Update);

		var userGame = new UserGame
		{
			UserId = loggedUserId,
			GameId = gameId
		};

		await _userGameRepository.CreateAsync(userGame);
	}
}
