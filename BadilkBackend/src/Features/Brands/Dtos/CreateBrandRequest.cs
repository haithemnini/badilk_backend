namespace BadilkBackend.src.Features.Brands.Dtos;

public sealed class CreateBrandRequest
{
    public required string Name { get; init; }
    public string? LogoUrl { get; init; }
    public string? BannerUrl { get; init; }
}

