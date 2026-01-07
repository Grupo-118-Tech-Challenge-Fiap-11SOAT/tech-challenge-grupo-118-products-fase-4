using System.Diagnostics.CodeAnalysis;

namespace Products.Infra.DataBase.Contexts;

[ExcludeFromCodeCoverage]
public class MongoDbSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
}
