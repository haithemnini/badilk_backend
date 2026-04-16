using BadilkBackend.src.Features.Banners.Dtos;
using BadilkBackend.src.Features.Banners.Models;
using BadilkBackend.src.Infra.Database;
using Microsoft.EntityFrameworkCore;

namespace BadilkBackend.src.Features.Banners.Repos;

public sealed class BannersRepo(AppDbContext db) : IBannersRepo
{
    public async Task<IReadOnlyList<BannerDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var banners = await db.Banners
            .AsNoTracking()
            .OrderBy(b => b.Id)
            .Select(b => new BannerDto(b.Id, b.Title, b.ImageUrl, b.LinkUrl, b.IsEnabled, b.ExpiryDate, b.CreatedAt, b.UpdatedAt))
            .ToListAsync(cancellationToken);

        return banners;
    }

    public async Task<IReadOnlyList<BannerDto>> GetActiveAsync(DateTime utcNow, CancellationToken cancellationToken = default)
    {
        var banners = await db.Banners
            .AsNoTracking()
            .Where(b => b.IsEnabled && (b.ExpiryDate == null || b.ExpiryDate > utcNow))
            .OrderBy(b => b.Id)
            .Select(b => new BannerDto(b.Id, b.Title, b.ImageUrl, b.LinkUrl, b.IsEnabled, b.ExpiryDate, b.CreatedAt, b.UpdatedAt))
            .ToListAsync(cancellationToken);

        return banners;
    }

    public async Task<BannerDto?> GetByIdAsync(Guid bannerId, CancellationToken cancellationToken = default)
    {
        var banner = await db.Set<Banner>()
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == bannerId, cancellationToken);

        if (banner is null)
            return null;

        return new BannerDto(banner.Id, banner.Title, banner.ImageUrl, banner.LinkUrl, banner.IsEnabled, banner.ExpiryDate, banner.CreatedAt, banner.UpdatedAt);
    }

    public async Task<BannerDto> CreateAsync(CreateBannerRequest request, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var banner = new Banner
        {
            Id = Guid.NewGuid(),
            Title = request.Title.Trim(),
            ImageUrl = request.ImageUrl.Trim(),
            LinkUrl = request.LinkUrl?.Trim(),
            IsEnabled = request.IsEnabled,
            ExpiryDate = request.ExpiryDate,
            CreatedAt = now,
            UpdatedAt = now,
        };

        db.Add(banner);
        await db.SaveChangesAsync(cancellationToken);

        return new BannerDto(banner.Id, banner.Title, banner.ImageUrl, banner.LinkUrl, banner.IsEnabled, banner.ExpiryDate, banner.CreatedAt, banner.UpdatedAt);
    }

    public async Task<bool> UpdateAsync(Guid bannerId, UpdateBannerRequest request, CancellationToken cancellationToken = default)
    {
        var banner = await db.Set<Banner>().FirstOrDefaultAsync(b => b.Id == bannerId, cancellationToken);
        if (banner is null)
            return false;

        banner.Title = request.Title?.Trim() ?? banner.Title;
        banner.ImageUrl = request.ImageUrl?.Trim() ?? banner.ImageUrl;
        banner.LinkUrl = request.LinkUrl?.Trim() ?? banner.LinkUrl;
        if (request.IsEnabled.HasValue)
            banner.IsEnabled = request.IsEnabled.Value;
        if (request.ExpiryDate.HasValue)
            banner.ExpiryDate = request.ExpiryDate;
        banner.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid bannerId, CancellationToken cancellationToken = default)
    {
        var banner = await db.Banners.FirstOrDefaultAsync(b => b.Id == bannerId, cancellationToken);
        if (banner is null)
            return false;

        db.Remove(banner);
        await db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
