using System;

namespace Compradon.Warehouse
{
    /// <summary>
    /// Provides an abstraction of entity which uses a GUID as a primary key in the warehouse system.
    /// </summary>
    public abstract class Entity : Entity<Guid>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Entity"/>.
        /// </summary>
        /// <remarks>
        /// The Key property is initialized to form a new GUID value.
        /// </remarks>
        public Entity()
        {
            Key = Guid.NewGuid();
            CreationDate = DateTime.Now;
        }
    }

    /// <summary>
    /// Provides an abstraction of entity in the warehouse system.
    /// </summary>
    /// <typeparam name="TKey">The type used for the primary key for the entity.</typeparam>
    public abstract class Entity<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets the primary key of the entity.
        /// </summary>
        public TKey Key { get; protected set; }

        /// <summary>
        /// Represents a point in time when an entity was creation, typically
        /// expressed as a date and time of day, relative to Coordinated Universal Time (UTC).
        /// </summary>
        public DateTimeOffset CreationDate { get; protected set; }

        /// <summary>
        /// Represents a point in time when an entity was deletion, typically
        /// expressed as a date and time of day, relative to Coordinated Universal Time (UTC).
        /// </summary>
        public DateTimeOffset? DeletionDate { get; private set; }

        /// <summary>
        /// Gets a flag indicating whether the entity has been deleted.
        /// </summary>
        public bool Removed { get; private set; }
    }
}
