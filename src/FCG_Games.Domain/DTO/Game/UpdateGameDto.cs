using FCG_Games.Domain.Enums;

namespace FCG_Games.Domain.DTO;

public record UpdateGameDto(string Title, string? Description, string Developer, string Publisher, decimal Price, DateOnly ReleaseDate, ICollection<Genre> Genres);
