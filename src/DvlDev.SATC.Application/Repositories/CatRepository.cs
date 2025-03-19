using DvlDev.SATC.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace DvlDev.SATC.Application.Repositories;

public class CatRepository(DataContext context) : ICatRepository
{
	public async Task<bool> CreateAsync(Cat cat, CancellationToken cancellationToken = default)
	{
		if (await ExistsAsync(cat.CatId, cancellationToken))
			return false;
		
		context.Cats.Add(cat);
		
		return await context.SaveChangesAsync(cancellationToken) > 0;
	}

	public async Task<bool> CreateBulkAsync(IEnumerable<Cat> cats, CancellationToken cancellationToken = default)
	{
		await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
		
		try
		{
			var tagCache = await context.Tags
				.ToDictionaryAsync(t => t.Name, StringComparer.OrdinalIgnoreCase, cancellationToken);
			
			foreach (var cat in cats)
			{
				if (await ExistsAsync(cat.CatId, cancellationToken))
					continue;

				foreach (var catTag in cat.CatTags)
				{
					var tagName = catTag.Tag.Name;
					if (tagCache.TryGetValue(tagName, out var existingTag))
					{
						// Reuse existing tag instance.
						catTag.Tag = existingTag;
					}
					else
					{
						// Create new tag, add it to the context and update the cache.
						var newTag = new Tag
						{
							Name = tagName,
							CreatedOn = DateTime.UtcNow
						};
						context.Tags.Add(newTag);
						tagCache[tagName] = newTag;
						catTag.Tag = newTag;
					}
				}
				
				context.Cats.Add(cat);
			}
			await context.SaveChangesAsync(cancellationToken);
			await transaction.CommitAsync(cancellationToken);
			return true;
		}
		catch (Exception e)
		{
			await transaction.RollbackAsync(cancellationToken);
			throw;
		}
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

	public async Task<IEnumerable<Cat>> GetAllAsync(GetAllCatsOptions options, CancellationToken cancellationToken = default)
	{
		var cats = await context.Cats.ToListAsync(cancellationToken);
		
		return cats;
	}

	public async Task<bool> ExistsAsync(string catId, CancellationToken cancellationToken = default)
	{
		return await context.Cats.AnyAsync(c => c.CatId == catId, cancellationToken);
	}
}