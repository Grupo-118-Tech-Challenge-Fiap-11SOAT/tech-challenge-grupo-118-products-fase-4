using FluentAssertions;
using MongoDB.Bson;
using Moq;
using Products.Application.UseCases;
using Products.Domain.Common.Exceptions;
using Products.Domain.Entities;
using Products.Infra.DataBase.Repositories.Interfaces;
using System.Net;

namespace Products.UnitTests.Products.Application.UseCases;

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
    public async Task ExecuteAsync_WhenProductDrinkExist_ShouldReturnOk()
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
    public async Task ExecuteAsync_WhenProductSnackExist_ShouldReturnOk()
    {
        //Arrange
        var id = ObjectId.GenerateNewId();

        List<ImageProduct> imagesProduct = new List<ImageProduct>();

        var productEntity = new Snack(ObjectId.GenerateNewId(), "X-Salada", 12.50m, true, DateTime.Now, DateTime.Now, null, imagesProduct);
        _productRepositoryMock.Setup(x => x.GetProductByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(productEntity);

        //Act
        var result = await _useCase.ExecuteAsync("123", CancellationToken.None);

        //Assert
        result.Data.Should().NotBeNull();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ExecuteAsync_WhenProductDessertExist_ShouldReturnOk()
    {
        //Arrange
        var id = ObjectId.GenerateNewId();

        List<ImageProduct> imagesProduct = new List<ImageProduct>();

        var productEntity = new Dessert(ObjectId.GenerateNewId(), "Sorvete", 12.50m, true, "M", DateTime.Now, DateTime.Now, imagesProduct);
        _productRepositoryMock.Setup(x => x.GetProductByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(productEntity);

        //Act
        var result = await _useCase.ExecuteAsync("123", CancellationToken.None);

        //Assert
        result.Data.Should().NotBeNull();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ExecuteAsync_WhenProductAccompanimentExist_ShouldReturnOk()
    {
        //Arrange
        var id = ObjectId.GenerateNewId();

        List<ImageProduct> imagesProduct = new List<ImageProduct>();

        var productEntity = new Accompaniment(ObjectId.GenerateNewId(), "Batata-Frita", 12.50m, true, DateTime.Now, DateTime.Now, "M");
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

    [Fact]
    public async Task ExecuteAsync_WhenInvalidObjectId_ShouldReturnBadRequest()
    {
        // Arrange
        _productRepositoryMock
            .Setup(x => x.GetProductByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidObjectIdException("Invalid ObjectId: Invalid id"));

        // Act
        var result = await _useCase.ExecuteAsync("invalid-id", CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Message.Should().Contain("Invalid id");
        result.Data.Should().BeNull();
    }

    [Fact]
    public async Task ExecuteAsync_WhenUnexpectedError_ShouldReturnInternalServerError()
    {
        // Arrange
        _productRepositoryMock
            .Setup(x => x.GetProductByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("boom"));

        // Act
        var result = await _useCase.ExecuteAsync("123", CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        result.Message.Should().Be("Internal Error");
        result.Data.Should().BeNull();
    }


}
