using FluentAssertions;
using MongoDB.Bson;
using Moq;
using Products.Application.Common.Models;
using Products.Application.Dtos;
using Products.Application.UseCases;
using Products.Domain.Entities;
using Products.Infra.DataBase.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTests.Products.Application.UseCases;

public class GetProductByTypeUseCaseTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly GetProductByTypeUseCase _useCase;

    public GetProductByTypeUseCaseTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _useCase = new GetProductByTypeUseCase(_productRepositoryMock.Object);
    }
    [Fact]
    public async Task ExecuteAsync_WhenProductsExist_ShouldReturnOk()
    {
        //Arrange
        var idCoca = ObjectId.GenerateNewId();
        var idPepsi = ObjectId.GenerateNewId();

        List<ImageProduct> imagesProduct = new List<ImageProduct>();
        List<ProductDto> expectedData = new List<ProductDto>{
            new ProductDto(idCoca, "Coca-Cola", 12.50m, true, imagesProduct),
            new ProductDto(idPepsi, "Pepsi", 12.50m, true, imagesProduct)
            };

        var productsEntities = new List<Product>()
        {
            new Drink(idCoca,"Coca-Cola", 12.50m, true, DateTime.Now, DateTime.Now, imagesProduct),
            new Drink(idPepsi,"Pepsi", 12.50m, true, DateTime.Now, DateTime.Now, imagesProduct),
        };
        _productRepositoryMock.Setup(x => x.GetProductByTypeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(productsEntities);

        //Act
        var result = await _useCase.ExecuteAsync("123", CancellationToken.None);

        //Assert
        result.Data.Should().BeEquivalentTo(expectedData);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ExecuteAsync_WhenProductNotExist_ShouldReturnFail()
    {
        //Arrange
        _productRepositoryMock.Setup(x => x.GetProductByTypeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync((List<Product>?)null);

        //Act
        var result = await _useCase.ExecuteAsync("123", CancellationToken.None);

        //Assert
        result.Data.Should().BeNull();
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
