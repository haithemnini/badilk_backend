using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BadilkBackend.src.Infra.Database.Migrations
{
    /// <inheritdoc />
    public partial class BrandIdGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // If the table was dropped out-of-band but the migration history exists,
            // we "repair" it by creating the table with the latest shape.
            migrationBuilder.Sql("""
DO $$
BEGIN
  IF to_regclass('public."Brands"') IS NOT NULL THEN
    DROP TABLE "Brands";
  END IF;

  CREATE TABLE "Brands" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    "LogoUrl" text,
    "BannerUrl" text,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Brands" PRIMARY KEY ("Id")
  );
END $$;
""");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Brands",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }
    }
}
