using BadilkBackend.src.Infra;
using Asp.Versioning;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BadilkBackend.src.Core.Bootstrap;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBadilkServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfraSetup(configuration);

        services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new QueryStringApiVersionReader("api-version"),
                    new HeaderApiVersionReader("x-api-version"));
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, _, _) =>
            {
                document.Info = new()
                {
                    Title = "Badilk Backend API",
                    Version = "v1",
                    Description = "Badilk Backend API Documentation",
                    Contact = new()
                    {
                        Name = "Badilk Backend API",
                        Email = "support@badilk.com",
                        Url = new Uri("https://www.badilk.com")
                    },
                    License = new()
                    {
                        Name = "Badilk Backend API License",
                        Url = new Uri("https://www.badilk.com/license")
                    },
                };

                return Task.CompletedTask;
            });
        });

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });

        services.AddControllers();

        return services;
    }
}

