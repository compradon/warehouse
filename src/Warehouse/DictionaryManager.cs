using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Compradon.Warehouse
{
    /// <summary>
    /// Provides the APIs for managing warehouse dictionaries which uses a <see cref="int" /> as a primary key in a persistence store.
    /// </summary>
    public class DictionaryManager : DictionaryManager<int>
    {
        
    }

    /// <summary>
    /// Provides the APIs for managing warehouse dictionaries in a persistence store.
    /// </summary>
    /// <typeparam name="TKey">The type used for the primary key for the dictionary and elements.</typeparam>
    public class DictionaryManager<TKey> : IDisposable
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
        protected internal IDictionaryStore<TKey> Store { get; }

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

        #region Methods

        /// <summary>
        /// Deletes the specified <paramref name="dictionary"/> from the backing store.
        /// </summary>
        /// <param name="dictionary">The dictionary to delete.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="WarehouseResult"/> of the operation.
        /// </returns>
        public virtual async Task<WarehouseResult> DeleteAsync(WarehouseDictionary<TKey> dictionary)
        {
            ThrowIfDisposed();

            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));

            return await DeleteAsync(dictionary.Key);
        }

        /// <summary>
        /// Deletes the specified <paramref name="dictionaryKey"/> from the backing store.
        /// </summary>
        /// <param name="dictionaryKey">The primary key of the dictionary.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="WarehouseResult"/> of the operation.
        /// </returns>
        public virtual async Task<WarehouseResult> DeleteAsync(TKey dictionaryKey)
        {
            ThrowIfDisposed();

            if (dictionaryKey == null) throw new ArgumentNullException(nameof(dictionaryKey));

            return await Store.DeleteAsync(dictionaryKey, CancellationToken);
        }

        /// <summary>
        /// Gets an dictionary, if any, who has the specified <paramref name="dictionaryKey"/>.
        /// </summary>
        /// <param name="dictionaryKey">The primary key of the dictionary.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the dictionary matching the specified <paramref name="dictionaryKey"/> if it exists.
        /// </returns>
        public virtual async Task<WarehouseDictionary<TKey>> GetAsync(TKey dictionaryKey)
        {
            ThrowIfDisposed();

            if (dictionaryKey == null) throw new ArgumentNullException(nameof(dictionaryKey));

            var result = await Store.GetAsync(dictionaryKey, CancellationToken);

            if (result.Succeeded) return result.Value;
            if (result.Exception != null) throw result.Exception;

            return null;
        }

        /// <summary>
        /// Gets an dictionary, if any, who has the specified <paramref name="alias"/>.
        /// </summary>
        /// <param name="alias">The unique alias of the dictionary.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the dictionary matching the specified <paramref name="alias"/> if it exists.
        /// </returns>
        public virtual async Task<WarehouseDictionary<TKey>> FindByAliasAsync(string alias)
        {
            ThrowIfDisposed();

            if (alias == null) throw new ArgumentNullException(nameof(alias));

            var result = await Store.FindByAliasAsync(alias, CancellationToken);

            if (result.Succeeded) return result.Value;
            if (result.Exception != null) throw result.Exception;

            return null;
        }

        /// <summary>
        /// Save the specified <paramref name="dictionary"/> in the backing store.
        /// </summary>
        /// <param name="dictionary">The dictionary to save.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="WarehouseResult"/> of the operation.
        /// </returns>
        public virtual async Task<WarehouseResult> SaveAsync(WarehouseDictionary<TKey> dictionary)
        {
            ThrowIfDisposed();

            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));

            return await Store.SaveAsync(dictionary, CancellationToken);
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
