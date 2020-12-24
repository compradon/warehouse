using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Compradon.Warehouse.Database.SqlServer
{
    /// <summary>
    /// Provides an abstraction class for connecting to the warehouse database.
    /// </summary>
    public class SqlServerConnector : DatabaseConnector
    {
        /// <summary>
        /// Constructs a new instance of <see cref="SqlServerConnector"/>.
        /// </summary>
        /// <param name="connectionString">The connection string used to connect to the database.</param>
        public SqlServerConnector(string connectionString) : base(connectionString)
        {

        }

        /// <summary>
        /// Creates a database connection as an asynchronous operation.
        /// </summary>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IDbConnection"/> of the creation operation.</returns>
        protected override Task<IDbConnection> InitializeСonnectionAsync()
        {
            var connection = new SqlConnection(ConnectionString);

            return Task.FromResult<IDbConnection>(connection);
        }
    }
}
