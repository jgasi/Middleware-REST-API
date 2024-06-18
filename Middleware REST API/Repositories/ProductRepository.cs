using Microsoft.EntityFrameworkCore;
using Middleware_REST_API.Model;

namespace Middleware_REST_API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ContextDb _context;

        public ProductRepository(ContextDb context) 
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _context.Products.FindAsync(id) ?? throw new Exception("Product not found");
        }

        public async Task<IEnumerable<Product>> GetProductsByCategory(string category)
        {
            return await _context.Products.Where(p => p.Category == category).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            return await _context.Products.Where(p => p.Price >= minPrice && p.Price <= maxPrice).ToListAsync();
        }

        public async Task<IEnumerable<Product>> SearchProductsByName(string name)
        {
            return await _context.Products.Where(p => p.Name.Contains(name)).ToListAsync();
        }
    }
}
