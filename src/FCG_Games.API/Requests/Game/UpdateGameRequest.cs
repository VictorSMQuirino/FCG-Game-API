namespace FCG_Games.API.Requests.Game;

public record UpdateGameRequest(string Title, string? Description, string Developer, string Publisher, decimal Price, DateOnly ReleaseDate);
