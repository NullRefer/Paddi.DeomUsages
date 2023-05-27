﻿using Bogus;

using Paddi.DemoUsages.ApiDemo.Dtos;
using Paddi.DemoUsages.ApiDemo.Services.IServices;

namespace Paddi.DemoUsages.ApiDemo.Services;

public class WeatherForecastService : IWeatherForecastService
{
    public async Task<IEnumerable<WeatherForecast>> GetAllAsync()
    {
        var all = await LocalDs(10);
        return all;
    }

    public async Task<WeatherForecast?> GetWeatherForecastAsync(long id)
    {
        var all = await LocalDs(1000);
        return all.FirstOrDefault(w => w.Id == id);
    }

    public async Task<bool> IsTodaySunnyAsync()
    {
        var all = await LocalDs();
        return all.Any(w => w.TemperatureC == 24);
    }

    private Task<IEnumerable<WeatherForecast>> LocalDs(int count = 10)
    {
        var summaries = new[]
        {
            "Sunny Day", "Rainy Day", "Cloudy Day"
        };

        var faker = new Faker<WeatherForecast>();
        faker.RuleFor(e => e.Id, (f, p) => f.IndexGlobal)
             .RuleFor(e => e.Date, (f, p) => f.Date.Past(3))
             .RuleFor(e => e.TemperatureC, (f, p) => f.Random.Int(20, 35))
             .RuleFor(e => e.Summary, (f, p) => f.PickRandom(summaries));

        return Task.FromResult(faker.Generate(count).AsEnumerable());
    }
}