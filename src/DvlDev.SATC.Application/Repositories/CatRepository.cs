using DvlDev.SATC.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace DvlDev.SATC.Application.Repositories;

public class CatRepository(DataContext context) : ICatRepository
{
	public async Task<bool> CreateAsync(Cat cat, CancellationToken cancellationToken = default)
	{
		context.Cats.Add(cat);
		
		return await context.SaveChangesAsync(cancellationToken) > 0;
	}

	public async Task<Cat?> GetCatByIdAsync(int id, CancellationToken cancellationToken = default)
	{
		var cat = await context.Cats.FirstOrDefaultAsync(c => c.Id == id, cancellationToken: cancellationToken);

		if (cat == null)
		{
			return null;
		}
		
		//TODO: Include CatTags
		
		return cat;
	}

	public async Task<Cat?> GetCatByCatIdAsync(string catId, CancellationToken cancellationToken = default)
	{
		var cat = await context.Cats.FirstOrDefaultAsync(c => c.CatId == catId, cancellationToken: cancellationToken);

		if (cat == null)
		{
			return null;
		}
		
		//TODO: Include CatTags
		
		return cat;
	}

	public async Task<IEnumerable<Cat>> GetAllAsync(CancellationToken cancellationToken = default)
	{
		var cats = await context.Cats.ToListAsync(cancellationToken);
		
		return cats;
	}
}