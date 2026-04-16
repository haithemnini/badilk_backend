using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BadilkBackend.src.Infra.Database.Migrations
{
    /// <inheritdoc />
    public partial class RenameUsersTablesToPascalCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Be tolerant to existing environments:
            // - Some DBs already have "Users"/"Profiles" (PascalCase)
            // - Some DBs still have users/profiles (lowercase)
            // We rename only when the source objects exist to avoid failing with 42P01.
            migrationBuilder.Sql(
                """
                DO $$
                BEGIN
                  -- Rename tables
                  IF to_regclass('public.users') IS NOT NULL AND to_regclass('public."Users"') IS NULL THEN
                    ALTER TABLE public.users RENAME TO "Users";
                  END IF;

                  IF to_regclass('public.profiles') IS NOT NULL AND to_regclass('public."Profiles"') IS NULL THEN
                    ALTER TABLE public.profiles RENAME TO "Profiles";
                  END IF;

                  -- Rename indexes (only if they exist)
                  IF to_regclass('public."IX_users_Email"') IS NOT NULL AND to_regclass('public."IX_Users_Email"') IS NULL THEN
                    ALTER INDEX public."IX_users_Email" RENAME TO "IX_Users_Email";
                  END IF;

                  IF to_regclass('public."IX_users_Provider_ProviderUserId"') IS NOT NULL AND to_regclass('public."IX_Users_Provider_ProviderUserId"') IS NULL THEN
                    ALTER INDEX public."IX_users_Provider_ProviderUserId" RENAME TO "IX_Users_Provider_ProviderUserId";
                  END IF;

                  -- Rename constraints (PK/FK) if present
                  IF EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'PK_users') THEN
                    ALTER TABLE public."Users" RENAME CONSTRAINT "PK_users" TO "PK_Users";
                  END IF;

                  IF EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'PK_profiles') THEN
                    ALTER TABLE public."Profiles" RENAME CONSTRAINT "PK_profiles" TO "PK_Profiles";
                  END IF;

                  IF EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'FK_profiles_users_UserId') THEN
                    ALTER TABLE public."Profiles" RENAME CONSTRAINT "FK_profiles_users_UserId" TO "FK_Profiles_Users_UserId";
                  END IF;
                END $$;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                DO $$
                BEGIN
                  -- Rename constraints back (if present)
                  IF EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'FK_Profiles_Users_UserId') THEN
                    ALTER TABLE public."Profiles" RENAME CONSTRAINT "FK_Profiles_Users_UserId" TO "FK_profiles_users_UserId";
                  END IF;

                  IF EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'PK_Profiles') THEN
                    ALTER TABLE public."Profiles" RENAME CONSTRAINT "PK_Profiles" TO "PK_profiles";
                  END IF;

                  IF EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'PK_Users') THEN
                    ALTER TABLE public."Users" RENAME CONSTRAINT "PK_Users" TO "PK_users";
                  END IF;

                  -- Rename indexes back (only if they exist)
                  IF to_regclass('public."IX_Users_Email"') IS NOT NULL AND to_regclass('public."IX_users_Email"') IS NULL THEN
                    ALTER INDEX public."IX_Users_Email" RENAME TO "IX_users_Email";
                  END IF;

                  IF to_regclass('public."IX_Users_Provider_ProviderUserId"') IS NOT NULL AND to_regclass('public."IX_users_Provider_ProviderUserId"') IS NULL THEN
                    ALTER INDEX public."IX_Users_Provider_ProviderUserId" RENAME TO "IX_users_Provider_ProviderUserId";
                  END IF;

                  -- Rename tables back
                  IF to_regclass('public."Users"') IS NOT NULL AND to_regclass('public.users') IS NULL THEN
                    ALTER TABLE public."Users" RENAME TO users;
                  END IF;

                  IF to_regclass('public."Profiles"') IS NOT NULL AND to_regclass('public.profiles') IS NULL THEN
                    ALTER TABLE public."Profiles" RENAME TO profiles;
                  END IF;
                END $$;
                """);
        }
    }
}
