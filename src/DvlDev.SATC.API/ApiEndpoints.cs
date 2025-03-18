namespace DvlDev.SATC.API;

public static class ApiEndpoints
{
	private const string ApiBase = "api";

	public static class Cats
	{
		private const string Base = $"{ApiBase}/cats";

		public const string Create = Base;
		public const string Get = $"{Base}/{{idOrCatId}}";
		public const string GetAll = Base;
	}
}