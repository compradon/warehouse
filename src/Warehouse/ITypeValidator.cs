using System;
using System.Threading.Tasks;

namespace Compradon.Warehouse
{
    /// <summary>
    /// Provides an abstraction for a validating an entity type which uses a <see cref="short" /> as a primary key.
    /// </summary>
    public interface ITypeValidator : ITypeValidator<short>
    {

    }

    /// <summary>
    /// Provides an abstraction for a validating an entity type.
    /// </summary>
    /// <typeparam name="TKey">The type used for the primary key for the entity type.</typeparam>
    public interface ITypeValidator<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Validates an entity type as an asynchronous operation.
        /// </summary>
        /// <param name="entity">The entity type to validate.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="WarehouseResult"/> of the asynchronous validation.</returns>
        Task<WarehouseResult> ValidateAsync(WarehouseType<TKey> entity);
    }
}
