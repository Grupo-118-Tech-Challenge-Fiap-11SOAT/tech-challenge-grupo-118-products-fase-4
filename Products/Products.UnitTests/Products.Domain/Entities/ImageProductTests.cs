using Products.Domain.Entities;

namespace Products.UnitTests.Domain.Entities;

public class ImageProductTests
{
    [Fact]
    public void Constructor_WithValidData_ShouldCreateImageProduct()
    {
        // Arrange
        var position = 1;
        var url = "https://cdn.site.com/image.png";

        // Act
        var image = new ImageProduct(position, url);

        // Assert
        Assert.Equal(position, image.Position);
        Assert.Equal(url, image.Url);
    }

    [Theory]
    [InlineData("https://cdn.site.com/image.txt")]
    [InlineData("https://cdn.site.com/image.pdf")]
    [InlineData("invalid-url")]
    public void Constructor_WithInvalidUrl_ShouldThrowException(string invalidUrl)
    {
        // Arrange
        var position = 1;

        // Act & Assert
        var exception = Assert.Throws<Exception>(() =>
            new ImageProduct(position, invalidUrl));

        Assert.Equal("The provided URL is not valid format.", exception.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_WithInvalidPosition_ShouldThrowException(int invalidPosition)
    {
        // Arrange
        var url = "https://cdn.site.com/image.jpg";

        // Act & Assert
        var exception = Assert.Throws<Exception>(() =>
            new ImageProduct(invalidPosition, url));

        Assert.Equal("Image position is invalid", exception.Message);
    }
}
