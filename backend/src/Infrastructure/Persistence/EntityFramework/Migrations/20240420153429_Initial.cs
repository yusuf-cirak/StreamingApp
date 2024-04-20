using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OperationClaims",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutboxMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: true),
                    Content = table.Column<string>(type: "text", nullable: true),
                    OccuredOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProcessedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Error = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "bytea", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "bytea", nullable: false),
                    ProfileImageUrl = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleOperationClaims",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    OperationClaimId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleOperationClaims", x => new { x.RoleId, x.OperationClaimId });
                    table.ForeignKey(
                        name: "FK_RoleOperationClaims_OperationClaims_OperationClaimId",
                        column: x => x.OperationClaimId,
                        principalTable: "OperationClaims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleOperationClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedByIp = table.Column<string>(type: "text", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                        name: "FK_StreamBlockedUsers_Users_StreamerId",
                        column: x => x.StreamerId,
                        principalTable: "Users",
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
                        name: "FK_StreamFollowerUsers_Users_StreamerId",
                        column: x => x.StreamerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StreamFollowerUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StreamOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StreamKey = table.Column<string>(type: "text", nullable: false),
                    StreamTitle = table.Column<string>(type: "text", nullable: true),
                    StreamDescription = table.Column<string>(type: "text", nullable: true),
                    MustBeFollower = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
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

            migrationBuilder.CreateTable(
                name: "Streams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StreamerId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Streams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Streams_Users_StreamerId",
                        column: x => x.StreamerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserOperationClaims",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    OperationClaimId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOperationClaims", x => new { x.OperationClaimId, x.UserId, x.Value });
                    table.ForeignKey(
                        name: "FK_UserOperationClaims_OperationClaims_OperationClaimId",
                        column: x => x.OperationClaimId,
                        principalTable: "OperationClaims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserOperationClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoleClaims",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoleClaims", x => new { x.UserId, x.RoleId, x.Value });
                    table.ForeignKey(
                        name: "FK_UserRoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoleClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OperationClaims_Id",
                table: "OperationClaims",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_Id",
                table: "RefreshTokens",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleOperationClaims_OperationClaimId",
                table: "RoleOperationClaims",
                column: "OperationClaimId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleOperationClaims_RoleId",
                table: "RoleOperationClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleOperationClaims_RoleId_OperationClaimId",
                table: "RoleOperationClaims",
                columns: new[] { "RoleId", "OperationClaimId" },
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
                name: "IX_StreamBlockedUsers_StreamerId_UserId",
                table: "StreamBlockedUsers",
                columns: new[] { "StreamerId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StreamBlockedUsers_UserId",
                table: "StreamBlockedUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StreamFollowerUsers_StreamerId",
                table: "StreamFollowerUsers",
                column: "StreamerId");

            migrationBuilder.CreateIndex(
                name: "IX_StreamFollowerUsers_StreamerId_UserId",
                table: "StreamFollowerUsers",
                columns: new[] { "StreamerId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StreamFollowerUsers_UserId",
                table: "StreamFollowerUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Streams_Id",
                table: "Streams",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Streams_StreamerId",
                table: "Streams",
                column: "StreamerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOperationClaims_OperationClaimId",
                table: "UserOperationClaims",
                column: "OperationClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOperationClaims_OperationClaimId_UserId_Value",
                table: "UserOperationClaims",
                columns: new[] { "OperationClaimId", "UserId", "Value" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserOperationClaims_UserId",
                table: "UserOperationClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleClaims_RoleId",
                table: "UserRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleClaims_UserId",
                table: "UserRoleClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleClaims_UserId_RoleId_Value",
                table: "UserRoleClaims",
                columns: new[] { "UserId", "RoleId", "Value" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Id",
                table: "Users",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OutboxMessages");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "RoleOperationClaims");

            migrationBuilder.DropTable(
                name: "StreamBlockedUsers");

            migrationBuilder.DropTable(
                name: "StreamFollowerUsers");

            migrationBuilder.DropTable(
                name: "StreamOptions");

            migrationBuilder.DropTable(
                name: "Streams");

            migrationBuilder.DropTable(
                name: "UserOperationClaims");

            migrationBuilder.DropTable(
                name: "UserRoleClaims");

            migrationBuilder.DropTable(
                name: "OperationClaims");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
