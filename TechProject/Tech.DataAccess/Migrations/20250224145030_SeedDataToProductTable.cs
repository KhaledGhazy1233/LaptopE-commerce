using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Tech.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataToProductTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Brand", "CategoryId", "Description", "GPU", "ImageUrl", "Name", "Price", "Processor", "RAM", "Resolution", "ScreenSize", "Storage" },
                values: new object[,]
                {
                    { 1, "Dell", 1, "A reliable laptop for everyday use.", "NVIDIA GTX 1650", "", "Dell Inspiron 15", 799.99m, "Intel i7", 16, "1920x1080", 15.6m, "512GB SSD" },
                    { 2, "HP", 2, "Gaming laptop with great performance.", "NVIDIA GTX 1650", "", "HP Pavilion Gaming", 999.99m, "Intel i5", 8, "1920x1080", 15.6m, "1TB HDD" },
                    { 3, "Apple", 1, "Thin, light, and powerful MacBook.", "Integrated", "", "MacBook Air 13", 999.99m, "M1 Chip", 8, "2560x1600", 13.3m, "256GB SSD" },
                    { 4, "Lenovo", 1, "2-in-1 laptop with a sleek design.", "Intel UHD Graphics 620", "", "Lenovo Yoga 730", 1099.99m, "Intel i7", 16, "1920x1080", 13.3m, "512GB SSD" },
                    { 5, "Corsair", 3, "High-precision gaming mouse.", null, "", "Corsair Gaming Mouse", 49.99m, null, 0, null, 0m, null }
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
