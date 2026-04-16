using BadilkBackend.src.Infra;
using Asp.Versioning;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BadilkBackend.src.Features.Auth.Options;
using BadilkBackend.src.Features.Auth.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BadilkBackend.src.Core.Bootstrap;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBadilkServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfraSetup(configuration);

        services.AddOptions<GoogleOidcOptions>()
            .Bind(configuration.GetSection(GoogleOidcOptions.SectionName));

        services.AddOptions<JwtOptions>()
            .Bind(configuration.GetSection(JwtOptions.SectionName));

        services.AddScoped<ITokenVerifier, GoogleTokenVerifier>();
        services.AddScoped<IJwtIssuer, JwtIssuer>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var jwt = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>() ?? new JwtOptions();
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwt.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwt.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SigningKey)),
                    ClockSkew = TimeSpan.FromMinutes(2),
                };
            });

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

