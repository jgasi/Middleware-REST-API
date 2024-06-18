using Middleware_REST_API.Model;

namespace Middleware_REST_API.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProductById(int id);
        Task<IEnumerable<Product>> GetProductsByCategory(string category);
        Task<IEnumerable<Product>> GetProductsByPriceRange(decimal minPrice, decimal maxPrice);
        Task<IEnumerable<Product>> SearchProductsByName(string name);
    }
}
