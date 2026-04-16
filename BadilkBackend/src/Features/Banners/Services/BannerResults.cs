using BadilkBackend.src.Features.Banners.Dtos;

namespace BadilkBackend.src.Features.Banners.Services;

public abstract record CreateBannerResult
{
    public sealed record Created(BannerDto Banner) : CreateBannerResult;

    public sealed record InvalidPayload : CreateBannerResult;
}

public abstract record UpdateBannerResult
{
    public sealed record Saved : UpdateBannerResult;

    public sealed record InvalidPayload : UpdateBannerResult;

    public sealed record BannerNotFound : UpdateBannerResult;
}
