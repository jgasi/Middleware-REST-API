using Middleware_REST_API.Model;
using Middleware_REST_API.Repositories;
using Middleware_REST_API.Services;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace Project_Tester.UnitTests
{
    [TestFixture]
    public class UnitTests_ProductService
    {
        private Mock<IProductRepository> _mockRepository;
        private IMemoryCache _memoryCache;
        private ProductService _productService;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<IProductRepository>();
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _productService = new ProductService(_mockRepository.Object, _memoryCache);
        }

        [TearDown]
        public void TearDown()
        {
            _memoryCache.Dispose();
        }

        [Test]
        public async Task GetAllProductsFromExternalApi_ReturnsProducts()
        {
            // Arrange
            var expectedProducts = new List<Product>
            {
                new Product { Id = 1, Title = "Product 1", Price = 10, Images = new List<string>(), Category = "Category 1", Description = "Description 1" },
                new Product { Id = 2, Title = "Product 2", Price = 20, Images = new List<string>(), Category = "Category 2", Description = "Description 2" }
            };

            _mockRepository.Setup(repo => repo.GetAllProductsFromExternalApi())
                           .ReturnsAsync(expectedProducts);

            // Act
            var result = await _productService.GetAllProductsFromExternalApi();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedProducts.Count, ((List<Product>)result).Count);
        }

        [Test]
        public async Task GetProductByIdFromExternalApi_ReturnsProduct()
        {
            // Arrange
            var productId = 1;
            var expectedProduct = new Product
            {
                Id = productId,
                Title = "Product 1",
                Price = 10,
                Images = new List<string> { "image1.jpg", "image2.jpg" },
                Category = "Category 1",
                Description = "Description 1"
            };

            _mockRepository.Setup(repo => repo.GetProductByIdFromExternalApi(productId))
                           .ReturnsAsync(expectedProduct);

            // Act
            var result = await _productService.GetProductByIdFromExternalApi(productId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedProduct.Id, result.Id);
            Assert.AreEqual(expectedProduct.Title, result.Title);
            Assert.AreEqual(expectedProduct.Price, result.Price);
            CollectionAssert.AreEqual(expectedProduct.Images, result.Images);
            Assert.AreEqual(expectedProduct.Category, result.Category);
            Assert.AreEqual(expectedProduct.Description, result.Description);

            // Check if product is cached
            Product cachedProduct;
            var cacheKey = $"Product-{productId}";
            var cacheResult = _memoryCache.TryGetValue(cacheKey, out cachedProduct);

            Assert.IsTrue(cacheResult);
            Assert.AreEqual(expectedProduct.Id, cachedProduct.Id);
            Assert.AreEqual(expectedProduct.Title, cachedProduct.Title);
            Assert.AreEqual(expectedProduct.Price, cachedProduct.Price);
            CollectionAssert.AreEqual(expectedProduct.Images, cachedProduct.Images);
            Assert.AreEqual(expectedProduct.Category, cachedProduct.Category);
            Assert.AreEqual(expectedProduct.Description, cachedProduct.Description);
        }



        [Test]
        public async Task GetProductsByCategoryAndPriceRangeFromExternalApi_FiltersProducts()
        {
            // Arrange
            var category = "Category 1";
            var minPrice = 1;
            var maxPrice = 20;
            var expectedProducts = new List<Product>
            {
                new Product { Id = 1, Title = "Product 1", Price = 10, Images = new List<string>(), Category = category, Description = "Description 1" },
                new Product { Id = 2, Title = "Product 2", Price = 25, Images = new List<string>(), Category = category, Description = "Description 2" }
            };

            var filteredProducts = new List<Product> { expectedProducts[0] };

            _mockRepository.Setup(repo => repo.GetProductsByCategoryAndPriceRangeFromExternalApi(category, minPrice, maxPrice))
                           .ReturnsAsync(filteredProducts);

            // Act
            var result = await _productService.GetProductsByCategoryAndPriceRangeFromExternalApi(category, minPrice, maxPrice);

            // Assert
            Assert.IsNotNull(result);
            var resultList = (List<Product>)result;
            Assert.AreEqual(filteredProducts.Count, resultList.Count);
            Assert.AreEqual(filteredProducts[0].Id, resultList[0].Id);
            Assert.AreEqual(filteredProducts[0].Title, resultList[0].Title);

            // Check if the filtered products are cached
            IEnumerable<Product> cachedProducts;
            var cacheKey = $"FilteredProducts-{category}-{minPrice}-{maxPrice}";
            var cacheResult = _memoryCache.TryGetValue(cacheKey, out cachedProducts);

            Assert.IsTrue(cacheResult);

            // Assert the count and contents of the cached products
            var cachedList = (List<Product>)cachedProducts;
            Assert.AreEqual(filteredProducts.Count, cachedList.Count);
            Assert.AreEqual(filteredProducts[0].Id, cachedList[0].Id);
            Assert.AreEqual(filteredProducts[0].Title, cachedList[0].Title);
        }

        [Test]
        public async Task GetProductsByCategoryFromExternalApi_FiltersProducts()
        {
            // Arrange
            var category = "Category 1";
            var expectedProducts = new List<Product>
            {
                new Product { Id = 1, Title = "Product 1", Price = 10, Images = new List<string>(), Category = category, Description = "Description 1" },
                new Product { Id = 2, Title = "Product 2", Price = 15, Images = new List<string>(), Category = category, Description = "Description 2" }
            };

            _mockRepository.Setup(repo => repo.GetProductsByCategoryFromExternalApi(category))
                           .ReturnsAsync(expectedProducts);

            // Act
            var result = await _productService.GetProductsByCategoryFromExternalApi(category);

            // Assert
            Assert.IsNotNull(result);
            var resultList = (List<Product>)result;
            Assert.AreEqual(expectedProducts.Count, resultList.Count);
            Assert.AreEqual(expectedProducts[0].Id, resultList[0].Id);
            Assert.AreEqual(expectedProducts[0].Title, resultList[0].Title);
            Assert.AreEqual(expectedProducts[1].Id, resultList[1].Id);
            Assert.AreEqual(expectedProducts[1].Title, resultList[1].Title);

            // Check if the filtered products are cached
            IEnumerable<Product> cachedProducts;
            var cacheKey = $"FilteredProducts-{category}";
            var cacheResult = _memoryCache.TryGetValue(cacheKey, out cachedProducts);

            Assert.IsTrue(cacheResult);
            var cachedList = (List<Product>)cachedProducts;
            Assert.AreEqual(expectedProducts.Count, cachedList.Count);
            Assert.AreEqual(expectedProducts[0].Id, cachedList[0].Id);
            Assert.AreEqual(expectedProducts[0].Title, cachedList[0].Title);
            Assert.AreEqual(expectedProducts[1].Id, cachedList[1].Id);
            Assert.AreEqual(expectedProducts[1].Title, cachedList[1].Title);
        }


        [Test]
        public async Task GetProductsByPriceRangeFromExternalApi_FiltersProducts()
        {
            // Arrange
            var minPrice = 10;
            var maxPrice = 20;
            var expectedProducts = new List<Product>
            {
                new Product { Id = 1, Title = "Product 1", Price = 15, Images = new List<string>(), Category = "Category 1", Description = "Description 1" },
                new Product { Id = 2, Title = "Product 2", Price = 18, Images = new List<string>(), Category = "Category 2", Description = "Description 2" }
            };

            _mockRepository.Setup(repo => repo.GetProductsByPriceRangeFromExternalApi(minPrice, maxPrice))
                           .ReturnsAsync(expectedProducts);

            // Act
            var result = await _productService.GetProductsByPriceRangeFromExternalApi(minPrice, maxPrice);

            // Assert
            Assert.IsNotNull(result);
            var resultList = (List<Product>)result;
            Assert.AreEqual(expectedProducts.Count, resultList.Count);
            Assert.AreEqual(expectedProducts[0].Id, resultList[0].Id);
            Assert.AreEqual(expectedProducts[0].Title, resultList[0].Title);
            Assert.AreEqual(expectedProducts[1].Id, resultList[1].Id);
            Assert.AreEqual(expectedProducts[1].Title, resultList[1].Title);

            // Check if the filtered products are cached
            IEnumerable<Product> cachedProducts;
            var cacheKey = $"FilteredProducts-{minPrice}-{maxPrice}";
            var cacheResult = _memoryCache.TryGetValue(cacheKey, out cachedProducts);

            Assert.IsTrue(cacheResult);
            var cachedList = (List<Product>)cachedProducts;
            Assert.AreEqual(expectedProducts.Count, cachedList.Count);
            Assert.AreEqual(expectedProducts[0].Id, cachedList[0].Id);
            Assert.AreEqual(expectedProducts[0].Title, cachedList[0].Title);
            Assert.AreEqual(expectedProducts[1].Id, cachedList[1].Id);
            Assert.AreEqual(expectedProducts[1].Title, cachedList[1].Title);
        }


        [Test]
        public async Task SearchProductsByNameFromExternalApi_SearchesProducts()
        {
            // Arrange
            var productName = "Product";
            var expectedProducts = new List<Product>
            {
                new Product { Id = 1, Title = "Product 1", Price = 15, Images = new List<string>(), Category = "Category 1", Description = "Description 1" },
                new Product { Id = 2, Title = "Product 2", Price = 18, Images = new List<string>(), Category = "Category 2", Description = "Description 2" }
            };

            _mockRepository.Setup(repo => repo.SearchProductsByNameFromExternalApi(productName))
                           .ReturnsAsync(expectedProducts);

            // Act
            var result = await _productService.SearchProductsByNameFromExternalApi(productName);

            // Assert
            Assert.IsNotNull(result);
            var resultList = (List<Product>)result;
            Assert.AreEqual(expectedProducts.Count, resultList.Count);
            Assert.AreEqual(expectedProducts[0].Id, resultList[0].Id);
            Assert.AreEqual(expectedProducts[0].Title, resultList[0].Title);
            Assert.AreEqual(expectedProducts[1].Id, resultList[1].Id);
            Assert.AreEqual(expectedProducts[1].Title, resultList[1].Title);

            // Check if the searched products are cached
            IEnumerable<Product> cachedProducts;
            var cacheKey = $"FilteredProducts-{productName}";
            var cacheResult = _memoryCache.TryGetValue(cacheKey, out cachedProducts);

            Assert.IsTrue(cacheResult);
            var cachedList = (List<Product>)cachedProducts;
            Assert.AreEqual(expectedProducts.Count, cachedList.Count);
            Assert.AreEqual(expectedProducts[0].Id, cachedList[0].Id);
            Assert.AreEqual(expectedProducts[0].Title, cachedList[0].Title);
            Assert.AreEqual(expectedProducts[1].Id, cachedList[1].Id);
            Assert.AreEqual(expectedProducts[1].Title, cachedList[1].Title);
        }
    }
}
