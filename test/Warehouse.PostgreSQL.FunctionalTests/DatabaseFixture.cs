using System;
using System.Data;
using System.Threading.Tasks;
using Compradon.Warehouse.Database;
using Npgsql;

namespace Compradon.Warehouse.PostgreSQL.FunctionalTests
{
    public class DatabaseFixture : IDisposable
    {
        public const string ConnectionString = "host=localhost;port=5432";

        private readonly string _database = $"warehouse_{Guid.NewGuid().ToString("N")}";

        public PostgresConnector Connector { get; }

        public bool Succeeded { get; private set; } = false;

        public DatabaseFixture()
        {
            try
            {
                using var connection = new NpgsqlConnection(ConnectionString);

                connection.Query($"CREATE DATABASE {_database}")
                    .RunAsync().GetAwaiter().GetResult();

                Connector = new PostgresConnector($"host=localhost;database={_database};port=5432");

                Succeeded = true;
            }
            catch { }
        }

        public async Task Build()
        {
            var builder = new PostgresDatabaseBuilder(Connector, null);
            var build = await builder.BuildAsync();

            Succeeded = build.Succeeded;
        }

        public void Dispose()
        {
            using var connection = new NpgsqlConnection(ConnectionString);

            connection.Query($"DROP DATABASE IF EXISTS {_database} WITH (FORCE)")
                .RunAsync().GetAwaiter().GetResult();

            if (Connector != null) Connector.Dispose();
        }
    }
}
