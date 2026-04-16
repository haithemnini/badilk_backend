using Asp.Versioning;
using BadilkBackend.src.Core.Dtos.Responses;
using BadilkBackend.src.Features.Auth.Dtos;
using BadilkBackend.src.Features.Auth.Services;
using BadilkBackend.src.Features.Users.Services;
using Microsoft.AspNetCore.Mvc;

namespace BadilkBackend.src.Features.Auth.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
[ApiVersion(1.0)]
public sealed class AuthController(
    ITokenVerifier tokenVerifier,
    IUsersService users,
    IJwtIssuer jwtIssuer) : ControllerBase
{
    [HttpPost("social/login")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> SocialLogin(
        [FromBody] SocialLoginRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Provider))
            return BadRequest(ApiResponse<AuthResponseDto>.Fail("provider is required", 400));

        if (string.IsNullOrWhiteSpace(request.IdToken))
            return BadRequest(ApiResponse<AuthResponseDto>.Fail("id_token is required", 400));

        try
        {
            SocialClaims claims = request.Provider.Trim().ToLowerInvariant() switch
            {
                "google" => await tokenVerifier.VerifyGoogleAsync(request.IdToken, cancellationToken),
                _ => throw new TokenVerificationException("Unsupported provider"),
            };

            var (user, profile, _) = await users.UpsertFromSocialClaimsAsync(claims, cancellationToken);

            var accessToken = jwtIssuer.CreateAccessToken(
                user,
                new ProfileTokenClaims(
                    profile.Role.ToLowerInvariant(),
                    profile.Status.ToLowerInvariant(),
                    profile.Plan.ToLowerInvariant()));

            var response = new AuthResponseDto(accessToken, user, profile);
            return Ok(ApiResponse<AuthResponseDto>.Ok(response));
        }
        catch (TokenVerificationException ex)
        {
            return BadRequest(ApiResponse<AuthResponseDto>.Fail(ex.Message, 400));
        }
    }
}

