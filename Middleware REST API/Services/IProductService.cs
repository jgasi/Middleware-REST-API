using Middleware_REST_API.Model;

namespace Middleware_REST_API.Services
{
    public interface IProductService
    {
        // Methods for API operations
        Task<IEnumerable<Product>> GetAllProductsFromExternalApi();
        Task<Product> GetProductByIdFromExternalApi(int id);
        Task<IEnumerable<Product>> GetProductsByCategoryAndPriceRangeFromExternalApi(string category, decimal minPrice, decimal maxPrice);
        Task<IEnumerable<Product>> GetProductsByCategoryFromExternalApi(string category);
        Task<IEnumerable<Product>> GetProductsByPriceRangeFromExternalApi(decimal minPrice, decimal maxPrice);
        Task<IEnumerable<Product>> SearchProductsByNameFromExternalApi(string name);


        // Methods for possible future database operations
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProductById(int id);
        Task<IEnumerable<Product>> GetProductsByCategoryAndPriceRange(string category, decimal minPrice, decimal maxPrice);
        Task<IEnumerable<Product>> GetProductsByCategory(string category);
        Task<IEnumerable<Product>> GetProductsByPriceRange(decimal minPrice, decimal maxPrice);
        Task<IEnumerable<Product>> SearchProductsByName(string name);
    }
}
