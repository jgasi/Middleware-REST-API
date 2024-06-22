using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Middleware_REST_API.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "Description", "Images", "Price", "Title" },
                values: new object[,]
                {
                    { 1, "Category A", "Description for Product 1", "[\"image1.jpg\",\"image2.jpg\"]", 99.99m, "Product 1" },
                    { 2, "Category B", "Description for Product 2", "[\"image3.jpg\",\"image4.jpg\"]", 149.99m, "Product 2" },
                    { 3, "Category C", "Description for Product 3", "[\"image5.jpg\",\"image6.jpg\"]", 1.99m, "Product 3" },
                    { 4, "Category D", "Description for Product 4", "[\"image7.jpg\",\"image8.jpg\"]", 14.99m, "Product 4" },
                    { 5, "Category E", "Description for Product 5", "[\"image9.jpg\",\"image10.jpg\"]", 5.99m, "Product 5" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
