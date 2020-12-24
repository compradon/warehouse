using Compradon.Warehouse.Database;
using Microsoft.Extensions.Logging;

namespace Compradon.Warehouse.SqlServer
{
    /// <summary>
    /// Provides the APIs for building a warehouse database schema.
    /// </summary>
    public class SqlServerDatabaseBuilder : DatabaseBuilder
    {
        /// <summary>
        /// Constructs a new instance of <see cref="SqlServerDatabaseBuilder"/>.
        /// </summary>
        public SqlServerDatabaseBuilder(
            IDatabaseConnector connector,
            ILogger<SqlServerDatabaseBuilder> logger = null) : base(connector, logger)
        {

        }
    }
}
