using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using BadilkBackend.src.Core.Dtos.Responses;
using BadilkBackend.src.Features.Brands.Dtos;
using BadilkBackend.src.Features.Brands.Services;

namespace BadilkBackend.src.Features.Brands.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
[ApiVersion(1.0)]
public class BrandsController(IBrandsService brands) : ControllerBase
{
    private readonly IBrandsService _brands = brands;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<BrandDto>>>> Get(CancellationToken cancellationToken)
    {
        var brands = await _brands.GetAllAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<BrandDto>>.Ok(brands));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<BrandDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var brand = await _brands.GetByIdAsync(id, cancellationToken);
        return brand is null
            ? NotFound(ApiResponse<BrandDto>.Fail("Brand not found", 404))
            : Ok(ApiResponse<BrandDto>.Ok(brand));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<BrandDto>>> Create([FromBody] CreateBrandRequest request, CancellationToken cancellationToken)
    {
        return await _brands.CreateAsync(request, cancellationToken) switch
        {
            CreateBrandResult.Created created => CreatedAtAction(nameof(GetById),
                new { id = created.Brand.Id, version = HttpContext.GetRequestedApiVersion()?.ToString() ?? "1.0" },
                ApiResponse<BrandDto>.Ok(created.Brand, "Created", 201)),
            CreateBrandResult.InvalidName => BadRequest(ApiResponse<BrandDto>.Fail("Name is required", 400)),
            _ => throw new InvalidOperationException(),
        };
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponse<BrandDto>>> Update(Guid id, [FromBody] UpdateBrandRequest request, CancellationToken cancellationToken)
    {
        return await _brands.UpdateAsync(id, request, cancellationToken) switch
        {
            UpdateBrandResult.Saved => Ok(ApiResponse<BrandDto>.Ok()),
            UpdateBrandResult.InvalidName => BadRequest(ApiResponse<BrandDto>.Fail("Name is required", 400)),
            UpdateBrandResult.BrandNotFound => NotFound(ApiResponse<BrandDto>.Fail("Brand not found", 404)),
            _ => throw new InvalidOperationException(),
        };
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponse<BrandDto>>> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _brands.DeleteAsync(id, cancellationToken);
        return deleted ? Ok(ApiResponse<BrandDto>.Ok()) : NotFound(ApiResponse<BrandDto>.Fail("Brand not found", 404));
    }
}
