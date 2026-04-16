namespace BadilkBackend.src.Features.Brands.Dtos;

using System.Text.Json.Serialization;

public sealed record BrandDto(

    [property: JsonPropertyName("id")]
    Guid Id,

    [property: JsonPropertyName("name")]
    string Name,

    [property: JsonPropertyName("logo_url")]
    string? LogoUrl,

    [property: JsonPropertyName("banner_url")]
    string? BannerUrl,

    [property: JsonPropertyName("created_at")]
    DateTime CreatedAt,

    [property: JsonPropertyName("updated_at")]
    DateTime UpdatedAt
    );

