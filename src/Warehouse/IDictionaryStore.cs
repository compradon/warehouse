using System;
using System.Threading;
using System.Threading.Tasks;

namespace Compradon.Warehouse
{
    /// <summary>
    /// Provides an abstraction for a storage and management of dictionaries.
    /// </summary>
    /// <typeparam name="TKey">The type used for the primary key for the dictionary.</typeparam>
    public interface IDictionaryStore<TKey> : IDisposable
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Deletes the specified dictionary by <paramref name="dictionaryKey"/> from the dictionary store.
        /// </summary>
        /// <param name="dictionaryKey">The dictionary key to delete.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="WarehouseResult"/> of the operation.</returns>
        Task<WarehouseResult> DeleteAsync(TKey dictionaryKey, CancellationToken cancellationToken);

        /// <summary>
        /// Finds and returns an dictionary, if any, who has the specified <paramref name="dictionaryKey"/>.
        /// </summary>
        /// <param name="dictionaryKey">The primary key of the dictionary.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the dictionary matching the specified <paramref name="dictionaryKey"/> if it exists.
        /// </returns>
        Task<WarehouseResult<WarehouseDictionary<TKey>>> GetAsync(TKey dictionaryKey, CancellationToken cancellationToken);

        /// <summary>
        /// Finds and returns an dictionary, if any, who has the specified <paramref name="alias"/>.
        /// </summary>
        /// <param name="alias">The unique alias of the dictionary.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the dictionary matching the specified <paramref name="alias"/> if it exists.
        /// </returns>
        Task<WarehouseResult<WarehouseDictionary<TKey>>> FindByAliasAsync(string alias, CancellationToken cancellationToken);

        /// <summary>
        /// Save the specified <paramref name="dictionary"/> in the backing store.
        /// </summary>
        /// <param name="dictionary">The dictionary to save.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="WarehouseResult"/> of the operation.</returns>
        Task<WarehouseResult> SaveAsync(WarehouseDictionary<TKey> dictionary, CancellationToken cancellationToken);
    }
}
