using Accounting.Server.Models;
using Accounting.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Accounting.Server.Models.DTOs.ProductDTOs;

namespace Accounting.Server.Services.Repositories
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductResponseDTO> CreateProduct(CreateProductDTO productDto)
        {
            var p = new Products
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Category = productDto.Category,
                Price = productDto.Price
            };

            _context.Products.Add(p);
            await _context.SaveChangesAsync();

            return new ProductResponseDTO(p.Id,
                    p.Name,
                    p.Description,
                    p.Category,
                    p.Price,
                    p.CreatedAt,
                    p.UpdatedAt);
        }

        public async Task DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                throw new Exception("Product not found");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ProductResponseDTO>> GetAllProducts()
        {
            return await _context.Products.Select(p => new ProductResponseDTO(
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Category,
                    p.Price,
                    p.CreatedAt,
                    p.UpdatedAt))
                .ToListAsync();
        }

        public async Task<ProductResponseDTO> GetProductById(int id)
        {
            var p = await _context.Products.FindAsync(id);
            if (p == null)
            {
                throw new Exception("Product not found");
            }

            return new ProductResponseDTO(p.Id,
                    p.Name,
                    p.Description,
                    p.Category,
                    p.Price,
                    p.CreatedAt,
                    p.UpdatedAt);
        }

        public async Task<ProductResponseDTO> UpdateProduct(UpdateProductDTO productDto)
        {
            var p = await _context.Products.FindAsync(productDto.Id);
            if (p == null)
            {
                throw new Exception("Product not found");
            }

            p.Name = productDto.Name;
            p.Description = productDto.Description;
            p.Category = productDto.Category;
            p.Price = productDto.Price;
            p.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new ProductResponseDTO(p.Id,
                    p.Name,
                    p.Description,
                    p.Category,
                    p.Price,
                    p.CreatedAt,
                    p.UpdatedAt);
        }
    }
}
