namespace BadilkBackend.src.Features.Auth.Dtos;

using System.Text.Json.Serialization;

public sealed class SocialLoginRequest
{
    [JsonPropertyName("provider")]
    public required string Provider { get; init; }

    [JsonPropertyName("id_token")]
    public required string IdToken { get; init; }
}

