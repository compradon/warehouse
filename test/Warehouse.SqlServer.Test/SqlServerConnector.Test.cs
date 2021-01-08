using System.Data;
using System.Threading.Tasks;
using Xunit;

namespace Compradon.Warehouse.SqlServer.Test
{
    public class SqlServerConnectorTest
    {
        public const string ConnectionString = "Server=tcp:localhost,1433;Initial Catalog=warehouse_tests;User ID=sa;Password=Pa55w0rd;";

        public bool ConnectionSucceeded { get; } = false;

        public SqlServerConnectorTest()
        {
            try
            {
                using var connector = new SqlServerConnector(ConnectionString);
                using var connection = connector.CreateConnectionAsync().GetAwaiter().GetResult();

                ConnectionSucceeded = connection.State == System.Data.ConnectionState.Open;
            }
            catch { }
        }

        #region Helpers

        public SqlServerConnector CreateSqlServerConnector()
        {
            return new SqlServerConnector(ConnectionString);
        }

        #endregion

        [SkippableFact]
        public async Task CreateConnection_NoParameters_ReturnsOpenConnection()
        {
            Skip.IfNot(ConnectionSucceeded);

            using var connector = new SqlServerConnector(ConnectionString);
            using var connection = await connector.CreateConnectionAsync();

            var expected = ConnectionState.Open;
            var actual = connection.State;

            Assert.Equal(expected, actual);
        }

        [SkippableFact]
        public async Task CreateConnection_True_ReturnsOpenConnection()
        {
            Skip.IfNot(ConnectionSucceeded);

            using var connector = new SqlServerConnector(ConnectionString);
            using var connection = await connector.CreateConnectionAsync(true);

            var expected = ConnectionState.Open;
            var actual = connection.State;

            Assert.Equal(expected, actual);
        }

        [SkippableFact]
        public async Task CreateConnection_False_ReturnsClosedConnection()
        {
            Skip.IfNot(ConnectionSucceeded);

            using var connector = new SqlServerConnector(ConnectionString);
            using var connection = await connector.CreateConnectionAsync(false);

            var expected = ConnectionState.Closed;
            var actual = connection.State;

            Assert.Equal(expected, actual);
        }

        [SkippableFact]
        public void Dispose_DoesNotThrow()
        {
            Skip.IfNot(ConnectionSucceeded);

            using var connector = new SqlServerConnector(ConnectionString);

            connector.Dispose();
        }

        [SkippableFact]
        public async Task Dispose_WithConnections_DoesNotThrowAsync()
        {
            Skip.IfNot(ConnectionSucceeded);

            using var connector = new SqlServerConnector(ConnectionString);

            var c1 = await connector.CreateConnectionAsync(true);
            var c2 = await connector.CreateConnectionAsync(false);

            connector.Dispose();
        }

        [SkippableFact]
        public void Dispose_AfterDispose_DoesNotThrow()
        {
            Skip.IfNot(ConnectionSucceeded);

            using var connector = new SqlServerConnector(ConnectionString);

            connector.Dispose();
            connector.Dispose();
        }
    }
}
