namespace FCG_Games.Domain.Entities;

public abstract class BaseEntity
{
	protected BaseEntity()
	{
		Id = Guid.NewGuid();
		CreatedAt = DateTime.UtcNow;
	}

	public Guid Id { get; set; }
	public DateTime CreatedAt { get; private set; }
	public DateTime? UpdatedAt { get; set; }
}
