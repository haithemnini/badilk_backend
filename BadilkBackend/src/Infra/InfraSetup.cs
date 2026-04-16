using BadilkBackend.src.Infra.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using BadilkBackend.src.Features.Brands.Repos;

namespace BadilkBackend.src.Infra;

public static class InfraSetupExtensions
{
    public static IServiceCollection AddInfraSetup(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<DatabaseOptions>()
            .Bind(configuration.GetSection(DatabaseOptions.SectionName))
            .Validate(o => !string.IsNullOrWhiteSpace(o.ConnectionString), $"{DatabaseOptions.SectionName}:ConnectionString is required.")
            .ValidateOnStart();

        var connectionString =
            configuration.GetSection(DatabaseOptions.SectionName).GetValue<string>(nameof(DatabaseOptions.ConnectionString))
            ?? configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException($"Connection string is not configured. Set '{DatabaseOptions.SectionName}:ConnectionString' or 'ConnectionStrings:DefaultConnection'.");

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddSingleton<NpgsqlDataSource>(_ => NpgsqlDataSource.Create(connectionString));
        services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

        //----------------------------------------------------------------------//
        // External Dependencies

        services.AddScoped<IBrandsRepo, BrandsRepo>();
        //----------------------------------------------------------------------//

        return services;
    }
}

