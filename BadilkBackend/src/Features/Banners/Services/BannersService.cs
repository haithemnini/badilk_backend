using BadilkBackend.src.Features.Banners.Dtos;
using BadilkBackend.src.Features.Banners.Repos;

namespace BadilkBackend.src.Features.Banners.Services;

public sealed class BannersService(IBannersRepo banners) : IBannersService
{
    public Task<IReadOnlyList<BannerDto>> GetAllAsync(CancellationToken cancellationToken = default) =>
        banners.GetAllAsync(cancellationToken);

    public Task<IReadOnlyList<BannerDto>> GetActiveAsync(CancellationToken cancellationToken = default) =>
        banners.GetActiveAsync(DateTime.UtcNow, cancellationToken);

    public Task<BannerDto?> GetByIdAsync(Guid bannerId, CancellationToken cancellationToken = default) =>
        banners.GetByIdAsync(bannerId, cancellationToken);

    public async Task<CreateBannerResult> CreateAsync(CreateBannerRequest request, CancellationToken cancellationToken = default)
    {
        if (!IsValidPayload(request.Title, request.ImageUrl))
            return new CreateBannerResult.InvalidPayload();

        var created = await banners.CreateAsync(request, cancellationToken);
        return new CreateBannerResult.Created(created);
    }

    public async Task<UpdateBannerResult> UpdateAsync(Guid bannerId, UpdateBannerRequest request, CancellationToken cancellationToken = default)
    {
        if (!IsValidPayload(request.Title, request.ImageUrl))
            return new UpdateBannerResult.InvalidPayload();

        var updated = await banners.UpdateAsync(bannerId, request, cancellationToken);
        return updated ? new UpdateBannerResult.Saved() : new UpdateBannerResult.BannerNotFound();
    }

    public Task<bool> DeleteAsync(Guid bannerId, CancellationToken cancellationToken = default) =>
        banners.DeleteAsync(bannerId, cancellationToken);

    private static bool IsValidPayload(string? title, string? imageUrl) =>
        !string.IsNullOrWhiteSpace(title)
        && !string.IsNullOrWhiteSpace(imageUrl);

}
