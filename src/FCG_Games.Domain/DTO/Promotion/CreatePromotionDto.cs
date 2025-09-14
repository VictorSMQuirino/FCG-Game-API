namespace FCG_Games.Domain.DTO.Promotion;

public record CreatePromotionDto(Guid GameId, int DiscountPercentage, DateOnly Deadline);
