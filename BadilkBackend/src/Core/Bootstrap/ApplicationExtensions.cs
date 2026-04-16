using BadilkBackend.src.Infra.Database;
using Asp.Versioning.Builder;
using BadilkBackend.src.Core.Dtos.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;
using System.Data.Common;

namespace BadilkBackend.src.Core.Bootstrap;

public static class ApplicationExtensions
{
    public static async Task<bool> EnsureDatabaseConnectionOrStopAsync(this WebApplication app, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var scope = app.Services.CreateAsyncScope();
            var db = scope.ServiceProvider.GetRequiredService<IDbConnectionFactory>();
            var appDb = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await using var conn = await db.OpenConnectionAsync(cancellationToken);
            await using var cmd = conn.CreateCommand();
            cmd.CommandText = "select 1;";
            _ = await cmd.ExecuteScalarAsync(cancellationToken);

            await appDb.Database.MigrateAsync(cancellationToken);

            app.Logger.LogInformation("Database connection successful");
            return true;
        }
        catch (Exception ex)
        {
            app.Logger.LogCritical(ex,
                "Database connection failed on startup. Check Database:ConnectionString in appsettings.Secrets.json");
            Environment.ExitCode = 1;
            return false;
        }
    }

    public static WebApplication MapBadilkEndpoints(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.MapGet("/", () => Results.Redirect("/scalar/v1")).ExcludeFromDescription();

        var api = app.NewVersionedApi();
        var v1 = api.MapGroup("/api/v{version:apiVersion}")
            .HasApiVersion(1.0);

        return app;
    }

    public static WebApplication UseBadilkMiddleware(this WebApplication app)
    {
        app.UseCors("AllowAll");
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        return app;
    }
}

