using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Compradon.Warehouse.Database
{
    /// <summary>
    /// Provides an abstraction service for connecting to the warehouse database.
    /// </summary>
    public interface IDatabaseConnector : IDisposable
    {
        /// <summary>
        /// Creates an database connection as an asynchronous operation.
        /// </summary>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="DbConnection"/> of the creation operation.</returns>
        Task<DbConnection> CreateConnectionAsync();
    }
}
