using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RipperdocShop.Api.Migrations
{
    /// <inheritdoc />
    public partial class ProductRatingSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                table: "product_ratings",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "product_ratings");
        }
    }
}
