using DvlDev.SATC.Application.Models;
using DvlDev.SATC.Application.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DvlDev.SATC.Application.UnitTests.Repositories;

public class CatRepositoryTests
{
	private readonly ICatRepository _catRepository;
	public CatRepositoryTests()
	{
		var options = new DbContextOptionsBuilder<DataContext>()
			.UseInMemoryDatabase(Guid.NewGuid().ToString())
			.Options;
		
		_catRepository = new CatRepository(new DataContext(options));
	}
	
	[Fact]
	public async Task GetCats_ShouldReturnAllCats()
	{
		// Arrange
		var cats = new List<Cat>
		{
			new Cat {Id = 1, CatId = "Cat1", Image = "/images/cat1.jpg", CreatedOn = DateTime.UtcNow, Width = 100, Height = 100},
			new Cat {Id = 2, CatId = "Cat2", Image =  "/images/cat2.jpg", CreatedOn = DateTime.UtcNow, Width = 100, Height = 100},
			new Cat {Id = 3, CatId = "Cat3", Image = "/images/cat3.jpg", CreatedOn = DateTime.UtcNow, Width = 100, Height = 100}
		};

		var catTags = new List<CatTag>
		{
			new ()
			{
				Cat = cats[0],
				Tag = new Tag()
				{
					Name = "Playful",
					CreatedOn = DateTime.UtcNow
				}
			},
			new()
			{
				Cat = cats[1],
				Tag = new Tag()
				{
					Name = "Agile",
					CreatedOn = DateTime.UtcNow
				}
			},
			new()
			{
				Cat = cats[2],
				Tag = new Tag()
				{
					Name = "Intelligent",
					CreatedOn = DateTime.UtcNow
				}
			}
			
		};

		for (var i = 0; i < cats.Count; i++)
		{
			cats[i].CatTags = [catTags[i]];
		}
		
		
		
		foreach (var cat in cats)
		{
			var isCreated = await _catRepository.CreateAsync(cat);
			
			Assert.True(isCreated);
		}
		
		// Act
		var result = await _catRepository.GetAllAsync(new GetAllCatsOptions{Page = 1, PageSize = 10});
		
		// Assert
		Assert.Equal(3, result.Count());
	}
}