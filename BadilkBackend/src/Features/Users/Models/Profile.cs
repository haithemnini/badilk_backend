using BadilkBackend.src.Core.Enums;

namespace BadilkBackend.src.Features.Users.Models;

public sealed class Profile
{
    public Guid UserId { get; set; }

    public User? User { get; set; }

    public UserRole Role { get; set; } = UserRole.User;

    public UserStatus Status { get; set; } = UserStatus.Active;

    public PlanType Plan { get; set; } = PlanType.Free;

    public DateTime StartDate { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public bool ShowAds { get; set; } = true;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}

