using FCG_Games.Domain.Enums;

namespace FCG_Games.API.Requests.Game;

public record CreateGameRequest(string Title, string? Description, string Developer, string Publisher, decimal Price, DateOnly ReleaseDate, ICollection<Genre> Genres);
