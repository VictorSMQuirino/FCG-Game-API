using FCG_Games.Domain.Entities;
using FCG_Games.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG_Games.Infrastructure.EntityTypeConfiguration;

public class GameEntityTypeConfiguration : IEntityTypeConfiguration<Game>
{
	public void Configure(EntityTypeBuilder<Game> builder)
	{
		builder.ToTable("Games");
		builder.HasKey(game => game.Id);
		builder.Property(game => game.Title).IsRequired().HasMaxLength(50);
		builder.Property(game => game.ReleaseDate).IsRequired();
		builder.Property(game => game.Price).IsRequired();
		builder.Property<List<Genre>>(game => game.Genres).HasColumnType("integer[]").IsRequired();

		builder.HasMany(game => game.UserGames).WithOne(userGame => userGame.Game);

		builder.HasIndex(game => game.Title).IsUnique();
	}
}
