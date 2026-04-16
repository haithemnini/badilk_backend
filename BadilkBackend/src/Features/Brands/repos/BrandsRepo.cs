using BadilkBackend.src.Features.Brands.Dtos;
using BadilkBackend.src.Features.Brands.Models;
using BadilkBackend.src.Infra.Database;
using Microsoft.EntityFrameworkCore;

namespace BadilkBackend.src.Features.Brands.Repos;

public sealed class BrandsRepo(AppDbContext db) : IBrandsRepo
{
    public async Task<IReadOnlyList<BrandDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var brands = await db.Brands
            .AsNoTracking()
            .OrderBy(b => b.Id)
            .Select(b => new BrandDto(b.Id, b.Name, b.LogoUrl, b.BannerUrl, b.CreatedAt, b.UpdatedAt))
            .ToListAsync(cancellationToken);

        return brands;
    }

    public async Task<BrandDto?> GetByIdAsync(Guid brandId, CancellationToken cancellationToken = default)
    {
        var brand = await db.Set<Brand>()
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == brandId, cancellationToken);

        if (brand is null)
            return null;

        return new BrandDto(brand.Id, brand.Name, brand.LogoUrl, brand.BannerUrl, brand.CreatedAt, brand.UpdatedAt);

    }

    public async Task<BrandDto> CreateAsync(CreateBrandRequest request, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var brand = new Brand
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            LogoUrl = request.LogoUrl,
            BannerUrl = request.BannerUrl,
            CreatedAt = now,
            UpdatedAt = now,
        };

        db.Add(brand);
        await db.SaveChangesAsync(cancellationToken);

        return new BrandDto(brand.Id, brand.Name, brand.LogoUrl, brand.BannerUrl, brand.CreatedAt, brand.UpdatedAt);
    }

    public async Task<bool> UpdateAsync(Guid brandId, UpdateBrandRequest request, CancellationToken cancellationToken = default)
    {
        var brand = await db.Set<Brand>().FirstOrDefaultAsync(b => b.Id == brandId, cancellationToken);
        if (brand is null)
            return false;

        brand.Name = request.Name?.Trim() ?? brand.Name;
        brand.LogoUrl = request.LogoUrl ?? brand.LogoUrl;
        brand.BannerUrl = request.BannerUrl ?? brand.BannerUrl;
        brand.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid brandId, CancellationToken cancellationToken = default)
    {
        var brand = await db.Brands.FirstOrDefaultAsync(b => b.Id == brandId, cancellationToken);
        if (brand is null)
            return false;

        db.Remove(brand);
        await db.SaveChangesAsync(cancellationToken);
        return true;
    }


}

