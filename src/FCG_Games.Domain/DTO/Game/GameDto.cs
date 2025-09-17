namespace FCG_Games.Domain.DTO;

public record GameDto(Guid Id, string Title, string? Description, string Developer, string Publisher, decimal Price, DateOnly ReleaseDate);
