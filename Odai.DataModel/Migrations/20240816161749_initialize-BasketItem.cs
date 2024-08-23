using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Odai.DataModel.Migrations
{
    /// <inheritdoc />
    public partial class initializeBasketItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "BasketItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_BasketItems_UserId",
                table: "BasketItems",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItems_AspNetUsers_UserId",
                table: "BasketItems",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketItems_AspNetUsers_UserId",
                table: "BasketItems");

            migrationBuilder.DropIndex(
                name: "IX_BasketItems_UserId",
                table: "BasketItems");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BasketItems");
        }
    }
}
