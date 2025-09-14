namespace FCG_Games.Domain.DTO;

public record GameDto(Guid Id, string Title, decimal Price, DateOnly ReleaseDate);
