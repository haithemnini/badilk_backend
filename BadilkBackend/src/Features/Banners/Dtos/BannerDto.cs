namespace BadilkBackend.src.Features.Banners.Dtos;

using System.Text.Json.Serialization;

public sealed record BannerDto(
    [property: JsonPropertyName("id")]
    Guid Id,

    [property: JsonPropertyName("title")]
    string Title,

    [property: JsonPropertyName("image_url")]
    string ImageUrl,

    [property: JsonPropertyName("link_url")]
    string? LinkUrl,

    [property: JsonPropertyName("is_enabled")]
    bool IsEnabled,

    [property: JsonPropertyName("expiry_date")]
    DateTime? ExpiryDate,

    [property: JsonPropertyName("created_at")]
    DateTime CreatedAt,

    [property: JsonPropertyName("updated_at")]
    DateTime UpdatedAt
);
