using System.Diagnostics;

namespace FCG_Games.API.Middlewares;

public class LoggingMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<LoggingMiddleware> _logger;

	public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
	{
		_next = next;
		_logger = logger;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		var stopwatch = Stopwatch.StartNew();

		var request = context.Request;
		var method = request.Method;
		var path = request.Path;
		var queryString = request.QueryString.HasValue ? request.QueryString.Value : string.Empty;

		_logger.LogInformation(
			"Starting: {Method} {Path} {QueryString}",
			method,
			path,
			queryString
			);

		await _next(context);

		var statusCode = context.Response.StatusCode;
		stopwatch.Stop();
		var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
		var isSuccess = context.Response.StatusCode < 400;
		Action<string, object[]> logAction = isSuccess
			? _logger.LogInformation
			: _logger.LogWarning;

		logAction(
			"Finished: {Method} {Path} {QueryString} => Status code: {StatusCode} in {ElapsedMilliseconds} ms",
			[method, path, queryString, statusCode, elapsedMilliseconds]
			);
	}
}
