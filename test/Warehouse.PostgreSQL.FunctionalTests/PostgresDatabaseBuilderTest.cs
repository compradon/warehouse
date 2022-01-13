using System.Threading.Tasks;
using Xunit;

namespace Compradon.Warehouse.PostgreSQL.FunctionalTests
{
    public class PostgresDatabaseBuilderTest
    {
        [SkippableFact]
        public async Task Create()
        {
            var connector = new PostgresConnector("connection string");
            var builder = new PostgresDatabaseBuilder(connector, null);

            await builder.BuildAsync();
        }
    }
}
