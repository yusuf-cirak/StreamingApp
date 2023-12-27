using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class UserOperationClaim_RoleOperationClaim_Value : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoleClaims",
                table: "UserRoleClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserOperationClaims",
                table: "UserOperationClaims");

            migrationBuilder.DropIndex(
                name: "IX_UserOperationClaims_UserId",
                table: "UserOperationClaims");

            migrationBuilder.DropIndex(
                name: "IX_StreamModerators_UserId",
                table: "StreamModerators");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "UserRoleClaims",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "UserOperationClaims",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoleClaims",
                table: "UserRoleClaims",
                columns: new[] { "UserId", "RoleId", "Value" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserOperationClaims",
                table: "UserOperationClaims",
                columns: new[] { "OperationClaimId", "UserId", "Value" });

            migrationBuilder.CreateIndex(
                name: "IX_UserOperationClaims_OperationClaimId",
                table: "UserOperationClaims",
                column: "OperationClaimId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserOperationClaims_UserId",
                table: "UserOperationClaims",
                column: "UserId",
                unique: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoleClaims",
                table: "UserRoleClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserOperationClaims",
                table: "UserOperationClaims");

            migrationBuilder.DropIndex(
                name: "IX_UserOperationClaims_OperationClaimId",
                table: "UserOperationClaims");

            migrationBuilder.DropIndex(
                name: "IX_UserOperationClaims_UserId",
                table: "UserOperationClaims");

            migrationBuilder.DropIndex(
                name: "IX_StreamModerators_StreamerId",
                table: "StreamModerators");

            migrationBuilder.DropIndex(
                name: "IX_StreamModerators_UserId",
                table: "StreamModerators");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "UserRoleClaims");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "UserOperationClaims");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoleClaims",
                table: "UserRoleClaims",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserOperationClaims",
                table: "UserOperationClaims",
                columns: new[] { "OperationClaimId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserOperationClaims_UserId",
                table: "UserOperationClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StreamModerators_UserId",
                table: "StreamModerators",
                column: "UserId");
        }
    }
}
