using Products.Application.Common.Models;
using Products.Application.Dtos;
using Products.Application.UseCases.Interfaces;
using Products.Domain.Entities;
using Products.Infra.DataBase.Repositories.Interfaces;
using System.Net;

namespace Products.Application.UseCases;

public class GetProductsUseCase : IGetProductsUseCase
{
    private readonly IProductRepository _productRepository;

    public GetProductsUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<List<ProductDto>>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        Result<List<ProductDto>> result = new();

        try
        {
            var products = await _productRepository.GetProductsAsync(cancellationToken);
            var response = new List<ProductDto>();

            foreach (var item in products)
            {
                var images = new List<ImageProductDto>();

                foreach (var image in item.Images) 
                {
                    images.Add(new ImageProductDto() { Position = image.Position, Url = image.Url });
                }


                ProductDto dto = item switch
                {
                    Snack s => new SnackDto() { Id = s.Id.ToString(), Name = s.Name, Price = s.Price, IsActive = s.IsActive, Images = images, Ingredients = s.Ingredients },
                    Accompaniment a => new AccompanimentDto() { Id = a.Id.ToString(), Name = a.Name, Price = a.Price, IsActive = a.IsActive, Images = images, Size = a.Size },
                    Dessert d => new DessertDto() { Id = d.Id.ToString(), Name = d.Name, Price = d.Price, IsActive = d.IsActive, Images = images, PortionSize = d.PortionSize },
                    Drink dr => new DrinkDto() { Id = dr.Id.ToString(), Name = dr.Name, Price = dr.Price, IsActive = dr.IsActive, Images = images, Size = dr.Size },
                    _ => throw new NotSupportedException("Unsupported product type")
                };

                response.Add(dto);
            }

            return result.Ok(response, HttpStatusCode.OK);
        }
        catch (Exception)
        {
            return result.Fail("Internal Error", HttpStatusCode.InternalServerError);
        }
    }
}
