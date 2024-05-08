using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BookingKTX.Migrations
{
    /// <inheritdoc />
    public partial class _104 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SqlCartID",
                table: "tb_product",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tb_cart",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: false),
                    customerID = table.Column<long>(type: "bigint", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_cart", x => x.ID);
                    table.ForeignKey(
                        name: "FK_tb_cart_tb_customer_customerID",
                        column: x => x.customerID,
                        principalTable: "tb_customer",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_product_SqlCartID",
                table: "tb_product",
                column: "SqlCartID");

            migrationBuilder.CreateIndex(
                name: "IX_tb_cart_customerID",
                table: "tb_cart",
                column: "customerID");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_product_tb_cart_SqlCartID",
                table: "tb_product",
                column: "SqlCartID",
                principalTable: "tb_cart",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_product_tb_cart_SqlCartID",
                table: "tb_product");

            migrationBuilder.DropTable(
                name: "tb_cart");

            migrationBuilder.DropIndex(
                name: "IX_tb_product_SqlCartID",
                table: "tb_product");

            migrationBuilder.DropColumn(
                name: "SqlCartID",
                table: "tb_product");
        }
    }
}
