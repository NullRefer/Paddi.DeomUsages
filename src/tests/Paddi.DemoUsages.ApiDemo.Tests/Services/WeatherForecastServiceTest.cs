using FluentAssertions;

using Paddi.DemoUsages.ApiDemo.Services;

namespace Paddi.DemoUsages.ApiDemo.Tests.Services;

public class WeatherForecastServiceTest
{
    [Fact]
    public async Task GetAllAsync_ReturnAll()
    {
        var sut = GetSut();

        var result = await sut.GetAllAsync();

        result.Should().NotBeEmpty();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task GetWeatherForecastAsync_ReturnGivenId(long id)
    {
        var sut = GetSut();

        var result = await sut.GetWeatherForecastAsync(id);

        result.Should().NotBeNull();
    }

    private static WeatherForecastService GetSut() => new();
}
