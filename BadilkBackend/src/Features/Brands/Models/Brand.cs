namespace BadilkBackend.src.Features.Brands.Models;

public sealed class Brand
{
    public Guid Id { get; set; }
    public required string Name { get; set; }

    public string? LogoUrl { get; set; }
    public string? BannerUrl { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
