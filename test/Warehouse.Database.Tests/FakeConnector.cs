using System.Data;
using System.Threading.Tasks;

namespace Compradon.Warehouse.Database.Tests
{
    public class FakeConnector : DatabaseConnector
    {
        public FakeConnector(string connectionString) : base(connectionString)
        {

        }

        protected override Task<IDbConnection> InitializeСonnectionAsync()
        {
            var connection = new FakeConnection(ConnectionString);

            return Task.FromResult<IDbConnection>(connection);
        }
    }
}
