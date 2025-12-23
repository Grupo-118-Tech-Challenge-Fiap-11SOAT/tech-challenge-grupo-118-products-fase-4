using FluentAssertions;
using MongoDB.Bson;
using Moq;
using Products.Application.Dtos;
using Products.Application.UseCases;
using Products.Domain.Entities;
using Products.Infra.DataBase.Repositories.Interfaces;
using System.Net;

namespace Products.UnitTests.Products.Application.UseCases;

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
        var cocaId = ObjectId.GenerateNewId();
        var juiceId = ObjectId.GenerateNewId();

        List<ImageProduct> imagesProduct = new List<ImageProduct>();
        List<DrinkDto> expectedData = new List<DrinkDto>{
            new DrinkDto(cocaId, "Coca-Cola",12.50m, true, imagesProduct),
            new DrinkDto(juiceId, "Suco de Uva",12.50m, true, imagesProduct, "P", "Uva")
            };

        var productsEntities = new List<Product>()
        {
            new Drink(cocaId,"Coca-Cola", 12.50m, true, null, DateTime.Now, DateTime.Now, null, imagesProduct),
            new Drink(juiceId, "Suco de Uva", 12.50m, true, "P", DateTime.Now, DateTime.Now, "Uva", imagesProduct),
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
