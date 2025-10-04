using FCG_Games.Domain.Enums;

namespace FCG_Games.Domain.Entities;

public class UserGame : BaseEntity
{
	public Guid UserId { get; set; }
	public Guid GameId { get; set; }
	public Game? Game { get; set; }
	public GameAccessState GameAccessState { get; set; }
}
