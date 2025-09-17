namespace FCG_Games.API.Responses.Game;

public record GetGameByIdResponse(Guid Id, string Title, string? Description, string Developer, string Publisher, decimal Price, DateOnly ReleaseDate);
