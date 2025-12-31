using MongoDB.Bson;
using Products.Domain.Entities;

namespace Products.UnitTests.Domain.Entities;

public class AccompanimentTests
{
    [Fact]
    public void Constructor_WithFullParameters_ShouldCreateAccompanimentCorrectly()
    {
        // Arrange
        var id = ObjectId.GenerateNewId();
        var name = "Batata Frita";
        var price = 9.90m;
        var isActive = true;
        var createdAt = DateTimeOffset.UtcNow.AddDays(-1);
        var updatedAt = DateTimeOffset.UtcNow;
        var size = "Grande";
        var images = new List<ImageProduct>();

        // Act
        var accompaniment = new Accompaniment(
            id,
            name,
            price,
            isActive,
            createdAt,
            updatedAt,
            size,
            images
        );

        // Assert
        Assert.Equal(id, accompaniment.Id);
        Assert.Equal(name, accompaniment.Name);
        Assert.Equal(price, accompaniment.Price);
        Assert.True(accompaniment.IsActive);
        Assert.Equal(createdAt, accompaniment.CreatedAt);
        Assert.Equal(updatedAt, accompaniment.UpdatedAt);
        Assert.Equal(size, accompaniment.Size);
        Assert.Equal(images, accompaniment.Images);
    }

    [Fact]
    public void Constructor_WithoutId_ShouldCreateAccompanimentWithDefaultBaseValues()
    {
        // Arrange
        var name = "Molho Especial";
        var price = 2.50m;
        var isActive = true;
        var size = "Pequeno";

        // Act
        var accompaniment = new Accompaniment(
            name,
            price,
            isActive,
            size
        );

        // Assert
        Assert.Equal(name, accompaniment.Name);
        Assert.Equal(price, accompaniment.Price);
        Assert.True(accompaniment.IsActive);
        Assert.Equal(size, accompaniment.Size);

        Assert.True(accompaniment.CreatedAt <= DateTimeOffset.UtcNow);
        Assert.True(accompaniment.UpdatedAt <= DateTimeOffset.UtcNow);
    }

    [Fact]
    public void Accompaniment_ShouldInheritFromProduct()
    {
        // Arrange & Act
        var accompaniment = new Accompaniment(
            "Onion Rings",
            7.90m,
            true,
            "Médio"
        );

        // Assert
        Assert.IsAssignableFrom<Product>(accompaniment);
    }
}
