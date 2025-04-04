using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourtConnect.Migrations
{
    /// <inheritdoc />
    public partial class AnnounceIdinMatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnnounceId",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Matches_AnnounceId",
                table: "Matches",
                column: "AnnounceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Announces_AnnounceId",
                table: "Matches",
                column: "AnnounceId",
                principalTable: "Announces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Announces_AnnounceId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_AnnounceId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "AnnounceId",
                table: "Matches");
        }
    }
}
