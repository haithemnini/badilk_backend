using BadilkBackend.src.Features.Users.Dtos;

namespace BadilkBackend.src.Features.Auth.Services;

public interface IJwtIssuer
{
    string CreateAccessToken(UserDto user, ProfileTokenClaims profile);
}

public sealed record ProfileTokenClaims(string Role, string Status, string Plan);

