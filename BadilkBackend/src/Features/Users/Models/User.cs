using System.Text.Json;

namespace BadilkBackend.src.Features.Users.Models;

public sealed class User
{
    public Guid Id { get; set; }

    public required string Provider { get; set; }

    public required string ProviderUserId { get; set; }

    public string? Email { get; set; }

    public string? Name { get; set; }

    public string? AvatarUrl { get; set; }

    public JsonDocument RawUserMeta { get; set; } = JsonDocument.Parse("{}");

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime LastSeen { get; set; }

    public Profile? Profile { get; set; }
}

