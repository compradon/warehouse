using System;
using System.Data;
using System.Threading.Tasks;
using Xunit;

namespace Compradon.Warehouse.Database.Tests
{
    public class DatabaseConnectorTest
    {
        [Fact]
        public void Constructor_StringNull_ThrowsArgumentNullException()
        {
            Action actual = () => new FakeConnector(null);
            Assert.Throws<ArgumentNullException>(actual);
        }

        [Fact]
        public void Constructor_StringEmpty_ThrowsArgumentException()
        {
            Action actual = () => new FakeConnector(string.Empty);
            Assert.Throws<ArgumentException>(actual);
        }

        [Fact]
        public void Constructor_ConnectionString_AssignsConnectionStringProperty()
        {
            var connector = new FakeConnector("connection string");

            var expected = "connection string";
            var actual = connector.ConnectionString;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Constructor_ConnectionString_ReturnsCount()
        {
            var connector = new FakeConnector("connection string");

            int expected = 0;
            int actual = connector.Count;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task CreateConnection_NoParameters_ReturnsOpenConnection()
        {
            var connection = await new FakeConnector("connection string").CreateConnectionAsync();

            var expected = ConnectionState.Open;
            var actual = connection.State;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task CreateConnection_True_ReturnsOpenConnection()
        {
            var connection = await new FakeConnector("connection string").CreateConnectionAsync(true);

            var expected = ConnectionState.Open;
            var actual = connection.State;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task CreateConnection_False_ReturnsClosedConnection()
        {
            var connection = await new FakeConnector("connection string").CreateConnectionAsync(false);

            var expected = ConnectionState.Closed;
            var actual = connection.State;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task Count_WithOneConnection_ReturnsOne()
        {
            var connector = new FakeConnector("connection string");

            await connector.CreateConnectionAsync();

            var expected = 1;
            var actual = connector.Count;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task Count_WithTwoConnection_ReturnsTwo()
        {
            var connector = new FakeConnector("connection string");

            await connector.CreateConnectionAsync(true);
            await connector.CreateConnectionAsync(false);

            var expected = 2;
            var actual = connector.Count;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Dispose_DoesNotThrow()
        {
            var connector = new FakeConnector("connection string");

            connector.Dispose();
        }

        [Fact]
        public async Task Dispose_WithConnections_DoesNotThrowAsync()
        {
            var connector = new FakeConnector("connection string");

            var c1 = await connector.CreateConnectionAsync(true);
            var c2 = await connector.CreateConnectionAsync(false);

            connector.Dispose();
        }

        [Fact]
        public void Dispose_AfterDispose_DoesNotThrow()
        {
            var connector = new FakeConnector("connection string");

            connector.Dispose();
            connector.Dispose();
        }
    }
}
