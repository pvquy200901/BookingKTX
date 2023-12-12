using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BookingKTX.Migrations
{
    /// <inheritdoc />
    public partial class _101 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_action",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_action", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tb_customer",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: false),
                    username = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    phone = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    address = table.Column<string>(type: "text", nullable: false),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_customer", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tb_file",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    key = table.Column<string>(type: "text", nullable: false),
                    link = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_file", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tb_role",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    des = table.Column<string>(type: "text", nullable: false),
                    note = table.Column<string>(type: "text", nullable: false),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_role", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tb_state",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_state", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tb_type",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_type", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tb_shop",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    createdTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    lastestTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    typeID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_shop", x => x.ID);
                    table.ForeignKey(
                        name: "FK_tb_shop_tb_type_typeID",
                        column: x => x.typeID,
                        principalTable: "tb_type",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "tb_order",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    customerID = table.Column<long>(type: "bigint", nullable: true),
                    code = table.Column<string>(type: "text", nullable: false),
                    shopID = table.Column<long>(type: "bigint", nullable: true),
                    stateID = table.Column<long>(type: "bigint", nullable: true),
                    note = table.Column<string>(type: "text", nullable: false),
                    total = table.Column<double>(type: "double precision", nullable: false),
                    createdTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    lastestTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isFinish = table.Column<bool>(type: "boolean", nullable: false),
                    isDelete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_order", x => x.ID);
                    table.ForeignKey(
                        name: "FK_tb_order_tb_customer_customerID",
                        column: x => x.customerID,
                        principalTable: "tb_customer",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_tb_order_tb_shop_shopID",
                        column: x => x.shopID,
                        principalTable: "tb_shop",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_tb_order_tb_state_stateID",
                        column: x => x.stateID,
                        principalTable: "tb_state",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "tb_user",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: false),
                    username = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    token = table.Column<string>(type: "text", nullable: false),
                    displayName = table.Column<string>(type: "text", nullable: false),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    images = table.Column<List<string>>(type: "text[]", nullable: true),
                    idHub = table.Column<string>(type: "text", nullable: false),
                    phoneNumber = table.Column<string>(type: "text", nullable: false),
                    avatar = table.Column<string>(type: "text", nullable: false),
                    roleID = table.Column<long>(type: "bigint", nullable: true),
                    SqlShopID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_user", x => x.ID);
                    table.ForeignKey(
                        name: "FK_tb_user_tb_role_roleID",
                        column: x => x.roleID,
                        principalTable: "tb_role",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_tb_user_tb_shop_SqlShopID",
                        column: x => x.SqlShopID,
                        principalTable: "tb_shop",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "tb_product",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    shopID = table.Column<long>(type: "bigint", nullable: true),
                    SqlOrderID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_product", x => x.ID);
                    table.ForeignKey(
                        name: "FK_tb_product_tb_order_SqlOrderID",
                        column: x => x.SqlOrderID,
                        principalTable: "tb_order",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_tb_product_tb_shop_shopID",
                        column: x => x.shopID,
                        principalTable: "tb_shop",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "tb_logOrder",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    orderID = table.Column<long>(type: "bigint", nullable: true),
                    userID = table.Column<long>(type: "bigint", nullable: true),
                    actionID = table.Column<long>(type: "bigint", nullable: true),
                    time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    note = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_logOrder", x => x.ID);
                    table.ForeignKey(
                        name: "FK_tb_logOrder_tb_action_actionID",
                        column: x => x.actionID,
                        principalTable: "tb_action",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_tb_logOrder_tb_order_orderID",
                        column: x => x.orderID,
                        principalTable: "tb_order",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_tb_logOrder_tb_user_userID",
                        column: x => x.userID,
                        principalTable: "tb_user",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_logOrder_actionID",
                table: "tb_logOrder",
                column: "actionID");

            migrationBuilder.CreateIndex(
                name: "IX_tb_logOrder_orderID",
                table: "tb_logOrder",
                column: "orderID");

            migrationBuilder.CreateIndex(
                name: "IX_tb_logOrder_userID",
                table: "tb_logOrder",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "IX_tb_order_customerID",
                table: "tb_order",
                column: "customerID");

            migrationBuilder.CreateIndex(
                name: "IX_tb_order_shopID",
                table: "tb_order",
                column: "shopID");

            migrationBuilder.CreateIndex(
                name: "IX_tb_order_stateID",
                table: "tb_order",
                column: "stateID");

            migrationBuilder.CreateIndex(
                name: "IX_tb_product_shopID",
                table: "tb_product",
                column: "shopID");

            migrationBuilder.CreateIndex(
                name: "IX_tb_product_SqlOrderID",
                table: "tb_product",
                column: "SqlOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_tb_shop_typeID",
                table: "tb_shop",
                column: "typeID");

            migrationBuilder.CreateIndex(
                name: "IX_tb_user_roleID",
                table: "tb_user",
                column: "roleID");

            migrationBuilder.CreateIndex(
                name: "IX_tb_user_SqlShopID",
                table: "tb_user",
                column: "SqlShopID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_file");

            migrationBuilder.DropTable(
                name: "tb_logOrder");

            migrationBuilder.DropTable(
                name: "tb_product");

            migrationBuilder.DropTable(
                name: "tb_action");

            migrationBuilder.DropTable(
                name: "tb_user");

            migrationBuilder.DropTable(
                name: "tb_order");

            migrationBuilder.DropTable(
                name: "tb_role");

            migrationBuilder.DropTable(
                name: "tb_customer");

            migrationBuilder.DropTable(
                name: "tb_shop");

            migrationBuilder.DropTable(
                name: "tb_state");

            migrationBuilder.DropTable(
                name: "tb_type");
        }
    }
}
