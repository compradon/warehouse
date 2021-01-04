using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Compradon.Warehouse
{
    /// <summary>
    /// Provides the APIs for managing warehouse entity types which uses a <see cref="short" /> as a primary key in a persistence store.
    /// </summary>
    public class TypeManager : TypeManager<WarehouseType, short>
    {
        #region Constructors

        /// <summary>
        /// Constructs a new instance of <see cref="TypeManager"/>.
        /// </summary>
        public TypeManager(
            ITypeStore store,
            IOptions<WarehouseOptions> options,
            IEnumerable<ITypeValidator> validators,
            ILogger<TypeManager> logger,
            WarehouseErrorDescriber errorDescriber = null) : base(store, options, validators, logger, errorDescriber)
        {

        }

        #endregion
    }

    /// <summary>
    /// Provides the APIs for managing warehouse entity types in a persistence store.
    /// </summary>
    /// <typeparam name="TWarehouseType">The type used for the primary key for the entity.</typeparam>
    /// <typeparam name="TKey">The type used for the primary key for the entity type.</typeparam>
    public class TypeManager<TWarehouseType, TKey> : IDisposable
        where TWarehouseType : WarehouseType<TKey>
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
        protected internal ITypeStore<TWarehouseType, TKey> Store { get; }

        /// <summary>
        /// The <see cref="ITypeValidator"/> used to validate entity types.
        /// </summary>
        public IEnumerable<ITypeValidator<TWarehouseType, TKey>> Validators { get; }

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
        /// Constructs a new instance of <see cref="TypeManager{TWarehouseType, TKey}"/>.
        /// </summary>
        public TypeManager(
            ITypeStore<TWarehouseType, TKey> store,
            IOptions<WarehouseOptions> options,
            IEnumerable<ITypeValidator<TWarehouseType, TKey>> validators,
            ILogger<TypeManager<TWarehouseType, TKey>> logger,
            WarehouseErrorDescriber errorDescriber = null)
        {
            if (store == null) throw new ArgumentNullException(nameof(store));

            Store = store;
            Options = options?.Value ?? new WarehouseOptions();
            Validators = validators;
            Logger = logger;
            ErrorDescriber = errorDescriber ?? new WarehouseErrorDescriber();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes the specified <paramref name="warehouseType"/> from the backing store.
        /// </summary>
        /// <param name="warehouseType">The entity type to delete.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="WarehouseResult"/> of the operation.
        /// </returns>
        public virtual async Task<WarehouseResult> DeleteAsync(TWarehouseType warehouseType)
        {
            ThrowIfDisposed();

            if (warehouseType == null) throw new ArgumentNullException(nameof(warehouseType));

            return await DeleteAsync(warehouseType.Key);
        }

        /// <summary>
        /// Deletes the specified <paramref name="warehouseTypeKey"/> from the backing store.
        /// </summary>
        /// <param name="warehouseTypeKey">The primary key of the entity type.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="WarehouseResult"/> of the operation.
        /// </returns>
        public virtual async Task<WarehouseResult> DeleteAsync(TKey warehouseTypeKey)
        {
            ThrowIfDisposed();

            if (warehouseTypeKey == null) throw new ArgumentNullException(nameof(warehouseTypeKey));

            return await Store.DeleteAsync(warehouseTypeKey, CancellationToken);
        }

        /// <summary>
        /// Gets all entity types from the backing store.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IEnumerable{EntityType}"/> of the operation.
        /// </returns>
        public virtual async Task<IEnumerable<TWarehouseType>> GetAllAsync()
        {
            ThrowIfDisposed();

            var result = await Store.GetAllAsync(CancellationToken);

            if (result.Succeeded) return result.Value;
            if (result.Exception != null) throw result.Exception;

            return null;
        }

        /// <summary>
        /// Gets an entity type, if any, who has the specified <paramref name="warehouseTypeKey"/>.
        /// </summary>
        /// <param name="warehouseTypeKey">The primary key of the entity type.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the entity type matching the specified <paramref name="warehouseTypeKey"/> if it exists.
        /// </returns>
        public virtual async Task<TWarehouseType> GetAsync(TKey warehouseTypeKey)
        {
            ThrowIfDisposed();

            if (warehouseTypeKey == null) throw new ArgumentNullException(nameof(warehouseTypeKey));

            var result = await Store.GetAsync(warehouseTypeKey, CancellationToken);

            if (result.Succeeded) return result.Value;
            if (result.Exception != null) throw result.Exception;

            return null;
        }

        /// <summary>
        /// Finds and returns an entity type, if any, who has the specified <paramref name="alias"/>.
        /// </summary>
        /// <param name="alias">The alias of the entity type.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the entity type matching the specified <paramref name="alias"/> if it exists.
        /// </returns>
        public virtual async Task<TWarehouseType> FindByAliasAsync(string alias)
        {
            ThrowIfDisposed();

            if (alias == null) throw new ArgumentNullException(nameof(alias));

            var result = await Store.FindByAliasAsync(alias, CancellationToken);

            if (result.Succeeded) return result.Value;
            if (result.Exception != null) throw result.Exception;

            return null;
        }

        /// <summary>
        /// Finds and returns an entity type, if any, who has the specified <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The class type matching to the entity type.</typeparam>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the entity type matching the specified <typeparamref name="T"/> if it exists.
        /// </returns>
        public virtual async Task<TWarehouseType> FindByTypeAsync<T>()
        {
            ThrowIfDisposed();

            return await FindByTypeAsync(typeof(T));
        }

        /// <summary>
        /// Finds and returns an entity type, if any, who has the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The class type matching to the entity type.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the entity type matching the specified <paramref name="type"/> if it exists.
        /// </returns>
        public virtual async Task<TWarehouseType> FindByTypeAsync(Type type)
        {
            ThrowIfDisposed();

            if (type == null) throw new ArgumentNullException(nameof(type));

            var result = await Store.FindByTypeAsync(type.FullName, CancellationToken);

            if (result.Succeeded) return result.Value;
            if (result.Exception != null) throw result.Exception;

            return null;
        }

        /// <summary>
        /// Save the specified <paramref name="warehouseType"/> in the backing store.
        /// </summary>
        /// <param name="warehouseType">The entity type to save.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="WarehouseResult"/> of the operation.
        /// </returns>
        public virtual async Task<WarehouseResult> SaveAsync(TWarehouseType warehouseType)
        {
            ThrowIfDisposed();

            if (warehouseType == null) throw new ArgumentNullException(nameof(warehouseType));

            var result = await ValidateAsync(warehouseType);

            return result.Succeeded ? await Store.SaveAsync(warehouseType, CancellationToken) : result;
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Should return <see cref="WarehouseResult.Success"/> if validation is successful. This is called before saving the entity type via Create or Update.
        /// </summary>
        /// <param name="warehouseType">The entity type.</param>
        /// <returns>A <see cref="WarehouseResult"/> representing whether validation was successful.</returns>
        protected async Task<WarehouseResult> ValidateAsync(TWarehouseType warehouseType)
        {
            var errors = new List<WarehouseError>();

            foreach (var validator in Validators)
            {
                var result = await validator.ValidateAsync(warehouseType);
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
