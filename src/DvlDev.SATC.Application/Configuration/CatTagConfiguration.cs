using DvlDev.SATC.Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DvlDev.SATC.Application.Configuration;

public class CatTagConfiguration : IEntityTypeConfiguration<CatTag>
{
    public void Configure(EntityTypeBuilder<CatTag> builder)
    {
        builder.ToTable("CatTags");
        builder.HasKey(x => new { x.CatId, x.TagId });
    }
}