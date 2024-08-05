using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReadMe.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class datas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "ProductId", "Author", "Description", "ISBN", "ListPrice", "Price", "Price100", "Price50", "Title" },
                values: new object[] { 1, "Prabin", "Hello", "56464", 20.0, 17.5, 16.0, 16.5, "Action" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "ProductId",
                keyValue: 1);
        }
    }
}
