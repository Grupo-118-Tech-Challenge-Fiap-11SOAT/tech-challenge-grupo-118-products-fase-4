using MongoDB.Bson;
using Products.Domain.Entities;
using System.Text.Json.Serialization;

namespace Products.Application.Dtos;

public class AccompanimentDto : ProductDto
{
    [JsonPropertyName("size")]
    public string Size { get; set; } = null!;
    public AccompanimentDto()
    {
        
    }

    public AccompanimentDto(string name, decimal price, bool isActive, List<ImageProduct> images, string size) : base(name, price, isActive, images)
    {
        Size = size;
    }

    public AccompanimentDto(ObjectId id , string name, decimal price, bool isActive, List<ImageProduct> images, string size) : base(id, name, price, isActive, images)
    {
        Size = size;
    }
}
