namespace FCG_Games.API.Requests.Game;

public record GetRecommendationsRequest(Guid UserId, int? TopGenredCount, int? RecommentetionSize);
