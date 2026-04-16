namespace BadilkBackend.src.Features.Users.Dtos;

using System.Text.Json.Serialization;

public sealed record UpdateMyUserRequest(
    [property: JsonPropertyName("name")]
    string? Name,

    [property: JsonPropertyName("avatar_url")]
    string? AvatarUrl,

    [property: JsonPropertyName("email")]
    string? Email);

