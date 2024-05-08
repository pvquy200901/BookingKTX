using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingKTX.Migrations
{
    /// <inheritdoc />
    public partial class _107 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_cart_tb_customer_customerID",
                table: "tb_cart");

            migrationBuilder.DropIndex(
                name: "IX_tb_cart_customerID",
                table: "tb_cart");

            migrationBuilder.DropColumn(
                name: "customerID",
                table: "tb_cart");

            migrationBuilder.AddColumn<long>(
                name: "cartID",
                table: "tb_customer",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_customer_cartID",
                table: "tb_customer",
                column: "cartID");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_customer_tb_cart_cartID",
                table: "tb_customer",
                column: "cartID",
                principalTable: "tb_cart",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_customer_tb_cart_cartID",
                table: "tb_customer");

            migrationBuilder.DropIndex(
                name: "IX_tb_customer_cartID",
                table: "tb_customer");

            migrationBuilder.DropColumn(
                name: "cartID",
                table: "tb_customer");

            migrationBuilder.AddColumn<long>(
                name: "customerID",
                table: "tb_cart",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_cart_customerID",
                table: "tb_cart",
                column: "customerID");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_cart_tb_customer_customerID",
                table: "tb_cart",
                column: "customerID",
                principalTable: "tb_customer",
                principalColumn: "ID");
        }
    }
}
