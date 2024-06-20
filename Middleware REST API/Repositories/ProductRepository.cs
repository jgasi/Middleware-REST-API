using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Middleware_REST_API.Exceptions;
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


        // Methods for API operations
        public async Task<IEnumerable<Product>> GetAllProductsFromExternalApi()
        {
            var response = await _httpClient.GetAsync("https://dummyjson.com/products");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(json);
            var productsJson = jsonObject["products"].ToString();

            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(productsJson);

            foreach (var product in products)
            {
                if (product.Description != null && product.Description.Length > 100)
                {
                    product.Description = product.Description.Substring(0, 100);
                }
            }

            return products;
        }


        public async Task<Product> GetProductByIdFromExternalApi(int id)
        {
            var response = await _httpClient.GetAsync($"https://dummyjson.com/products/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new ProductNotFoundException($"Product with ID {id} not found");
            }

            var json = await response.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<Product>(json);

            if (product.Description != null && product.Description.Length > 100)
            {
                product.Description = product.Description.Substring(0, 100);
            }

            return product;
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAndPriceRangeFromExternalApi(string category, decimal minPrice, decimal maxPrice)
        {
            var response = await _httpClient.GetAsync($"https://dummyjson.com/products/category/{category}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(json);
            var productsJson = jsonObject["products"].ToString();
            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(productsJson);

            var filteredProducts = products.Where(p => p.Category == category && p.Price >= minPrice && p.Price <= maxPrice);

            foreach (var product in filteredProducts)
            {
                if (product.Description != null && product.Description.Length > 100)
                {
                    product.Description = product.Description.Substring(0, 100);
                }
            }

            return filteredProducts;
        }


        public async Task<IEnumerable<Product>> GetProductsByCategoryFromExternalApi(string category)
        {
            var response = await _httpClient.GetAsync($"https://dummyjson.com/products/category/{category}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(json);
            var productsJson = jsonObject["products"].ToString();

            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(productsJson);

            foreach (var product in products)
            {
                if (product.Description != null && product.Description.Length > 100)
                {
                    product.Description = product.Description.Substring(0, 100);
                }
            }
            var filteredProducts = products.Where(p => p.Category == category);


            return filteredProducts;
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

            foreach (var product in filteredProducts)
            {
                if (product.Description != null && product.Description.Length > 100)
                {
                    product.Description = product.Description.Substring(0, 100);
                }
            }

            return filteredProducts;
        }

        public async Task<IEnumerable<Product>> SearchProductsByNameFromExternalApi(string name)
        {
            var response = await _httpClient.GetAsync($"https://dummyjson.com/products/search?q={name}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(json);
            var productsJson = jsonObject["products"].ToString();

            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(productsJson);

            foreach (var product in products)
            {
                if (product.Description != null && product.Description.Length > 100)
                {
                    product.Description = product.Description.Substring(0, 100);
                }
            }
            var filteredProducts = products.Where(p => p.Title.Contains(name, StringComparison.OrdinalIgnoreCase));


            return filteredProducts;
        }



        // Methods for possible future database operations
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductById(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                throw new ProductNotFoundException($"Product with ID {id} not found");
            }
            return product;
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAndPriceRange(string category, decimal minPrice, decimal maxPrice)
        {
            return await _context.Products.Where(p => p.Category == category && p.Price >= minPrice && p.Price <= maxPrice).ToListAsync();
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
            return await _context.Products.Where(p => p.Title.Contains(name)).ToListAsync();
        }
    }
}
