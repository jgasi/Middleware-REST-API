using System.Net;
using Middleware_REST_API.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Project_Tester.IntegrationTests
{
    [TestFixture]
    public class IntegrationTests
    {
        private HttpClient _httpClient;

        [SetUp]
        public void Setup()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://dummyjson.com");
        }

        [Test]
        public async Task GetAllProductsVerifyResponseOfEndpoint_ReturnsProducts()
        {
            // Act
            var response = await _httpClient.GetAsync("/products");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(responseBody);
            var productsArray = jsonObject["products"].ToObject<JArray>();

            Assert.IsTrue(productsArray.Count > 0);
            var firstProduct = productsArray[0].ToObject<Product>();
            Assert.IsNotNull(firstProduct);
            Assert.IsFalse(string.IsNullOrEmpty(firstProduct.Title));
            Assert.AreEqual("Essence Mascara Lash Princess", firstProduct.Title);
            Assert.AreEqual(9.99, firstProduct.Price);
        }

        [Test]
        public async Task GetProductByIdVerifyResponseOfEndpoint_ReturnsSingleProduct()
        {
            //Arrange
            int productId = 1;

            // Act
            var response = await _httpClient.GetAsync($"/products/{productId}");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();
            var product = JObject.Parse(responseBody).ToObject<Product>();

            Assert.IsNotNull(product);
            Assert.IsFalse(string.IsNullOrEmpty(product.Title));
            Assert.AreEqual("Essence Mascara Lash Princess", product.Title);
            Assert.AreEqual(productId, product.Id);
            Assert.AreEqual(9.99, product.Price);
        }

        [Test]
        public async Task GetProductsByCategoryAndPriceRangeVerifyResponseOfEndpoint_ReturnsFilteredProducts()
        {
            // Arrange
            string category = "beauty";
            decimal minPrice = 1;
            decimal maxPrice = 20;

            // Act
            var response = await _httpClient.GetAsync($"/products/category/{category}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(json);
            var productsJson = jsonObject["products"].ToString();
            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(productsJson);

            var filteredProducts = products.Where(p => p.Category == category && p.Price >= minPrice && p.Price <= maxPrice);

            // Assert
            Assert.IsTrue(filteredProducts.Any());

            foreach (var product in filteredProducts)
            {
                Assert.AreEqual(category, product.Category);
                Assert.IsTrue(product.Price >= minPrice && product.Price <= maxPrice);
                Assert.IsFalse(string.IsNullOrEmpty(product.Title));
            }
        }

        [Test]
        public async Task GetProductsByCategoryVerifyResponseOfEndpoint_ReturnsProducts()
        {
            // Arrange
            string category = "beauty";

            // Act
            var response = await _httpClient.GetAsync($"/products/category/{category}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(json);
            var productsJson = jsonObject["products"].ToString();
            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(productsJson);

            // Assert
            Assert.IsTrue(products.Any());

            foreach (var product in products)
            {
                Assert.AreEqual(category, product.Category);
                Assert.IsFalse(string.IsNullOrEmpty(product.Title));
            }
        }

        [Test]
        public async Task GetProductsByPriceRangeVerifyResponseOfEndpoint_ReturnsFilteredProducts()
        {
            // Arrange
            decimal minPrice = 10;
            decimal maxPrice = 50;

            // Act
            var response = await _httpClient.GetAsync("https://dummyjson.com/products");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(json);
            var productsJson = jsonObject["products"].ToString();
            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(productsJson);

            var filteredProducts = products.Where(p => p.Price >= minPrice && p.Price <= maxPrice);

            // Assert
            Assert.IsTrue(filteredProducts.Any());

            foreach (var product in filteredProducts)
            {
                Assert.IsTrue(product.Price >= minPrice && product.Price <= maxPrice);
                Assert.IsFalse(string.IsNullOrEmpty(product.Title));
            }
        }

        [Test]
        public async Task SearchProductsByNameVerifyResponseOfEndpoint_ReturnsFilteredProducts()
        {
            // Arrange
            string productName = "Mascara";

            // Act
            var response = await _httpClient.GetAsync($"https://dummyjson.com/products/search?q={productName}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(json);
            var productsJson = jsonObject["products"].ToString();
            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(productsJson);

            // Assert
            Assert.IsNotNull(products);
            Assert.IsTrue(products.Any());

            foreach (var product in products)
            {
                Assert.IsTrue(product.Title.Contains(productName, StringComparison.OrdinalIgnoreCase));
                Assert.IsFalse(string.IsNullOrEmpty(product.Title));
            }
        }



        [TearDown]
        public void TearDown()
        {
            _httpClient.Dispose();
        }
    }
}
