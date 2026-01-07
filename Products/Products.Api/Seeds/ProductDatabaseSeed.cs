using System.Diagnostics.CodeAnalysis;
using MongoDB.Bson;
using Products.Domain.Entities;
using Products.Infra.DataBase.Repositories.Interfaces;

namespace Products.Api.Seeds;

/// <summary>
/// Service to execute a seed on the Products database.
/// </summary>
[ExcludeFromCodeCoverage]
public class ProductDatabaseSeed : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public ProductDatabaseSeed(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Start method to execute the seed.
    /// </summary>
    /// <param name="cancellationToken"></param>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var productRepository = scope.ServiceProvider
            .GetRequiredService<IProductRepository>();

        var products = await productRepository.GetProductsAsync(cancellationToken);

        if (products.Any())
            return;

        var seedProducts = new List<Product>
        {
            new Snack(ObjectId.GenerateNewId(), "X-Salada", 12, true, DateTime.Now, DateTime.Now,
                new List<string>() { "Pão", "Queijo", "Hamburgues" }),
            new Snack(ObjectId.GenerateNewId(), "X-Bacon", 15, true, DateTime.Now, DateTime.Now,
                new List<string>() { "Pão", "Queijo", "Hamburgues", "Bacon" }),
            new Snack(ObjectId.GenerateNewId(), "Misto Quente", 7, true, DateTime.Now, DateTime.Now,
                new List<string>() { "Pão", "Queijo", "Presunto" }),
            new Snack(ObjectId.GenerateNewId(), "Bauru", 8, false, DateTime.Now, DateTime.Now,
                new List<string>() { "Pão", "Queijo", "Presunto", "Tomate" }),
            new Drink(ObjectId.GenerateNewId(), "Suco de Uva", 12, true, "M", DateTime.Now, DateTime.Now, "Uva"),
            new Drink(ObjectId.GenerateNewId(), "Suco de Maracuja", 12, false, "M", DateTime.Now, DateTime.Now,
                "Maracuja"),
            new Drink(ObjectId.GenerateNewId(), "Coca-Cola", 12, true, "M", DateTime.Now, DateTime.Now),
            new Drink(ObjectId.GenerateNewId(), "Pepsi", 12, true, "M", DateTime.Now, DateTime.Now),
            new Dessert(ObjectId.GenerateNewId(), "Sorvete", 10, true, "M", DateTime.Now, DateTime.Now),
            new Dessert(ObjectId.GenerateNewId(), "Brownie", 10, true, "P", DateTime.Now, DateTime.Now),
            new Dessert(ObjectId.GenerateNewId(), "Milk-Shake", 12, true, "P", DateTime.Now, DateTime.Now)
        };

        await productRepository.CreateManyAsync(seedProducts, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}