using FCG_Games.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG_Games.Infrastructure.EntityTypeConfiguration;

public class PromotionEntityTypeConfiguration : IEntityTypeConfiguration<Promotion>
{
	public void Configure(EntityTypeBuilder<Promotion> builder)
	{
		builder.ToTable("Promotions");
		builder.HasKey(promotion => promotion.Id);
		builder.Property(promotion => promotion.DiscountPercentage).IsRequired();
		builder.Property(promotion => promotion.Deadline).IsRequired();

		builder.HasOne(promotion => promotion.Game);

		builder.HasIndex(promotion => new { promotion.GameId, promotion.Active }).IsUnique();
	}
}
