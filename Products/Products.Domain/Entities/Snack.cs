
using MongoDB.Bson.Serialization.Attributes;

namespace Products.Domain.Entities;

[BsonDiscriminator("Snack")]
public class Snack : Product
{
    public Snack(string name, decimal price, bool isActive, DateTimeOffset createdAt, DateTimeOffset updatedAt, List<ImageProduct>? images = null) : base(name, price, isActive, createdAt, updatedAt, images)
    {
    }

    public List<string> Ingredients { get; set; } = new();
}
