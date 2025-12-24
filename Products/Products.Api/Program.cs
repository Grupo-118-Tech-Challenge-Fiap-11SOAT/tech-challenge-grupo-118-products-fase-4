using Products.Application.UseCases;
using Products.Application.UseCases.Interfaces;
using Products.Infra.DataBase.Contexts;
using Products.Infra.DataBase.Repositories;
using Products.Infra.DataBase.Repositories.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.OpenApi.Models;

[assembly: ExcludeFromCodeCoverage]

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MongoDb
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddSingleton<MongoDbContext>();

builder.Services.AddScoped<IGetProductByIdUseCase, GetProductByIdUseCase>();
builder.Services.AddScoped<IGetProductByTypeUseCase, GetProductByTypeUseCase>();
builder.Services.AddScoped<ICreateProductUseCase, CreateProductUseCase>();
builder.Services.AddScoped<IGetProductsUseCase, GetProductsUseCase>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Tech Challenge - Fast Food API - Products - Fase 4",
        Version = "v1",
        Description =
            "API para gerenciamento de produtos para lanchonete",
        Contact = new OpenApiContact
        {
            Name = "Grupo 118 - Sabrina Cardoso | Tiago Koch | Tiago Oliveira | Túlio Rezende | Vinícius Nunes",
            Url = new Uri(
                "https://github.com/Grupo-118-Tech-Challenge-Fiap-11SOAT/tech-challenge-grupo-118-products-fase-4")
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(s =>
{
    s.SwaggerEndpoint("../swagger/v1/swagger.json", "Tech Challenge - Fast Food API - Products");
    s.RoutePrefix = string.Empty;
    s.DocumentTitle = "Tech Challenge - Fast Food API - Products - Fase 4 | Swagger";
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
