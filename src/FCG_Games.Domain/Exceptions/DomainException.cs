using FCG_Games.Domain.Enums;
using System.Net;

namespace FCG_Games.Domain.Exceptions;

public class DomainException(
	string message,
	string? entityName = null,
	string? propertyName = null,
	object? attemptedValue = null,
	HttpStatusCode statusCode = HttpStatusCode.BadRequest
	) : AppException(message, ErrorCode.DomainError, statusCode)
{
	public string EntityName { get; set; } = entityName ?? string.Empty;
	public string PropertyName { get; set; } = propertyName ?? string.Empty;
	public object? AttemptedValue { get; set; } = attemptedValue;
}
