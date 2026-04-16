using BadilkBackend.src.Features.Banners.Dtos;

namespace BadilkBackend.src.Features.Banners.Repos;

public interface IBannersRepo
{
    Task<IReadOnlyList<BannerDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<BannerDto>> GetActiveAsync(DateTime utcNow, CancellationToken cancellationToken = default);

    Task<BannerDto?> GetByIdAsync(Guid bannerId, CancellationToken cancellationToken = default);

    Task<BannerDto> CreateAsync(CreateBannerRequest request, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(Guid bannerId, UpdateBannerRequest request, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(Guid bannerId, CancellationToken cancellationToken = default);
}
