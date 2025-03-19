namespace DvlDev.SATC.Application.Models;

public class CatTag
{
	public int CatId { get; init; }
	public Cat? Cat { get; set; }
	public int TagId { get; init; }
	public Tag? Tag { get; set; }
}