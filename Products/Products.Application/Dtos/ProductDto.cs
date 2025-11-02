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

    public ProductDto(string name, decimal price, bool isActive)
    {
        Name = name;
        Price = price;
        IsActive = isActive;
    }

    public ProductDto(ObjectId id, string name, decimal price, List<ImageProduct> images, bool isActive)
    {
        Id = id;
        Name = name;
        Price = price;
        Images = images;
        IsActive = isActive;
    }
}
