using System;
using System.Threading.Tasks;

namespace Compradon.Warehouse
{
    /// <summary>
    /// Provides an abstraction for a validating an entity which uses a GUID as a primary key.
    /// </summary>
    /// <typeparam name="TEntity">The type used for the entity.</typeparam>
    public interface IEntityValidator<TEntity> : IEntityValidator
    {

    }

    /// <summary>
    /// Provides an abstraction for a validating an entity.
    /// </summary>
    public interface IEntityValidator
    {
        /// <summary>
        /// Validates an entity as an asynchronous operation.
        /// </summary>
        /// <param name="entity">The entity to validate.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="WarehouseResult"/> of the asynchronous validation.</returns>
        Task<WarehouseResult> ValidateAsync<TEntity, TKey>(TEntity entity)
            where TEntity : WarehouseEntity<TKey>
            where TKey : IEquatable<TKey>;
    }
}
