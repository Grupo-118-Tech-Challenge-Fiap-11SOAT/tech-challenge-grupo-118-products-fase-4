using MongoDB.Bson;
using Products.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Products.Application.Dtos;

public class DessertDto : ProductDto
{
    [JsonPropertyName("portionSize")]
    public string PortionSize { get; set; } = null!;

    public DessertDto()
    {
        
    }
    public DessertDto(string name, decimal price, bool isActive, List<ImageProduct> images, string portionSize) : base(name, price, isActive, images)
    {
        PortionSize = portionSize;
    }

    public DessertDto(ObjectId id, string name, decimal price, bool isActive, List<ImageProduct> images, string portionSize) : base(id, name, price, isActive, images)
    {
        PortionSize = portionSize;
    }
}
