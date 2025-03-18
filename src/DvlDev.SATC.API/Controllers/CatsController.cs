using DvlDev.SATC.API.Mappers;
using DvlDev.SATC.Application.Services;
using DvlDev.SATC.Contracts.Requests;
using DvlDev.SATC.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace DvlDev.SATC.API.Controllers;

[ApiController]
public class CatsController(ICatService catService) : ControllerBase
{
	[HttpPost(ApiEndpoints.Cats.Fetch)]
	[ProducesResponseType(StatusCodes.Status201Created)]
	public async Task<IActionResult> Fetch()
	{
		return Created();
	}
	
	[HttpPost(ApiEndpoints.Cats.Get)]
	[ProducesResponseType(typeof(CatResponse),StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async  Task<IActionResult> Get([FromRoute] string idOrCatId, CancellationToken cancellationToken = default)
	{
		var cat = int.TryParse(idOrCatId, out var catId) 
			? await catService.GetCatByIdAsync(catId, cancellationToken)
			: await catService.GetCatByCatIdAsync(idOrCatId, cancellationToken);

		if (cat is null)
		{
			return NotFound();
		}

		var response = cat.MapToResponse();
		return Ok(response);
	}

	[HttpPost(ApiEndpoints.Cats.GetAll)]
	[ProducesResponseType(typeof(CatsResponse), StatusCodes.Status200OK)]
	public async Task<IActionResult> GetAll([FromQuery] GetAllCatsRequest req,
		CancellationToken cancellationToken = default)
	{
		var options = req.MapToOptions();
		
		var cats = await catService.GetAllAsync(options, cancellationToken);
		
		var catsCount = cats.Count();
		return Ok(cats.MapToResponse(req.Page, req.PageSize, catsCount));
	}
	
}