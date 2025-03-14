using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourtConnect.Migrations
{
    /// <inheritdoc />
    public partial class ConfirmAnnounce : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ConfirmGuest",
                table: "Announces",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ConfirmHost",
                table: "Announces",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmGuest",
                table: "Announces");

            migrationBuilder.DropColumn(
                name: "ConfirmHost",
                table: "Announces");
        }
    }
}
