
using MongoDB.Bson.Serialization.Attributes;

namespace Products.Domain.Entities;

[BsonDiscriminator("Dessert")]
public class Dessert : Product
{
    public Dessert(string name, decimal price, bool isActive, DateTimeOffset createdAt, DateTimeOffset updatedAt, List<ImageProduct>? images = null) : base(name, price, isActive, createdAt, updatedAt, images)
    {
    }

    public string PortionSize { get; set; } = null!;
}
