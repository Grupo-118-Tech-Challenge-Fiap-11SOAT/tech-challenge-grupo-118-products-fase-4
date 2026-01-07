using MongoDB.Bson;
using Products.Domain.Entities;

namespace Products.UnitTests.Domain.Entities;

public class SnackTests
{
    [Fact]
    public void Constructor_WithFullParameters_ShouldCreateSnackCorrectly()
    {
        // Arrange
        var id = ObjectId.GenerateNewId();
        var name = "Hambúrguer";
        var price = 18.90m;
        var isActive = true;
        var createdAt = DateTimeOffset.UtcNow.AddDays(-1);
        var updatedAt = DateTimeOffset.UtcNow;
        var ingredients = new List<string> { "Pão", "Carne", "Queijo" };
        var images = new List<ImageProduct>();

        // Act
        var snack = new Snack(
            id,
            name,
            price,
            isActive,
            createdAt,
            updatedAt,
            ingredients,
            images
        );

        // Assert
        Assert.Equal(id, snack.Id);
        Assert.Equal(name, snack.Name);
        Assert.Equal(price, snack.Price);
        Assert.True(snack.IsActive);
        Assert.Equal(createdAt, snack.CreatedAt);
        Assert.Equal(updatedAt, snack.UpdatedAt);
        Assert.Equal(ingredients, snack.Ingredients);
        Assert.Equal(images, snack.Images);
    }

    [Fact]
    public void Constructor_WithoutId_ShouldCreateSnackCorrectly()
    {
        // Arrange
        var name = "Coxinha";
        var price = 7.50m;
        var isActive = true;
        var ingredients = new List<string> { "Frango", "Massa" };

        // Act
        var snack = new Snack(
            name,
            price,
            isActive,
            ingredients
        );

        // Assert
        Assert.Equal(name, snack.Name);
        Assert.Equal(price, snack.Price);
        Assert.True(snack.IsActive);
        Assert.Equal(ingredients, snack.Ingredients);

        Assert.NotNull(snack.Images);
        Assert.Empty(snack.Images);
        Assert.True(snack.CreatedAt <= DateTimeOffset.UtcNow);
        Assert.True(snack.UpdatedAt <= DateTimeOffset.UtcNow);
    }

    [Fact]
    public void Snack_ShouldInheritFromProduct()
    {
        // Arrange & Act
        var snack = new Snack(
            "Empada",
            6.00m,
            true,
            new List<string> { "Massa", "Frango" }
        );

        // Assert
        Assert.IsAssignableFrom<Product>(snack);
    }
}
