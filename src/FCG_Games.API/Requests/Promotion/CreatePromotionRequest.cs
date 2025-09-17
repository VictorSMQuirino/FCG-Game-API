namespace FCG_Games.API.Requests.Promotion;

public record CreatePromotionRequest(Guid GameId, int DiscountPercentage, DateOnly Deadline);
