using FCG_Games.Domain.DTO;

namespace FCG_Games.API.Responses.Promotion;

public record GetPromotionByIdResponse(Guid Id, GameDto Game, int DiscountPercentage, DateOnly Deadline, bool Active);
