namespace BadilkBackend.src.Features.Users.Dtos;

using System.Text.Json.Serialization;

public sealed record ProfileDto(
    [property: JsonPropertyName("role")]
    string Role,

    [property: JsonPropertyName("status")]
    string Status,

    [property: JsonPropertyName("plan")]
    string Plan,

    [property: JsonPropertyName("start_date")]
    DateTime StartDate,

    [property: JsonPropertyName("expiry_date")]
    DateTime? ExpiryDate,

    [property: JsonPropertyName("show_ads")]
    bool ShowAds);

