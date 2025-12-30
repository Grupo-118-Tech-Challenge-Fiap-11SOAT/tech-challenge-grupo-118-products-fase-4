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
    public async Task ExecuteAsync_WhenDrinkProductsExist_ShouldReturnOk()
    {
        //Arrange
        var cocaId = ObjectId.GenerateNewId();
        var juiceId = ObjectId.GenerateNewId();

        List<ImageProductDto> imagesProductDto = new List<ImageProductDto>();

        List<ProductDto> expectedData = new List<ProductDto>{
            new DrinkDto(){Id = cocaId.ToString(), Name = "Coca-Cola", Price = 12.50m, IsActive = true, Images = imagesProductDto},
            new DrinkDto(){Id = juiceId.ToString(), Name = "Suco de Uva", Price = 12.50m, IsActive = true, Images = imagesProductDto, Size = "P", Flavor = "Uva" }
            };

        List<ImageProduct> imagesProduct = new List<ImageProduct>();

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
    public async Task ExecuteAsync_WhenSnackProductsExist_ShouldReturnOk()
    {
        //Arrange
        var hamburgerId = ObjectId.GenerateNewId();
        List<ImageProductDto> imagesProductDto = new List<ImageProductDto>();

        List<ProductDto> expectedData = new List<ProductDto>{
            new SnackDto(){Id = hamburgerId.ToString(), Name = "Hambúrguer", Price = 20.00m, IsActive = true, Images = imagesProductDto, Ingredients = ["Pão, carne, queijo, alface, tomate"] }
        };

        List<ImageProduct> imagesProduct = new List<ImageProduct>();

        var productsEntities = new List<Product>()
        {
            new Snack(hamburgerId, "Hambúrguer", 20.00m, true, DateTime.Now, DateTime.Now,["Pão, carne, queijo, alface, tomate"], imagesProduct)
        };

        _productRepositoryMock.Setup(x => x.GetProductByTypeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(productsEntities);

        //Act
        var result = await _useCase.ExecuteAsync("123", CancellationToken.None);

        //Assert
        result.Data.Should().BeEquivalentTo(expectedData);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ExecuteAsync_WhenDessertProductsExist_ShouldReturnOk()
    {
        //Arrange
        var cheesecakeId = ObjectId.GenerateNewId();
        List<ImageProductDto> imagesProductDto = new List<ImageProductDto>();

        List<ProductDto> expectedData = new List<ProductDto>{
            new DessertDto(){Id = cheesecakeId.ToString(), Name = "Pudim", Price = 15.00m, IsActive = true, Images = imagesProductDto, PortionSize = "Médio" },
        };

        List<ImageProduct> imagesProduct = new List<ImageProduct>();

        var productsEntities = new List<Product>()
        {
            new Dessert(cheesecakeId, "Pudim", 15.00m, true, "Médio", DateTime.Now, DateTime.Now, imagesProduct),
        };

        _productRepositoryMock.Setup(x => x.GetProductByTypeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(productsEntities);

        //Act
        var result = await _useCase.ExecuteAsync("123", CancellationToken.None);

        //Assert
        result.Data.Should().BeEquivalentTo(expectedData);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task ExecuteAsync_WhenAccompanimentProductsExist_ShouldReturnOk()
    {
        //Arrange
        var frenchFriesId = ObjectId.GenerateNewId();
        List<ImageProductDto> imagesProductDto = new List<ImageProductDto>();

        List<ProductDto> expectedData = new List<ProductDto>{
            new AccompanimentDto(){Id = frenchFriesId.ToString(), Name = "Batata Frita", Price = 10.00m, IsActive = true, Images = imagesProductDto, Size = "Grande" },
        };

        List<ImageProduct> imagesProduct = new List<ImageProduct>();

        var productsEntities = new List<Product>()
        {
            new Accompaniment(frenchFriesId, "Batata Frita", 10.00m, true, DateTime.Now, DateTime.Now, "Grande", imagesProduct),
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
