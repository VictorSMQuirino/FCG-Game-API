namespace FCG_Games.Domain.Entities;

public class Game : BaseEntity
{
	public required string Title { get; set; }
	public required decimal Price { get; set; }
	public required DateOnly ReleaseDate { get; set; }
	public ICollection<UserGame> UserGames { get; set; }
}
