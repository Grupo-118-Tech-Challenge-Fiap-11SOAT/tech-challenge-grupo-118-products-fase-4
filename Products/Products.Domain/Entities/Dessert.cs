using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Products.Domain.Entities;

[BsonDiscriminator("Dessert")]
public class Dessert : Product
{
    public Dessert(ObjectId id,string name, decimal price, bool isActive, string portionSize, DateTimeOffset createdAt, DateTimeOffset updatedAt, List<ImageProduct>? images = null) : base(id, name, price, isActive, createdAt, updatedAt, images)
    {
        PortionSize = portionSize;
    }

    public Dessert(string name, decimal price, bool isActive, string portionSize, List<ImageProduct>? images = null) : base(name, price, isActive, images)
    {
        PortionSize = portionSize;
    }

    [BsonElement("portionSize")]
    public string PortionSize { get; set; } = null!;
}
