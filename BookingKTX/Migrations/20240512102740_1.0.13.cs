using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingKTX.Migrations
{
    /// <inheritdoc />
    public partial class _1013 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "stateID",
                table: "tb_logOrder",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_logOrder_stateID",
                table: "tb_logOrder",
                column: "stateID");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_logOrder_tb_state_stateID",
                table: "tb_logOrder",
                column: "stateID",
                principalTable: "tb_state",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_logOrder_tb_state_stateID",
                table: "tb_logOrder");

            migrationBuilder.DropIndex(
                name: "IX_tb_logOrder_stateID",
                table: "tb_logOrder");

            migrationBuilder.DropColumn(
                name: "stateID",
                table: "tb_logOrder");
        }
    }
}
