namespace FCG_Games.API.Responses;

public record ErrorResponse(string Code, string Message, object? Details, DateTime Timestamp);
