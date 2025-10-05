namespace FCG_Games.Domain.External.Payments.DTO;

public class PurchaseData
{
	public required string UserId { get; set; }
	public required string GameId { get; set; }
	public required string PaymentInfo { get; set; }
}
