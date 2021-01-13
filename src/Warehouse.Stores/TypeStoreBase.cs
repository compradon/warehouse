using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Compradon.Warehouse.Database;

namespace Compradon.Warehouse.Stores
{
    /// <summary>
    /// Represents a new instance of a persistence store for warehouse entity types, using the default implementation
    /// of <see cref="ITypeStore{TWarehouseType, TKey}"/> with a <see cref="short" /> as a primary key.
    /// </summary>
    public abstract class TypeStoreBase : TypeStoreBase<WarehouseType, short>
    {
        /// <summary>
        /// Constructs a new instance of <see cref="TypeStoreBase"/>.
        /// </summary>
        /// <param name="connector">The <see cref="IDatabaseConnector"/>.</param>
        /// <param name="errorDescriber">The <see cref="WarehouseErrorDescriber"/>.</param>
        protected TypeStoreBase(
            IDatabaseConnector connector,
            WarehouseErrorDescriber errorDescriber) : base(connector, errorDescriber)
        {

        }
    }

    /// <summary>
    /// Represents a new instance of a persistence store for warehouse entity types, using the default implementation
    /// of <see cref="ITypeStore{TWarehouseType, TKey}"/>.
    /// </summary>
    public abstract class TypeStoreBase<TWarehouseType, TKey> : ITypeStore<TWarehouseType, TKey>
        where TWarehouseType : WarehouseType<TKey>
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
        /// Constructs a new instance of <see cref="TypeStoreBase{TWarehouseType, TKey}"/>.
        /// </summary>
        /// <param name="connector">The <see cref="IDatabaseConnector"/>.</param>
        /// <param name="errorDescriber">The <see cref="WarehouseErrorDescriber"/>.</param>
        public TypeStoreBase(
            IDatabaseConnector connector,
            WarehouseErrorDescriber errorDescriber)
        {
            if (connector == null) throw new ArgumentNullException(nameof(connector));
            if (errorDescriber == null) throw new ArgumentNullException(nameof(errorDescriber));

            Connector = connector;
            ErrorDescriber = errorDescriber;
        }

        #endregion

        #region ITypeStore

        /// <summary>
        /// Deletes the specified warehouse type from the store, if any, who has the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key of warehouse type to delete.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        public abstract Task<WarehouseResult> DeleteAsync(TKey key, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes the specified <paramref name="warehouseType"/> from the store.
        /// </summary>
        /// <param name="warehouseType">The warehouse type to delete.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        public virtual async Task<WarehouseResult> DeleteAsync(TWarehouseType warehouseType, CancellationToken cancellationToken = default(CancellationToken))
            => await DeleteAsync(warehouseType.Key, cancellationToken);

        /// <summary>
        /// Finds and returns an warehouse type, if any, who has the specified <paramref name="alias"/>.
        /// </summary>
        /// <param name="alias">The unique alias of the warehouse type.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the warehouse type matching the specified <paramref name="alias"/> if it exists.
        /// </returns>
        public abstract Task<WarehouseResult<TWarehouseType>> FindByAliasAsync(string alias, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Finds and returns an warehouse type, if any, who has the specified <paramref name="className"/>.
        /// </summary>
        /// <param name="className">The class name of the warehouse type.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the warehouse type matching the specified <paramref name="className"/> if it exists.
        /// </returns>
        public abstract Task<WarehouseResult<TWarehouseType>> FindByClassAsync(string className, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets all an warehouse types from the database.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="WarehouseResult"/> of the operation.</returns>
        public abstract Task<WarehouseResult<IEnumerable<TWarehouseType>>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Finds and returns an warehouse type, if any, who has the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The primary key of the warehouse type.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the warehouse type matching the specified <paramref name="key"/> if it exists.
        /// </returns>
        public abstract Task<WarehouseResult<TWarehouseType>> GetAsync(TKey key, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Save the specified <paramref name="warehouseType"/> in the database.
        /// </summary>
        /// <param name="warehouseType">The warehouse type to save.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="WarehouseResult"/> of the operation.</returns>
        public abstract Task<WarehouseResult> SaveAsync(TWarehouseType warehouseType, CancellationToken cancellationToken = default(CancellationToken));

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
