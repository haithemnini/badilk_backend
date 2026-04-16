namespace BadilkBackend.src.Features.Banners.Models;

public sealed class Banner
{
    public Guid Id { get; set; }

    public required string Title { get; set; }

    public required string ImageUrl { get; set; }

    public string? LinkUrl { get; set; }

    public bool IsEnabled { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
