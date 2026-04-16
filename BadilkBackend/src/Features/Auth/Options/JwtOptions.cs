namespace BadilkBackend.src.Features.Auth.Options;

public sealed class JwtOptions
{
    public const string SectionName = "Auth:Jwt";

    public string Issuer { get; init; } = "badilk-backend";

    public string Audience { get; init; } = "badilk-app";

    public string SigningKey { get; init; } = "";

    public int AccessTokenMinutes { get; init; } = 60 * 24 * 30; // 30 days
}

