using DvlDev.SATC.Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DvlDev.SATC.Application.Configuration;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
	public void Configure(EntityTypeBuilder<Tag> builder)
	{ 
		builder.ToTable("Tags");
		
		builder.HasKey(tag => tag.Id);
		builder.Property(tag => tag.Id).ValueGeneratedOnAdd();

		builder.Property(tag => tag.Name).IsRequired().HasMaxLength(50);
		builder.Property(tag => tag.CreatedOn).IsRequired();
		
		builder.HasIndex(tag => tag.Name).IsUnique();
		
		builder.HasMany(tag => tag.CatTags)
			.WithOne(catTag => catTag.Tag)
			.HasForeignKey(catTag => catTag.TagId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}