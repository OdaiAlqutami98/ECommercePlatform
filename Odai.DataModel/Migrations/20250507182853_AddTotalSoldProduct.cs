using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Odai.DataModel.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalSoldProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalSold",
                table: "Product",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalSold",
                table: "Product");
        }
    }
}
