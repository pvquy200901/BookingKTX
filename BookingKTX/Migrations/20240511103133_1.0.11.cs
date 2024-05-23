using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingKTX.Migrations
{
    /// <inheritdoc />
    public partial class _1011 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_cartOrder_tb_order_SqlOrderID",
                table: "tb_cartOrder");

            migrationBuilder.DropIndex(
                name: "IX_tb_cartOrder_SqlOrderID",
                table: "tb_cartOrder");

            migrationBuilder.DropColumn(
                name: "SqlOrderID",
                table: "tb_cartOrder");

            migrationBuilder.AddColumn<long>(
                name: "SqlOrderID",
                table: "tb_cartProduct",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_cartProduct_SqlOrderID",
                table: "tb_cartProduct",
                column: "SqlOrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_cartProduct_tb_order_SqlOrderID",
                table: "tb_cartProduct",
                column: "SqlOrderID",
                principalTable: "tb_order",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_cartProduct_tb_order_SqlOrderID",
                table: "tb_cartProduct");

            migrationBuilder.DropIndex(
                name: "IX_tb_cartProduct_SqlOrderID",
                table: "tb_cartProduct");

            migrationBuilder.DropColumn(
                name: "SqlOrderID",
                table: "tb_cartProduct");

            migrationBuilder.AddColumn<long>(
                name: "SqlOrderID",
                table: "tb_cartOrder",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_cartOrder_SqlOrderID",
                table: "tb_cartOrder",
                column: "SqlOrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_cartOrder_tb_order_SqlOrderID",
                table: "tb_cartOrder",
                column: "SqlOrderID",
                principalTable: "tb_order",
                principalColumn: "ID");
        }
    }
}
