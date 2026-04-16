using BadilkBackend.src.Features.Brands.Dtos;

namespace BadilkBackend.src.Features.Brands.Services;

public interface IBrandsService
{
    Task<IReadOnlyList<BrandDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<BrandDto?> GetByIdAsync(Guid brandId, CancellationToken cancellationToken = default);

    Task<CreateBrandResult> CreateAsync(CreateBrandRequest request, CancellationToken cancellationToken = default);

    Task<UpdateBrandResult> UpdateAsync(Guid brandId, UpdateBrandRequest request, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(Guid brandId, CancellationToken cancellationToken = default);
}
