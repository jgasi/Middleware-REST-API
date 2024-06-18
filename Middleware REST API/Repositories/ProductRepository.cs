using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Middleware_REST_API.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Middleware_REST_API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ContextDb _context;
        private readonly HttpClient _httpClient;

        public ProductRepository(ContextDb context, HttpClient httpClient) 
        {
            _context = context;
            _httpClient = httpClient;
        }

        // Methods for possible future database operations
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



        // Methods for API operations
        public async Task<IEnumerable<Product>> GetAllProductsFromExternalApi()
        {
            var response = await _httpClient.GetAsync("https://dummyjson.com/products");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(json);
            var productsJson = jsonObject["products"].ToString();


            return JsonConvert.DeserializeObject<IEnumerable<Product>>(productsJson);
        }


        public async Task<Product> GetProductByIdFromExternalApi(int id)
        {
            var response = await _httpClient.GetAsync($"https://dummyjson.com/products/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Product with ID {id} not found.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<Product>(json);

            return product;
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryFromExternalApi(string category)
        {
            var response = await _httpClient.GetAsync($"https://dummyjson.com/products/category/{category}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(json);
            var productsJson = jsonObject["products"].ToString();

            return JsonConvert.DeserializeObject<IEnumerable<Product>>(productsJson);
        }

        public async Task<IEnumerable<Product>> GetProductsByPriceRangeFromExternalApi(decimal minPrice, decimal maxPrice)
        {
            var response = await _httpClient.GetAsync($"https://dummyjson.com/products");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(json);
            var productsJson = jsonObject["products"].ToString();
            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(productsJson);

            var filteredProducts = products.Where(p => p.Price >= minPrice && p.Price <= maxPrice);


            return filteredProducts;
        }

        public async Task<IEnumerable<Product>> SearchProductsByNameFromExternalApi(string name)
        {
            var response = await _httpClient.GetAsync($"https://dummyjson.com/products/search?q={name}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(json);
            var productsJson = jsonObject["products"].ToString();

            return JsonConvert.DeserializeObject<IEnumerable<Product>>(productsJson);
        }
    }
}
