﻿using Moq;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using FluentAssertions;
using System.Linq.Expressions;
using Paddi.DemoUsages.ApiDemo.Dtos.Dict;
using Paddi.DemoUsages.ApiDemo.Repository;
using Bogus;

namespace Paddi.DemoUsages.ApiDemo.Services.Tests
{
    public class DictRepoServiceTests
    {
        private readonly Mock<IRepository<Dict>> _repoMock;
        private readonly Mock<ILogger<DictRepoService>> _loggerMock;

        public DictRepoServiceTests()
        {
            _repoMock = new Mock<IRepository<Dict>>();
            _loggerMock = new Mock<ILogger<DictRepoService>>();
        }

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


        private Faker<Dict> Faker => new Faker<Dict>()
            .RuleFor(e => e.Id, f => f.IndexGlobal)
            .RuleFor(e => e.Key, f => f.Random.String2(4))
            .RuleFor(e => e.Value, f => f.Random.String2(8))
            .RuleFor(e => e.IsDeleted, f => true);

        private DictRepoService GetSut() => new DictRepoService(_repoMock.Object, _loggerMock.Object);
    }
}
