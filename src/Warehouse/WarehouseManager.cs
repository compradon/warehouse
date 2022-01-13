using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Compradon.Warehouse
{
    /// <summary>
    /// Provides the APIs for managing entity in a persistence store .
    /// </summary>
    public class WarehouseManager : WarehouseManager<WarehouseEntity, Guid>
    {
        /// <summary>
        /// Constructs a new instance of <see cref="WarehouseManager"/>.
        /// </summary>
        public WarehouseManager(
            IEntityStore store,
            IOptions<WarehouseOptions> options,
            IEnumerable<IEntityValidator> validators,
            ILogger<WarehouseManager> logger,
            WarehouseErrorDescriber errorDescriber = null) : base(store, options, validators, logger, errorDescriber)
        {

        }
    }

    /// <summary>
    /// Provides the APIs for managing entity in a persistence store.
    /// </summary>
    /// <typeparam name="TEntity">The type of the class representing a entity.</typeparam>
    /// <typeparam name="TKey">The type used for the primary key for the entity.</typeparam>
    public class WarehouseManager<TEntity, TKey> : IDisposable
        where TEntity : WarehouseEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        #region Variables

        private bool _disposed;

        #endregion

        #region Properties

        /// <summary>
        /// The <see cref="WarehouseOptions"/> used to configure the warehouse system.
        /// </summary>
        public WarehouseOptions Options { get; }

        /// <summary>
        /// Gets or sets the persistence store the manager operates over.
        /// </summary>
        /// <value>
        /// The persistence store the manager operates over.
        /// </value>
        protected internal IEntityStore<TEntity, TKey> Store { get; }

        /// <summary>
        /// The <see cref="IEntityValidator"/> used to validate entities.
        /// </summary>
        public IEnumerable<IEntityValidator> Validators { get; }
        
        /// <summary>
        /// Gets the <see cref="WarehouseErrorDescriber"/> used to provider error messages.
        /// </summary>
        /// <value>
        /// The <see cref="WarehouseErrorDescriber"/> used to provider error messages.
        /// </value>
        public WarehouseErrorDescriber ErrorDescriber { get; }

        /// <summary>
        /// The <see cref="ILogger"/> used to log messages from the manager.
        /// </summary>
        /// <value>
        /// The <see cref="ILogger"/> used to log messages from the manager.
        /// </value>
        public virtual ILogger Logger { get; set; }

        /// <summary>
        /// The cancellation token used to cancel operations.
        /// </summary>
        protected virtual CancellationToken CancellationToken => CancellationToken.None;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new instance of <see cref="WarehouseManager{TEntity, TKey}"/>.
        /// </summary>
        public WarehouseManager(
            IEntityStore<TEntity, TKey> store,
            IOptions<WarehouseOptions> options,
            IEnumerable<IEntityValidator> validators,
            ILogger<WarehouseManager<TEntity, TKey>> logger,
            WarehouseErrorDescriber errorDescriber = null)
        {
            if (store == null) throw new ArgumentNullException(nameof(store));

            Store = store;
            Options = options?.Value ?? new WarehouseOptions();
            Validators = validators;
            ErrorDescriber = errorDescriber ?? new WarehouseErrorDescriber();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes the specified <paramref name="entity"/> from the backing store.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="WarehouseResult"/> of the operation.
        /// </returns>
        public virtual async Task<WarehouseResult> DeleteAsync(WarehouseEntity<TKey> entity)
        {
            ThrowIfDisposed();

            if (entity == null) throw new ArgumentNullException(nameof(entity));

            return await DeleteAsync(entity.Key);
        }

        /// <summary>
        /// Deletes the specified <paramref name="entityId"/> from the backing store.
        /// </summary>
        /// <param name="entityId">The entity to delete.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="WarehouseResult"/> of the operation.
        /// </returns>
        public virtual async Task<WarehouseResult> DeleteAsync(TKey entityId)
        {
            ThrowIfDisposed();

            if (entityId == null) throw new ArgumentNullException(nameof(entityId));

            return await Store.DeleteAsync(entityId, CancellationToken);
        }

        /// <summary>
        /// Finds and returns an entities, if any, who has the specified conditions.
        /// </summary>
        /// <typeparam name="T">The entities type to search for.</typeparam>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="WarehouseResult"/> of the operation.</returns>
        public virtual async Task<WarehousePagination<T>> FindAsync<T>()
            where T : TEntity
        {
            ThrowIfDisposed();

            var result = await Store.FindAsync<T>(CancellationToken);

            if (result.Succeeded) return result.Value;
            if (result.Exception != null) throw result.Exception;

            return null;
        }

        /// <summary>
        /// Finds and returns an entity, if any, who has the specified <paramref name="entityId"/>.
        /// </summary>
        /// <typeparam name="T">The entity type to search for.</typeparam>
        /// <param name="entityId">The entity ID to search for.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the entity matching the specified <paramref name="entityId"/> if it exists.
        /// </returns>
        public virtual async Task<T> FindByIdAsync<T>(TKey entityId)
            where T : TEntity
        {
            ThrowIfDisposed();

            var result = await Store.FindByIdAsync<T>(entityId, CancellationToken);

            if (result.Succeeded) return result.Value;
            if (result.Exception != null) throw result.Exception;

            return null;
        }

        /// <summary>
        /// Creates an instance of the entity type designated by the specified entity type parameter, using the parameterless constructor.
        /// </summary>
        /// <typeparam name="T">The type of entity to create.</typeparam>
        public virtual Task<T> NewAsync<T>()
            where T : TEntity
        {
            var entity = Activator.CreateInstance<T>();

            return Task.FromResult<T>(entity);
        }

        /// <summary>
        /// Save the specified <paramref name="entity"/> in the backing store.
        /// </summary>
        /// <param name="entity">The entity to save.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="WarehouseResult"/> of the operation.
        /// </returns>
        public virtual async Task<WarehouseResult> SaveAsync(TEntity entity)
        {
            ThrowIfDisposed();

            if (entity == null) throw new ArgumentNullException(nameof(entity));
            
            var result = await ValidateAsync(entity);

            return result.Succeeded ? await Store.SaveAsync(entity, CancellationToken) : result;
        }

        /// <summary>
        /// Should return <see cref="WarehouseResult.Success"/> if validation is successful. This is called before saving the entity via Create or Update.
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <returns>A <see cref="WarehouseResult"/> representing whether validation was successful.</returns>
        protected async Task<WarehouseResult> ValidateAsync(TEntity entity)
        {
            var errors = new List<WarehouseError>();

            foreach (var validator in Validators)
            {
                var result = await validator.ValidateAsync(entity);
                if (result.Succeeded) continue;

                errors.AddRange(result.Errors);
            }

            return errors.Count == 0 ? WarehouseResult.Success : WarehouseResult.Failed(errors.ToArray());
        }

        #endregion

        #region IDisposable

        /// <summary>
        /// Releases all resources used by the warehouse manager.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the warehouse manager and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                Store.Dispose();

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
