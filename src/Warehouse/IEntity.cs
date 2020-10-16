using System;

namespace Compradon.Warehouse
{
    /// <summary>
    /// Provides an abstraction of entity which uses a GUID as a primary key in the warehouse system.
    /// </summary>
    public interface IEntity : IEntity<Guid>
    {

    }

    /// <summary>
    /// Provides an abstraction of entity in the warehouse system.
    /// </summary>
    /// <typeparam name="TKey">The type used for the primary key for the entity.</typeparam>
    public interface IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets the unique identifier of the entity.
        /// </summary>
        TKey Id { get; }

        /// <summary>
        /// Gets the entity creation date and time, in UTC.
        /// </summary>
        DateTimeOffset CreationDate { get; }

        /// <summary>
        /// Gets or sets a flag indicating if a entity has removed.
        /// </summary>
        bool Removed { get; set; }
    }
}
