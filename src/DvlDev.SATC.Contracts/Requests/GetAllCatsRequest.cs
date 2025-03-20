namespace DvlDev.SATC.Contracts.Requests;

public class GetAllCatsRequest : PagedRequest
{
	public string? Tag { get; init; }
	public string? SortBy { get; init; } 
}