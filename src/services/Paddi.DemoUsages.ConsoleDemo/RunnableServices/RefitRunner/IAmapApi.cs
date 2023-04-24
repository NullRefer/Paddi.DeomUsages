using Refit;

namespace Paddi.DemoUsages.ConsoleDemo.RunnableServices.RefitRunner;

/// <summary>
/// Amap api
/// </summary>
public interface IAmapApi
{
    [Get("/v3/weather/weatherInfo")]
    Task<AmapWeatherResultDto> GetWeatherAsync(string key, int city);
}

internal static class CityCodes
{
    internal const int Guangzhou = 440100;
    internal const int Liwan = 440103;
    internal const int Haizhu = 440105;
    internal const int Tianhe = 440106;
    internal const int Baiyun = 440111;
    internal const int Huangpu = 440112;
}
