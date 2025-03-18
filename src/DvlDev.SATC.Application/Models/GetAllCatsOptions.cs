namespace DvlDev.SATC.Application.Models;

public class GetAllCatsOptions
{
	public string? Tags { get; set; }
	public string? SortField { get; set; }
	public SortOrder? SortOrder { get; set; }
	public int Page { get; set; }
	public int PageSize { get; set; }
}

public enum SortOrder
{
	Unsorted,
	Ascending,
	Descending
}