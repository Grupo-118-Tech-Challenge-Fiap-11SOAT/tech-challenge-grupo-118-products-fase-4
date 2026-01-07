using MongoDB.Bson;
using Products.Domain.Entities;

namespace Products.UnitTests.Domain.Entities;

public class DrinkTests
{
    [Fact]
    public void Constructor_WithoutId_ShouldCreateDrinkCorrectly()
    {
        // Arrange
        var name = "Refrigerante";
        var price = 6.50m;
        var isActive = true;
        var size = "350ml";
        var flavor = "Cola";

        // Act
        var drink = new Drink(
            name,
            price,
            isActive,
            size,
            flavor
        );

        // Assert
        Assert.Equal(name, drink.Name);
        Assert.Equal(price, drink.Price);
        Assert.True(drink.IsActive);
        Assert.Equal(size, drink.Size);
        Assert.Equal(flavor, drink.Flavor);

        Assert.True(drink.CreatedAt <= DateTimeOffset.UtcNow);
        Assert.True(drink.UpdatedAt <= DateTimeOffset.UtcNow);
    }

    [Fact]
    public void Constructor_WithId_ShouldCreateDrinkCorrectly()
    {
        // Arrange
        var id = ObjectId.GenerateNewId();
        var name = "Suco Natural";
        var price = 8.90m;
        var isActive = true;
        var size = "500ml";
        var flavor = "Laranja";
        var createdAt = DateTimeOffset.UtcNow.AddDays(-1);
        var updatedAt = DateTimeOffset.UtcNow;
        var images = new List<ImageProduct>();

        // Act
        var drink = new Drink(
            id,
            name,
            price,
            isActive,
            size,
            createdAt,
            updatedAt,
            flavor,
            images
        );

        // Assert
        Assert.Equal(id, drink.Id);
        Assert.Equal(name, drink.Name);
        Assert.Equal(price, drink.Price);
        Assert.True(drink.IsActive);
        Assert.Equal(size, drink.Size);
        Assert.Equal(flavor, drink.Flavor);
        Assert.Equal(createdAt, drink.CreatedAt);
        Assert.Equal(updatedAt, drink.UpdatedAt);
        Assert.Equal(images, drink.Images);
    }

    [Fact]
    public void Drink_ShouldAllowNullFlavor()
    {
        // Arrange & Act
        var drink = new Drink(
            "Água",
            3.00m,
            true,
            "500ml",
            null
        );

        // Assert
        Assert.Null(drink.Flavor);
    }

    [Fact]
    public void Drink_ShouldInheritFromProduct()
    {
        // Arrange & Act
        var drink = new Drink(
            "Chá Gelado",
            7.00m,
            true,
            "450ml"
        );

        // Assert
        Assert.IsAssignableFrom<Product>(drink);
    }
}
