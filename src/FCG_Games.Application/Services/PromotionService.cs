using FCG_Games.Application.Converters;
using FCG_Games.Application.Validators.Promotion;
using FCG_Games.Domain.DTO.Promotion;
using FCG_Games.Domain.Entities;
using FCG_Games.Domain.Exceptions;
using FCG_Games.Domain.Extensions;
using FCG_Games.Domain.Interfaces.Repositories;
using FCG_Games.Domain.Interfaces.Services;

namespace FCG_Games.Application.Services;

public class PromotionService : IPromotionService
{
	private readonly IPromotionRepository _promotionRepository;
	private readonly IGameRepository _gameRepository;

	public PromotionService(IPromotionRepository promotionRepository, IGameRepository gameRepository)
	{
		_promotionRepository = promotionRepository;
		_gameRepository = gameRepository;
	}


	public async Task<Guid> CreateAsync(CreatePromotionDto dto)
	{
		var validationResult = await new CreatePromotionValidator().ValidateAsync(dto);

		if (!validationResult.IsValid)
			throw new ValidationErrorsException(validationResult.Errors.ToErrorsPropertyDictionary());

		var gameExists = await _gameRepository.ExistsBy(game => game.Id == dto.GameId);

		if (!gameExists) throw new NotFoundException(nameof(Game), dto.GameId);

		var promotion = dto.ToEntity();

		await _promotionRepository.CreateAsync(promotion);

		return promotion.Id;
	}

	public async Task UpdateAsync(Guid id, UpdatePromotionDto dto)
	{
		var validationResult = await new UpdatePromotionValidator().ValidateAsync(dto);

		if (!validationResult.IsValid)
			throw new ValidationErrorsException(validationResult.Errors.ToErrorsPropertyDictionary());

		var promotion = await _promotionRepository.GetByIdAsync(id) ?? throw new NotFoundException(nameof(Promotion), id);

		var gameExists = await _gameRepository.ExistsBy(game => game.Id == dto.GameId);

		if (!gameExists) throw new NotFoundException(nameof(Game), dto.GameId);

		promotion = dto.ToEntity(promotion);

		await _promotionRepository.UpdateAsync(promotion);
	}

	public async Task<PromotionDto?> GetByIdAsync(Guid id)
	{
		var promotion = await _promotionRepository.GetByIdAsync(id, p => p.Game!) ?? throw new NotFoundException(nameof(Promotion), id);

		var gameDto = promotion.Game!.ToDto();

		return promotion.ToDto(gameDto);
	}

	public async Task<ICollection<PromotionDto>> GetAllAsync()
	{
		var promotionList = await _promotionRepository.GetAllAsync(p => p.Game!);

		return promotionList.ToDtoList();
	}

	public async Task<ICollection<PromotionDto>> GetAllActiveAsync()
	{
		var activePromotionsList = await _promotionRepository.GetAllActive();

		return activePromotionsList.ToDtoList();
	}

	public async Task ActivePromotionAsync(Guid id)
	{
		var promotion = await _promotionRepository.GetByIdAsync(id) ?? throw new NotFoundException(nameof(Promotion), id);

		if (promotion.Active) throw new DomainException("The promotion is already active.");

		var existsAnActivePromotionForGame = await _promotionRepository.ExistsBy(p =>
			p.GameId == promotion.GameId &&
			p.Id != id &&
			p.Active);

		if (existsAnActivePromotionForGame)
			throw new DomainException("Already exists an active promotion for this game");

		promotion.Active = true;
		await _promotionRepository.UpdateAsync(promotion);
	}

	public async Task DeactivePromotionAsync(Guid id)
	{
		var promotion = await _promotionRepository.GetByIdAsync(id) ?? throw new NotFoundException(nameof(Promotion), id);

		if (!promotion.Active) throw new DomainException("The promotion is already deactive.");

		promotion.Active = false;
		await _promotionRepository.UpdateAsync(promotion);
	}
}
