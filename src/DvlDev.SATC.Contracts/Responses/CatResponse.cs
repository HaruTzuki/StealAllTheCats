namespace DvlDev.SATC.Contracts.Responses;

public class CatResponse
{
	public required int Id { get; init; }
	public required string CatId { get; init; }
	public required int Width { get; init; }
	public required int Height { get; init; }
	public required string Image { get; init; }
	public List<string> Tags { get; init; } = [];
}