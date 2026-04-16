namespace BadilkBackend.src.Features.Banners.Dtos;

public sealed class CreateBannerRequest
{
    public required string Title { get; init; }

    public required string ImageUrl { get; init; }

    public string? LinkUrl { get; init; }

    public bool IsEnabled { get; init; } = true;

    public DateTime? ExpiryDate { get; init; }
}
