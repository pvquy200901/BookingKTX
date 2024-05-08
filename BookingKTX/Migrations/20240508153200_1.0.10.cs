using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BookingKTX.Migrations
{
    /// <inheritdoc />
    public partial class _1010 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_product_tb_cart_SqlCartID",
                table: "tb_product");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_product_tb_order_SqlOrderID",
                table: "tb_product");

            migrationBuilder.DropIndex(
                name: "IX_tb_product_SqlCartID",
                table: "tb_product");

            migrationBuilder.DropIndex(
                name: "IX_tb_product_SqlOrderID",
                table: "tb_product");

            migrationBuilder.DropColumn(
                name: "SqlCartID",
                table: "tb_product");

            migrationBuilder.DropColumn(
                name: "SqlOrderID",
                table: "tb_product");

            migrationBuilder.CreateTable(
                name: "tb_cartOrder",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    productID = table.Column<long>(type: "bigint", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    SqlOrderID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_cartOrder", x => x.ID);
                    table.ForeignKey(
                        name: "FK_tb_cartOrder_tb_order_SqlOrderID",
                        column: x => x.SqlOrderID,
                        principalTable: "tb_order",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_tb_cartOrder_tb_product_productID",
                        column: x => x.productID,
                        principalTable: "tb_product",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "tb_cartProduct",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    productID = table.Column<long>(type: "bigint", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    SqlCartID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_cartProduct", x => x.ID);
                    table.ForeignKey(
                        name: "FK_tb_cartProduct_tb_cart_SqlCartID",
                        column: x => x.SqlCartID,
                        principalTable: "tb_cart",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_tb_cartProduct_tb_product_productID",
                        column: x => x.productID,
                        principalTable: "tb_product",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_cartOrder_productID",
                table: "tb_cartOrder",
                column: "productID");

            migrationBuilder.CreateIndex(
                name: "IX_tb_cartOrder_SqlOrderID",
                table: "tb_cartOrder",
                column: "SqlOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_tb_cartProduct_productID",
                table: "tb_cartProduct",
                column: "productID");

            migrationBuilder.CreateIndex(
                name: "IX_tb_cartProduct_SqlCartID",
                table: "tb_cartProduct",
                column: "SqlCartID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_cartOrder");

            migrationBuilder.DropTable(
                name: "tb_cartProduct");

            migrationBuilder.AddColumn<long>(
                name: "SqlCartID",
                table: "tb_product",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SqlOrderID",
                table: "tb_product",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_product_SqlCartID",
                table: "tb_product",
                column: "SqlCartID");

            migrationBuilder.CreateIndex(
                name: "IX_tb_product_SqlOrderID",
                table: "tb_product",
                column: "SqlOrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_product_tb_cart_SqlCartID",
                table: "tb_product",
                column: "SqlCartID",
                principalTable: "tb_cart",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_product_tb_order_SqlOrderID",
                table: "tb_product",
                column: "SqlOrderID",
                principalTable: "tb_order",
                principalColumn: "ID");
        }
    }
}
