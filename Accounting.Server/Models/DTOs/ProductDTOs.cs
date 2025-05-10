namespace Accounting.Server.Models.DTOs
{
    public class ProductDTOs
    {
        public record ProductResponseDTO(int Id, string Name, string Description, string Category, decimal Price, DateTime CreatedAt, DateTime? UpdatedAt);
        public record CreateProductDTO(string Name, string Description, string Category, decimal Price);
        public record UpdateProductDTO(int Id, string Name, string Description, string Category, decimal Price);
        public record DeleteProductDTO(int Id);
    }
}
