using FCG_Games.Application.Converters;
using FCG_Games.Application.Validators.Game;
using FCG_Games.Domain.DTO;
using FCG_Games.Domain.Entities;
using FCG_Games.Domain.Exceptions;
using FCG_Games.Domain.Extensions;
using FCG_Games.Domain.Interfaces.Repositories;
using FCG_Games.Domain.Interfaces.Services;

namespace FCG_Games.Application.Services;

public class GameService : IGameService
{
	private readonly IGameRepository _gameRepository;

	public GameService(IGameRepository gameRepository)
	{
		_gameRepository = gameRepository;
	}

	public async Task<Guid> CreateAsync(CreateGameDto dto)
	{
		var validationResult = await new CreateGameValidator().ValidateAsync(dto);

		if (!validationResult.IsValid)
			throw new ValidationErrorsException(validationResult.Errors.ToErrorsPropertyDictionary());

		if (await _gameRepository.ExistsBy(game => game.Title.ToUpper().Equals(dto.Title.ToUpper())))
			throw new DuplicatedEntityException(nameof(Game), nameof(Game.Title), dto.Title);

		var newGame = dto.ToEntity();

		await _gameRepository.CreateAsync(newGame);

		return newGame.Id;
	}

	public async Task UpdateAsync(Guid id, UpdateGameDto dto)
	{
		var validationResult = await new UpdateGameValidator().ValidateAsync(dto);

		if (!validationResult.IsValid)
			throw new ValidationErrorsException(validationResult.Errors.ToErrorsPropertyDictionary());

		var game = await _gameRepository.GetByIdAsync(id) ?? throw new NotFoundException(nameof(Game), id);

		if (await _gameRepository.ExistsBy(gameInDb => gameInDb.Title.ToUpper().Equals(dto.Title.ToUpper()) && gameInDb.Id != game.Id))
			throw new DuplicatedEntityException(nameof(Game), nameof(Game.Title), dto.Title);

		game = dto.ToEntity(game);
		await _gameRepository.UpdateAsync(game);
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
