using FCG_Games.Domain.Entities;
using FCG_Games.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG_Games.Infrastructure.EntityTypeConfiguration;

public class UserGameEntityTypeConfiguration : IEntityTypeConfiguration<UserGame>
{
	public void Configure(EntityTypeBuilder<UserGame> builder)
	{
		builder.ToTable("UserGames");
		builder.HasKey(userGame => userGame.Id);
		builder.Property(userGame => userGame.GameAccessState).IsRequired().HasDefaultValue(GameAccessState.Blocked);

		builder.HasIndex(userGame => new { userGame.GameId, userGame.UserId }).IsUnique();
	}
}
