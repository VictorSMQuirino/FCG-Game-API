namespace FCG_Games.Domain.DTO;

public record CreateGameDto(string Title, decimal Price, DateOnly ReleaseDate);
