namespace DvlDev.SATC.Contracts.Requests;

public class GetAllCatsRequest : PagedRequest
{
	public required string? CatId { get; init; }
	public required string? SortBy { get; init; } 
}