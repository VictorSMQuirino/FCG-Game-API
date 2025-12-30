using Elastic.Clients.Elasticsearch;
using FCG.RabbitMQ.Events;
using FCG_Games.Domain.DTO.Elasticsearch.ElasticsearchDocuments;
using FCG_Games.Domain.Enums;
using FCG_Games.Domain.Exceptions;
using FCG_Games.Domain.Interfaces.Repositories;
using FCG_Games.Domain.Interfaces.Services;
using MassTransit;

namespace FCG_Games.API.Consumers;

public class PaymentSucceededConsumer : IConsumer<PaymentSucceededEvent>
{
	private readonly IUserGameRepository _userGameRepository;
	private readonly ElasticsearchClient _elasticsearchClient;
	private readonly IApplicationUserService _applicationUserService;
	private readonly IConfiguration _configuration;
	private readonly ILogger<PaymentSucceededConsumer> _logger;

	public PaymentSucceededConsumer(
		IUserGameRepository userGameRepository,
		ElasticsearchClient elasticsearchClient,
		IApplicationUserService applicationUserService,
		IConfiguration configuration,
		ILogger<PaymentSucceededConsumer> logger)
	{
		_userGameRepository = userGameRepository;
		_elasticsearchClient = elasticsearchClient;
		_applicationUserService = applicationUserService;
		_configuration = configuration;
		_logger = logger;
	}

	public async Task Consume(ConsumeContext<PaymentSucceededEvent> context)
	{
		_logger.LogInformation("Consuming PaymentSucceededEvent for UserId: {UserId}, GameId: {GameId}",
			context.Message.UserId, context.Message.GameId);

		var loggedUserId = _applicationUserService.GetUserId();
		var msg = context.Message;

		var userGameInDb = await _userGameRepository.GetBy(ug =>
			ug.UserId == context.Message.UserId &&
			ug.GameId == context.Message.GameId &&
			ug.GameAccessState == GameAccessState.Blocked
			);

		if (userGameInDb is not null)
		{
			userGameInDb.GameAccessState = GameAccessState.Guaranteed;

			await _userGameRepository.UpdateAsync(userGameInDb);

			var elasticUpdateResponse = await _elasticsearchClient
				.UpdateAsync<GameDocument, object>(
				_configuration["Elasticsearch:Index"]!,
				msg.GameId,
				u => u.Script(s =>
					s.Source("ctx._source.ownerUserIds.add(params.userId)")
					.Params(p => p.Add("userId", loggedUserId))
					)
				);

			if (!elasticUpdateResponse.IsValidResponse)
				throw new ElasticsearchException(ElasticsearchOperation.Update);
		}
	}
}
