using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FindYOU.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToChatEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ChatEntries",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ChatEntries_UserId",
                table: "ChatEntries",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatEntries_Users_UserId",
                table: "ChatEntries",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatEntries_Users_UserId",
                table: "ChatEntries");

            migrationBuilder.DropIndex(
                name: "IX_ChatEntries_UserId",
                table: "ChatEntries");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ChatEntries");
        }
    }
}
