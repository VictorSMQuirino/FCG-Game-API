namespace FCG_Games.Domain.DTO;

public record CreateGameDto(string Title, string? Description, string Developer, string Publisher, decimal Price, DateOnly ReleaseDate);
