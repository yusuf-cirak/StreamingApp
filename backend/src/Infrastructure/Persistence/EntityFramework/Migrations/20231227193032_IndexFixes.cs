using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class IndexFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserRoleClaims_RoleId",
                table: "UserRoleClaims");

            migrationBuilder.DropIndex(
                name: "IX_UserOperationClaims_UserId",
                table: "UserOperationClaims");

            migrationBuilder.DropIndex(
                name: "IX_RoleOperationClaims_RoleId",
                table: "RoleOperationClaims");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleClaims_RoleId",
                table: "UserRoleClaims",
                column: "RoleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleClaims_UserId",
                table: "UserRoleClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOperationClaims_UserId",
                table: "UserOperationClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StreamFollowerUsers_StreamerId",
                table: "StreamFollowerUsers",
                column: "StreamerId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleOperationClaims_RoleId",
                table: "RoleOperationClaims",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserRoleClaims_RoleId",
                table: "UserRoleClaims");

            migrationBuilder.DropIndex(
                name: "IX_UserRoleClaims_UserId",
                table: "UserRoleClaims");

            migrationBuilder.DropIndex(
                name: "IX_UserOperationClaims_UserId",
                table: "UserOperationClaims");

            migrationBuilder.DropIndex(
                name: "IX_StreamFollowerUsers_StreamerId",
                table: "StreamFollowerUsers");

            migrationBuilder.DropIndex(
                name: "IX_RoleOperationClaims_RoleId",
                table: "RoleOperationClaims");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleClaims_RoleId",
                table: "UserRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOperationClaims_UserId",
                table: "UserOperationClaims",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleOperationClaims_RoleId",
                table: "RoleOperationClaims",
                column: "RoleId",
                unique: true);
        }
    }
}
