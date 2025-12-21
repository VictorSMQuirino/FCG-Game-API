namespace FCG.RabbitMQ.Events;

public record GamePurchaseRequestedEvent(string UserId, string GameId, string PaymentInfo, decimal GamePrice, DateTime CreatedAt);
