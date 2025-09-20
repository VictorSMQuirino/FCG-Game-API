using Elastic.Clients.Elasticsearch;
using FCG_Games.Application.Converters;
using FCG_Games.Application.Validators.Game;
using FCG_Games.Domain.DTO;
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

	public GameService(IGameRepository gameRepository, ElasticsearchClient elasticClient, IConfiguration configuration)
	{
		_gameRepository = gameRepository;
		_elasticClient = elasticClient;
		_configuration = configuration;
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

			var gameDocument = game.ToElasticsearchDocument();

			var elasticResponse = await _elasticClient.IndexAsync(gameDocument, idx => idx.Index(_configuration["Elasticsearch:Index"]!).Id(gameDocument.Id));

			if (!elasticResponse.IsSuccess()) throw new ElasticsearchException(ElasticsearchOperation.Index, $"{_configuration["Elasticsearch:Index"]}", $"{gameDocument.Id}");

			await transaction.CommitAsync();
		}
		catch (Exception ex)
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
}
