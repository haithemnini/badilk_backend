namespace BadilkBackend.src.Features.Users.Dtos;

using System.Text.Json.Serialization;

public sealed record UserWithProfileDto(
    [property: JsonPropertyName("user")]
    UserDto User,

    [property: JsonPropertyName("profile")]
    ProfileDto Profile);

