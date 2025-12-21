namespace FCG.RabbitMQ.Events;

public record PaymentSucceededEvent(Guid OrderId, Guid UserId, Guid GameId, DateTime CreatedAt);
