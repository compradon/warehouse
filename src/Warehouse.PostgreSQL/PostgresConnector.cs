using System.Data;
using System.Threading.Tasks;
using Npgsql;

namespace Compradon.Warehouse.Database.Postgres
{
    /// <summary>
    /// Provides an abstraction class for connecting to the warehouse database.
    /// </summary>
    public class PostgresConnector : DatabaseConnector
    {
        /// <summary>
        /// Constructs a new instance of <see cref="PostgresConnector"/>.
        /// </summary>
        /// <param name="connectionString">The connection string used to connect to the database.</param>
        public PostgresConnector(string connectionString) : base(connectionString)
        {

        }

        /// <summary>
        /// Creates a database connection as an asynchronous operation.
        /// </summary>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IDbConnection"/> of the creation operation.</returns>
        protected override Task<IDbConnection> InitializeСonnectionAsync()
        {
            var connection = new NpgsqlConnection(ConnectionString);

            return Task.FromResult<IDbConnection>(connection);
        }
    }
}
