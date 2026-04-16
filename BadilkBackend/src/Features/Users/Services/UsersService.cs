using BadilkBackend.src.Features.Auth.Services;
using BadilkBackend.src.Features.Users.Dtos;
using BadilkBackend.src.Features.Users.Models;
using BadilkBackend.src.Features.Users.Repos;
using BadilkBackend.src.Core.Enums;

namespace BadilkBackend.src.Features.Users.Services;

public sealed class UsersService(IUsersRepo users) : IUsersService
{
    public async Task<(UserDto User, ProfileDto Profile, bool IsNew)> UpsertFromSocialClaimsAsync(
        SocialClaims claims,
        CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        var existing = await users.GetByProviderAsync(claims.Provider, claims.ProviderUserId, cancellationToken);
        if (existing is null)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Provider = claims.Provider,
                ProviderUserId = claims.ProviderUserId,
                Email = claims.Email,
                Name = claims.Name,
                AvatarUrl = claims.AvatarUrl,
                RawUserMeta = claims.Raw,
                CreatedAt = now,
                UpdatedAt = now,
                LastSeen = now,
                Profile = new Profile
                {
                    UserId = Guid.Empty, // set by relationship
                    Plan = PlanType.Free,
                    ShowAds = true,
                    StartDate = now,
                    CreatedAt = now,
                    UpdatedAt = now,
                },
            };

            // ensure FK correct
            user.Profile.UserId = user.Id;

            await users.AddAsync(user, cancellationToken);
            await users.SaveChangesAsync(cancellationToken);

            return (ToDto(user), ToDto(user.Profile), true);
        }

        existing.LastSeen = now;
        existing.UpdatedAt = now;

        existing.Email = claims.Email ?? existing.Email;
        existing.Name = claims.Name ?? existing.Name;
        existing.AvatarUrl = claims.AvatarUrl ?? existing.AvatarUrl;
        existing.RawUserMeta = claims.Raw;

        if (existing.Profile is null)
        {
            existing.Profile = new Profile
            {
                UserId = existing.Id,
                Plan = PlanType.Free,
                ShowAds = true,
                StartDate = now,
                CreatedAt = now,
                UpdatedAt = now,
            };
        }
        else
        {
            existing.Profile.UpdatedAt = now;
        }

        await users.SaveChangesAsync(cancellationToken);

        return (ToDto(existing), ToDto(existing.Profile), false);
    }

    private static UserDto ToDto(User user) =>
        new(
            user.Id,
            user.Provider,
            user.ProviderUserId,
            user.Email,
            user.Name,
            user.AvatarUrl,
            user.LastSeen);

    private static ProfileDto ToDto(Profile profile) =>
        new(
            profile.Role.ToString().ToLowerInvariant(),
            profile.Status.ToString().ToLowerInvariant(),
            profile.Plan.ToString().ToLowerInvariant(),
            profile.StartDate,
            profile.ExpiryDate,
            profile.ShowAds);
}

