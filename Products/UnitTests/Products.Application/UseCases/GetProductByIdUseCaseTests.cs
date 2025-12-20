using FluentAssertions;
using MongoDB.Bson;
using Moq;
using Products.Application.UseCases;
using Products.Domain.Entities;
using Products.Infra.DataBase.Repositories.Interfaces;
using System.Net;

namespace UnitTests.Products.Application.UseCases;

public class GetProductByIdUseCaseTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly GetProductByIdUseCase _useCase;

    public GetProductByIdUseCaseTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _useCase = new GetProductByIdUseCase(_productRepositoryMock.Object);
    }
    [Fact]
    public async Task ExecuteAsync_WhenProductExist_ShouldReturnOk()
    {
        //Arrange
        var id = ObjectId.GenerateNewId();

        List<ImageProduct> imagesProduct = new List<ImageProduct>();

        var productEntity = new Drink(ObjectId.GenerateNewId(), "Coca-Cola",12.50m, true, "M", DateTime.Now, DateTime.Now, null, imagesProduct);
        _productRepositoryMock.Setup(x => x.GetProductByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(productEntity);

        //Act
        var result = await _useCase.ExecuteAsync("123", CancellationToken.None);

        //Assert
        result.Data.Should().NotBeNull();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ExecuteAsync_WhenProductNotExist_ShouldReturnFail()
    {
        //Arrange
        _productRepositoryMock.Setup(x => x.GetProductByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync((Product?)null);

        //Act
        var result = await _useCase.ExecuteAsync("123", CancellationToken.None);

        //Assert
        result.Data.Should().BeNull();
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
