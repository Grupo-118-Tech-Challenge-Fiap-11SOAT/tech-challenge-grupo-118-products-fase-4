using MongoDB.Bson;
using Products.Domain.Entities;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace Products.Application.Dtos;

public class DrinkDto : ProductDto
{
    [JsonPropertyName("size")]
    public string Size { get; set; } = null!;

    [JsonPropertyName("flavor")]
    public string? Flavor { get; set; }

    public DrinkDto()
    {
        
    }

    public DrinkDto(string name, decimal price, bool isActive, List<ImageProduct> images, string size = null, string flavor = null) : base(name, price, isActive, images)
    {
        Flavor = flavor;
        Size = size;
    }

    public DrinkDto(ObjectId id ,string name, decimal price, bool isActive, List<ImageProduct> images, string size = null, string flavor = null) : base(id, name, price, isActive, images)
    {
        Flavor = flavor;
        Size = size;
    }
}
