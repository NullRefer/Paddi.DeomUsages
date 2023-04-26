using Refit;

namespace Paddi.DemoUsages.ConsoleDemo.RunnableServices.RefitRunner;

[Runner("Refit service", "Use refit to simplify Http request", "https://github.com/reactiveui/refit")]
public class RefitRunner : RunnerBase
{
    private const string AmapApiKey = "a7707676285cf6287b3f8849ffc5caff";

    protected override async Task RunInternalAsync(CancellationToken token = default)
    {
        var weather = await GetWeatherAsync();
        if (DateTime.Now < new DateTime(2023, 5, 1))
        {
            await RunSctAsync(weather);
        }
    }

    private async Task<AmapWeatherResultDto> GetWeatherAsync()
    {
        var amapClient = RestService.For<IAmapApi>("https://restapi.amap.com");
        var weather = await amapClient.GetWeatherAsync(AmapApiKey, CityCodes.Tianhe);
        Log($"Amap weather for Guangzhou: {JsonConvert.SerializeObject(weather)}");
        return weather;
    }

    private async Task RunSctAsync(AmapWeatherResultDto weather)
    {
        var refitClient = RestService.For<ISctRefitApi>("https://sctapi.ftqq.com");
        var user = await refitClient.NotifyAsync("Refit Test", "This is a message from refit client.");
        Log($"Get user: {user}");
    }
}
