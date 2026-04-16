namespace BadilkBackend.src.Features.Banners.Dtos;

public sealed class UpdateBannerRequest
{
    public string? Title { get; init; }

    public string? ImageUrl { get; init; }

    public string? LinkUrl { get; init; }

    public bool? IsEnabled { get; init; }

    public DateTime? ExpiryDate { get; init; }
}
