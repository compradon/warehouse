// Licensed to the Compradon Inc. under one or more agreements.
// The Compradon Inc. licenses this file to you under the MIT license.

using System;

namespace Compradon.Warehouse;

/// <summary>
/// Provides an abstraction of entity which uses a GUID as a primary key in the warehouse system.
/// </summary>
public abstract class WarehouseEntity : WarehouseEntity<Guid>
{
    /// <summary>
    /// Initializes a new instance of <see cref="WarehouseEntity"/>.
    /// </summary>
    /// <remarks>
    /// The Key property is initialized to form a new GUID value.
    /// </remarks>
    public WarehouseEntity()
    {
        Key = Guid.NewGuid();
        CreationDate = DateTime.Now;
    }
}

/// <summary>
/// Provides an abstraction of entity in the warehouse system.
/// </summary>
/// <typeparam name="TKey">The type used for the primary key for the entity.</typeparam>
public abstract class WarehouseEntity<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Gets the primary key of the entity.
    /// </summary>
    public TKey Key { get; protected set; }

    /// <summary>
    /// Gets a flag indicating that the entity is read-only.
    /// </summary>
    public bool IsReadOnly { get; private set; }

    /// <summary>
    /// Represents a point in time when an entity was creation, typically
    /// expressed as a date and time of day, relative to Coordinated Universal Time (UTC).
    /// </summary>
    public DateTimeOffset CreationDate { get; protected set; }

    /// <summary>
    /// Represents a point in time when an entity was updation, typically
    /// expressed as a date and time of day, relative to Coordinated Universal Time (UTC).
    /// </summary>
    public DateTimeOffset UpdationDate { get; protected set; }

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
