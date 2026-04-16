using BadilkBackend.src.Features.Brands.Dtos;

namespace BadilkBackend.src.Features.Brands.Repos;

public interface IBrandsRepo
{
    Task<IReadOnlyList<BrandDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<BrandDto?> GetByIdAsync(Guid brandId, CancellationToken cancellationToken = default);
    Task<BrandDto> CreateAsync(CreateBrandRequest request, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Guid brandId, UpdateBrandRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid brandId, CancellationToken cancellationToken = default);
}

