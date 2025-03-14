using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourtConnect.Migrations
{
    /// <inheritdoc />
    public partial class GuestUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GuestUserId",
                table: "Announces",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuestUserId",
                table: "Announces");
        }
    }
}
