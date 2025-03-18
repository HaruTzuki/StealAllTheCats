using DvlDev.SATC.Application.Models;
using DvlDev.SATC.Application.Repositories;

namespace DvlDev.SATC.Application.Services;

public class CatService(ICatRepository catRepository) : ICatService
{
	public Task<bool> CreateAsync(Cat cat, CancellationToken cancellationToken = default)
	{
		// TODO: Validation
		return catRepository.CreateAsync(cat, cancellationToken);
	}

	public Task<Cat?> GetCatByIdAsync(int id, CancellationToken cancellationToken = default)
	{
		return catRepository.GetCatByIdAsync(id, cancellationToken);
	}

	public Task<Cat?> GetCatByCatIdAsync(string catId, CancellationToken cancellationToken = default)
	{
		return catRepository.GetCatByCatIdAsync(catId, cancellationToken);
	}

	public Task<IEnumerable<Cat>> GetAllAsync(CancellationToken cancellationToken = default)
	{
		//TODO: Validation
		return catRepository.GetAllAsync(cancellationToken);
	}
}