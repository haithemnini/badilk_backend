using Microsoft.Extensions.Configuration;

namespace BadilkBackend.src.Core.Bootstrap;

public static class ConfigurationExtensions
{
    public static IConfigurationManager AddBadilkConfiguration(this IConfigurationManager configuration)
    {
        // Local secrets (NOT committed)
        configuration.AddJsonFile("appsettings.Secrets.json", optional: true, reloadOnChange: true);
        configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);
        return configuration;
    }
}

