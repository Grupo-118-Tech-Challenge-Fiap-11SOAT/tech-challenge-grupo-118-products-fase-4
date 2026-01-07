using FluentAssertions;
using Products.Domain.Entities;
using Products.Infra.DataBase.Repositories.Interfaces;
using Reqnroll;

[Binding]
public class ProductSteps
{
    private readonly ScenarioContext _scenarioContext;
    private IProductRepository _repository = null!;
    private Product? _createdProduct;
    private List<Product> _resultListProducts = new();

    public ProductSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [Given("the MongoDB database is ready")]
    public void GivenTheMongoDBDatabaseIsReady()
    {
        _repository = _scenarioContext.Get<IProductRepository>();
    }

    [Given("products exist in the database")]
    public async Task GivenProductsExistInTheDatabase()
    {
        await _repository.CreateProductAsync(
            new Snack("X-Burger", 10, true, new() { "Pão", "Hamburguer" })
        );

        await _repository.CreateProductAsync(
            new Snack("X-Salada", 12, true, new() { "Pão", "Hamburguer", "Alface" })
        );
    }

    [Given(@"products of type ""(.*)"" exist")]
    public async Task GivenProductsOfTypeExist(string type)
    {
        Product product = type switch
        {
            "Snack" => new Snack(
                "X-Burger", 10, true, new() { "Pão", "Hambúrguer" }),

            "Dessert" => new Dessert(
                "Brownie", 8, true, "Chocolate"),

            "Accompaniment" => new Accompaniment(
                "Batata Frita", 6, true, "M"),

            "Drink" => new Drink(
                "Coca-Cola", 5, true, "M"),

            _ => throw new ArgumentException($"Invalid product type: {type}")
        };

        await _repository.CreateProductAsync(product);
    }

    [When("I save a new product")]
    public async Task WhenISaveANewProduct()
    {
        var product = new Snack(
            "X-Salada",
            12,
            true,
            new List<string> { "Pão", "Hamburguer", "Queijo", "Tomate", "Alface" }
        );

        _createdProduct = await _repository.CreateProductAsync(product);
    }

    [When("I request all products")]
    public async Task WhenIRequestAllProducts()
    {
        _resultListProducts = await _repository.GetProductsAsync();
    }

    [When(@"I request products by type ""(.*)""")]
    public async Task WhenIRequestProductsByType(string type)
    {
        _resultListProducts = await _repository.GetProductByTypeAsync(type);
    }

    [Then("the product must be persisted in MongoDB")]
    public async Task ThenTheProductMustBePersistedInMongoDB()
    {
        var result = await _repository.GetProductByIdAsync(_createdProduct!.Id.ToString());

        result.Should().NotBeNull();
        result!.Name.Should().Be("X-Salada");
    }

    [Then("all products must be returned")]
    public void ThenAllProductsMustBeReturned()
    {
        _resultListProducts.Should().NotBeNull();
        _resultListProducts.Should().HaveCount(2);

        _resultListProducts.Select(p => p.Name)
               .Should()
               .Contain(new[] { "X-Burger", "X-Salada" });
    }

    [Then(@"only products of type ""(.*)"" must be returned")]
    public void ThenOnlyProductsOfTypeMustBeReturned(string type)
    {
        _resultListProducts.Should().NotBeNull();
        _resultListProducts.Should().NotBeEmpty();

        _resultListProducts.Should().AllSatisfy(product =>
        {
            product.GetType().Name.Should().Be(type);
        });
    }
}
