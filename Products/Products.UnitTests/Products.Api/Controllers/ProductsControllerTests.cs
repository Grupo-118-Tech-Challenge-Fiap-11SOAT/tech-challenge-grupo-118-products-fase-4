using FluentAssertions;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Moq;
using Products.Api.Controllers;
using Products.Application.Common.Models;
using Products.Application.Dtos;
using Products.Application.Enums;
using Products.Application.UseCases;
using Products.Application.UseCases.Interfaces;
using System.Net;

namespace Products.UnitTests.Products.Api.Controllers;

public class ProductsControllerTests
{
    private readonly Mock<ILogger<ProductsController>> _loggerMock;
    private readonly Mock<IGetProductByIdUseCase> _getProductByIdUseCaseMock;
    private readonly Mock<IGetProductByTypeUseCase> _getProductByTypeUseCaseMock;
    private readonly Mock<ICreateProductUseCase> _createProductUseCaseMock;
    private readonly Mock<IGetProductsUseCase> _getProductsUseCaseMock;
    private readonly Mock<IGetActiveProductsByIdsUseCase> _getActiveProductsByIdsUseCaseMock;
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _loggerMock = new Mock<ILogger<ProductsController>>();
        _getProductByIdUseCaseMock = new Mock<IGetProductByIdUseCase>();
        _getProductByTypeUseCaseMock = new Mock<IGetProductByTypeUseCase>();
        _getProductsUseCaseMock = new Mock<IGetProductsUseCase>();
        _createProductUseCaseMock = new Mock<ICreateProductUseCase>();
        _getActiveProductsByIdsUseCaseMock = new Mock<IGetActiveProductsByIdsUseCase>();

