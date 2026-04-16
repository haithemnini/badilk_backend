using BadilkBackend.src.Features.Users.Models;
using BadilkBackend.src.Infra.Database;
using Microsoft.EntityFrameworkCore;

namespace BadilkBackend.src.Features.Users.Repos;

public sealed class UsersRepo(AppDbContext db) : IUsersRepo
{
    public Task<User?> GetByProviderAsync(string provider, string providerUserId, CancellationToken cancellationToken = default) =>
        db.Users
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(
                u => u.Provider == provider && u.ProviderUserId == providerUserId,
                cancellationToken);

    public Task AddAsync(User user, CancellationToken cancellationToken = default) =>
        db.AddAsync(user, cancellationToken).AsTask();

    public Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        db.SaveChangesAsync(cancellationToken);
}

