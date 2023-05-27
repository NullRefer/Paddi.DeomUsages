using Paddi.DemoUsages.ApiDemo.Services;
using Paddi.DemoUsages.ApiDemo.Services.IServices;

namespace Paddi.DemoUsages.ApiDemo.Extensions;

public static class ServiceRegistrationExtension
{
    public static IServiceCollection AddPaddiServices(this IServiceCollection services)
    {
        services.AddScoped<IWeatherForecastService, WeatherForecastService>();
        return services;
    }
}
