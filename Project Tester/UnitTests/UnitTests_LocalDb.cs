using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Middleware_REST_API.Exceptions;
using Middleware_REST_API.Model;
using Middleware_REST_API.Repositories;
using Middleware_REST_API.Services;
using Moq;
using NUnit.Framework;

namespace Project_Tester.UnitTests
{
    public class UnitTests_LocalDb
    {
        private ContextDb _context;
        private ProductRepository _productRepository;
        private Mock<ILogger<ProductService>> _loggerMock;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ContextDb>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ContextDb(options);
            _loggerMock = new Mock<ILogger<ProductService>>();

            _productRepository = new ProductRepository(_context, null, _loggerMock.Object);
        }

        private void SeedDatabase()
        {
            var products = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Title = "Product 1",
                    Price = 99.99m,
                    Description = "Description 1",
                    Category = "Category 1",
                    Images = new List<string> { "image1.jpg", "image2.jpg" }
                },
                new Product
                {
                    Id = 2,
                    Title = "Product 2",
                    Price = 149.99m,
                    Description = "Description 2",
                    Category = "Category 2",
                    Images = new List<string> { "image3.jpg" }
                }
            };

            _context.Products.AddRange(products);
            _context.SaveChanges();
        }

        private void ClearDatabase()
        {
            _context.Products.RemoveRange(_context.Products);
            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetAllProducts_ReturnsProducts_WhenProductsExist()
        {
            // Arrange
            ClearDatabase();
            SeedDatabase();

            // Act
            var result = await _productRepository.GetAllProducts();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());

            var product1 = result.FirstOrDefault(p => p.Id == 1);
            Assert.IsNotNull(product1);
            Assert.AreEqual("Product 1", product1.Title);
            Assert.AreEqual(99.99m, product1.Price);
            Assert.AreEqual("Description 1", product1.Description);
            Assert.AreEqual("Category 1", product1.Category);
            CollectionAssert.AreEqual(new List<string> { "image1.jpg", "image2.jpg" }, product1.Images);

            var product2 = result.FirstOrDefault(p => p.Id == 2);
            Assert.IsNotNull(product2);
            Assert.AreEqual("Product 2", product2.Title);
            Assert.AreEqual(149.99m, product2.Price);
            Assert.AreEqual("Description 2", product2.Description);
            Assert.AreEqual("Category 2", product2.Category);
            CollectionAssert.AreEqual(new List<string> { "image3.jpg" }, product2.Images);
        }

        [Test]
        public void GetAllProducts_ThrowsProductNotFoundException_WhenNoProductsExist()
        {
            // Arrange
            ClearDatabase();

            // Act
            var ex = Assert.ThrowsAsync<ProductNotFoundException>(async () => await _productRepository.GetAllProducts());

            // Assert
            Assert.AreEqual("Products not found.", ex.Message);
        }

        [Test]
        public async Task GetProductById_ReturnsProduct_WhenProductExists()
        {
            // Arrange
            ClearDatabase();
            SeedDatabase();
            int existingProductId = 1;

            // Act
            var result = await _productRepository.GetProductById(existingProductId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(existingProductId, result.Id);
        }

        [Test]
        public void GetProductById_ThrowsProductNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            ClearDatabase();
            int nonExistingProductId = 1;

            // Act
            var ex = Assert.ThrowsAsync<ProductNotFoundException>(async () => await _productRepository.GetProductById(nonExistingProductId));

            // Assert
            Assert.AreEqual($"Product with ID {nonExistingProductId} not found", ex.Message);
        }

        [Test]
        public async Task GetProductsByCategoryAndPriceRange_ReturnsProducts_WhenProductsExist()
        {
            // Arrange
            ClearDatabase();
            SeedDatabase();
            string category = "Category 1";
            decimal minPrice = 50.0m;
            decimal maxPrice = 100.0m;

            // Act
            var result = await _productRepository.GetProductsByCategoryAndPriceRange(category, minPrice, maxPrice);

            // Assert
            Assert.IsNotEmpty(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Product 1", result.First().Title);
        }

        [Test]
        public void GetProductsByCategoryAndPriceRange_ThrowsProductNotFoundException_WhenNoProductsExist()
        {
            // Arrange
            ClearDatabase();
            string category = "Non-existent Category";
            decimal minPrice = 0.0m;
            decimal maxPrice = 100.0m;

            // Act & Assert
            Assert.ThrowsAsync<ProductNotFoundException>(async () => await _productRepository.GetProductsByCategoryAndPriceRange(category, minPrice, maxPrice));
        }

        [Test]
        public async Task GetProductsByCategory_ReturnsProducts_WhenProductsExist()
        {
            // Arrange
            ClearDatabase();
            SeedDatabase();
            string category = "Category 1";

            // Act
            var result = await _productRepository.GetProductsByCategory(category);

            // Assert
            Assert.IsNotEmpty(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Product 1", result.First().Title);
        }

        [Test]
        public void GetProductsByCategory_ThrowsProductNotFoundException_WhenNoProductsExist()
        {
            // Arrange
            ClearDatabase();
            string category = "Non-existent Category";

            // Act & Assert
            Assert.ThrowsAsync<ProductNotFoundException>(async () => await _productRepository.GetProductsByCategory(category));
        }

        [Test]
        public async Task GetProductsByPriceRange_ReturnsProducts_WhenProductsExist()
        {
            // Arrange
            ClearDatabase();
            SeedDatabase();
            decimal minPrice = 100m;
            decimal maxPrice = 200m;

            // Act
            var result = await _productRepository.GetProductsByPriceRange(minPrice, maxPrice);

            // Assert
            Assert.IsNotEmpty(result);
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.All(p => p.Price >= minPrice && p.Price <= maxPrice));
        }

        [Test]
        public void GetProductsByPriceRange_ThrowsProductNotFoundException_WhenNoProductsExist()
        {
            // Arrange
            ClearDatabase();
            decimal minPrice = 500m;
            decimal maxPrice = 600m;

            // Act & Assert
            Assert.ThrowsAsync<ProductNotFoundException>(async () => await _productRepository.GetProductsByPriceRange(minPrice, maxPrice));
        }

        [Test]
        public async Task SearchProductsByName_ReturnsProducts_WhenProductsExist()
        {
            // Arrange
            ClearDatabase();
            SeedDatabase();
            string productName = "Product";

            // Act
            var result = await _productRepository.SearchProductsByName(productName);

            // Assert
            Assert.IsNotEmpty(result);
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(p => p.Title.Contains(productName)));
        }

        [Test]
        public void SearchProductsByName_ThrowsProductNotFoundException_WhenNoProductsExist()
        {
            // Arrange
            ClearDatabase();
            string productName = "Non-existent Product";

            // Act & Assert
            Assert.ThrowsAsync<ProductNotFoundException>(async () => await _productRepository.SearchProductsByName(productName));
        }
    }
}
