namespace FCG_Games.API.Requests.Promotion;

public record UpdatePromotionRequest(Guid GameId, int DiscountPercentage, DateOnly Deadline);
