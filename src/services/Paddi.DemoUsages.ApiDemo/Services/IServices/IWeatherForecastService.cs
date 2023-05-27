using Paddi.DemoUsages.ApiDemo.Dtos;

namespace Paddi.DemoUsages.ApiDemo.Services.IServices;

public interface IWeatherForecastService
{
    Task<IEnumerable<WeatherForecast>> GetAllAsync();
    Task<WeatherForecast?> GetWeatherForecastAsync(long id);
    Task<bool> IsTodaySunnyAsync();
}
