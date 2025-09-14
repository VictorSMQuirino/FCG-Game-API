namespace FCG_Games.Domain.DTO.Promotion;

public record PromotionDto(Guid Id, GameDto Game, int DiscountPercentage, DateOnly Deadline, bool Active);
