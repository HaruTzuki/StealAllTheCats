using DvlDev.SATC.Application.Models;

namespace DvlDev.SATC.Application.Services;

public interface ICatService
{
	Task<bool> CreateAsync(Cat cat, CancellationToken cancellationToken = default);
	Task<Cat?> GetCatByIdAsync(int id, CancellationToken cancellationToken = default);
	Task<Cat?> GetCatByCatIdAsync(string catId, CancellationToken cancellationToken = default);
	Task<IEnumerable<Cat>> GetAllAsync(GetAllCatsOptions options, CancellationToken cancellationToken = default);
	Task<bool> FetchAsync(CancellationToken cancellationToken = default);
	Task<int> GetCountAsync(string? tag, CancellationToken cancellationToken = default);
}