using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        var productId = "123";

        var images = new List<ImageProductDto>
    {
        new(1, "https://example.com/front.png"),
        new(2, "https://example.com/back.png")
    };

        var ingredients = new List<string> { "pão", "hamburguer", "queijo" };

        var productDto = new SnackDto
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Name = "x-burger",
            Price = 12,
            IsActive = true,
            Images = images,
            Ingredients = ingredients
        };

        var expectedResult = new Result<ProductDto?>().Ok(productDto, HttpStatusCode.OK);

        _getProductByIdUseCaseMock
            .Setup(x => x.ExecuteAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var action = await _controller.GetByIdAsync(productId, CancellationToken.None);

        var objectResult = action.Should()
            .BeAssignableTo<ObjectResult>()
            .Subject;

        var result = ExtractResult<ProductDto?>(action);

        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Data.Should().BeEquivalentTo(productDto);
    }

    [Fact]
    public async Task GetByIdAsync_WhenNotFound_ShouldReturnNotFound()
    {
        // Arrange
        var productId = "123";

        var expectedResult =
            new Result<ProductDto?>().Fail("Product not found", HttpStatusCode.NotFound);

        _getProductByIdUseCaseMock
            .Setup(x => x.ExecuteAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var action = await _controller.GetByIdAsync(productId, CancellationToken.None);

        // Assert
        var objectResult = action.Should()
            .BeAssignableTo<ObjectResult>()
            .Subject;

        objectResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);

        var result = ExtractResult<ProductDto?>(action);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.Message.Should().Be("Product not found");
        result.Data.Should().BeNull();
    }



    [Fact]
    public async Task GetAsync_WhenThereAreProducts_ShouldReturnAllProducts()
    {
        var products = new List<ProductDto>
    {
        BuildProductDto("Hamburguer", ProductType.Snack),
        BuildProductDto("Suco", ProductType.Drink),
        BuildProductDto("Batata-Frita", ProductType.Accompaniment)
    };

        var expectedResult = new Result<List<ProductDto>>().Ok(products, HttpStatusCode.OK);

        _getProductsUseCaseMock
            .Setup(x => x.ExecuteAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var action = await _controller.GetAsync(CancellationToken.None);

        var objectResult = action.Should()
            .BeAssignableTo<ObjectResult>()
            .Subject;

        var result = ExtractResult<List<ProductDto>>(action);

        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Data.Should().NotBeEmpty();
    }


    [Fact]
    public async Task GetAsync_WhenInternalError_ShouldReturnInternalServerError()
    {
        var expectedResult =
            new Result<List<ProductDto>>().Fail("Internal Error", HttpStatusCode.InternalServerError);

        _getProductsUseCaseMock
            .Setup(x => x.ExecuteAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var action = await _controller.GetAsync(CancellationToken.None);

        action.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

        var result = ExtractResult<List<ProductDto>>(action);

        result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        result.Message.Should().Be("Internal Error");
    }


    [Fact]
    public async Task GetByType_WhenFound_ShouldReturnProducts()
    {
        var products = new List<ProductDto>
    {
        BuildProductDto("Hamburguer", ProductType.Snack),
        BuildProductDto("X-Salada", ProductType.Snack)
    };

        var expectedResult = new Result<List<ProductDto>>().Ok(products, HttpStatusCode.OK);

        _getProductByTypeUseCaseMock
            .Setup(x => x.ExecuteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var action = await _controller.GetByTypeAsync("snack", CancellationToken.None);

        var objectResult = action.Should().BeAssignableTo<ObjectResult>().Subject;

        var result = ExtractResult<List<ProductDto>>(action);

        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Data.Should().NotBeEmpty();
    }


    [Fact]
    public async Task GetByType_WhenNotFound_ShouldReturnNotFound()
    {
        var expectedResult =
            new Result<List<ProductDto>>().Fail("Product not found", HttpStatusCode.NotFound);

        _getProductByTypeUseCaseMock
            .Setup(x => x.ExecuteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var action = await _controller.GetByTypeAsync("snack", CancellationToken.None);

        var objectResult = action.Should()
            .BeAssignableTo<ObjectResult>()
            .Subject;

        var result = ExtractResult<List<ProductDto>>(action);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.Data.Should().BeNull();
    }


    [Fact]
    public async Task PostAsync_WhenInsert_ShouldReturnProduct()
    {
        var productDto = BuildProductDto("x-burger", ProductType.Snack);

        var expectedResult =
            new Result<ProductDto>().Ok(productDto, HttpStatusCode.OK);

        _createProductUseCaseMock
            .Setup(x => x.ExecuteAsync(It.IsAny<ProductDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var action = await _controller.PostAsync(CancellationToken.None, productDto);

        var objectResult = action.Should().BeAssignableTo<ObjectResult>().Subject;

        var result = ExtractResult<ProductDto>(action);

        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Data.Should().NotBeNull();
    }


    [Fact]
    public async Task GetByIdsAsync_WhenFoundProducts_ShouldReturnOk()
    {
        var ids = new List<string> { "123", "213" };

        var products = new List<ProductDto>
    {
        new SnackDto { Id = "123", Name = "Burger" },
        new DrinkDto { Id = "213", Name = "Coke" }
    };

        var expectedResult =
            new Result<List<ProductDto>>().Ok(products, HttpStatusCode.OK);

        _getActiveProductsByIdsUseCaseMock
            .Setup(x => x.ExecuteAsync(ids, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var action = await _controller.GetActiveByIdsAsync(ids, CancellationToken.None);

        var objectResult = action.Should().BeAssignableTo<ObjectResult>().Subject;

        var result = ExtractResult<List<ProductDto>>(action);

        result.StatusCode.Should().Be(HttpStatusCode.OK);
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

    static Result<T> ExtractResult<T>(IActionResult actionResult)
    {
        var objectResult = actionResult as ObjectResult;
        objectResult.Should().NotBeNull();

        return objectResult!.Value.Should()
            .BeOfType<Result<T>>()
            .Subject;
    }

}
