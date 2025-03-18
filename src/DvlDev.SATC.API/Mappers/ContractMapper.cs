using DvlDev.SATC.Application.Models;
using DvlDev.SATC.Contracts.Responses;

namespace DvlDev.SATC.API.Mappers;

public static class ContractMapper
{
	public static CatResponse MapToResponse(this Cat cat)
	{
		return new CatResponse
		{
			Id = cat.Id,
			CatId = cat.CatId,
			Width = cat.Width,
			Height = cat.Height,
			Image = cat.Image
		};
	}

	public static CatsResponse MapToResponse(this IEnumerable<Cat> cats, int page, int pageSize, int totalCount)
	{
		return new CatsResponse
		{
			Items = cats.Select(c => c.MapToResponse()),
			Page = page,
			PageSize = pageSize,
			Total = totalCount
		};
	}
}