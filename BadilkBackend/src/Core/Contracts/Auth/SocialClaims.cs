using System.Text.Json;

namespace BadilkBackend.src.Core.Contracts.Auth;

public sealed record SocialClaims(
    string Provider,
    string ProviderUserId,
    string? Email,
    bool EmailVerified,
    string? Name,
    string? AvatarUrl,
    JsonDocument Raw);

