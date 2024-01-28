namespace Paddi.DemoUsages.ApiDemo.IntegrationTests.Controllers;

//public class SysControllerTests : IClassFixture<ApiDemoWebApplicationFactory>
//{
//    private readonly ApiDemoWebApplicationFactory _factory;

//    public SysControllerTests(ApiDemoWebApplicationFactory factory)
//    {
//        _factory = factory;
//    }

//    [Fact]
//    public async Task GetRedisOption_ReturnRedisOption()
//    {
//        // Arrange
//        var client = _factory.CreateClient();
//        var url = $"/sys/redis-option";

//        // Act
//        var result = await client.GetFromJsonAsync<ApiResultDto<RedisOption>>(url);

//        // Assert
//        result.Should().NotBeNull();
//    }
//}
