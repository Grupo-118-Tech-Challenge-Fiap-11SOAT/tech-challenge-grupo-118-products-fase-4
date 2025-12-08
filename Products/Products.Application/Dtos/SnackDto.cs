using MongoDB.Bson;
using Products.Domain.Entities;
using System.Text.Json.Serialization;

namespace Products.Application.Dtos;

public class SnackDto : ProductDto
{
    [JsonRequired]
    [JsonPropertyName("ingredients")]
    public List<string> Ingredients { get; set; } = new();
    public SnackDto()
    {
        
    }

    public SnackDto(string name, decimal price, bool isActive, List<ImageProduct> images, List<string> ingredients) : base(name, price, isActive, images)
    {
        Ingredients = ingredients;
    }

    public SnackDto(ObjectId id , string name, decimal price, bool isActive, List<ImageProduct> images, List<string> ingredients) : base(id, name, price, isActive, images)
    {
        Ingredients = ingredients;
    }

}
