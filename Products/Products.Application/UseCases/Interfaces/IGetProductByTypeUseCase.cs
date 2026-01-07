using Products.Application.Common.Models;
using Products.Application.Dtos;

namespace Products.Application.UseCases.Interfaces;

public interface IGetProductByTypeUseCase
{
    Task<Result<List<ProductDto>?>> ExecuteAsync(string type, CancellationToken cancellationToken = default);
}
