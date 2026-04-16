using BadilkBackend.src.Features.Banners.Dtos;

namespace BadilkBackend.src.Features.Banners.Services;

public interface IBannersService
{
    Task<IReadOnlyList<BannerDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<BannerDto>> GetActiveAsync(CancellationToken cancellationToken = default);

    Task<BannerDto?> GetByIdAsync(Guid bannerId, CancellationToken cancellationToken = default);

    Task<CreateBannerResult> CreateAsync(CreateBannerRequest request, CancellationToken cancellationToken = default);

    Task<UpdateBannerResult> UpdateAsync(Guid bannerId, UpdateBannerRequest request, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(Guid bannerId, CancellationToken cancellationToken = default);
}
