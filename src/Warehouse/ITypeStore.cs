using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Compradon.Warehouse
{
    /// <summary>
    /// Provides an abstraction for a storage and management of entity types which uses a <see cref="short" /> as a primary key.
    /// </summary>
    public interface ITypeStore : ITypeStore<WarehouseType, short>
    {

    }

    /// <summary>
    /// Provides an abstraction for a storage and management of entity types.
    /// </summary>
    /// <typeparam name="TWarehouseType">The type of the class representing a entity type.</typeparam>
    /// <typeparam name="TKey">The type of the primary key for a entity type.</typeparam>
    public interface ITypeStore<TWarehouseType, TKey> : IDisposable
        where TWarehouseType : WarehouseType<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Deletes the specified entity type by <paramref name="typeKey"/> from the entity types store.
        /// </summary>
        /// <param name="typeKey">The entity type key to delete.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="WarehouseResult"/> of the operation.</returns>
        Task<WarehouseResult> DeleteAsync(TKey typeKey, CancellationToken cancellationToken);

        /// <summary>
        /// Gets all an entity types from the entity types store.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="WarehouseResult"/> of the operation.</returns>
        Task<WarehouseResult<IEnumerable<TWarehouseType>>> GetAllAsync(CancellationToken cancellationToken);
        
        /// <summary>
        /// Finds and returns an entity type, if any, who has the specified <paramref name="typeKey"/>.
        /// </summary>
        /// <param name="typeKey">The primary key of the entity type.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the entity type matching the specified <paramref name="typeKey"/> if it exists.
        /// </returns>
        Task<WarehouseResult<TWarehouseType>> GetAsync(TKey typeKey, CancellationToken cancellationToken);

        /// <summary>
        /// Finds and returns an entity type, if any, who has the specified <paramref name="alias"/>.
        /// </summary>
        /// <param name="alias">The unique alias of the entity type.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the entity type matching the specified <paramref name="alias"/> if it exists.
        /// </returns>
        Task<WarehouseResult<TWarehouseType>> FindByAliasAsync(string alias, CancellationToken cancellationToken);

        /// <summary>
        /// Finds and returns an entity type, if any, who has the specified <paramref name="className"/>.
        /// </summary>
        /// <param name="className">The full name of class of the entity type.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the entity type matching the specified <paramref name="className"/> if it exists.
        /// </returns>
        Task<WarehouseResult<TWarehouseType>> FindByTypeAsync(string className, CancellationToken cancellationToken);

        /// <summary>
        /// Save the specified <paramref name="warehouseType"/> in the backing store.
        /// </summary>
        /// <param name="warehouseType">The entity type to save.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="WarehouseResult"/> of the operation.</returns>
        Task<WarehouseResult> SaveAsync(TWarehouseType warehouseType, CancellationToken cancellationToken);
    }
}
