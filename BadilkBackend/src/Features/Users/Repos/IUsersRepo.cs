using BadilkBackend.src.Features.Users.Models;

namespace BadilkBackend.src.Features.Users.Repos;

public interface IUsersRepo
{
    Task<User?> GetByProviderAsync(string provider, string providerUserId, CancellationToken cancellationToken = default);

    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<User>> ListAsync(CancellationToken cancellationToken = default);

    Task AddAsync(User user, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

