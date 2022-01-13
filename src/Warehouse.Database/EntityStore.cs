using System;
using System.Threading;
using System.Threading.Tasks;

namespace Compradon.Warehouse.Database
{
    /// <summary>
    /// Represents a new instance of a persistence store for warehouse entities, using the default implementation
    /// of <see cref="IEntityStore{TEntity, TKey}"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of the class representing a entity.</typeparam>
    /// <typeparam name="TKey">The type used for the primary key for the entity.</typeparam>
    public abstract class EntityStore<TEntity, TKey> : IEntityStore<TEntity, TKey>
        where TEntity : WarehouseEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        #region Variables

        private bool _disposed;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the <see cref="IDatabaseConnector"/>.
        /// </summary>
        public IDatabaseConnector Connector { get; set; }

        /// <summary>
        /// Gets the <see cref="WarehouseErrorDescriber"/> used to provider error messages.
        /// </summary>
        /// <value>
        /// The <see cref="WarehouseErrorDescriber"/> used to provider error messages.
        /// </value>
        public WarehouseErrorDescriber ErrorDescriber { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new instance of <see cref="EntityStore{TEntity, TKey}"/>.
        /// </summary>
        /// <param name="connector">The <see cref="IDatabaseConnector"/>.</param>
        /// <param name="errorDescriber">The <see cref="WarehouseErrorDescriber"/>.</param>
        public EntityStore(
            IDatabaseConnector connector,
            WarehouseErrorDescriber errorDescriber)
        {
            if (connector == null) throw new ArgumentNullException(nameof(connector));
            if (errorDescriber == null) throw new ArgumentNullException(nameof(errorDescriber));

            Connector = connector;
            ErrorDescriber = errorDescriber;
        }

        #endregion

        #region IEntityStore

        /// <summary>
        /// Deletes the specified entity by <paramref name="key"/> from the entity store.
        /// </summary>
        /// <param name="key">The entity key to delete.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="WarehouseResult"/> of the update operation.</returns>
        public abstract Task<WarehouseResult> DeleteAsync(TKey key, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes the specified entity from the entity store.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="WarehouseResult"/> of the update operation.</returns>
        public virtual async Task<WarehouseResult> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
            => await DeleteAsync(entity.Key, cancellationToken);

        /// <summary>
        /// Finds and returns an entities, if any, who has the specified conditions.
        /// </summary>
        /// <typeparam name="T">The entities type to search for.</typeparam>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="WarehouseResult"/> of the find operation.</returns>
        public abstract Task<WarehouseResult<WarehousePagination<T>>> FindAsync<T>(CancellationToken cancellationToken = default(CancellationToken)) where T : TEntity;

        /// <summary>
        /// Finds and returns a entity, if any, who has the specified <paramref name="key"/>.
        /// </summary>
        /// <typeparam name="T">The entities type to search for.</typeparam>
        /// <param name="key">The entity ID to search for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the entity matching the specified <paramref name="key"/> if it exists.
        /// </returns>
        public abstract Task<WarehouseResult<T>> FindByIdAsync<T>(TKey key, CancellationToken cancellationToken = default(CancellationToken)) where T : TEntity;

        /// <summary>
        /// Save the specified <paramref name="entity"/> in the entity store.
        /// </summary>
        /// <param name="entity">The entity to save.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="WarehouseResult"/> of the operation.</returns>
        public abstract Task<WarehouseResult> SaveAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));

        #endregion

        #region IDisposable

        /// <summary>
        /// Releases all resources used by the store.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the store and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                // TODO: dispose managed state (managed objects)

                _disposed = true;
            }
        }

        /// <summary>
        /// Throws if this class has been disposed.
        /// </summary>
        protected void ThrowIfDisposed()
        {
            if (_disposed) throw new ObjectDisposedException(GetType().Name);
        }

        #endregion
    }
}
