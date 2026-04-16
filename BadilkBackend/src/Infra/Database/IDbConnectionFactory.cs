using System.Data.Common;

namespace BadilkBackend.src.Infra.Database;

public interface IDbConnectionFactory
{
    Task<DbConnection> OpenConnectionAsync(CancellationToken cancellationToken = default);
}

