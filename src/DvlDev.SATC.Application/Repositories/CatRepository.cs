using System.Reflection;
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
		catch (Exception)
		{
			await transaction.RollbackAsync(cancellationToken);
			throw;
		}
	}

	public async Task<Cat?> GetCatByIdAsync(int id, CancellationToken cancellationToken = default)
	{
		var cat = await context.Cats
			.Include(ct => ct.CatTags)
			.ThenInclude(ct => ct.Tag)
			.FirstOrDefaultAsync(c => c.Id == id, cancellationToken: cancellationToken);

		return cat;
	}

	public async Task<Cat?> GetCatByCatIdAsync(string catId, CancellationToken cancellationToken = default)
	{
		var cat = await context.Cats
			.Include(ct => ct.CatTags)
			.ThenInclude(ct => ct.Tag)
			.FirstOrDefaultAsync(c => c.CatId == catId, cancellationToken: cancellationToken);

		return cat;
	}

	public async Task<IEnumerable<Cat>> GetAllAsync(GetAllCatsOptions options,
		CancellationToken cancellationToken = default)
	{
		var query = context.Cats
			.Include(ct => ct.CatTags)
			.ThenInclude(ct => ct.Tag)
			.Where(c => options.Tags == null || c.CatTags.Any(ct => options.Tags.Contains(ct.Tag!.Name)));
			
		if (!string.IsNullOrEmpty(options.SortField))
		{
			// Resolve the actual property name with case-insensitive comparison.
			var sortProperty = typeof(Cat)
				.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.FirstOrDefault(p => string.Equals(p.Name, options.SortField, StringComparison.OrdinalIgnoreCase))
				?.Name;

			if (sortProperty is not null)
			{
				query = options.SortOrder == SortOrder.Ascending
					? query.OrderBy(c => EF.Property<object>(c, sortProperty))
					: query.OrderByDescending(c => EF.Property<object>(c, sortProperty));
			}
		}
		
		query = query.Skip(options.Page * options.PageSize)
			.Take(options.PageSize);

		return await query.ToListAsync(cancellationToken);
	}

	public async Task<bool> ExistsAsync(string catId, CancellationToken cancellationToken = default)
	{
		return await context.Cats.AnyAsync(c => c.CatId == catId, cancellationToken);
	}

	public Task<int> GetCountAsync(string? tag, CancellationToken cancellationToken = default)
	{
		if (tag?.Trim() == string.Empty)
		{
			tag = null;
		}
		return context.Cats.CountAsync(x => tag == null || x.CatTags.Any(ct => ct.Tag.Name == tag), cancellationToken);
	}
}