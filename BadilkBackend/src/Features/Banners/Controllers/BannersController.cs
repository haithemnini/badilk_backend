using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using BadilkBackend.src.Core.Dtos.Responses;
using BadilkBackend.src.Features.Banners.Dtos;
using BadilkBackend.src.Features.Banners.Services;

namespace BadilkBackend.src.Features.Banners.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
[ApiVersion(1.0)]
public class BannersController(IBannersService banners) : ControllerBase
{
    private readonly IBannersService _banners = banners;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<BannerDto>>>> Get(CancellationToken cancellationToken)
    {
        var list = await _banners.GetAllAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<BannerDto>>.Ok(list));
    }

    [HttpGet("active")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<BannerDto>>>> GetActive(CancellationToken cancellationToken)
    {
        var list = await _banners.GetActiveAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<BannerDto>>.Ok(list));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<BannerDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var banner = await _banners.GetByIdAsync(id, cancellationToken);
        return banner is null
            ? NotFound(ApiResponse<BannerDto>.Fail("Banner not found", 404))
            : Ok(ApiResponse<BannerDto>.Ok(banner));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<BannerDto>>> Create([FromBody] CreateBannerRequest request, CancellationToken cancellationToken)
    {
        return await _banners.CreateAsync(request, cancellationToken) switch
        {
            CreateBannerResult.Created created => CreatedAtAction(nameof(GetById),
                new { id = created.Banner.Id, version = HttpContext.GetRequestedApiVersion()?.ToString() ?? "1.0" },
                ApiResponse<BannerDto>.Ok(created.Banner, "Created", 201)),
            CreateBannerResult.InvalidPayload => BadRequest(ApiResponse<BannerDto>.Fail("Title, image_url, and link_url are required", 400)),
            _ => throw new InvalidOperationException(),
        };
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponse<BannerDto>>> Update(Guid id, [FromBody] UpdateBannerRequest request, CancellationToken cancellationToken)
    {
        return await _banners.UpdateAsync(id, request, cancellationToken) switch
        {
            UpdateBannerResult.Saved => Ok(ApiResponse<BannerDto>.Ok()),
            UpdateBannerResult.InvalidPayload => BadRequest(ApiResponse<BannerDto>.Fail("Title, image_url, and link_url are required", 400)),
            UpdateBannerResult.BannerNotFound => NotFound(ApiResponse<BannerDto>.Fail("Banner not found", 404)),
            _ => throw new InvalidOperationException(),
        };
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponse<BannerDto>>> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _banners.DeleteAsync(id, cancellationToken);
        return deleted ? Ok(ApiResponse<BannerDto>.Ok()) : NotFound(ApiResponse<BannerDto>.Fail("Banner not found", 404));
    }
}
