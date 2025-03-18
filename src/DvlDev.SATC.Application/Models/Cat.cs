namespace DvlDev.SATC.Application.Models;

public class Cat : BaseEntity
{
	public required string CatId { get; init; }
	public required int Width { get; init; }
	public required int Height { get; init; }
	public required string Image { get; init; }

	public List<CatTag> CatTags { get; set; } = [];
}