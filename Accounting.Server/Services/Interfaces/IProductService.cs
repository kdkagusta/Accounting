using static Accounting.Server.Models.DTOs.ProductDTOs;

namespace Accounting.Server.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductResponseDTO>> GetAllProducts();
        Task<ProductResponseDTO> GetProductById(int id);
        Task<ProductResponseDTO> CreateProduct(CreateProductDTO productDto);
        Task<ProductResponseDTO> UpdateProduct(UpdateProductDTO productDto);
        Task DeleteProduct(int id);
    }
}
