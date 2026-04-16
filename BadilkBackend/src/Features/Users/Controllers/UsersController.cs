using Asp.Versioning;
using BadilkBackend.src.Core.Dtos.Responses;
using BadilkBackend.src.Features.Users.Dtos;
using BadilkBackend.src.Features.Users.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BadilkBackend.src.Features.Users.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
[ApiVersion(1.0)]
[Authorize]
public sealed class UsersController(IUsersService users) : ControllerBase
{
    [HttpGet("me")]
    public async Task<ActionResult<ApiResponse<UserWithProfileDto>>> GetMe(CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var myUserId))
            return Unauthorized(ApiResponse<UserWithProfileDto>.Fail("Unauthorized", 401));

        var dto = await users.GetByIdAsync(myUserId, cancellationToken);
        return dto is null
            ? NotFound(ApiResponse<UserWithProfileDto>.Fail("User not found", 404))
            : Ok(ApiResponse<UserWithProfileDto>.Ok(dto));
    }

    [HttpPut("me")]
    public async Task<ActionResult<ApiResponse<UserWithProfileDto>>> UpdateMe(
        [FromBody] UpdateMyUserRequest request,
        CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var myUserId))
            return Unauthorized(ApiResponse<UserWithProfileDto>.Fail("Unauthorized", 401));

        var dto = await users.UpdateMeAsync(myUserId, request, cancellationToken);
        return dto is null
            ? NotFound(ApiResponse<UserWithProfileDto>.Fail("User not found", 404))
            : Ok(ApiResponse<UserWithProfileDto>.Ok(dto));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<UserWithProfileDto>>>> List(CancellationToken cancellationToken)
    {
        if (!IsAdmin())
            return Forbid();

        var list = await users.ListAsync(cancellationToken);
        return Ok(ApiResponse<List<UserWithProfileDto>>.Ok(list));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<UserWithProfileDto>>> GetById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        if (!IsAdmin())
            return Forbid();

        var dto = await users.GetByIdAsync(id, cancellationToken);
        return dto is null
            ? NotFound(ApiResponse<UserWithProfileDto>.Fail("User not found", 404))
            : Ok(ApiResponse<UserWithProfileDto>.Ok(dto));
    }

    [HttpPut("{id:guid}/profile")]
    public async Task<ActionResult<ApiResponse<UserWithProfileDto>>> UpdateProfile(
        [FromRoute] Guid id,
        [FromBody] UpdateUserProfileRequest request,
        CancellationToken cancellationToken)
    {
        if (!IsAdmin())
            return Forbid();

        try
        {
            var dto = await users.UpdateProfileAsync(id, request, cancellationToken);
            return dto is null
                ? NotFound(ApiResponse<UserWithProfileDto>.Fail("User not found", 404))
                : Ok(ApiResponse<UserWithProfileDto>.Ok(dto));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<UserWithProfileDto>.Fail(ex.Message, 400));
        }
    }

    private bool IsAdmin()
    {
        var role = User.FindFirstValue("role");
        return string.Equals(role, "admin", StringComparison.OrdinalIgnoreCase);
    }

    private bool TryGetUserId(out Guid userId)
    {
        userId = default;
        var sub = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        return Guid.TryParse(sub, out userId);
    }
}

