using Compradon.Warehouse.Database;
using Microsoft.Extensions.Logging;

namespace Compradon.Warehouse.PostgreSQL
{
    /// <summary>
    /// Provides the APIs for building a warehouse database schema.
    /// </summary>
    public class PostgresDatabaseBuilder : DatabaseBuilder
    {
        /// <summary>
        /// Constructs a new instance of <see cref="PostgresDatabaseBuilder"/>.
        /// </summary>
        public PostgresDatabaseBuilder(
            IDatabaseConnector connector,
            ILogger<PostgresDatabaseBuilder> logger = null) : base(connector, logger)
        {

        }
    }
}
