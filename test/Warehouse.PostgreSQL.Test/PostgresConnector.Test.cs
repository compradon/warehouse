using System.Data;
using System.Threading.Tasks;
using Xunit;

namespace Compradon.Warehouse.Database.Postgres.Test
{
    public class PostgresConnectorTest
    {
        public const string ConnectionString = "host=localhost;database=warehouse_tests;port=5432";

        public bool ConnectionSucceeded { get; } = false;

        public PostgresConnectorTest()
        {
            try
            {
                using var connector = new PostgresConnector(ConnectionString);
                using var connection = connector.CreateConnectionAsync().GetAwaiter().GetResult();

                ConnectionSucceeded = connection.State == System.Data.ConnectionState.Open;
            }
            catch { }
        }

        #region Helpers

        public PostgresConnector CreatePostgresConnector()
        {
            return new PostgresConnector(ConnectionString);
        }

        #endregion

        [SkippableFact]
        public async Task CreateConnection_NoParameters_ReturnsOpenConnection()
        {
            Skip.IfNot(ConnectionSucceeded);

            using var connector = new PostgresConnector(ConnectionString);
            using var connection = await connector.CreateConnectionAsync();

            var expected = ConnectionState.Open;
            var actual = connection.State;

            Assert.Equal(expected, actual);
        }

        [SkippableFact]
        public async Task CreateConnection_True_ReturnsOpenConnection()
        {
            Skip.IfNot(ConnectionSucceeded);

            using var connector = new PostgresConnector(ConnectionString);
            using var connection = await connector.CreateConnectionAsync(true);

            var expected = ConnectionState.Open;
            var actual = connection.State;

            Assert.Equal(expected, actual);
        }

        [SkippableFact]
        public async Task CreateConnection_False_ReturnsClosedConnection()
        {
            Skip.IfNot(ConnectionSucceeded);

            using var connector = new PostgresConnector(ConnectionString);
            using var connection = await connector.CreateConnectionAsync(false);

            var expected = ConnectionState.Closed;
            var actual = connection.State;

            Assert.Equal(expected, actual);
        }

        [SkippableFact]
        public void Dispose_DoesNotThrow()
        {
            Skip.IfNot(ConnectionSucceeded);

            using var connector = new PostgresConnector(ConnectionString);

            connector.Dispose();
        }

        [SkippableFact]
        public async Task Dispose_WithConnections_DoesNotThrowAsync()
        {
            Skip.IfNot(ConnectionSucceeded);

            using var connector = new PostgresConnector(ConnectionString);

            var c1 = await connector.CreateConnectionAsync(true);
            var c2 = await connector.CreateConnectionAsync(false);

            connector.Dispose();
        }

        [SkippableFact]
        public void Dispose_AfterDispose_DoesNotThrow()
        {
            Skip.IfNot(ConnectionSucceeded);

            using var connector = new PostgresConnector(ConnectionString);

            connector.Dispose();
            connector.Dispose();
        }
    }
}
