using Products.Application.Common.Models;
using Products.Application.Dtos;

namespace Products.Application.UseCases.Interfaces;

public interface IGetActiveProductsByIdsUseCase
{
    Task<Result<List<ProductDto>>> ExecuteAsync(List<string> ids, CancellationToken cancellationToken = default);
}
