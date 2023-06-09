﻿namespace Paddi.DemoUsages.ApiDemo.Services;

public class WeatherForecastService : IWeatherForecastService
{
    public async Task<IEnumerable<WeatherForecast>> GetAllAsync(CancellationToken token = default)
    {
        var all = await LocalDs(10);
        return all;
    }

    public async Task<WeatherForecast?> GetWeatherForecastAsync(long id, CancellationToken token = default)
    {
        var all = await LocalDs(1000);
        return all.FirstOrDefault(w => w.Id == id);
    }

    public async Task<bool> IsTodaySunnyAsync(CancellationToken token = default)
    {
        var all = await LocalDs();
        return all.Any(w => w.TemperatureC == 24);
    }

    public async Task<IEnumerable<WeatherForecast>> PageAsync(WeatherForecastSearchDto search, CancellationToken token = default)
    {
        Expression<Func<WeatherForecast, bool>> filters = x => true;
        filters = filters.AndIf(!string.IsNullOrWhiteSpace(search.Summary), e => e.Summary.Contains(search.Summary!))
                         .AndIf(search.MinTemp.HasValue, e => e.TemperatureC > search.MinTemp!.Value)
                         .AndIf(search.MaxTemp.HasValue, e => e.TemperatureC < search.MaxTemp!.Value);

        var all = await LocalDs(10);
        var result = all.Where(filters.Compile()).ToList();
        return result;
    }

    private static Task<IEnumerable<WeatherForecast>> LocalDs(int count = 10)
    {
        var summaries = new[]
        {
            "Sunny Day", "Rainy Day", "Cloudy Day"
        };

        var faker = new Faker<WeatherForecast>()
            .UseSeed(1000)
            .RuleFor(e => e.Id, f => f.IndexFaker)
            .RuleFor(e => e.Date, f => f.Date.Past(3))
            .RuleFor(e => e.TemperatureC, f => f.Random.Int(0, 45))
            .RuleFor(e => e.Summary, f => f.PickRandom(summaries));

        return Task.FromResult(faker.Generate(count).AsEnumerable());
    }
}
