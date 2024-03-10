using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StreamOption_MustBeFollower : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StreamChatMessages");

            migrationBuilder.AddColumn<bool>(
                name: "MustBeFollower",
                table: "StreamOptions",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MustBeFollower",
                table: "StreamOptions");

            migrationBuilder.CreateTable(
                name: "StreamChatMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StreamerId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    StreamId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StreamChatMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StreamChatMessages_Streams_StreamerId",
                        column: x => x.StreamerId,
                        principalTable: "Streams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StreamChatMessages_Users_StreamerId",
                        column: x => x.StreamerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StreamChatMessages_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StreamChatMessages_Id",
                table: "StreamChatMessages",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StreamChatMessages_StreamerId",
                table: "StreamChatMessages",
                column: "StreamerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StreamChatMessages_UserId",
                table: "StreamChatMessages",
                column: "UserId",
                unique: true);
        }
    }
}
