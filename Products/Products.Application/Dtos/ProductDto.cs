using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace Products.Application.Dtos;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(SnackDto), "snack")]
[JsonDerivedType(typeof(AccompanimentDto), "accompaniment")]
[JsonDerivedType(typeof(DessertDto), "dessert")]
[JsonDerivedType(typeof(DrinkDto), "drink")]
public abstract class ProductDto
{
    [JsonPropertyName("id")] public string? Id { get; set; }

    [JsonRequired]
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonRequired]
    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("images")] public List<ImageProductDto> Images { get; set; } = new List<ImageProductDto>();

    [JsonRequired]
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }

    [JsonPropertyName("type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyOrder(-1)]
    public string? Type { get; set; }

    public ProductDto()
    {
    }
}

public class ImageProductDto
{
    [JsonPropertyName("position")] public int Position { get; set; }

    [JsonPropertyName("url")] public string Url { get; set; }

    public ImageProductDto(int position, string url)
    {
        Position = position;
        Url = url;
    }

    public ImageProductDto()
    {
    }
}