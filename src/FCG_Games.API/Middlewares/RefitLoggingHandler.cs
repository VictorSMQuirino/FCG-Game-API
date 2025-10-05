namespace FCG_Games.API.Middlewares;

public class RefitLoggingHandler : DelegatingHandler
{
	private readonly ILogger<RefitLoggingHandler> _logger;

	public RefitLoggingHandler(ILogger<RefitLoggingHandler> logger)
	{
		_logger = logger;
	}

	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		_logger.LogInformation("--- REQUISIÇÃO HTTP SAINDO ---");
		_logger.LogInformation("Endpoint: {Method} {Uri}", request.Method, request.RequestUri);

		if (request.Content != null)
		{
			// Lê o corpo como string para podermos logar
			var body = await request.Content.ReadAsStringAsync(cancellationToken);
			_logger.LogInformation("Corpo da Requisição: {Body}", body);
		}
		else
		{
			_logger.LogWarning("Corpo da Requisição: [VAZIO]");
		}
		_logger.LogInformation("----------------------------");

		return await base.SendAsync(request, cancellationToken);
	}
}
