using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Products.Domain.Entities;

[BsonDiscriminator("Dessert")]
public class Dessert : Product
{
    public Dessert(ObjectId id,string name, decimal price, bool isActive, DateTimeOffset createdAt, DateTimeOffset updatedAt, List<ImageProduct>? images = null) : base(id, name, price, isActive, createdAt, updatedAt, images)
    {
    }

    public Dessert(string name, decimal price, bool isActive, List<ImageProduct>? images = null) : base(name, price, isActive, images)
    {
    }

    public string PortionSize { get; set; } = null!;
}
