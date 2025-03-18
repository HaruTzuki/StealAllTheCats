using DvlDev.SATC.Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DvlDev.SATC.Application.Configuration;

public class CatConfiguration : IEntityTypeConfiguration<Cat>
{
	public void Configure(EntityTypeBuilder<Cat> builder)
	{
		builder.ToTable("Cats");
		
		builder.HasKey(cat => cat.Id);
		builder.Property(cat => cat.Id).ValueGeneratedOnAdd();

		builder.Property(cat => cat.CatId).IsRequired().HasMaxLength(50);
		builder.Property(cat => cat.Width).IsRequired();
		builder.Property(cat => cat.Height).IsRequired();
		builder.Property(cat => cat.Image).IsRequired();
		builder.Property(cat => cat.CreatedOn).IsRequired();

		builder.HasIndex(cat => cat.CatId).IsUnique();

		builder.HasMany(cat => cat.CatTags)
			.WithOne(catTag => catTag.Cat)
			.HasForeignKey(catTag => catTag.CatId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}