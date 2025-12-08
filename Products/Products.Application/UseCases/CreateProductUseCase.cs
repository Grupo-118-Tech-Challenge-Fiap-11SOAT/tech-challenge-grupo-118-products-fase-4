using Products.Application.Common.Models;
using Products.Application.Dtos;
using Products.Application.UseCases.Interfaces;
using Products.Domain.Entities;
using Products.Infra.DataBase.Repositories.Interfaces;
using System.Net;
namespace Products.Application.UseCases;

public class CreateProductUseCase : ICreateProductUseCase
{
    private readonly IProductRepository _productRepository;

    public CreateProductUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<ProductDto>> ExecuteAsync(ProductDto productDto, CancellationToken cancellationToken)
    {
        Result<ProductDto> result = new Result<ProductDto>();

        List<ImageProduct> images = new List<ImageProduct>();

        foreach (var item in productDto.Images)
        {
            images.Add(new ImageProduct(item.Position, item.Url));
        }

        try
        {      
            Product product = productDto switch
            {
                SnackDto s => new Snack(s.Name, s.Price, s.IsActive, s.Ingredients, images),
                AccompanimentDto a => new Accompaniment(a.Name, a.Price, a.IsActive, a.Size, images),
                DessertDto d => new Dessert(d.Name, d.Price, d.IsActive, d.PortionSize, images),
                DrinkDto d => new Drink(d.Name, d.Price, d.IsActive, d.Size, d.Flavor, images),
                _ => throw new NotSupportedException("unsuported type")
            };

            await _productRepository.CreateProductAsync(product);

            return result.Ok(productDto, HttpStatusCode.Created);
        }
        catch (Exception ex)
        {
            return result.Fail("Internal Error: " + ex.Message, HttpStatusCode.InternalServerError);
        }
        
    }
}
