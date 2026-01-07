using FluentAssertions;
using MongoDB.Bson;
using Moq;
using Products.Application.Dtos;
using Products.Application.UseCases;
using Products.Domain.Entities;
using Products.Infra.DataBase.Repositories.Interfaces;
using System.Net;

namespace Products.UnitTests.Products.Application.UseCases;

public class GetActiveProductsByIdsUseCaseTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly GetActiveProductsByIdsUseCase _useCase;

    public GetActiveProductsByIdsUseCaseTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _useCase = new GetActiveProductsByIdsUseCase(_productRepositoryMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenProductsFound_ShouldReturnOkWithMappedDtos()
    {
        // Arrange
        var ids = new List<string> { "1", "2" };

        var products = new List<Product>
        {
            new Snack(ObjectId.GenerateNewId(), "X-Salada", 12, true, DateTime.Now, DateTime.Now, new List<string>(){"Pão", "Queijo", "Hamburgues"}),
            new Drink(ObjectId.GenerateNewId(), "Suco de Uva", 12, true, "M", DateTime.Now, DateTime.Now, "Uva")
        };

        _productRepositoryMock
            .Setup(r => r.GetActiveProductsByIdsAsync(ids, It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        // Act
        var result = await _useCase.ExecuteAsync(ids);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Data.Should().HaveCount(2);

        result.Data[0].Should().BeOfType<SnackDto>();
        result.Data[1].Should().BeOfType<DrinkDto>();
    }

    [Fact]
    public async Task ExecuteAsync_WhenNoProductsFound_ShouldReturnOkWithEmptyList()
    {
        // Arrange
        var ids = new List<string> { "1", "2" };

        _productRepositoryMock
            .Setup(r => r.GetActiveProductsByIdsAsync(ids, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Product>());

        // Act
        var result = await _useCase.ExecuteAsync(ids);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ExecuteAsync_WhenRepositoryThrowsException_ShouldReturnInternalServerError()
    {
        // Arrange
        var ids = new List<string> { "1" };

        _productRepositoryMock
            .Setup(r => r.GetActiveProductsByIdsAsync(ids, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB error"));

        // Act
        var result = await _useCase.ExecuteAsync(ids);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        result.Message.Should().Be("Internal Error");
        result.Data.Should().BeNull();
    }
}
