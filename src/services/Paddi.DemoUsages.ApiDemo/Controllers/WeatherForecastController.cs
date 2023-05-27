using Microsoft.AspNetCore.Mvc;

using Paddi.DemoUsages.ApiDemo.Dtos;
using Paddi.DemoUsages.ApiDemo.Services.IServices;

namespace Paddi.DemoUsages.ApiDemo.Controllers;

[ApiController]
[Route("weather-foreasts")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IWeatherForecastService _service;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<ResultDto<IEnumerable<WeatherForecast>>>> GetAllAsync()
    {
        var result = Result(await _service.GetAllAsync());
        return result;
    }

    [HttpGet("sunny-today")]
    public async Task<ResultDto<bool>> TodayIsSunnyAsync()
    {
        var result = Result(await _service.IsTodaySunnyAsync());
        return result;
    }

    public ResultDto<T> Result<T>(T result) => new(result);
}
