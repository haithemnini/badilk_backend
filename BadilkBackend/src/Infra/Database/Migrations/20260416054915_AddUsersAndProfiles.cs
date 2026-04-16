using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BadilkBackend.src.Infra.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddUsersAndProfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Provider = table.Column<string>(type: "text", nullable: false),
                    ProviderUserId = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    EmailVerified = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    AvatarUrl = table.Column<string>(type: "text", nullable: true),
                    RawUserMeta = table.Column<JsonDocument>(type: "jsonb", nullable: false, defaultValueSql: "'{}'::jsonb"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    LastSeen = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    {
                        table.PrimaryKey("PK_users", x => x.Id);
                    }
                }
            );

            migrationBuilder.CreateTable(
                name: "Profiles",
                    columns: table => new
                    {
                        UserId = table.Column<Guid>(type: "uuid", nullable: false),
                        Role = table.Column<string>(type: "text", nullable: false, defaultValue: "user"),
                        Status = table.Column<string>(type: "text", nullable: false, defaultValue: "active"),
                        Plan = table.Column<string>(type: "text", nullable: false, defaultValue: "free"),
                        StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                        ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                        ShowAds = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                        CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                        UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_profiles", x => x.UserId);
                        table.ForeignKey(
                            name: "FK_profiles_users_UserId",
                            column: x => x.UserId,
                            principalTable: "users",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                    });

            migrationBuilder.CreateIndex(
                name: "IX_users_Email",
                table: "users",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_users_Provider_ProviderUserId",
                table: "users",
                columns: ["Provider", "ProviderUserId"],
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "profiles");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
