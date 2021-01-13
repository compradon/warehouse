using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Compradon.Warehouse.Database;

namespace Compradon.Warehouse.Stores
{
    /// <summary>
    /// Represents a new instance of a persistence store for warehouse entity types, using the default implementation
    /// of <see cref="DictionaryStoreBase{TDictionary, TKey}"/> with a <see cref="int" /> as a primary key.
    /// </summary>
    public abstract class DictionaryStoreBase : DictionaryStoreBase<WarehouseDictionary, int>
    {
        /// <summary>
        /// Constructs a new instance of <see cref="DictionaryStoreBase"/>.
        /// </summary>
        /// <param name="connector">The <see cref="IDatabaseConnector"/>.</param>
        /// <param name="errorDescriber">The <see cref="WarehouseErrorDescriber"/>.</param>
        protected DictionaryStoreBase(
            IDatabaseConnector connector,
            WarehouseErrorDescriber errorDescriber) : base(connector, errorDescriber)
        {

        }
    }

    /// <summary>
    /// Represents a new instance of a persistence store for warehouse dictionaries, using the default implementation
    /// of <see cref="IDictionaryStore{TDictionary, TKey}"/>.
    /// </summary>
    /// <typeparam name="TDictionary">The type used for the dictionary.</typeparam>
    /// <typeparam name="TKey">The type used for the primary key for the dictionary.</typeparam>
    public abstract class DictionaryStoreBase<TDictionary, TKey> : IDictionaryStore<TDictionary, TKey>
        where TDictionary : WarehouseDictionary<TKey>
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
        /// Constructs a new instance of <see cref="DictionaryStoreBase{TDictionary, TKey}"/>.
        /// </summary>
        /// <param name="connector">The <see cref="IDatabaseConnector"/>.</param>
        /// <param name="errorDescriber">The <see cref="WarehouseErrorDescriber"/>.</param>
        public DictionaryStoreBase(
            IDatabaseConnector connector,
            WarehouseErrorDescriber errorDescriber)
        {
            if (connector == null) throw new ArgumentNullException(nameof(connector));
            if (errorDescriber == null) throw new ArgumentNullException(nameof(errorDescriber));

            Connector = connector;
            ErrorDescriber = errorDescriber;
        }

        #endregion

        #region IDictionaryStore

        /// <summary>
        /// Deletes the specified dictionary by <paramref name="dictionaryKey"/> from the dictionary store.
        /// </summary>
        /// <param name="dictionaryKey">The dictionary key to delete.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="WarehouseResult"/> of the operation.</returns>
        public abstract Task<WarehouseResult> DeleteAsync(TKey dictionaryKey, CancellationToken cancellationToken);

        /// <summary>
        /// Finds and returns an dictionary, if any, who has the specified <paramref name="alias"/>.
        /// </summary>
        /// <param name="alias">The unique alias of the dictionary.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the dictionary matching the specified <paramref name="alias"/> if it exists.
        /// </returns>
        public abstract Task<WarehouseResult<TDictionary>> FindByAliasAsync(string alias, CancellationToken cancellationToken);

        /// <summary>
        /// Finds and returns an dictionary, if any, who has the specified <paramref name="dictionaryKey"/>.
        /// </summary>
        /// <param name="dictionaryKey">The primary key of the dictionary.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the dictionary matching the specified <paramref name="dictionaryKey"/> if it exists.
        /// </returns>
        public abstract Task<WarehouseResult<TDictionary>> GetAsync(TKey dictionaryKey, CancellationToken cancellationToken);

        /// <summary>
        /// Save the specified <paramref name="dictionary"/> in the database.
        /// </summary>
        /// <param name="dictionary">The dictionary to save.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="WarehouseResult"/> of the operation.</returns>
        public abstract Task<WarehouseResult> SaveAsync(TDictionary dictionary, CancellationToken cancellationToken);

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
