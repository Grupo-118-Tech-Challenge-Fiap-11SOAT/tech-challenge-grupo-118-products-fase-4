using Products.Application.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace Products.Api.Examples;

/// <summary>
/// Class to create multiple examples for Swagger documentation
/// </summary>
public class ProductsExample : IMultipleExamplesProvider<ProductDto>
{
    /// <summary>
    /// Create examples for Swagger documentation
    /// </summary>
    /// <returns></returns>
    public IEnumerable<SwaggerExample<ProductDto>> GetExamples()
    {
        yield return SwaggerExample.Create<ProductDto>("Accompaniment Example", new AccompanimentDto
        {
            Type = "accompaniment",
            Id = null,
            Name = "Batata Frita",
            Price = 12,
            Images =
            [
                new(1, "https://example.com/images/batata_frita.jpg")
            ],
            IsActive = true,
            Size = "Média",
        });

        yield return SwaggerExample.Create<ProductDto>(
            "Dessert Example", new DessertDto
            {
                Type = "dessert",
                Id = null,
                Name = "Cheesecake",
                Price = 15,
                Images =
                [
                    new(1, "https://example.com/images/cheesecake.jpg")
                ],
                IsActive = true,
                PortionSize = "1 fatia",
            });

        yield return SwaggerExample.Create<ProductDto>("Drink Example", new DrinkDto
        {
            Type = "drink",
            Id = null,
            Name = "Refrigerante",
            Price = 7.50m,
            Images =
            [
                new(1, "https://example.com/images/drink.jpg")
            ],
            IsActive = true,
            Size = "500ml",
            Flavor = "Cola"
        });

        yield return SwaggerExample.Create<ProductDto>("Snack Example", new SnackDto
        {
            Type = "snack",
            Id = null,
            Name = "Hambúrguer Artesanal",
            Price = 29.90m,
            Images =
            [
                new(1, "https://example.com/images/hamburguer.jpg")
            ],
            IsActive = true,
            Ingredients = ["Pão", "Carne", "Queijo", "Alface"]
        });
    }
}