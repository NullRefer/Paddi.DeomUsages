using Paddi.DemoUsages.ApiDemo.Dtos.Dict;

namespace Paddi.DemoUsages.ApiDemo.IntegrationTests.Controllers;

public class DictControllerTest : IClassFixture<ApiDemoWebApplicationFactory>
{
    private readonly ApiDemoWebApplicationFactory _factory;

    public DictControllerTest(ApiDemoWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreateAsync_ReturnOk_WhenNotExists()
    {
        // Arrange
        var client = _factory.CreateClient();
        var url = "dicts";

        // act
        var response = await client.PostAsJsonAsync(url, new DictDto
        {
            Key = "testKey",
            Value = "testValue"
        });

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadFromJsonAsync<ApiResultDto<Dict>>();
        content.Should().NotBeNull();
        content.Code.Should().Be(2000);
        content.Data.Key.Should().Be("testKey");
        content.Data.Value.Should().Be("testValue");
    }
}
