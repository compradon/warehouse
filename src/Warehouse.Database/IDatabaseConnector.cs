using System;
using System.Data;
using System.Threading.Tasks;

namespace Compradon.Warehouse.Database
{
    /// <summary>
    /// Provides an abstraction service for connecting to the warehouse database.
    /// </summary>
    public interface IDatabaseConnector : IDisposable
    {
        /// <summary>
        /// Creates a database connection as an asynchronous operation.
        /// </summary>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IDbConnection"/> of the creation operation.</returns>
        /// <param name="open">Indicates the need to immediately open a connection to the database.</param>
        Task<IDbConnection> CreateConnectionAsync(bool open = true);
    }
}
