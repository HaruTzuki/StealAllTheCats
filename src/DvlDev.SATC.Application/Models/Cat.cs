namespace DvlDev.SATC.Application.Models;

public class Cat : BaseEntity
{
	public required string CatId { get; init; }
	public int Width { get; init; }
	public int Height { get; init; }
	public string? Image { get; set; }

	public List<CatTag> CatTags { get; set; } = [];
}