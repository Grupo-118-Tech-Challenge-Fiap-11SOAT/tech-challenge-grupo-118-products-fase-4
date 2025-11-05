using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Xml.Linq;

namespace Products.Domain.Entities;

[BsonDiscriminator("Snack")]
public class Snack : Product
{
    public Snack(ObjectId id ,string name, decimal price, bool isActive, DateTimeOffset createdAt, DateTimeOffset updatedAt, List<ImageProduct>? images = null) : base(id, name, price, isActive, createdAt, updatedAt, images)
    {
    }

    public Snack(string name, decimal price, bool isActive, List<ImageProduct>? images = null) : base(name, price, isActive, images)
    {
    }

    public List<string> Ingredients { get; set; } = new();
}
