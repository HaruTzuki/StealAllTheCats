using System.Net.Http.Headers;

namespace DvlDev.SATC.API.Helpers.Extensions;

public static class HttpClientServiceCollectionExtensions
{
    public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient("CatSaS", c =>
        {
            c.BaseAddress = new Uri("https://api.thecatapi.com/");
            c.DefaultRequestHeaders.Add("Accept", "*/*");
            c.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
            {
                NoCache = true
            };
            c.DefaultRequestHeaders.Add("x-api-key", configuration["CatSaS:ApiKey"]);
        });
        
        return services;
    }
}