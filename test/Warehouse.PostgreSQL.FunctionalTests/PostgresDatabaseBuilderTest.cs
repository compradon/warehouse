using System.Threading.Tasks;
using Xunit;

namespace Compradon.Warehouse.PostgreSQL.FunctionalTests
{
    public class PostgresDatabaseBuilderTest
    {
        [SkippableFact]
        public async Task Build_OnEmptyDatabase_ReturnsSuccessResult()
        {
            var connector = new PostgresConnector("connection string");
            var builder = new PostgresDatabaseBuilder(connector, null);

            await builder.BuildAsync();
        }

        [SkippableFact]
        public async Task Build_OnWarehouseDatabase_ReturnsFailResult()
        {
            var connector = new PostgresConnector("connection string");
            var builder = new PostgresDatabaseBuilder(connector, null);

            await builder.BuildAsync();
        }

        [SkippableFact]
        public async Task Clear_OnEmptyDatabase_ReturnsSuccessResult()
        {
            var connector = new PostgresConnector("connection string");
            var builder = new PostgresDatabaseBuilder(connector, null);

            await builder.ClearAsync();
        }

        [SkippableFact]
        public async Task Clear_OnWarehouseDatabase_ReturnsSuccessResult()
        {
            var connector = new PostgresConnector("connection string");
            var builder = new PostgresDatabaseBuilder(connector, null);

            await builder.BuildAsync();
        }
    }
}
