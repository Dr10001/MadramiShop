using MadramiShop.Shared.Dtos;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadramiShop.Apis;

public interface IProductApi
{
    [Get("/api/products")]
    Task<ProductDto[]> GetProductsAsync();
}
