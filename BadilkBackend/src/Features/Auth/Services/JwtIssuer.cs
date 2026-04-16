using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BadilkBackend.src.Features.Auth.Options;
using BadilkBackend.src.Features.Users.Dtos;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BadilkBackend.src.Features.Auth.Services;

public sealed class JwtIssuer(IOptions<JwtOptions> options) : IJwtIssuer
{
    public string CreateAccessToken(UserDto user, ProfileTokenClaims profile)
    {
        if (string.IsNullOrWhiteSpace(options.Value.SigningKey))
            throw new InvalidOperationException($"Missing config: {JwtOptions.SectionName}:SigningKey");

        var now = DateTime.UtcNow;
        var expires = now.AddMinutes(options.Value.AccessTokenMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new("provider", user.Provider),
            new("provider_user_id", user.ProviderUserId),
        };

        if (!string.IsNullOrWhiteSpace(user.Email))
            claims.Add(new(JwtRegisteredClaimNames.Email, user.Email));

        claims.Add(new("role", profile.Role));
        claims.Add(new("status", profile.Status));
        claims.Add(new("plan", profile.Plan));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SigningKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: options.Value.Issuer,
            audience: options.Value.Audience,
            claims: claims,
            notBefore: now,
            expires: expires,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

