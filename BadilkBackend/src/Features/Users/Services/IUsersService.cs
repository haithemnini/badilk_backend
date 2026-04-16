using BadilkBackend.src.Features.Auth.Services;
using BadilkBackend.src.Features.Users.Dtos;

namespace BadilkBackend.src.Features.Users.Services;

public interface IUsersService
{
    Task<(UserDto User, ProfileDto Profile, bool IsNew)> UpsertFromSocialClaimsAsync(
        SocialClaims claims,
        CancellationToken cancellationToken = default);

    Task<UserWithProfileDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<UserWithProfileDto>> ListAsync(CancellationToken cancellationToken = default);

    Task<UserWithProfileDto?> UpdateMeAsync(Guid myUserId, UpdateMyUserRequest request, CancellationToken cancellationToken = default);

    Task<UserWithProfileDto?> UpdateProfileAsync(Guid userId, UpdateUserProfileRequest request, CancellationToken cancellationToken = default);
}

