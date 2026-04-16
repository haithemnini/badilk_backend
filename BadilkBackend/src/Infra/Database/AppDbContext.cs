using Microsoft.EntityFrameworkCore;
using BadilkBackend.src.Features.Banners.Models;
using BadilkBackend.src.Features.Brands.Models;
using BadilkBackend.src.Features.Users.Models;
using BadilkBackend.src.Core.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BadilkBackend.src.Infra.Database;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Brand> Brands => Set<Brand>();

    public DbSet<Banner> Banners => Set<Banner>();

    public DbSet<User> Users => Set<User>();

    public DbSet<Profile> Profiles => Set<Profile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(b =>
        {
            b.ToTable("Users");
            b.HasKey(x => x.Id);

            b.Property(x => x.Provider).IsRequired();
            b.Property(x => x.ProviderUserId).IsRequired();

            b.HasIndex(x => new { x.Provider, x.ProviderUserId }).IsUnique();
            b.HasIndex(x => x.Email);

            b.Property(x => x.RawUserMeta)
                .HasColumnType("jsonb")
                .HasDefaultValueSql("'{}'::jsonb");

            b.Property(x => x.CreatedAt).HasDefaultValueSql("now()");
            b.Property(x => x.UpdatedAt).HasDefaultValueSql("now()");
            b.Property(x => x.LastSeen).HasDefaultValueSql("now()");

            b.HasOne(x => x.Profile)
                .WithOne(x => x.User)
                .HasForeignKey<Profile>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Profile>(b =>
        {
            b.ToTable("Profiles");
            b.HasKey(x => x.UserId);

            var userRoleConverter = new ValueConverter<UserRole, string>(
                v => v.ToString().ToLowerInvariant(),
                v => Enum.Parse<UserRole>(v, true));

            var userStatusConverter = new ValueConverter<UserStatus, string>(
                v => v.ToString().ToLowerInvariant(),
                v => Enum.Parse<UserStatus>(v, true));

            var planTypeConverter = new ValueConverter<PlanType, string>(
                v => v.ToString().ToLowerInvariant(),
                v => Enum.Parse<PlanType>(v, true));

            b.Property(x => x.Role)
                .HasConversion(userRoleConverter)
                .HasDefaultValue(UserRole.User);

            b.Property(x => x.Status)
                .HasConversion(userStatusConverter)
                .HasDefaultValue(UserStatus.Active);

            b.Property(x => x.Plan)
                .HasConversion(planTypeConverter)
                .HasDefaultValue(PlanType.Free);

            b.Property(x => x.StartDate).HasDefaultValueSql("now()");
            b.Property(x => x.CreatedAt).HasDefaultValueSql("now()");
            b.Property(x => x.UpdatedAt).HasDefaultValueSql("now()");

            b.Property(x => x.ShowAds).HasDefaultValue(true);
        });
    }
}

