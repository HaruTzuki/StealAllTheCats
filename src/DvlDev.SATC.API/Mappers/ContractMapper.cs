using DvlDev.SATC.Application.Models;
using DvlDev.SATC.Contracts.Requests;
using DvlDev.SATC.Contracts.Responses;

namespace DvlDev.SATC.API.Mappers;

public static class ContractMapper
{
	public static CatResponse MapToResponse(this Cat cat, string? host)
	{
		
		return new CatResponse
		{
			Id = cat.Id,
			CatId = cat.CatId,
			Width = cat.Width,
			Height = cat.Height,
			Image = string.IsNullOrEmpty(host)? cat.Image! : host + cat.Image!,
			Tags = cat.CatTags.Select(catTag => catTag.Tag!.Name).ToList()
		};
	}

	public static CatsResponse MapToResponse(this IEnumerable<Cat> cats, int page, int pageSize, int totalCount, string? host)
	{
		return new CatsResponse
		{
			Items = cats.Select(c => c.MapToResponse(host)),
			Page = page,
			PageSize = pageSize,
			Total = totalCount
		};
	}

	public static GetAllCatsOptions MapToOptions(this GetAllCatsRequest request)
	{
		return new GetAllCatsOptions
		{
			Tags = request.Tag,
			SortField = request.SortBy?.Trim('+', '-'),
			SortOrder = request.SortBy is null ? SortOrder.Unsorted :
				request.SortBy.StartsWith('-') ? SortOrder.Descending : SortOrder.Ascending,
			Page = request.Page,
			PageSize = request.PageSize
		};
	}
}