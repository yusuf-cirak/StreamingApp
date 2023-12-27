using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStreamModerators_AddValueToRoleOperationClaimsAndUserRoleClaims : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StreamModerators");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Id",
                table: "Users",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Streams_Id",
                table: "Streams",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StreamChatMessages_Id",
                table: "StreamChatMessages",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Id",
                table: "Roles",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_Id",
                table: "RefreshTokens",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OperationClaims_Id",
                table: "OperationClaims",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Id",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Streams_Id",
                table: "Streams");

            migrationBuilder.DropIndex(
                name: "IX_StreamChatMessages_Id",
                table: "StreamChatMessages");

            migrationBuilder.DropIndex(
                name: "IX_Roles_Id",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_Name",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_Id",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_OperationClaims_Id",
                table: "OperationClaims");

            migrationBuilder.CreateTable(
                name: "StreamModerators",
                columns: table => new
                {
                    StreamerId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StreamModerators", x => new { x.StreamerId, x.UserId });
                    table.ForeignKey(
                        name: "FK_StreamModerators_Streamers_StreamerId",
                        column: x => x.StreamerId,
                        principalTable: "Streamers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StreamModerators_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StreamModerators_StreamerId",
                table: "StreamModerators",
                column: "StreamerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StreamModerators_UserId",
                table: "StreamModerators",
                column: "UserId",
                unique: true);
        }
    }
}
