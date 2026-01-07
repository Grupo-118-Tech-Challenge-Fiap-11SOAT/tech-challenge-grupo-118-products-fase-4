using FluentAssertions;
using MongoDB.Bson;
using Moq;
using Products.Application.Dtos;
using Products.Application.UseCases;
using Products.Domain.Entities;
using Products.Infra.DataBase.Repositories.Interfaces;
using System.Net;

namespace Products.UnitTests.Products.Application.UseCases;

public class CreateProductUseCaseTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly CreateProductUseCase _useCase;

    public CreateProductUseCaseTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _useCase = new CreateProductUseCase(_productRepositoryMock.Object);
    }
    [Fact]
    public async Task ExecuteAsync_WhenProductDrinkInsert_ShouldReturnOk()
    {
        //Arrange
        List<ImageProductDto> imagesProductDto = new List<ImageProductDto>();
        List<ImageProduct> imagesProduct = new List<ImageProduct>();

        var newProduct = new DrinkDto() { Name = "Coca-Cola", Price = 12.50m, IsActive = true, Images = imagesProductDto, Size = "M" };
        var drinkEntity = new Drink(ObjectId.GenerateNewId(), "Coca-Cola", 12.50m, true, "M", DateTime.Now, DateTime.Now, null, imagesProduct);

        _productRepositoryMock.Setup(x => x.CreateProductAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>())).ReturnsAsync(drinkEntity);

        //Act
        var result = await _useCase.ExecuteAsync(newProduct, CancellationToken.None);

        //Assert
        result.Data.Should().NotBeNull();
        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task ExecuteAsync_WhenProductSnackInsert_ShouldReturnOk()
    {
        //Arrange
        List<ImageProductDto> imagesProductDto = new List<ImageProductDto>();
        List<ImageProduct> imagesProduct = new List<ImageProduct>();

        var newProduct = new SnackDto() { Name = "X-Salada", Price = 12.50m, IsActive = true, Images = imagesProductDto };
        var drinkEntity = new Snack(ObjectId.GenerateNewId(), "X-Salada", 12.50m, true, DateTime.Now, DateTime.Now, null, imagesProduct);

        _productRepositoryMock.Setup(x => x.CreateProductAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>())).ReturnsAsync(drinkEntity);

        //Act
        var result = await _useCase.ExecuteAsync(newProduct, CancellationToken.None);

        //Assert
        result.Data.Should().NotBeNull();
        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task ExecuteAsync_WhenProductDessertInsert_ShouldReturnOk()
    {
        //Arrange
        List<ImageProductDto> imagesProductDto = new List<ImageProductDto>();
        List<ImageProduct> imagesProduct = new List<ImageProduct>();

        var newProduct = new DessertDto() { Name = "Sorvete", Price = 12.50m, IsActive = true, Images = imagesProductDto };
        var drinkEntity = new Dessert(ObjectId.GenerateNewId(), "Sorvete", 12.50m, true, "M", DateTime.Now, DateTime.Now, imagesProduct);

        _productRepositoryMock.Setup(x => x.CreateProductAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>())).ReturnsAsync(drinkEntity);

        //Act
        var result = await _useCase.ExecuteAsync(newProduct, CancellationToken.None);

        //Assert
        result.Data.Should().NotBeNull();
        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task ExecuteAsync_WhenProductAccompanimentInsert_ShouldReturnOk()
    {
        //Arrange
        List<ImageProductDto> imagesProductDto = new List<ImageProductDto>();
        List<ImageProduct> imagesProduct = new List<ImageProduct>();

        var newProduct = new AccompanimentDto() { Name = "Batata-Frita", Price = 12.50m, IsActive = true, Images = imagesProductDto };
        var drinkEntity = new Accompaniment(ObjectId.GenerateNewId(), "Batata-Frita", 12.50m, true, DateTime.Now, DateTime.Now, "M");

        _productRepositoryMock.Setup(x => x.CreateProductAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>())).ReturnsAsync(drinkEntity);

        //Act
        var result = await _useCase.ExecuteAsync(newProduct, CancellationToken.None);

        //Assert
        result.Data.Should().NotBeNull();
        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task ExecuteAsync_WhenInternalError_ShouldReturnFail()
    {
        //Arrange
        List<ImageProductDto> imagesProduct = new List<ImageProductDto>();

        var newProduct = new DrinkDto() { Name = "Coca-Cola", Price = 12.50m, IsActive = true, Images = imagesProduct, Size = "M" };

        _productRepositoryMock.Setup(x => x.CreateProductAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Internal error"));

        //Act
        var result = await _useCase.ExecuteAsync(newProduct, CancellationToken.None);

        //Assert
        result.Data.Should().BeNull();
        result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
}
