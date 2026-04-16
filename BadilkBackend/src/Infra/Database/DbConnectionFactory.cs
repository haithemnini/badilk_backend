using Npgsql;
using System.Data.Common;

namespace BadilkBackend.src.Infra.Database;

public sealed class DbConnectionFactory(NpgsqlDataSource dataSource) : IDbConnectionFactory
{
    public async Task<DbConnection> OpenConnectionAsync(CancellationToken cancellationToken = default)
        => (DbConnection)await dataSource.OpenConnectionAsync(cancellationToken);
}
