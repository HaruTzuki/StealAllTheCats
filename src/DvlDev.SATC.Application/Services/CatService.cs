using System.Text.Json;
using DvlDev.SATC.Application.Models;
using DvlDev.SATC.Application.Repositories;
using DvlDev.SATC.Contracts.Responses;
using DvlDev.SATC.Shared.Graphics;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DvlDev.SATC.Application.Services;

public class CatService(
	ICatRepository catRepository,
	IValidator<Cat> catValidator,
	IValidator<GetAllCatsOptions> getAllCatsOptionsValidator,
	IHttpClientFactory httpClientFactory,
	ILogger<CatService> logger) : ICatService
{
	const string WebRoot = "wwwroot";
	
	public async Task<bool> CreateAsync(Cat cat, CancellationToken cancellationToken = default)
	{
		await catValidator.ValidateAndThrowAsync(cat, cancellationToken);
		return await catRepository.CreateAsync(cat, cancellationToken);
	}

	public Task<Cat?> GetCatByIdAsync(int id, CancellationToken cancellationToken = default)
	{
		return catRepository.GetCatByIdAsync(id, cancellationToken);
	}

	public Task<Cat?> GetCatByCatIdAsync(string catId, CancellationToken cancellationToken = default)
	{
		return catRepository.GetCatByCatIdAsync(catId, cancellationToken);
	}

	public async Task<IEnumerable<Cat>> GetAllAsync(GetAllCatsOptions options, CancellationToken cancellationToken = default)
	{
		await getAllCatsOptionsValidator.ValidateAndThrowAsync(options, cancellationToken);
		return await catRepository.GetAllAsync(options, cancellationToken);
	}

	public async Task<bool> FetchAsync(CancellationToken cancellationToken = default)
	{
		using var httpClient = httpClientFactory.CreateClient("CatSaS");
		var response = await httpClient.GetAsync(
			"v1/images/search?size=med&mime_types=jpg&format=json&has_breeds=true&order=RANDOM&page=0&include_breeds=1&limit=25",
			cancellationToken);
		if (!response.IsSuccessStatusCode)
			return false;

		var content = await response.Content.ReadAsStringAsync(cancellationToken);
		var cats = JsonSerializer.Deserialize<IEnumerable<CaaSResponse.Cat>>(content);

		if (cats is null)
			return false;
		
		List<Cat> _cats = [];

		try
		{
			await Task.WhenAll(cats.Select(async cat =>
			{
				foreach (var breed in cat.Breeds)
				{
					var catModel = new Cat
					{
						CatId = cat.Id,
						Height = cat.Height,
						Width = cat.Width,
						CreatedOn = DateTime.UtcNow
					};

					var temperaments = breed.Temperament.Split(",");

					foreach (var temperament in temperaments)
					{
						var tag = new Tag
						{
							Name = temperament.Trim(),
							CreatedOn = DateTime.UtcNow
						};

						catModel.CatTags.Add(new CatTag
						{
							Cat = catModel,
							Tag = tag
						});
					}

					catModel.Image = await ImageHelper.DownloadImageFromUrl(cat.Url, "images", WebRoot, catModel.CatId);

					_cats.Add(catModel);
				}
			}));
			
			await catRepository.CreateBulkAsync(_cats, cancellationToken);
		}
		catch (Exception e)
		{
			logger.LogError("An error has occurred while fetching cats: {Error}", e.Message);
			// Delete Images
			if (_cats.Count != 0)
			{
				ImageHelper.DeleteBulkImages(_cats.Select(c => c.Image!));
			}
		}

		return true;
	}

	public Task<int> GetCountAsync(string? tag, CancellationToken cancellationToken = default)
	{
		return catRepository.GetCountAsync(tag, cancellationToken);
	}
}