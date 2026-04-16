using Microsoft.EntityFrameworkCore;
using BadilkBackend.src.Features.Banners.Models;
using BadilkBackend.src.Features.Brands.Models;

namespace BadilkBackend.src.Infra.Database;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Brand> Brands => Set<Brand>();

    public DbSet<Banner> Banners => Set<Banner>();
}

