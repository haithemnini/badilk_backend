namespace BadilkBackend.src.Features.Users.Dtos;

using System.Text.Json.Serialization;

public sealed record UpdateUserProfileRequest(
    // expected values: "user" | "admin"
    [property: JsonPropertyName("role")]
    string? Role,

    // expected values: "active" | "suspended" | "banned"
    [property: JsonPropertyName("status")]
    string? Status,

    // expected values: "free" | "pro" | "premium"
    [property: JsonPropertyName("plan")]
    string? Plan,

    [property: JsonPropertyName("expiry_date")]
    DateTime? ExpiryDate,

    [property: JsonPropertyName("show_ads")]
    bool? ShowAds);

