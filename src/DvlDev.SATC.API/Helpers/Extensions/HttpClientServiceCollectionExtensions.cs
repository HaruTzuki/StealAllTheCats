namespace DvlDev.SATC.API.Helpers.Extensions;

public static class HttpClientServiceCollectionExtensions
{
    public static IServiceCollection AddHttpClients(this IServiceCollection services)
    {
        services.AddHttpClient("CatSaS", c =>
        {
            c.BaseAddress = new Uri("http://localhost:5000");
        });
        
        return services;
    }
}