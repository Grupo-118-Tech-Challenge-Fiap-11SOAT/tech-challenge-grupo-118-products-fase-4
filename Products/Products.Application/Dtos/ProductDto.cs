using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Products.Application.Enums;
using Products.Domain.Entities;
using System.Text.Json.Serialization;

namespace Products.Application.Dtos;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(SnackDto), "snack")]
[JsonDerivedType(typeof(AccompanimentDto), "accompaniment")]
[JsonDerivedType(typeof(DessertDto), "dessert")]
[JsonDerivedType(typeof(DrinkDto), "drink")]
public abstract class ProductDto
{
    [JsonPropertyName("id")]
    public ObjectId Id { get; set; }

    [JsonRequired]
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonRequired]
    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("images")]
    public List<ImageProductDto> Images { get; set; } = new List<ImageProductDto>();

    [JsonRequired]
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }

    public ProductDto() { }

    public ProductDto(string name, decimal price, bool isActive, List<ImageProduct> images)
    {
        Name = name;
        Price = price;
        IsActive = isActive;
        foreach (var item in images)
        {
            Images.Add(new ImageProductDto(item.Position, item.Url));
        }
    }

    public ProductDto(ObjectId id, string name, decimal price, bool isActive, List<ImageProduct> images)
    {
        Id = id;
        Name = name;
        Price = price;
        IsActive = isActive;
        foreach (var item in images)
        {
            Images.Add(new ImageProductDto(item.Position, item.Url));
        }     
    }
}

public class ImageProductDto
{
    [JsonPropertyName("position")]
    public int Position { get;set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    public ImageProductDto(int position, string url)
    {
        Position = position;
        Url = url;
    }

    public ImageProductDto()
    {
        
    }
}
