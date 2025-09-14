namespace FCG_Games.Domain.DTO.Promotion;

public record UpdatePromotionDto(Guid GameId, int DiscountPercentage, DateOnly Deadline);
