using MongoDB.Bson;
using Products.Domain.Entities;

namespace Products.Application.Dtos;

public class ProductDto
{
    public ObjectId Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public List<ImageProduct> Images { get; set; }
    public bool IsActive { get; set; }

    public ProductDto(string name, decimal price, bool isActive, List<ImageProduct> images)
    {
        Name = name;
        Price = price;
        IsActive = isActive;
        Images = images;
    }

    public ProductDto(ObjectId id, string name, decimal price, bool isActive, List<ImageProduct> images)
    {
        Id = id;
        Name = name;
        Price = price;
        Images = images;
        IsActive = isActive;
    }
}
