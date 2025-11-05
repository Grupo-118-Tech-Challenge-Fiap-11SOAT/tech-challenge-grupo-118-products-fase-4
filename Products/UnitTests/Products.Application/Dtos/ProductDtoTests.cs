using System.Collections.Generic;
using FluentAssertions;
using MongoDB.Bson;
using Products.Application.Dtos;
using Products.Domain.Entities;

namespace UnitTests.Products.Application.Dtos;

public class ProductDtoTests
{
    [Fact]
    public void Constructor_WithBasicParameters_ShouldInitializeProperties()
    {
        // Arrange
        var name = "Coca-Cola";
        var price = 5.99m;
        var isActive = true;
        var images = new List<ImageProduct>
        {
            new ImageProduct(1, "https://example.com/imagem.png")
        };

        // Act
        var dto = new ProductDto(name, price, isActive, images);

        // Assert
        dto.Name.Should().Be(name);
        dto.Price.Should().Be(price);
        dto.IsActive.Should().BeTrue();
        dto.Images.Should().BeEquivalentTo(images);
        dto.Id.Should().Be(ObjectId.Empty);
    }

    [Fact]
    public void Constructor_WithAllParameters_ShouldInitializeAllProperties()
    {
        // Arrange
        var id = ObjectId.GenerateNewId();
        var name = "Pepsi";
        var price = 4.50m;
        var isActive = false;
        var images = new List<ImageProduct>
        {
            new ImageProduct(1, "https://example.com/front.png"),
            new ImageProduct(2, "https://example.com/back.png")
        };

        // Act
        var dto = new ProductDto(id, name, price, isActive, images);

        // Assert
        dto.Id.Should().Be(id);
        dto.Name.Should().Be(name);
        dto.Price.Should().Be(price);
        dto.Images.Should().BeEquivalentTo(images);
        dto.IsActive.Should().BeFalse();
    }
}
