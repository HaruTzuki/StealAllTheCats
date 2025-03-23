using System.Net;
using System.Reflection;
using System.Text.Json;
using DvlDev.SATC.Application.Models;
using DvlDev.SATC.Application.Repositories;
using DvlDev.SATC.Application.Services;
using DvlDev.SATC.Application.Validators;
using DvlDev.SATC.Contracts.Responses;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;

namespace DvlDev.SATC.Application.UnitTests.Services;

public class CatServicesTests
{
	ICatService _catService;
	public CatServicesTests()
	{
		ICatRepository catRepository = new MockCatRepository();
		var catValidator = new CatValidator();
		var getAllCatsOptionsValidator = new GetAllCatsOptionsValidator();

		var filePath = Path.Combine(AppContext.BaseDirectory, "integrationdata", "api.json");
		
		var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
		handlerMock
			.Protected()
			.Setup<Task<HttpResponseMessage>>(
				"SendAsync",
				ItExpr.IsAny<HttpRequestMessage>(),
				ItExpr.IsAny<CancellationToken>())
			.ReturnsAsync(new HttpResponseMessage
			{
				StatusCode = HttpStatusCode.OK,
				Content = new StringContent(File.ReadAllText(filePath))
			});
		handlerMock
			.Protected()
			.Setup("Dispose", ItExpr.IsAny<bool>());
		var httpClient = new HttpClient(handlerMock.Object)
		{
			BaseAddress = new Uri("https://fakeurl.com/")
		};
		var httpClientFactoryMock = new Mock<IHttpClientFactory>();
		httpClientFactoryMock
			.Setup(f => f.CreateClient("CatSaS"))
			.Returns(httpClient);
		
		// Create mock for IWebHostEnvironment.
		

		// Create mock for ILogger<CatService>.
		var loggerMock = new Mock<ILogger<CatService>>();
		
		_catService = new CatService(
			catRepository,
			catValidator,
			getAllCatsOptionsValidator,
			httpClientFactoryMock.Object,
			loggerMock.Object);
	}
	
	
	[Fact]
	public async Task CreateAsync_WithValidCat_ReturnsTrue()
	{
		//Arrange
		var cat = new Cat
		{
			CatId = Guid.NewGuid().ToString("N"),
			Height = 100,
			Width = 100,
			CreatedOn = DateTime.UtcNow
		};
		//Act
		var result = await _catService.CreateAsync(cat);
		
		//Assert
		Assert.True(result);
	}
	
	[Fact]
	public async Task CreateAsync_WithInvalidCat_ThrowsValidationException()
	{
		var cat = new Cat
		{
			CatId = "123",
			Height = -5,
			Width = 100,
			CreatedOn = DateTime.UtcNow
		};
		cat.CatTags.Add(new CatTag { Tag = new Tag { Name = "Invalid" , CreatedOn = DateTime.UtcNow } });
		await Assert.ThrowsAsync<ValidationException>(() => _catService.CreateAsync(cat));
	}
	
	[Fact]
	public async Task GetCatByIdAsync_WithValidId_ReturnsCat()
	{
		//Arrange
		const int id = 5;
		
		//Act
		var result = await _catService.GetCatByIdAsync(id);
		
		//Assert
		Assert.NotNull(result);
	}
	
	[Fact]
	public async Task GetCatByIdAsync_WithInvalidId_ReturnsNull()
	{
		//Arrange
		const int invalidId = -2;
		
		//Act
		var result = await _catService.GetCatByIdAsync(invalidId);
		
		//Assert
		Assert.Null(result);
	}
	
	[Fact]
	public async Task GetCatByCatIdAsync_WithValidCatId_ReturnsCat()
	{
		const string catId = "2hPNn0Wrr";
		var result = await _catService.GetCatByCatIdAsync(catId);
		Assert.NotNull(result);
	}
	
	[Fact]
	public async Task GetCatByCatIdAsync_WithInvalidCatId_ReturnsNull()
	{
		//Arrange
		
		//Act
		var result = await _catService.GetCatByCatIdAsync("Invalid");
		
		//Assert
		Assert.Null(result);
	}
	
	[Fact]
	public async Task GetAllAsync_WithValidOptions_ReturnsCats()
	{
		//Arrange	
		
		//Act
		var result = await _catService.GetAllAsync(new GetAllCatsOptions()
		{
			PageSize = 1,
			Page = 10
		});
		
		//Assert
		Assert.NotEmpty(result);
	}
	
	[Fact]
	public async Task GetAllAsync_WithInvalidOptions_ThrowsValidationException()
	{
		//Arrange
		var options = new GetAllCatsOptions
		{
			PageSize = 0,
			Page = 0
		};
		
		//Act
		//Assert
		await Assert.ThrowsAsync<ValidationException>(() => _catService.GetAllAsync(options));
	}
	
	[Fact]
	public async Task FetchAsync_ReturnsTrue()
	{
		var result = await _catService.FetchAsync();
		Assert.True(result);
	}
	
	[Fact]
	public async Task GetCountAsync_WithValidTag_ReturnsCount()
	{
		var cat = new Cat
		{
			CatId = "123",
			Height = 100,
			Width = 100,
			CreatedOn = DateTime.UtcNow
		};
		cat.CatTags.Add(new CatTag { Tag = new Tag { Name = "Valid" , CreatedOn = DateTime.UtcNow } });
		await _catService.CreateAsync(cat);
		var result = await _catService.GetCountAsync("Valid");
		Assert.Equal(1, result);
	}
	
