namespace DvlDev.SATC.Application.Models;

public class CatTag
{
	public required int CatId { get; init; }
	public Cat? Cat { get; set; }
	public required int TagId { get; init; }
	public Tag? Tag { get; set; }
}