using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class ChatDisabled_ChatDelaySecond : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChatDelaySecond",
                table: "Streams",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "ChatDisabled",
                table: "Streams",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatDelaySecond",
                table: "Streams");

            migrationBuilder.DropColumn(
                name: "ChatDisabled",
                table: "Streams");
        }
    }
}
