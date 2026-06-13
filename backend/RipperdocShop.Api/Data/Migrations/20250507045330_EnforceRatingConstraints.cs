using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RipperdocShop.Api.Migrations
{
    /// <inheritdoc />
    public partial class EnforceRatingConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_product_ratings_product_id",
                table: "product_ratings");

            migrationBuilder.AlterColumn<string>(
                name: "image_url",
                table: "products",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "ix_product_ratings_product_id_user_id",
                table: "product_ratings",
                columns: new[] { "product_id", "user_id" },
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_ProductRatings_Score_Range",
                table: "product_ratings",
                sql: "score >= 1 AND score <= 5");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_product_ratings_product_id_user_id",
                table: "product_ratings");

            migrationBuilder.DropCheckConstraint(
                name: "CK_ProductRatings_Score_Range",
                table: "product_ratings");

            migrationBuilder.AlterColumn<string>(
                name: "image_url",
                table: "products",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2048)",
                oldMaxLength: 2048);

            migrationBuilder.CreateIndex(
                name: "ix_product_ratings_product_id",
                table: "product_ratings",
                column: "product_id");
        }
    }
}
