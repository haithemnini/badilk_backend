namespace BadilkBackend.src.Features.Users.Dtos;

using System.Text.Json.Serialization;

public sealed record UserDto(
    [property: JsonPropertyName("id")]
    Guid Id,

    [property: JsonPropertyName("provider")]
    string Provider,

    [property: JsonPropertyName("provider_user_id")]
    string ProviderUserId,

    [property: JsonPropertyName("email")]
    string? Email,

    [property: JsonPropertyName("name")]
    string? Name,

    [property: JsonPropertyName("avatar_url")]
    string? AvatarUrl,

    [property: JsonPropertyName("last_seen")]
    DateTime LastSeen);

