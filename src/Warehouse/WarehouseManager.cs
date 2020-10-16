using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Compradon.Warehouse
{
    /// <summary>
    /// Provides the APIs for managing entity in a persistence store.
    /// </summary>
    /// <typeparam name="TKey">The type used for the primary key for the entity.</typeparam>
    public class WarehouseManager<TKey> : IDisposable
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
        /// <value>The persistence store the manager operates over.</value>
        protected internal IEntityStore<TKey> Store { get; }

        /// <summary>
        /// The <see cref="IEntityValidator{TKey}"/> used to validate entities.
        /// </summary>
        public IEnumerable<IEntityValidator<TKey>> Validators { get; }
        
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
        /// Constructs a new instance of <see cref="WarehouseManager{TKey}"/>.
        /// </summary>
        public WarehouseManager(
            IEntityStore<TKey> store,
            IOptions<WarehouseOptions> options,
            IEnumerable<IEntityValidator<TKey>> validators,
            ILogger<WarehouseManager<TKey>> logger,
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
        /// Creates the specified <paramref name="entity"/> in the backing store, as an asynchronous operation.
        /// </summary>
        /// <param name="entity">The entity to create.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="WarehouseResult"/> of the operation.
        /// </returns>
        public virtual async Task<WarehouseResult> CreateAsync(IEntity<TKey> entity)
        {
            ThrowIfDisposed();

            var result = await ValidateAsync(entity);

            return result.Succeeded ? await Store.CreateAsync(entity, CancellationToken) : result;
        }

        /// <summary>
        /// Deletes the specified <paramref name="entity"/> from the backing store.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="WarehouseResult"/> of the operation.
        /// </returns>
        public virtual Task<WarehouseResult> DeleteAsync(IEntity<TKey> entity)
        {
            ThrowIfDisposed();

            if (entity == null) throw new ArgumentNullException(nameof(entity));

            return Store.DeleteAsync(entity, CancellationToken);
        }

        /// <summary>
        /// Updates the specified <paramref name="entity"/> in the backing store.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="WarehouseResult"/> of the operation.
        /// </returns>
        public virtual async Task<WarehouseResult> UpdateAsync(IEntity<TKey> entity)
        {
            ThrowIfDisposed();

            if (entity == null) throw new ArgumentNullException(nameof(entity));
            
            var result = await ValidateAsync(entity);

            return result.Succeeded ? await Store.UpdateAsync(entity, CancellationToken) : result;
        }

        /// <summary>
        /// Should return <see cref="WarehouseResult.Success"/> if validation is successful. This is called before saving the entity via Create or Update.
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <returns>A <see cref="WarehouseResult"/> representing whether validation was successful.</returns>
        protected async Task<WarehouseResult> ValidateAsync(IEntity<TKey> entity)
        {
            var errors = new List<WarehouseError>();

            foreach (var validator in Validators)
            {
                var result = await validator.ValidateAsync(this, entity);
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
