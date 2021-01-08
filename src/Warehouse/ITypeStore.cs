using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Compradon.Warehouse
{
    /// <summary>
    /// Provides an abstraction for a storage and management of warehouse types which uses a <see cref="short" /> as a primary key.
    /// </summary>
    public interface ITypeStore : ITypeStore<WarehouseType, short>
    {

    }

    /// <summary>
    /// Provides an abstraction for a storage and management of warehouse types.
    /// </summary>
    /// <typeparam name="TWarehouseType">The type of the class representing a warehouse type.</typeparam>
    /// <typeparam name="TKey">The type of the primary key for a warehouse type.</typeparam>
    public interface ITypeStore<TWarehouseType, TKey> : IDisposable
        where TWarehouseType : WarehouseType<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Deletes the specified warehouse type by <paramref name="key"/> from the database.
        /// </summary>
        /// <param name="key">The primary key of the warehouse type to delete.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="WarehouseResult"/> of the operation.</returns>
        Task<WarehouseResult> DeleteAsync(TKey key, CancellationToken cancellationToken);

        /// <summary>
        /// Gets all an warehouse types from the database.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="WarehouseResult"/> of the operation.</returns>
        Task<WarehouseResult<IEnumerable<TWarehouseType>>> GetAllAsync(CancellationToken cancellationToken);
        
        /// <summary>
        /// Finds and returns an warehouse type, if any, who has the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The primary key of the warehouse type.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the warehouse type matching the specified <paramref name="key"/> if it exists.
        /// </returns>
        Task<WarehouseResult<TWarehouseType>> GetAsync(TKey key, CancellationToken cancellationToken);

        /// <summary>
        /// Finds and returns an warehouse type, if any, who has the specified <paramref name="alias"/>.
        /// </summary>
        /// <param name="alias">The unique alias of the warehouse type.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the warehouse type matching the specified <paramref name="alias"/> if it exists.
        /// </returns>
        Task<WarehouseResult<TWarehouseType>> FindByAliasAsync(string alias, CancellationToken cancellationToken);

        /// <summary>
        /// Finds and returns an warehouse type, if any, who has the specified <paramref name="className"/>.
        /// </summary>
        /// <param name="className">The class name of the warehouse type.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the warehouse type matching the specified <paramref name="className"/> if it exists.
        /// </returns>
        Task<WarehouseResult<TWarehouseType>> FindByClassAsync(string className, CancellationToken cancellationToken);

        /// <summary>
        /// Save the specified <paramref name="warehouseType"/> in the database.
        /// </summary>
        /// <param name="warehouseType">The warehouse type to save.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="WarehouseResult"/> of the operation.</returns>
        Task<WarehouseResult> SaveAsync(TWarehouseType warehouseType, CancellationToken cancellationToken);
    }
}
