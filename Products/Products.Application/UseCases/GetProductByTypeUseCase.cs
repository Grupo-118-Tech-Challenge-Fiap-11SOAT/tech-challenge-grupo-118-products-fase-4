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
            ProductDto dto = p switch
            {
                Snack s => new SnackDto(s.Id, s.Name, s.Price, s.IsActive, s.Images, s.Ingredients),
                Accompaniment a => new AccompanimentDto(a.Id, a.Name, a.Price, a.IsActive, a.Images, a.Size),
                Dessert d => new DessertDto(d.Id, d.Name, d.Price, d.IsActive, d.Images, d.PortionSize),
                Drink dr => new DrinkDto(dr.Id, dr.Name, dr.Price, dr.IsActive, dr.Images, dr.Size, dr.Flavor),
                _ => throw new NotSupportedException($"Unsupported product type: {p.GetType().Name}")
            };

            products.Add(dto);
        }

        return result.Ok(products, HttpStatusCode.OK);
    }
}
