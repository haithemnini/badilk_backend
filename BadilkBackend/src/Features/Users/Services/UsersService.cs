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

    public async Task<UserWithProfileDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await users.GetByIdAsync(id, cancellationToken);
        return user is null || user.Profile is null ? null : ToWithProfileDto(user);
    }

    public async Task<List<UserWithProfileDto>> ListAsync(CancellationToken cancellationToken = default)
    {
        var list = await users.ListAsync(cancellationToken);
        return [.. list
            .Where(u => u.Profile is not null)
            .Select(ToWithProfileDto)];
    }

    public async Task<UserWithProfileDto?> UpdateMeAsync(
        Guid myUserId,
        UpdateMyUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var user = await users.GetByIdAsync(myUserId, cancellationToken);
        if (user is null) return null;

        var now = DateTime.UtcNow;
        user.UpdatedAt = now;

        if (!string.IsNullOrWhiteSpace(request.Name))
            user.Name = request.Name.Trim();

        if (!string.IsNullOrWhiteSpace(request.AvatarUrl))
            user.AvatarUrl = request.AvatarUrl.Trim();

        if (!string.IsNullOrWhiteSpace(request.Email))
            user.Email = request.Email.Trim();

        await users.SaveChangesAsync(cancellationToken);

        if (user.Profile is null) return null;
        return ToWithProfileDto(user);
    }

    public async Task<UserWithProfileDto?> UpdateProfileAsync(
        Guid userId,
        UpdateUserProfileRequest request,
        CancellationToken cancellationToken = default)
    {
        var user = await users.GetByIdAsync(userId, cancellationToken);
        if (user is null) return null;

        var now = DateTime.UtcNow;

        user.Profile ??= new Profile
        {
            UserId = user.Id,
            Plan = PlanType.Free,
            ShowAds = true,
            StartDate = now,
            CreatedAt = now,
            UpdatedAt = now,
        };

        var profile = user.Profile;
        profile.UpdatedAt = now;

        if (!string.IsNullOrWhiteSpace(request.Role))
            profile.Role = ParseEnumOrThrow<UserRole>(request.Role);

        if (!string.IsNullOrWhiteSpace(request.Status))
            profile.Status = ParseEnumOrThrow<UserStatus>(request.Status);

        if (request.ShowAds is not null)
            profile.ShowAds = request.ShowAds.Value;

        if (!string.IsNullOrWhiteSpace(request.Plan))
        {
            var newPlan = ParseEnumOrThrow<PlanType>(request.Plan);

            if (newPlan is PlanType.Pro or PlanType.Premium)
            {
                if (request.ExpiryDate is null)
                    throw new ArgumentException("expiry_date is required when plan is pro/premium");

                profile.Plan = newPlan;
                profile.ExpiryDate = DateTime.SpecifyKind(request.ExpiryDate.Value, DateTimeKind.Utc);
            }
            else
            {
                // Free
                profile.Plan = PlanType.Free;
                profile.ExpiryDate = null;
            }
        }

        await users.SaveChangesAsync(cancellationToken);

        return ToWithProfileDto(user);
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

    private static UserWithProfileDto ToWithProfileDto(User user) =>
        new(ToDto(user), ToDto(user.Profile!));

    private static TEnum ParseEnumOrThrow<TEnum>(string value)
        where TEnum : struct, Enum
    {
        var trimmed = value.Trim();
        if (Enum.TryParse<TEnum>(trimmed, ignoreCase: true, out var parsed))
            return parsed;

        throw new ArgumentException($"Invalid value '{trimmed}'");
    }
}

