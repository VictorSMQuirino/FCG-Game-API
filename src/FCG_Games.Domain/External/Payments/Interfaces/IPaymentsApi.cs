using FCG.RabbitMQ.Events;
using FCG_Games.Domain.External.Payments.DTO;
using Refit;

namespace FCG_Games.Domain.External.Payments.Interfaces;

public interface IPaymentsApi
{
	[Post("/api/process-payment")]
	Task<DurableOrchestrationStatus> StartPaymentProcessingAsync([Body] GamePurchaseRequestedEvent data);
}
