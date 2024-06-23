using Microsoft.EntityFrameworkCore;
using Middleware_REST_API.Model;
using System.Collections.Generic;

namespace Middleware_REST_API.Model
{
    public class ContextDb : DbContext
    {
        public ContextDb(DbContextOptions<ContextDb> options) : base(options)
        {
            // Ensure the database is created if it doesn't exist
            Database.EnsureCreated();
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Title = "Product 1", Price = 99.99m, Description = "Description for Product 1", Images = new List<string> { "image1.jpg", "image2.jpg" }, Category = "Category A" },
                new Product { Id = 2, Title = "Product 2", Price = 149.99m, Description = "Description for Product 2", Images = new List<string> { "image3.jpg", "image4.jpg" }, Category = "Category B" },
                new Product { Id = 3, Title = "Product 3", Price = 1.99m, Description = "Description for Product 3", Images = new List<string> { "image5.jpg", "image6.jpg" }, Category = "Category C" },
                new Product { Id = 4, Title = "Product 4", Price = 14.99m, Description = "Description for Product 4", Images = new List<string> { "image7.jpg", "image8.jpg" }, Category = "Category D" },
                new Product { Id = 5, Title = "Product 5", Price = 5.99m, Description = "Description for Product 5", Images = new List<string> { "image9.jpg", "image10.jpg" }, Category = "Category E" }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
