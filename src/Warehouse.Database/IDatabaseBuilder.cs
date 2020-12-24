using System.Threading.Tasks;

namespace Compradon.Warehouse.Database
{
    /// <summary>
    /// Provides an abstraction for building a warehouse database schema.
    /// </summary>
    public interface IDatabaseBuilder
    {
        /// <summary>
        /// Drops the warehouse database schema.
        /// </summary>
        Task<WarehouseResult> ClearAsync();

        /// <summary>
        /// Creates the warehouse database schema.
        /// </summary>
        Task<WarehouseResult> BuildAsync();
    }
}
