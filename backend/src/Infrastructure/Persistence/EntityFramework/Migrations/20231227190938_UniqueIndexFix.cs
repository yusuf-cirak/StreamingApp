using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class UniqueIndexFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserRoleClaims_UserId_RoleId_Value",
                table: "UserRoleClaims",
                columns: new[] { "UserId", "RoleId", "Value" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserOperationClaims_OperationClaimId_UserId_Value",
                table: "UserOperationClaims",
                columns: new[] { "OperationClaimId", "UserId", "Value" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StreamFollowerUsers_StreamerId_UserId",
                table: "StreamFollowerUsers",
                columns: new[] { "StreamerId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StreamBlockedUsers_StreamerId_UserId",
                table: "StreamBlockedUsers",
                columns: new[] { "StreamerId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleOperationClaims_RoleId_OperationClaimId",
                table: "RoleOperationClaims",
                columns: new[] { "RoleId", "OperationClaimId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserRoleClaims_UserId_RoleId_Value",
                table: "UserRoleClaims");

            migrationBuilder.DropIndex(
                name: "IX_UserOperationClaims_OperationClaimId_UserId_Value",
                table: "UserOperationClaims");

            migrationBuilder.DropIndex(
                name: "IX_StreamFollowerUsers_StreamerId_UserId",
                table: "StreamFollowerUsers");

            migrationBuilder.DropIndex(
                name: "IX_StreamBlockedUsers_StreamerId_UserId",
                table: "StreamBlockedUsers");

            migrationBuilder.DropIndex(
                name: "IX_RoleOperationClaims_RoleId_OperationClaimId",
                table: "RoleOperationClaims");
        }
    }
}
