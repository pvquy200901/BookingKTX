using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingKTX.Migrations
{
    /// <inheritdoc />
    public partial class _108 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "image",
                table: "tb_shop",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image",
                table: "tb_shop");
        }
    }
}
