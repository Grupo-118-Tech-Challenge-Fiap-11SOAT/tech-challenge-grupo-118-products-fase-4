using Products.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.Infra.DataBase.Repositories.Interfaces;

public interface IProductRepository
{
    Task<Product> GetProductByIdAsync(string id, CancellationToken cancellationToken = default);
}
