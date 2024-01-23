using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class StreamOptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StreamBlockedUsers_Streamers_StreamerId",
                table: "StreamBlockedUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_StreamChatMessages_Streamers_StreamerId",
                table: "StreamChatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_StreamFollowerUsers_Streamers_StreamerId",
                table: "StreamFollowerUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Streams_Streamers_StreamerId",
                table: "Streams");

            migrationBuilder.DropTable(
                name: "Streamers");

            migrationBuilder.CreateTable(
                name: "StreamOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StreamKey = table.Column<string>(type: "text", nullable: false),
                    StreamTitle = table.Column<string>(type: "text", nullable: true),
                    StreamDescription = table.Column<string>(type: "text", nullable: true),
                    ChatDisabled = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    ChatDelaySecond = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StreamOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StreamOptions_Users_Id",
                        column: x => x.Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_StreamBlockedUsers_Users_StreamerId",
                table: "StreamBlockedUsers",
                column: "StreamerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StreamChatMessages_Users_StreamerId",
                table: "StreamChatMessages",
                column: "StreamerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StreamFollowerUsers_Users_StreamerId",
                table: "StreamFollowerUsers",
                column: "StreamerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Streams_Users_StreamerId",
                table: "Streams",
                column: "StreamerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StreamBlockedUsers_Users_StreamerId",
                table: "StreamBlockedUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_StreamChatMessages_Users_StreamerId",
                table: "StreamChatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_StreamFollowerUsers_Users_StreamerId",
                table: "StreamFollowerUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Streams_Users_StreamerId",
                table: "Streams");

            migrationBuilder.DropTable(
                name: "StreamOptions");

            migrationBuilder.CreateTable(
                name: "Streamers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChatDelaySecond = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    ChatDisabled = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    StreamDescription = table.Column<string>(type: "text", nullable: true),
                    StreamKey = table.Column<string>(type: "text", nullable: false),
                    StreamTitle = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Streamers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Streamers_Users_Id",
                        column: x => x.Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_StreamBlockedUsers_Streamers_StreamerId",
                table: "StreamBlockedUsers",
                column: "StreamerId",
                principalTable: "Streamers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StreamChatMessages_Streamers_StreamerId",
                table: "StreamChatMessages",
                column: "StreamerId",
                principalTable: "Streamers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StreamFollowerUsers_Streamers_StreamerId",
                table: "StreamFollowerUsers",
                column: "StreamerId",
                principalTable: "Streamers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Streams_Streamers_StreamerId",
                table: "Streams",
                column: "StreamerId",
                principalTable: "Streamers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
