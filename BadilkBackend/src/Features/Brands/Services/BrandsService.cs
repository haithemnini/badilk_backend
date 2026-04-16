using BadilkBackend.src.Features.Brands.Dtos;
using BadilkBackend.src.Features.Brands.Repos;

namespace BadilkBackend.src.Features.Brands.Services;

public sealed class BrandsService(IBrandsRepo brands) : IBrandsService
{
    public Task<IReadOnlyList<BrandDto>> GetAllAsync(CancellationToken cancellationToken = default) =>
        brands.GetAllAsync(cancellationToken);

    public Task<BrandDto?> GetByIdAsync(Guid brandId, CancellationToken cancellationToken = default) =>
        brands.GetByIdAsync(brandId, cancellationToken);

    public async Task<CreateBrandResult> CreateAsync(CreateBrandRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return new CreateBrandResult.InvalidName();

        var created = await brands.CreateAsync(request, cancellationToken);
        return new CreateBrandResult.Created(created);
    }

    public async Task<UpdateBrandResult> UpdateAsync(Guid brandId, UpdateBrandRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return new UpdateBrandResult.InvalidName();

        var updated = await brands.UpdateAsync(brandId, request, cancellationToken);
        return updated ? new UpdateBrandResult.Saved() : new UpdateBrandResult.BrandNotFound();
    }

    public Task<bool> DeleteAsync(Guid brandId, CancellationToken cancellationToken = default) =>
        brands.DeleteAsync(brandId, cancellationToken);
}
