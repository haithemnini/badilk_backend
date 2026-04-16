namespace BadilkBackend.src.Features.Auth.Options;

public sealed class GoogleOidcOptions
{
    public const string SectionName = "Auth:Google";

    public string ClientId { get; init; } = "";
}

