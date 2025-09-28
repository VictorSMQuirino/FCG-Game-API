using FCG_Games.Domain.Enums;

namespace FCG_Games.Domain.Entities;

public class Game : BaseEntity
{
	public required string Title { get; set; }
	public string? Description { get; set; }
	public required string Developer { get; set; }
	public required string Publisher { get; set; }
	public required decimal Price { get; set; }
	public required DateOnly ReleaseDate { get; set; }
	public ICollection<UserGame> UserGames { get; set; } = [];
	public List<Genre> Genres { get; set; } = [];
}
