using System.Linq.Expressions;

using AutoMapper;

using Bogus;

using FluentAssertions;

using Microsoft.Extensions.Logging;

using MockQueryable.Moq;

using Moq;

using Paddi.DemoUsages.ApiDemo.Dtos;
using Paddi.DemoUsages.ApiDemo.Repository;

namespace Paddi.DemoUsages.ApiDemo.Services.UnitTests;

public class DictRepoServiceTests
{
    private readonly Mock<IRepository<Dict>> _repoMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ILogger<DictRepoService>> _loggerMock = new();

    private DictRepoService GetSut() => new(_repoMock.Object, _mapperMock.Object, _loggerMock.Object);

    [Fact]
    public async Task CreateAsync_ReturnNg_WhenKeyAlreadyExists()
    {
        // Arrange
        var testSet = Faker.Generate(10);
        var input = new DictDto
        {
            Key = testSet[0].Key,
            Value = testSet[0].Value
        };
        Expression<Func<IRepository<Dict>, IQueryable<Dict>>> expression = repo => repo.Where(e => e.Key == input.Key, It.IsAny<bool>());
        _repoMock.Setup(expression).Returns(testSet.BuildMock());

        // Act
        var sut = GetSut();
        var result = await sut.CreateAsync(input);

        // Assert
        result.Success.Should().BeFalse();
        result.Msg.Should().Be($"{input.Key} already exists");
        _repoMock.Verify(expression, Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ReturnOk_WhenKeyNotExists()
    {
        // Arrange
        var testSet = Faker.Generate(3);
        var input = new DictDto
        {
            Key = string.Join(',', testSet.Select(e => e.Key)),
            Value = "Any"
        };
        Expression<Func<IRepository<Dict>, IQueryable<Dict>>> anyExpr = repo => repo.Where(e => e.Key == input.Key, It.IsAny<bool>());
        _repoMock.Setup(anyExpr).Returns(testSet.Where(t => t.Key == input.Key).BuildMock());

        Expression<Func<IRepository<Dict>, Task<int>>> createExpr = repo => repo.CreateAsync(It.IsAny<Dict>(), It.IsAny<CancellationToken>());
        _repoMock.Setup(createExpr).ReturnsAsync(1);

        // Act
        var sut = GetSut();
        var result = await sut.CreateAsync(input);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Key.Should().Be(input.Key);
        result.Data!.Value.Should().Be(input.Value);
        _repoMock.Verify(anyExpr, Times.Once());
        _repoMock.Verify(createExpr, Times.Once());
    }

    [Fact]
    public async Task DeleteAsync_ReturnNg_WhenIdNotExists()
    {
        // Arrange
        var testSet = Faker.Generate(3);
        var id = testSet.Max(t => t.Id) + 1;
        _repoMock.Setup(e => e.GetAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((Dict?)null);

        // Act
        var sut = GetSut();
        var result = await sut.DeleteAsync(id);

        // Assert
        result.Success.Should().BeFalse();
        result.Msg.Should().Be($"Not found with id - {id}");
        _repoMock.Verify(e => e.GetAsync(id, It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task DeleteAsync_ReturnOk_WhenIdExists()
    {
        // Arrange
        var testSet = Faker.Generate(3);
        var id = testSet[0].Id;
        _repoMock.Setup(e => e.GetAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(testSet[0]);
        _repoMock.Setup(e => e.DeleteAsync(testSet[0], It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var sut = GetSut();
        var result = await sut.DeleteAsync(id);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().Be(1);
        _repoMock.Verify(e => e.GetAsync(id, It.IsAny<CancellationToken>()), Times.Once());
        _repoMock.Verify(e => e.DeleteAsync(testSet[0], It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task GetAsync_ReturnEntity_WhenIdExists()
    {
        // Arrange
        var testSet = Faker.Generate(3);
        var id = testSet[0].Id;
        _repoMock.Setup(e => e.GetAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(testSet[0]);

        // Act
        var sut = GetSut();
        var result = await sut.GetAsync(id);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().Be(testSet[0]);
        _repoMock.Verify(e => e.GetAsync(id, It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task GetAsync_ReturnNull_WhenIdNotExists()
    {
        // Arrange
        var testSet = Faker.Generate(3);
        var id = testSet.Max(e => e.Id) + 1;
        _repoMock.Setup(e => e.GetAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((Dict?)null);

        // Act
        var sut = GetSut();
        var result = await sut.GetAsync(id);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Data.Should().BeNull();
        _repoMock.Verify(e => e.GetAsync(id, It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task UpdateAsync_ReturnFalse_WhenIdNotExists()
    {
        // Arrange
        var testSet = Faker.Generate(3);
        var id = testSet.Max(e => e.Id) + 1;
        _repoMock.Setup(e => e.GetAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((Dict?)null);

        // Act
        var sut = GetSut();
        var result = await sut.UpdateAsync(id, new());

        // Assert
        result.Success.Should().BeFalse();
        result.Msg.Should().Be($"Not found with id - {id}");
        _repoMock.Verify(e => e.GetAsync(id, It.IsAny<CancellationToken>()), Times.Once());
    }

    [Theory]
    [InlineData("Key1", "Value1")]
    [InlineData("Key2", "Value2")]
    public async Task UpdateAsync_ReturnEntity_WhenIdExists(string key, string value)
    {
        // Arrange
        var testSet = Faker.Generate(3);
        var entity = testSet[0];
        var id = testSet[0].Id;
        var input = new DictDto
        {
            Key = key,
            Value = value
        };
        _repoMock.Setup(e => e.GetAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
        _repoMock.Setup(e => e.UpdateAsync(entity, It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var sut = GetSut();
        var result = await sut.UpdateAsync(id, input);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Key.Should().Be(key);
        result.Data!.Value.Should().Be(value);
        _repoMock.Verify(e => e.GetAsync(id, It.IsAny<CancellationToken>()), Times.Once());
        _repoMock.Verify(e => e.UpdateAsync(entity, It.IsAny<CancellationToken>()), Times.Once());
    }

    private static Faker<Dict> Faker => new Faker<Dict>()
        .RuleFor(e => e.Id, f => f.IndexGlobal)
        .RuleFor(e => e.Key, f => f.Random.String2(4))
        .RuleFor(e => e.Value, f => f.Random.String2(8))
        .RuleFor(e => e.IsDeleted, f => true);
}
