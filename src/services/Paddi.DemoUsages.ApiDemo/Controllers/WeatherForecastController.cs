namespace Paddi.DemoUsages.ApiDemo.Controllers;

[ApiController]
// [EnableRateLimiting("FixedWindow")]
[Route("weather-forecasts")]
public class WeatherForecastController : ControllerBase
{
    private readonly IWeatherForecastService _service;

    public WeatherForecastController(IWeatherForecastService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<ResultDto<IEnumerable<WeatherForecast>>>> GetAllAsync(CancellationToken token = default)
    {
        var result = this.Result(await _service.GetAllAsync(token));
        return result;
    }

    [HttpGet("{id:long}")]
    [ApiLogFilter]
    public async Task<ActionResult<ResultDto<WeatherForecast?>>> GetByIdAsync(long id, CancellationToken token = default)
    {
        var result = this.Result(await _service.GetWeatherForecastAsync(id, token));
        return result;
    }

    [HttpPost("page"), ApiLogFilter]
    public async Task<ActionResult<ResultDto<IEnumerable<WeatherForecast>>>> PageAsync([FromBody] WeatherForecastSearchDto search, CancellationToken token = default)
    {
        var result = this.Result(await _service.PageAsync(search, token));
        return result;
    }

    [HttpGet("sunny-today")]
    public async Task<ResultDto<bool>> TodayIsSunnyAsync(CancellationToken token = default)
    {
        var result = this.Result(await _service.IsTodaySunnyAsync(token));
        return result;
    }
}