	[Fact]
	public async Task GetCountAsync_WithInvalidTag_ReturnsZero()
	{
		var result = await _catService.GetCountAsync("Invalid");
		Assert.Equal(0, result);
	}
	
	[Fact]
	public async Task GetCountAsync_WithEmptyTag_ReturnsCount()
	{
		var result = await _catService.GetCountAsync(null);
		Assert.True(result > 0);
	}
	
	[Fact]
	public async Task GetCountAsync_WithWhitespaceTag_ReturnsCount()
	{
		var result = await _catService.GetCountAsync(" ");
		Assert.True(result > 0);
	}
	
	[Fact]
	public async Task GetCountAsync_WithNonExistentTag_ReturnsZero()
	{
		var result = await _catService.GetCountAsync("NonExistent");
		Assert.Equal(0, result);
	}
	
	[Fact]
	public async Task GetCountAsync_WithTag_ReturnsCount()
	{
		var result = await _catService.GetCountAsync("Playful");
		Assert.NotEqual(0, result);
	}
}


public class MockCatRepository : ICatRepository
{
	static readonly List<Cat> _cats = new List<Cat>();
	private static readonly object _lock = new object();
	
	public MockCatRepository()
	{
		InitialiseCats().GetAwaiter().GetResult();
	}
	
	public async Task<bool> CreateAsync(Cat cat, CancellationToken cancellationToken = default)
	{
		if (await ExistsAsync(cat.CatId, cancellationToken))
		{
			return false;
		}

		lock (_lock)
		{
			var maxId = _cats.Any() ? _cats.Max(x => x.Id) + 1 : 1;
			
			var propertyInfo = typeof(Cat).GetProperty(nameof(Cat.Id));
			propertyInfo?.SetValue(cat, maxId);
			
			_cats.Add(cat);
		}
		
		return true;
	}

	public async Task<bool> CreateBulkAsync(IEnumerable<Cat> cats, CancellationToken cancellationToken = default)
	{
		var distinctTagNames = _cats
			.SelectMany(cat => cat.CatTags)
			.Select(catTag => catTag.Tag)
			.Distinct()
			.ToDictionary(tagName => tagName.Name);

		foreach (var cat in cats)
		{
			if (await ExistsAsync(cat.CatId, cancellationToken))
			{
				continue;
			}

			foreach (var catTag in cat.CatTags)
			{
				var tagName = catTag.Tag.Name;
				if (distinctTagNames.TryGetValue(tagName, out var existingTag))
				{
					catTag.Tag = existingTag;
				}
				else
				{
					var newTag = new Tag { Name = tagName, CreatedOn = DateTime.UtcNow };
					distinctTagNames[tagName] = newTag;
					catTag.Tag = newTag;
				}
			}

			lock (_lock)
			{
				var maxId = _cats.Any() ? _cats.Max(x => x.Id) + 1 : 1;
			
				var propertyInfo = typeof(Cat).GetProperty(nameof(Cat.Id));
				propertyInfo?.SetValue(cat, maxId);
				
				_cats.Add(cat);
			}
		}

		return true;
	}

	public Task<Cat?> GetCatByIdAsync(int id, CancellationToken cancellationToken = default)
	{
		var cat = _cats.FirstOrDefault(x=>x.Id == id);
		return Task.FromResult(cat);
	}

	public Task<Cat?> GetCatByCatIdAsync(string catId, CancellationToken cancellationToken = default)
	{
		var cat = _cats.FirstOrDefault(x=>x.CatId == catId);
		return Task.FromResult(cat);
	}

	public Task<IEnumerable<Cat>> GetAllAsync(GetAllCatsOptions options, CancellationToken cancellationToken = default)
	{
		var query = _cats.Where(c => options.Tags == null || c.CatTags.Any(ct => options.Tags.Contains(ct.Tag!.Name))).AsQueryable();

		if (!string.IsNullOrEmpty(options.SortField))
		{
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
		
		return Task.FromResult<IEnumerable<Cat>>(query.ToList());
	}

	public Task<bool> ExistsAsync(string catId, CancellationToken cancellationToken = default)
	{
		return Task.FromResult(_cats.Any(c => c.CatId == catId));
	}

	public Task<int> GetCountAsync(string? tag, CancellationToken cancellationToken = default)
	{
		if (tag?.Trim() == string.Empty)
		{
			tag = null;
		}
		
		return Task.FromResult(_cats.Count(x => tag == null || x.CatTags.Any(ct => ct.Tag!.Name == tag)));
	}

	private static async Task InitialiseCats()
	{
		var jsonContent = JsonSerializer.Deserialize<IEnumerable<CaaSResponse.Cat>>(await File.ReadAllTextAsync("integrationdata/api.json"));

		var count = 0;
		await Task.WhenAll(jsonContent!.Select(cat =>
		{
			
			foreach (var breed in cat.Breeds)
			{
				var catModel = new Cat
				{
					Id = ++count,
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

				lock (_lock)
				{
					_cats.Add(catModel);
				}
			}

			return Task.CompletedTask;
		}));
	}
}