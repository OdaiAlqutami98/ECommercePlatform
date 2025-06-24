using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Odai.DataModel.Migrations
{
    /// <inheritdoc />
    public partial class AddMenu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("SET IDENTITY_INSERT [dbo].[Menu] ON");

            migrationBuilder.Sql(
            @"INSERT INTO [dbo].[Menu] (ID, NameKey, [Level], [Order], Url, Icon, ParentId) 
      VALUES 
        (1, 'Dashboard', 1, 1, '/layout/home', 'home-outline', NULL),
        (2, 'User Management', 1, 1, '', 'home-outline', NULL),
        (3, 'Product', 2, 2, '/admin/product', 'home-outline', 2),
        (4, 'Category', 2, 3, '/admin/category', 'home-outline', 2),
        (5, 'User Management', 2, 1, '/admin/user', 'home-outline', 2)"
            );

            migrationBuilder.Sql("SET IDENTITY_INSERT [dbo].[Menu] OFF");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
