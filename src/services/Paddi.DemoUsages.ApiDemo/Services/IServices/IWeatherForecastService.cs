using Paddi.DemoUsages.ApiDemo.Dtos;

namespace Paddi.DemoUsages.ApiDemo.Services.IServices;

public interface IWeatherForecastService : IAppService
{
    Task<IEnumerable<WeatherForecast>> PageAsync(WeatherForecastSearchDto search, CancellationToken token = default);
    Task<IEnumerable<WeatherForecast>> GetAllAsync(CancellationToken token = default);
    Task<WeatherForecast?> GetWeatherForecastAsync(long id, CancellationToken token = default);
    Task<bool> IsTodaySunnyAsync(CancellationToken token = default);
}
