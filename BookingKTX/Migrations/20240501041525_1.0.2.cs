using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingKTX.Migrations
{
    /// <inheritdoc />
    public partial class _102 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_user_tb_shop_SqlShopID",
                table: "tb_user");

            migrationBuilder.RenameColumn(
                name: "SqlShopID",
                table: "tb_user",
                newName: "shopID");

            migrationBuilder.RenameIndex(
                name: "IX_tb_user_SqlShopID",
                table: "tb_user",
                newName: "IX_tb_user_shopID");

            migrationBuilder.AddColumn<List<string>>(
                name: "images",
                table: "tb_product",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "shipperID",
                table: "tb_order",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "idhub",
                table: "tb_customer",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_tb_order_shipperID",
                table: "tb_order",
                column: "shipperID");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_order_tb_user_shipperID",
                table: "tb_order",
                column: "shipperID",
                principalTable: "tb_user",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_user_tb_shop_shopID",
                table: "tb_user",
                column: "shopID",
                principalTable: "tb_shop",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_order_tb_user_shipperID",
                table: "tb_order");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_user_tb_shop_shopID",
                table: "tb_user");

            migrationBuilder.DropIndex(
                name: "IX_tb_order_shipperID",
                table: "tb_order");

            migrationBuilder.DropColumn(
                name: "images",
                table: "tb_product");

            migrationBuilder.DropColumn(
                name: "shipperID",
                table: "tb_order");

            migrationBuilder.DropColumn(
                name: "idhub",
                table: "tb_customer");

            migrationBuilder.RenameColumn(
                name: "shopID",
                table: "tb_user",
                newName: "SqlShopID");

            migrationBuilder.RenameIndex(
                name: "IX_tb_user_shopID",
                table: "tb_user",
                newName: "IX_tb_user_SqlShopID");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_user_tb_shop_SqlShopID",
                table: "tb_user",
                column: "SqlShopID",
                principalTable: "tb_shop",
                principalColumn: "ID");
        }
    }
}
