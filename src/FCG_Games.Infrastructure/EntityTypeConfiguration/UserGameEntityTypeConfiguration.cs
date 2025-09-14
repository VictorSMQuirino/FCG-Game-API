using FCG_Games.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG_Games.Infrastructure.EntityTypeConfiguration;

public class UserGameEntityTypeConfiguration : IEntityTypeConfiguration<UserGame>
{
	public void Configure(EntityTypeBuilder<UserGame> builder)
	{
		builder.ToTable("UserGames");
		builder.HasKey(userGame => userGame.Id);

		builder.HasIndex(userGame => new { userGame.GameId, userGame.UserId }).IsUnique();
	}
}
