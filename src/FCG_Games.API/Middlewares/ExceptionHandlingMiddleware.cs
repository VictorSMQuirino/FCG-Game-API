using FCG_Games.API.Responses;
using FCG_Games.Domain.Enums;
using FCG_Games.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;

namespace FCG_Games.API.Middlewares;

public class ExceptionHandlingMiddleware : IExceptionHandler
{
	private readonly ILogger<ExceptionHandlingMiddleware> _logger;

	public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
	{
		_logger = logger;
	}

	public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
	{
		var request = httpContext.Request;
		var method = request.Method;
		var path = request.Path;
		var queryString = request.QueryString.HasValue ? request.QueryString.Value : string.Empty;
		int? statusCode = null;

		ErrorResponse? response = null;

		if (exception is AppException appException)
		{
			object? details = appException switch
			{
				DomainException ex => new { ex.EntityName, ex.PropertyName, ex.AttemptedValue },
				ValidationErrorsException ex => ex.Errors,
				NotFoundException ex => new { ex.EntityName, ex.Key },
				_ => null
			};

			response = new ErrorResponse(
				appException.ErrorCode.ToString(),
				appException.Message,
				details,
				DateTime.UtcNow
				);

			statusCode = (int)appException.StatusCode;

			httpContext.Response.StatusCode = statusCode.Value;

			_logger.LogWarning(
				"Warning in {Method} {Path}{QueryString} => Status code: {StatusCode}. Error code: {ErrorCode}. Message: {Message}",
				method,
				path,
				queryString,
				statusCode,
				appException.ErrorCode.ToString(),
				appException.Message
				);
		}
		else
		{
			response = new ErrorResponse(
				ErrorCode.UnexpectedError.ToString(),
				exception.Message,
				null,
				DateTime.UtcNow
				);

			statusCode = (int)HttpStatusCode.InternalServerError;

			_logger.LogError(
				"Error in {Method} {Path}{QueryString} => Status code: {StatusCode}. Message: {Message}",
				method,
				path,
				queryString,
				(int)statusCode,
				exception.Message);
		}

		httpContext.Response.ContentType = "application/json";

		var json = JsonSerializer.Serialize(response);
		await httpContext.Response.WriteAsync(json, cancellationToken);

		return true;
	}
}
