using MadramiShop.Api.Data;
using MadramiShop.Shared.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Runtime.InteropServices;

namespace MadramiShop.Api.Services
{
    public class ProductService
    {
        private readonly DataContext _context;

        public ProductService(DataContext context)
        {
            _context = context;
        }

        public async Task<ProductDto[]> GetProductsAsync() =>
            await _context.Products
            .AsNoTracking()
            .Select(p=> new ProductDto
            {
                Id = p.Id,
                ImageUrl = p.ImageUrl,
                Name = p.Name,
                Price = p.Price,
                Unit = p.Unit
            }).ToArrayAsync();
    }
}
