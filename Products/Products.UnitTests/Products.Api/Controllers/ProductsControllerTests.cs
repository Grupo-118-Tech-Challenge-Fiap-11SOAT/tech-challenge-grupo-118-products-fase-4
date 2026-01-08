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
    public async Task GetByIdAsync_WhenFound_ShouldReturnOk()
    {
        var productId = "123";
        var product = BuildProductDto("x-burger", ProductType.Snack);

        var expectedResult =
            new Result<ProductDto?>().Ok(product, HttpStatusCode.OK);

        _getProductByIdUseCaseMock
            .Setup(x => x.ExecuteAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var action = await _controller.GetByIdAsync(productId, CancellationToken.None);

        var objectResult = action.Should().BeOfType<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);

        var result = ExtractResult<ProductDto?>(action);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Data.Should().BeEquivalentTo(product);
    }

    [Fact]
    public async Task GetByIdAsync_WhenNotFound_ShouldReturnNotFound()
    {
        var productId = "123";

        var expectedResult =
            new Result<ProductDto?>().Fail("Product not found", HttpStatusCode.NotFound);

        _getProductByIdUseCaseMock
            .Setup(x => x.ExecuteAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var action = await _controller.GetByIdAsync(productId, CancellationToken.None);

        var objectResult = action.Should().BeOfType<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);

        var result = ExtractResult<ProductDto?>(action);
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.Message.Should().Be("Product not found");
        result.Data.Should().BeNull();
    }

    [Fact]
    public async Task GetAsync_WhenProductsExist_ShouldReturnOk()
    {
        var products = new List<ProductDto>
        {
            BuildProductDto("Burger", ProductType.Snack),
            BuildProductDto("Coke", ProductType.Drink)
        };

        var expectedResult =
            new Result<List<ProductDto>>().Ok(products, HttpStatusCode.OK);

        _getProductsUseCaseMock
            .Setup(x => x.ExecuteAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var action = await _controller.GetAsync(CancellationToken.None);

        var objectResult = action.Should().BeOfType<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);

        var result = ExtractResult<List<ProductDto>>(action);
        result.Data.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetAsync_WhenError_ShouldReturnInternalServerError()
    {
        var expectedResult =
            new Result<List<ProductDto>>().Fail("Internal Error", HttpStatusCode.InternalServerError);

        _getProductsUseCaseMock
            .Setup(x => x.ExecuteAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var action = await _controller.GetAsync(CancellationToken.None);

        var objectResult = action.Should().BeOfType<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

        var result = ExtractResult<List<ProductDto>>(action);
        result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        result.Message.Should().Be("Internal Error");
    }

    [Fact]
    public async Task GetByTypeAsync_WhenFound_ShouldReturnOk()
    {
        var products = new List<ProductDto>
        {
            BuildProductDto("Burger", ProductType.Snack)
        };

        var expectedResult =
            new Result<List<ProductDto>>().Ok(products, HttpStatusCode.OK);

        _getProductByTypeUseCaseMock
            .Setup(x => x.ExecuteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var action = await _controller.GetByTypeAsync("snack", CancellationToken.None);

        var objectResult = action.Should().BeOfType<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);

        var result = ExtractResult<List<ProductDto>>(action);
        result.Data.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetByTypeAsync_WhenNotFound_ShouldReturnNotFound()
    {
        var expectedResult =
            new Result<List<ProductDto>>().Fail("Product not found", HttpStatusCode.NotFound);

        _getProductByTypeUseCaseMock
            .Setup(x => x.ExecuteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var action = await _controller.GetByTypeAsync("snack", CancellationToken.None);

        var objectResult = action.Should().BeOfType<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);

        var result = ExtractResult<List<ProductDto>>(action);
        result.Data.Should().BeNull();
    }

    [Fact]
    public async Task PostAsync_WhenCreated_ShouldReturnOk()
    {
        var product = BuildProductDto("x-burger", ProductType.Snack);

        var expectedResult =
            new Result<ProductDto>().Ok(product, HttpStatusCode.OK);

        _createProductUseCaseMock
            .Setup(x => x.ExecuteAsync(It.IsAny<ProductDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var action = await _controller.PostAsync(CancellationToken.None, product);

        var objectResult = action.Should().BeOfType<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);

        var result = ExtractResult<ProductDto>(action);
        result.Data.Should().NotBeNull();
    }

    [Fact]
    public async Task GetActiveByIdsAsync_WhenFound_ShouldReturnOk()
    {
        var ids = new List<string> { "1", "2" };

        var products = new List<ProductDto>
        {
            BuildProductDto("Burger", ProductType.Snack),
            BuildProductDto("Coke", ProductType.Drink)
        };

        var expectedResult =
            new Result<List<ProductDto>>().Ok(products, HttpStatusCode.OK);

        _getActiveProductsByIdsUseCaseMock
            .Setup(x => x.ExecuteAsync(ids, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var action = await _controller.GetActiveByIdsAsync(ids, CancellationToken.None);

        var objectResult = action.Should().BeOfType<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);

        var result = ExtractResult<List<ProductDto>>(action);
        result.Data.Should().HaveCount(2);
    }

    // ===================== HELPERS =====================

    private ProductDto BuildProductDto(string name, ProductType type)
    {
        var images = new List<ImageProductDto>
        {
            new(1, "https://example.com/front.png")
        };

        return type switch
        {
            ProductType.Snack => new SnackDto
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = name,
                Price = 10,
                IsActive = true,
                Images = images,
                Ingredients = new() { "ingredient" }
            },
            ProductType.Drink => new DrinkDto
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = name,
                Price = 5,
                IsActive = true,
                Images = images,
                Size = "M",
                Flavor = "Cola"
            },
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static Result<T> ExtractResult<T>(IActionResult actionResult)
    {
        var objectResult = actionResult.Should().BeOfType<ObjectResult>().Subject;

        return objectResult.Value.Should()
            .BeOfType<Result<T>>()
            .Subject;
    }
}
