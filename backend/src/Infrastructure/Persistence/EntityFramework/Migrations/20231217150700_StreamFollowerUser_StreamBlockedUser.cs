using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class StreamFollowerUser_StreamBlockedUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StreamBlockedUsers",
                columns: table => new
                {
                    StreamerId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StreamBlockedUsers", x => new { x.StreamerId, x.UserId });
                    table.ForeignKey(
                        name: "FK_StreamBlockedUsers_Streamers_StreamerId",
                        column: x => x.StreamerId,
                        principalTable: "Streamers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StreamBlockedUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StreamFollowerUsers",
                columns: table => new
                {
                    StreamerId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StreamFollowerUsers", x => new { x.StreamerId, x.UserId });
                    table.ForeignKey(
                        name: "FK_StreamFollowerUsers_Streamers_StreamerId",
                        column: x => x.StreamerId,
                        principalTable: "Streamers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StreamFollowerUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StreamBlockedUsers_UserId",
                table: "StreamBlockedUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StreamFollowerUsers_UserId",
                table: "StreamFollowerUsers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StreamBlockedUsers");

            migrationBuilder.DropTable(
                name: "StreamFollowerUsers");
        }
    }
}
