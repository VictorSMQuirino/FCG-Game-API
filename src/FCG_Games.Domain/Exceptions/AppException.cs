using FCG_Games.Domain.Enums;
using System.Net;

namespace FCG_Games.Domain.Exceptions;

public abstract class AppException(string message, ErrorCode errorCode, HttpStatusCode statusCode = HttpStatusCode.BadRequest) : Exception(message)
{
	public ErrorCode ErrorCode { get; set; } = errorCode;
	public HttpStatusCode StatusCode { get; set; } = statusCode;
}
