using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class StreamsFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Streams_StreamerId",
                table: "Streams");

            migrationBuilder.DropColumn(
                name: "ChatDelaySecond",
                table: "Streams");

            migrationBuilder.DropColumn(
                name: "ChatDisabled",
                table: "Streams");

            migrationBuilder.AddColumn<int>(
                name: "ChatDelaySecond",
                table: "Streamers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "ChatDisabled",
                table: "Streamers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Streams_StreamerId",
                table: "Streams",
                column: "StreamerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Streams_StreamerId",
                table: "Streams");

            migrationBuilder.DropColumn(
                name: "ChatDelaySecond",
                table: "Streamers");

            migrationBuilder.DropColumn(
                name: "ChatDisabled",
                table: "Streamers");

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

            migrationBuilder.CreateIndex(
                name: "IX_Streams_StreamerId",
                table: "Streams",
                column: "StreamerId",
                unique: true);
        }
    }
}
