using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using BadilkBackend.src.Features.Auth.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace BadilkBackend.src.Features.Auth.Services;

public sealed class GoogleTokenVerifier(IOptions<GoogleOidcOptions> options) : ITokenVerifier
{
    private static readonly ConfigurationManager<OpenIdConnectConfiguration> _configurationManager =
        new(
            "https://accounts.google.com/.well-known/openid-configuration",
            new OpenIdConnectConfigurationRetriever(),
            new HttpDocumentRetriever { RequireHttps = true });

    public async Task<SocialClaims> VerifyGoogleAsync(string idToken, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(options.Value.ClientId))
            throw new InvalidOperationException($"Missing config: {GoogleOidcOptions.SectionName}:ClientId");

        try
        {
            var config = await _configurationManager.GetConfigurationAsync(cancellationToken);

            var handler = new JwtSecurityTokenHandler
            {
                // Prevent inbound claim type mapping (e.g. "sub" -> ClaimTypes.NameIdentifier)
                MapInboundClaims = false
            };
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuers = ["accounts.google.com", "https://accounts.google.com"],
                ValidateAudience = true,
                ValidAudience = options.Value.ClientId,
                ValidateLifetime = true,
                RequireSignedTokens = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKeys = config.SigningKeys,
                ClockSkew = TimeSpan.FromMinutes(2),
            };

            var principal = handler.ValidateToken(idToken, parameters, out var validatedToken);

            if (validatedToken is not JwtSecurityToken jwt)
                throw new TokenVerificationException("Invalid token");

            var sub =
                principal.FindFirstValue("sub") ??
                principal.FindFirstValue(ClaimTypes.NameIdentifier) ??
                jwt.Payload.Sub;
            if (string.IsNullOrWhiteSpace(sub))
                throw new TokenVerificationException("Missing sub claim");

            var email =
                principal.FindFirstValue("email") ??
                principal.FindFirstValue(ClaimTypes.Email);
            var emailVerifiedRaw = principal.FindFirstValue("email_verified");
            _ = bool.TryParse(emailVerifiedRaw, out var emailVerified);

            var name = principal.FindFirstValue("name");
            var picture = principal.FindFirstValue("picture");

            var raw = JsonDocument.Parse(jwt.Payload.SerializeToJson());

            return new SocialClaims(
                Provider: "google",
                ProviderUserId: sub,
                Email: email,
                EmailVerified: emailVerified,
                Name: name,
                AvatarUrl: picture,
                Raw: raw);
        }
        catch (SecurityTokenException ex)
        {
            throw new TokenVerificationException(ex.Message);
        }
    }
}

