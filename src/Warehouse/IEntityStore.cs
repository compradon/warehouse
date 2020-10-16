using System;
using System.Threading;
using System.Threading.Tasks;

namespace Compradon.Warehouse
{
    /// <summary>
    /// Provides an abstraction for a storage and management of entities which uses a GUID as a primary key.
    /// </summary>
    public interface IEntityStore : IEntityStore<Guid>
    {

    }

    /// <summary>
    /// Provides an abstraction for a storage and management of entities.
    /// </summary>
    /// <typeparam name="TKey">The type used for the primary key for the entity.</typeparam>
    public interface IEntityStore<TKey> : IDisposable
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Creates the specified <paramref name="entity"/> in the entity store.
        /// </summary>
        /// <param name="entity">The entity to create.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="WarehouseResult"/> of the creation operation.</returns>
        Task<WarehouseResult> CreateAsync(IEntity<TKey> entity, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes the specified <paramref name="entity"/> from the entity store.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="WarehouseResult"/> of the update operation.</returns>
        Task<WarehouseResult> DeleteAsync(IEntity<TKey> entity, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes the specified entity by <paramref name="entityId"/> from the entity store.
        /// </summary>
        /// <param name="entityId">The entity key to delete.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="WarehouseResult"/> of the update operation.</returns>
        Task<WarehouseResult> DeleteAsync(TKey entityId, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a entity, if any, who has the specified <paramref name="entityId"/>.
        /// </summary>
        /// <param name="entityId">The entity ID to search for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the user matching the specified <paramref name="entityId"/> if it exists.
        /// </returns>
        Task<WarehouseResult<TEntity>> GetAsync<TEntity>(TKey entityId, CancellationToken cancellationToken)
            where TEntity : IEntity<TKey>;

        /// <summary>
        /// Updates the specified <paramref name="entity"/> in the entity store.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="WarehouseResult"/> of the update operation.</returns>
        Task<WarehouseResult> UpdateAsync(IEntity<TKey> entity, CancellationToken cancellationToken);
    }
}
