using FCG_Games.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FCG_Games.Infrastructure.Data;

public class FCGGamesDbContext(DbContextOptions<FCGGamesDbContext> options) : DbContext(options)
{
	public DbSet<Game> Games { get; set; }
	public DbSet<Promotion> Promotions { get; set; }
	public DbSet<UserGame> UserGames { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(FCGGamesDbContext).Assembly);

		base.OnModelCreating(modelBuilder);
	}
}
