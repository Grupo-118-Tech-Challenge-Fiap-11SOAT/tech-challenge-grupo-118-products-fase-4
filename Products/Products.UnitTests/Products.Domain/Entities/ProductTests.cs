using MongoDB.Bson;
using Products.Domain.Entities;

namespace Products.UnitTests.Domain.Entities;

public class ProductTests
{
    [Fact]
    public void Constructor_WithoutId_ShouldInitializeBaseProductCorrectly()
    {
        // Arrange
        var name = "Água";
        var price = 3.50m;
        var isActive = true;
        var size = "500ml";

        // Act
        var product = new Drink(name, price, isActive, size);

        // Assert
        Assert.Equal(name, product.Name);
        Assert.Equal(price, product.Price);
        Assert.True(product.IsActive);

        Assert.NotNull(product.Images);
        Assert.Empty(product.Images);

        Assert.True(product.CreatedAt <= DateTimeOffset.UtcNow);
        Assert.True(product.UpdatedAt <= DateTimeOffset.UtcNow);
    }

    [Fact]
    public void Constructor_WithId_ShouldInitializeBaseProductCorrectly()
    {
        // Arrange
        var id = ObjectId.GenerateNewId();
        var createdAt = DateTimeOffset.UtcNow.AddDays(-2);
        var updatedAt = DateTimeOffset.UtcNow.AddDays(-1);

        // Act
        var product = new Drink(
            id,
            "Suco",
            7.90m,
            true,
            "300ml",
            createdAt,
            updatedAt
        );

        // Assert
        Assert.Equal(id, product.Id);
        Assert.Equal(createdAt, product.CreatedAt);
        Assert.Equal(updatedAt, product.UpdatedAt);
        Assert.NotNull(product.Images);
    }

    [Fact]
    public void Product_ShouldInitializeImagesList_WhenNullIsPassed()
    {
        // Arrange & Act
        var product = new Drink("Refrigerante", 6.00m, true, "350ml", null);

        // Assert
        Assert.NotNull(product.Images);
        Assert.Empty(product.Images);
    }
}
