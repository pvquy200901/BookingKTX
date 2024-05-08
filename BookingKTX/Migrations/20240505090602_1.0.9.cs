using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingKTX.Migrations
{
    /// <inheritdoc />
    public partial class _109 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "price",
                table: "tb_product",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "priceBuy",
                table: "tb_product",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "quantity",
                table: "tb_product",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "price",
                table: "tb_product");

            migrationBuilder.DropColumn(
                name: "priceBuy",
                table: "tb_product");

            migrationBuilder.DropColumn(
                name: "quantity",
                table: "tb_product");
        }
    }
}