        _controller = new ProductsController(
            _loggerMock.Object,
            _getProductByIdUseCaseMock.Object,
            _getProductByTypeUseCaseMock.Object,
            _getProductsUseCaseMock.Object,
            _createProductUseCaseMock.Object,
            _getActiveProductsByIdsUseCaseMock.Object
        );
    }

    [Fact]
    public async Task GetByIdAsync_WhenFound_ShouldReturnProduct()
    {
        // Arrange
        var productId = "123";
        var images = new List<ImageProductDto>
        {
             new ImageProductDto(1, "https://example.com/front.png"),
             new ImageProductDto(2, "https://example.com/back.png")
        };
        var ingredients = new List<string>() { "pão", "hamburguer", "queijo" };
        var productDto = new SnackDto() { Id = ObjectId.GenerateNewId().ToString(), Name = "x-burger", Price = 12, IsActive = true, Images = images, Ingredients = ingredients };

        var expectedResult = new Result<ProductDto?>().Ok(productDto, HttpStatusCode.OK);         

        _getProductByIdUseCaseMock
            .Setup(x => x.ExecuteAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.GetByIdAsync(productId, CancellationToken.None);

        // Assert
        result.Data.Should().Be(productDto);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        _getProductByIdUseCaseMock.Verify(x => x.ExecuteAsync(productId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WhenNotFound_ShouldReturnFail()
    {
        // Arrange
        var productId = "123";
        var expectedResult = new Result<ProductDto?>().Fail("Product not found", HttpStatusCode.NotFound);

        _getProductByIdUseCaseMock
            .Setup(x => x.ExecuteAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.GetByIdAsync(productId, CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.Message.Should().Be("Product not found");
        result.Data.Should().BeNull();
    }

    [Fact]
    public async Task GetAsync_WhenThereAreProducts_ShouldReturnAllProducts()
    {
        // Arrange
        var productSnack = BuildProductDto("Hamburguer", ProductType.Snack);
        var productDrink = BuildProductDto("Suco", ProductType.Drink);
        var productAccompaniment = BuildProductDto("Batata-Frita", ProductType.Accompaniment);

        var products = new List<ProductDto>() { productSnack, productDrink, productAccompaniment };

        var expectedResult = new Result<List<ProductDto>>().Ok(products, HttpStatusCode.OK);

        _getProductsUseCaseMock
            .Setup(x => x.ExecuteAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.GetAsync(CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Data.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetAsync_WhenInternalError_ShouldReturnFail()
    {
        // Arrange

        var expectedResult = new Result<List<ProductDto>>().Fail("Internal Error", HttpStatusCode.InternalServerError);

        _getProductsUseCaseMock
            .Setup(x => x.ExecuteAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.GetAsync(CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        result.Message.Should().Be("Internal Error");
        result.Data.Should().BeNull();
    }

    [Fact]
    public async Task GetByType_WhenFound_ShouldReturnProducts()
    {
        // Arrange
        var productFirst = BuildProductDto("Hamburguer", ProductType.Snack);
        var productSecond = BuildProductDto("X-Salada", ProductType.Snack);
        var productThird = BuildProductDto("X-Bacon", ProductType.Snack);

        var products = new List<ProductDto>() { productFirst, productSecond, productThird };

        var expectedResult = new Result<List<ProductDto>>().Ok(products, HttpStatusCode.OK);

        _getProductByTypeUseCaseMock
            .Setup(x => x.ExecuteAsync(It.IsAny <string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.GetByTypeAsync("snack",CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Data.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetByType_WhenNotFound_ShouldReturnFail()
    {
        var expectedResult = new Result<List<ProductDto>>().Fail("Product not found", HttpStatusCode.NotFound);

        _getProductByTypeUseCaseMock
            .Setup(x => x.ExecuteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.GetByTypeAsync("snack", CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.Data.Should().BeNull();
    }

    [Fact]
    public async Task PostAsync_WhenInsert_ShouldReturnProduct()
    {
        var images = new List<ImageProductDto>
        {
             new ImageProductDto(1, "https://example.com/front.png"),
             new ImageProductDto(2, "https://example.com/back.png")
        };
        var ingredients = new List<string>() { "pão", "hamburguer", "queijo" };
        var productDto = new SnackDto() { Name = "x-burger", Price = 12, IsActive = true, Images = images, Ingredients = ingredients };

        var expectedResult = new Result<ProductDto>().Ok(productDto, HttpStatusCode.OK);

        _createProductUseCaseMock
            .Setup(x => x.ExecuteAsync(It.IsAny<ProductDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.PostAsync(CancellationToken.None, productDto);

        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Data.Should().NotBeNull();
    }

    [Fact]
    public async Task GetByIdsAsync_WhenFoundProducts_ShouldReturnOk()
    {
        // Arrange
        var productsIds = new List<string> { "123", "213" };

        var products = new List<ProductDto>
    {
        new SnackDto { Id = "123", Name = "Burger" },
        new DrinkDto { Id = "213", Name = "Coke" }
    };

        var expectedResult =new Result<List<ProductDto>>().Ok(products, HttpStatusCode.OK);

        _getActiveProductsByIdsUseCaseMock
            .Setup(x => x.ExecuteAsync(productsIds, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.GetActiveByIdsAsync(productsIds, CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Data.Should().NotBeNull();
        result.Data.Should().HaveCount(2);
    }

    private ProductDto BuildProductDto(string name, ProductType type)
    {
        Random random = new Random();

        var productId = random.Next(1, 101);
        var images = new List<ImageProductDto>
        {
             new ImageProductDto(1, "https://example.com/front.png"),
             new ImageProductDto(2, "https://example.com/back.png")
        };
        var ingredients = new List<string>() { "ingrediente 1", "ingrediente 2", "ingrediente 3" };

        ProductDto dto = type switch
        {
            ProductType.Snack => new SnackDto() { Id = ObjectId.GenerateNewId().ToString(), Name = name, Price = 12, IsActive = true, Images = images, Ingredients = ingredients },
            ProductType.Drink => new DrinkDto() { Id = ObjectId.GenerateNewId().ToString(), Name = name, Price = 12, IsActive = true, Images = images, Size = "M", Flavor = "sabor 1" },
            ProductType.Accompaniment => new AccompanimentDto() { Id = ObjectId.GenerateNewId().ToString(), Name = name, Price = 12, IsActive = true, Images = images, Size = "M" }
        };

        return dto;
    }
}
