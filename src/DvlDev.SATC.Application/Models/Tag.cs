namespace DvlDev.SATC.Application.Models;

public class Tag : BaseEntity
{
	public required string Name { get; init; }
	
	public List<CatTag> CatTags { get; set; } = [];
}