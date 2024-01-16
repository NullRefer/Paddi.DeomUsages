using Moq;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using FluentAssertions;
using System.Linq.Expressions;
using Paddi.DemoUsages.ApiDemo.Dtos.Dict;
using Paddi.DemoUsages.ApiDemo.Repository;

namespace Paddi.DemoUsages.ApiDemo.Services.Tests
{
    public class DictRepoServiceTests
    {
        private readonly Mock<IRepository<Dict>> _repoMock;
        private readonly Mock<ILogger<DictRepoService>> _logger;

        public DictRepoServiceTests()
        {
            _repoMock = new Mock<IRepository<Dict>>();
            _logger = new Mock<ILogger<DictRepoService>>();
        }

        [Fact]
        public async Task CreateAsync_ThrowArgumentException_WhenInputKeyExists()
        {
            // Arrange
            var testSet = TestSet;
            var input = new DictDto
            {
                Key = testSet[0].Key
            };

            Expression<Func<IRepository<Dict>, IQueryable<Dict>>> expression = repo => repo.Where(e => e.Key == input.Key);
            _repoMock.Setup(expression).Returns(testSet.Where(e => e.Key == input.Key).BuildMock());

            // Act
            var sut = GetSut();

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(() => sut.CreateAsync(input));
        }

        // [Fact()]
        // public async Task GetAsync_ReturnNull_IdNotExists()
        // {
        //     // Arrange
        //     long id = 2;
        //     var testDataset = TestSet;
        //     _repoMock.Setup(m => m.Where(e => e.Key.Contains(id.ToString()) && !e.IsDeleted)).Returns(testDataset.BuildMock());

        //     // Act
        //     var sut = GetSut();
        //     var result = await sut.GetAsync(1);

        //     // Assert
        //     result.Should().NotBeNull();
        //     _output.WriteLine(result!.ToString());
        // }

        [Fact]
        public async Task GetCategoryAsync_ReturnNull_WhenNoMatch()
        {
            // Arrange
            var testDataset = TestSet;
            _repoMock.Setup(m => m.Where(e => e.Key == "Category" && !e.IsDeleted)).Returns(testDataset.BuildMock());

            // Act
            var sut = GetSut();
            var result = await sut.GetCategoryAsync("test1");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetCategoryAsync_ReturnNotNull_WhenMatch()
        {
            // Arrange
            var testDataset = TestSet;
            Expression<Func<IRepository<Dict>, IQueryable<Dict>>> expression = repo => repo.Where(e => e.Key == "Category" && !e.IsDeleted);
            _repoMock.Setup(expression).Returns(testDataset.BuildMock());

            // Act
            var sut = GetSut();
            var result = await sut.GetCategoryAsync("test");

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(1);
            result!.Key.Should().Be("11test33");
            _repoMock.Verify(expression, Times.Once());

        }

        private static List<Dict> TestSet => new()
        {
            new()
            {
                Id = 1,
                Key = "11test33",
                Value = "test",
                IsDeleted = false
            },
            new()
            {
                Id = 2,
                Key = "te2st",
                Value = "te2st",
                IsDeleted = false
            },
            new()
            {
                Id = 3,
                Key = "test",
                Value = "te2st",
                IsDeleted = true
            }
        };

        private DictRepoService GetSut() => new DictRepoService(_repoMock.Object, _logger.Object);
    }
}
