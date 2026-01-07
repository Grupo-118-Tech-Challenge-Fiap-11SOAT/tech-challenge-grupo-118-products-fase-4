using MongoDB.Bson;
using Products.Domain.Entities;

namespace Products.UnitTests.Domain.Entities;

public class DessertTests
{
    [Fact]
    public void Constructor_WithFullParameters_ShouldCreateDessertCorrectly()
    {
        // Arrange
        var id = ObjectId.GenerateNewId();
        var name = "Brownie";
        var price = 12.90m;
        var isActive = true;
        var portionSize = "Individual";
        var createdAt = DateTimeOffset.UtcNow.AddDays(-1);
        var updatedAt = DateTimeOffset.UtcNow;
        var images = new List<ImageProduct>();

        // Act
        var dessert = new Dessert(
            id,
            name,
            price,
            isActive,
            portionSize,
            createdAt,
            updatedAt,
            images
        );

        // Assert
        Assert.Equal(id, dessert.Id);
        Assert.Equal(name, dessert.Name);
        Assert.Equal(price, dessert.Price);
        Assert.True(dessert.IsActive);
        Assert.Equal(portionSize, dessert.PortionSize);
        Assert.Equal(createdAt, dessert.CreatedAt);
        Assert.Equal(updatedAt, dessert.UpdatedAt);
        Assert.Equal(images, dessert.Images);
    }

    [Fact]
    public void Constructor_WithoutId_ShouldCreateDessertWithDefaultBaseValues()
    {
        // Arrange
        var name = "Pudim";
        var price = 8.50m;
        var isActive = true;
        var portionSize = "Fatia";

        // Act
        var dessert = new Dessert(
            name,
            price,
            isActive,
            portionSize
        );

        // Assert
        Assert.Equal(name, dessert.Name);
        Assert.Equal(price, dessert.Price);
        Assert.True(dessert.IsActive);
        Assert.Equal(portionSize, dessert.PortionSize);

        Assert.True(dessert.CreatedAt <= DateTimeOffset.UtcNow);
        Assert.True(dessert.UpdatedAt <= DateTimeOffset.UtcNow);
    }

    [Fact]
    public void Dessert_ShouldInheritFromProduct()
    {
        // Arrange & Act
        var dessert = new Dessert(
            "Cheesecake",
            14.90m,
            true,
            "Fatia"
        );

        // Assert
        Assert.IsAssignableFrom<Product>(dessert);
    }
}
