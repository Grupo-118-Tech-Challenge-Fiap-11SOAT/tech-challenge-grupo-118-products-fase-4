using Microsoft.Extensions.DependencyInjection;
using Products.Infra.DataBase.Contexts;
using Products.Infra.DataBase.Repositories;
using Products.Infra.DataBase.Repositories.Interfaces;
using Reqnroll;
using Reqnroll.BoDi;
using Testcontainers.MongoDb;

[Binding]
public sealed class MongoDbHooks
{
    private static MongoDbContainer _mongoContainer = null!;
    private ServiceProvider _serviceProvider = null!;

    [BeforeTestRun]
    public static async Task BeforeTestRun()
    {
        _mongoContainer = new MongoDbBuilder()
            .WithImage("mongo:7.0")
            .WithCleanUp(true)
            .Build();

        await _mongoContainer.StartAsync();
    }

    [AfterTestRun]
    public static async Task AfterTestRun()
    {
        await _mongoContainer.StopAsync();
    }

    [BeforeScenario]
    public void BeforeScenario(ScenarioContext scenarioContext)
    {
        var services = new ServiceCollection();

        services.Configure<MongoDbSettings>(options =>
        {
            options.ConnectionString = _mongoContainer.GetConnectionString();
            options.DatabaseName = "products_bdd_test";
        });

        services.AddScoped<MongoDbContext>();
        services.AddScoped<IProductRepository, ProductRepository>();

        _serviceProvider = services.BuildServiceProvider();

        var repository = _serviceProvider.GetRequiredService<IProductRepository>();

        scenarioContext.Set(repository);
    }

    [AfterScenario]
    public async Task AfterScenario(ScenarioContext scenarioContext)
    {
        var repository = scenarioContext.Get<IProductRepository>();
        await repository.ClearProductsAsync();

        _serviceProvider.Dispose();
    }
}
