namespace Paddi.DemoUsages.ApiDemo.IntegrationTests.Controllers;

public class SysControllerTests : IClassFixture<ApiDemoWebApplicationFactory<Program>>
{
    private readonly ApiDemoWebApplicationFactory<Program> _factory;

    public SysControllerTests(ApiDemoWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetRedisOption_ReturnRedisOption()
    {
        // Arrange
        var client = _factory.CreateClient();
        var url = $"/sys/redis-option";

        // Act
        var result = await client.GetFromJsonAsync<ApiResultDto<RedisOption>>(url);

        // Assert
        result.Should().NotBeNull();
    }
}
