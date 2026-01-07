using Products.Application.Common.Models;
using Products.Application.Dtos;

namespace Products.Application.UseCases.Interfaces;

public interface IGetProductsUseCase
{
    Task<Result<List<ProductDto>>> ExecuteAsync(CancellationToken cancellationToken = default);
}
