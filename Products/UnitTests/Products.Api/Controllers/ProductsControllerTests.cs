using FluentAssertions;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Moq;
using Products.Application.Common.Models;
using Products.Application.Dtos;
using Products.Application.UseCases.Interfaces;
using Products.Domain.Entities;
using System.Net;
using WebApplication1.Controllers;

namespace UnitTests.Products.Api.Controllers;

public class ProductsControllerTests
{
    private readonly Mock<ILogger<ProductsController>> _loggerMock;
    private readonly Mock<IGetProductByIdUseCase> _getProductByIdUseCaseMock;
    private readonly Mock<IGetProductByTypeUseCase> _getProductByTypeUseCaseMock;
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _loggerMock = new Mock<ILogger<ProductsController>>();
        _getProductByIdUseCaseMock = new Mock<IGetProductByIdUseCase>();
        _getProductByTypeUseCaseMock = new Mock<IGetProductByTypeUseCase>();

        _controller = new ProductsController(
            _loggerMock.Object,
            _getProductByIdUseCaseMock.Object,
            _getProductByTypeUseCaseMock.Object
        );
    }

    [Fact]
    public async Task GetAsync_WhenFound_ShouldReturnProduct()
    {
        // Arrange
        var productId = "123";
        var images = new List<ImageProduct>
        {
             new ImageProduct(1, "https://example.com/front.png"),
             new ImageProduct(2, "https://example.com/back.png")
        };
        var product = new ProductDto(ObjectId.GenerateNewId(),"lanche", 12, true, images);

        var expectedResult = new Result<ProductDto?>().Ok(product, HttpStatusCode.OK);         

        _getProductByIdUseCaseMock
            .Setup(x => x.ExecuteAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.GetAsync(productId, CancellationToken.None);

        // Assert
        result.Data.Should().Be(product);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        _getProductByIdUseCaseMock.Verify(x => x.ExecuteAsync(productId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAsync_WhenNotFound_ShouldReturnFail()
    {
        // Arrange
        var productId = "123";
        var expectedResult = new Result<ProductDto?>().Fail("Product not found", HttpStatusCode.NotFound);

        _getProductByIdUseCaseMock
            .Setup(x => x.ExecuteAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.GetAsync(productId, CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.Message.Should().Be("Product not found");
        result.Data.Should().BeNull();
    }

    //[Fact]
    //public async Task GetByTypeAsync_ShouldReturnProducts_WhenCategoryExists()
    //{
    //    // Arrange
    //    var category = "drinks";
    //    var products = new List<ProductDto>
    //    {
    //        new ProductDto { Id = "1", Name = "Coca-Cola" },
    //        new ProductDto { Id = "2", Name = "Pepsi" }
    //    };

    //    _getProductByTypeUseCaseMock
    //        .Setup(x => x.ExecuteAsync(category, It.IsAny<CancellationToken>()))
    //        .ReturnsAsync(Result<List<ProductDto>?>.Success(products));

    //    // Act
    //    var result = await _controller.GetByTypeAsync(category, CancellationToken.None);

    //    // Assert
    //    result.IsSuccess.Should().BeTrue();
    //    result.Value.Should().NotBeNull();
    //    result.Value!.Should().HaveCount(2);
    //}

    //[Fact]
    //public async Task GetByTypeAsync_ShouldReturnFailure_WhenCategoryNotFound()
    //{
    //    // Arrange
    //    var category = "invalid";

    //    _getProductByTypeUseCaseMock
    //        .Setup(x => x.ExecuteAsync(category, It.IsAny<CancellationToken>()))
    //        .ReturnsAsync(Result<List<ProductDto>?>.Failure("Category not found"));

    //    // Act
    //    var result = await _controller.GetByTypeAsync(category, CancellationToken.None);

    //    // Assert
    //    result.IsSuccess.Should().BeFalse();
    //    result.Error.Should().Be("Category not found");
    //}
}
