namespace BadilkBackend.src.Features.Auth.Dtos;

using System.Text.Json.Serialization;
using BadilkBackend.src.Features.Users.Dtos;

public sealed record AuthResponseDto(
    [property: JsonPropertyName("access_token")]
    string AccessToken,

    // [property: JsonPropertyName("is_new")]
    // bool IsNew,

    [property: JsonPropertyName("user")]
    UserDto User,

    [property: JsonPropertyName("profile")]
    ProfileDto Profile);

