using System;
using System.Threading.Tasks;

namespace Compradon.Warehouse
{
    /// <summary>
    /// Provides an abstraction for a validating an entity which uses a GUID as a primary key.
    /// </summary>
    public interface IEntityValidator : IEntityValidator<Guid>
    {

    }

    /// <summary>
    /// Provides an abstraction for a validating an entity.
    /// </summary>
    /// <typeparam name="TKey">The type used for the primary key for the entity.</typeparam>
    public interface IEntityValidator<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Validates an entity as an asynchronous operation.
        /// </summary>
        /// <param name="manager">The <see cref="WarehouseManager{TKey}"/> managing the entity store.</param>
        /// <param name="entity">The entity to validate.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="WarehouseResult"/> of the asynchronous validation.</returns>
        Task<WarehouseResult> ValidateAsync(WarehouseManager<TKey> manager, Entity<TKey> entity);
    }
}
