using DvlDev.SATC.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace DvlDev.SATC.Application;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
	#region DbSets
	public DbSet<Cat> Cats { get; set; }
	public DbSet<Tag> Tags { get; set; }
	public DbSet<CatTag> CatTags { get; set; }
	#endregion
}