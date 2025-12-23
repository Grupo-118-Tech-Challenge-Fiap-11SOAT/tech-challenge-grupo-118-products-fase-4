using Products.Application.Common.Models;
using Products.Application.Dtos;
using Products.Application.UseCases.Interfaces;
using Products.Domain.Entities;
using Products.Infra.DataBase.Repositories.Interfaces;
using System.Net;

namespace Products.Application.UseCases;

public class GetProductByTypeUseCase : IGetProductByTypeUseCase
{
    private readonly IProductRepository _productRepository;
    public GetProductByTypeUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<List<ProductDto>?>> ExecuteAsync(string type, CancellationToken cancellationToken = default)
    {
        Result<List<ProductDto>?> result = new();

        var persistedProducts = await _productRepository.GetProductByTypeAsync(type, cancellationToken);

        if (persistedProducts == null || !persistedProducts.Any())
            return result.Fail("Products not found", HttpStatusCode.NotFound);

        var products = new List<ProductDto>();

        foreach (var p in persistedProducts)
        {

            var images = new List<ImageProductDto>();
            foreach(var image in p.Images)
            {
                images.Add(new ImageProductDto() { Url = image.Url, Position = image.Position });
            }

            ProductDto dto = p switch
            {
                Snack s => new SnackDto() { Id = s.Id, Name = s.Name, Price = s.Price, IsActive = s.IsActive, Images = images, Ingredients = s.Ingredients },
                Accompaniment a => new AccompanimentDto() { Id = a.Id, Name = a.Name, Price = a.Price, IsActive = a.IsActive, Images = images, Size = a.Size },
                Dessert d => new DessertDto() { Id = d.Id, Name = d.Name, Price = d.Price, IsActive = d.IsActive, Images = images, PortionSize = d.PortionSize },
                Drink dr => new DrinkDto() { Id = dr.Id, Name = dr.Name, Price = dr.Price, IsActive = dr.IsActive, Images = images, Size = dr.Size, Flavor = dr.Flavor },
                _ => throw new NotSupportedException($"Unsupported product type: {p.GetType().Name}")
            };

            products.Add(dto);
        }

        return result.Ok(products, HttpStatusCode.OK);
    }
}
