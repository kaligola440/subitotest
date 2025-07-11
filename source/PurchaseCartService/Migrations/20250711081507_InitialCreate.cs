using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PurchaseCartService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    VatRate = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "Price", "VatRate" },
                values: new object[,]
                {
                    { 1, 2.0, 0.10000000000000001 },
                    { 2, 1.5, 0.10000000000000001 },
                    { 3, 3.0, 0.10000000000000001 },
                    { 4, 15.9, 0.040000000000000001 },
                    { 5, 28.0, 0.040000000000000001 },
                    { 6, 399.99000000000001, 0.22 },
                    { 7, 899.0, 0.22 },
                    { 8, 24.949999999999999, 0.22 },
                    { 9, 49.899999999999999, 0.22 },
                    { 10, 59.990000000000002, 0.22 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
