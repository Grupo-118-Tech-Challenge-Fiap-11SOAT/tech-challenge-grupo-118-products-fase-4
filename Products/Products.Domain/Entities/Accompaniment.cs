using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.Domain.Entities;

[BsonDiscriminator("Accompaniment")]
public class Accompaniment : Product
{
    public Accompaniment(string name, decimal price, bool isActive, DateTimeOffset createdAt, DateTimeOffset updatedAt, List<ImageProduct>? images = null) : base(name, price, isActive, createdAt, updatedAt, images)
    {
    }

    public string Size { get; set; } = null!;
}
