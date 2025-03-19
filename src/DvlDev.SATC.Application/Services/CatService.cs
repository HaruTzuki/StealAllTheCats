﻿using DvlDev.SATC.Application.Models;
using DvlDev.SATC.Application.Repositories;
using FluentValidation;

namespace DvlDev.SATC.Application.Services;

public class CatService(ICatRepository catRepository, IValidator<Cat> catValidator, IHttpClientFactory httpClientFactory) : ICatService
{
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

	public Task<IEnumerable<Cat>> GetAllAsync(GetAllCatsOptions options, CancellationToken cancellationToken = default)
	{
		//TODO: Validation
		return catRepository.GetAllAsync(options, cancellationToken);
	}
	public async Task<bool> FetchAsync(CancellationToken cancellationToken = default)
	{
		using var httpClient = httpClientFactory.CreateClient();
		var response = await httpClient.GetAsync("https://api.thecatapi.com/v1/images/search", cancellationToken);

		return true;
	}
}