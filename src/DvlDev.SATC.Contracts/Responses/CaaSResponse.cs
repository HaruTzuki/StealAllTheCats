using System.Text.Json.Serialization;

namespace DvlDev.SATC.Contracts.Responses;

public class CaaSResponse
{
	public class Cat
	{
		[JsonPropertyName("id")]
		public required string Id { get; init; }
		[JsonPropertyName("url")]
		public required string Url { get; init; }
		[JsonPropertyName("width")]
		public required int Width { get; init; }
		[JsonPropertyName("height")]
		public required int Height { get; init; }
		[JsonPropertyName("breeds")]
		public required List<Breed> Breeds { get; init; }
	}

	public class Breed
	{
		[JsonPropertyName("temperament")]
		public required string Temperament { get; init; }
	}
}