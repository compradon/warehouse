using System.Threading.Tasks;

namespace Compradon.Warehouse.Database
{
    /// <summary>
    /// Provides an abstraction for building a warehouse database schema.
    /// </summary>
    public interface IDatabaseBuilder
    {
        /// <summary>
        /// Creates the warehouse database schema.
        /// </summary>
        Task CreateAsync();

        /// <summary>
        /// Drops the warehouse database schema.
        /// </summary>
        Task DropAsync();

        /// <summary>
        /// Determines whether the specified warehouse database schema exists.
        /// </summary>
        Task<bool> ExistsAsync();

        /// <summary>
        /// Gets the version of the warehouse database schema.
        /// </summary>
        Task<string> VersionAsync();
    }
}
