using FluentAssertions;
using MongoDB.Bson;
using Moq;
using Products.Application.UseCases;
using Products.Domain.Entities;
using Products.Infra.DataBase.Repositories.Interfaces;
using System.Net;

namespace Products.UnitTests.Products.Application.UseCases;

public class GetProductsUseCaseTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly GetProductsUseCase _useCase;

    public GetProductsUseCaseTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _useCase = new GetProductsUseCase(_productRepositoryMock.Object);
    }
    [Fact]
    public async Task ExecuteAsync_WhenProductsExist_ShouldReturnOk()
    {
        //Arrange
        List<ImageProduct> imagesProduct = new List<ImageProduct>();

        var drinkEntity = new Drink(ObjectId.GenerateNewId(), "Coca-Cola", 12.50m, true, "M", DateTime.Now, DateTime.Now, null, imagesProduct);
        var snackEntity = new Snack(ObjectId.GenerateNewId(), "X-Salada", 12.50m, true, DateTime.Now, DateTime.Now, null, imagesProduct);
        var accompanimentEntity = new Accompaniment(ObjectId.GenerateNewId(), "Batata-Frita", 12.50m, true, DateTime.Now, DateTime.Now, "M", imagesProduct);
        var dessertEntity = new Dessert(ObjectId.GenerateNewId(), "Sorvete", 12.50m, true, "M", DateTime.Now, DateTime.Now, imagesProduct);
        var products = new List<Product>() { drinkEntity, snackEntity, accompanimentEntity, dessertEntity };

        _productRepositoryMock.Setup(x => x.GetProductsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(products);

        //Act
        var result = await _useCase.ExecuteAsync(CancellationToken.None);

        //Assert
        result.Data.Should().NotBeNull();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Data.Count.Should().Be(4);
    }

    [Fact]
    public async Task ExecuteAsync_WhenInternalError_ShouldReturnFail()
    {
        //Arrange
        _productRepositoryMock.Setup(x => x.GetProductsAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Internal error")); ;

        //Act
        var result = await _useCase.ExecuteAsync(CancellationToken.None);

        //Assert
        result.Data.Should().BeNull();
        result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
}
