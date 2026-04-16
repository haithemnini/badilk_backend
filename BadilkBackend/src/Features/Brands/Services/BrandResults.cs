using BadilkBackend.src.Features.Brands.Dtos;

namespace BadilkBackend.src.Features.Brands.Services;

public abstract record CreateBrandResult
{
    public sealed record Created(BrandDto Brand) : CreateBrandResult;

    public sealed record InvalidName : CreateBrandResult;
}

public abstract record UpdateBrandResult
{
    public sealed record Saved : UpdateBrandResult;

    public sealed record InvalidName : UpdateBrandResult;

    public sealed record BrandNotFound : UpdateBrandResult;
}
