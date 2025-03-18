using DvlDev.SATC.Application.Repositories;
using DvlDev.SATC.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DvlDev.SATC.Application;

public static class ApplicationServiceCollectionExtensions
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddSingleton<ICatRepository, CatRepository>();
		services.AddSingleton<ICatService, CatService>();
		
		
		// Validators
		services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Singleton);
		
		return services;
	}
}