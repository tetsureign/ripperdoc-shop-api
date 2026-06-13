using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RipperdocShop.Api.Migrations
{
    /// <inheritdoc />
    public partial class FeaturedProductCheck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_featured",
                table: "products",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_featured",
                table: "products");
        }
    }
}
