// Licensed to the Compradon Inc. under one or more agreements.
// The Compradon Inc. licenses this file to you under the MIT license.

using System;
using System.Threading.Tasks;

namespace Compradon.Warehouse;

/// <summary>
/// Provides an abstraction for a validating an entity which uses a GUID as a primary key.
/// </summary>
/// <typeparam name="TEntity">The type used for the entity.</typeparam>
public interface IEntityValidator<TEntity> : IEntityValidator<TEntity, Guid>
    where TEntity : WarehouseEntity
{
    
}

/// <summary>
/// Provides an abstraction for a validating an entity.
/// </summary>
/// <typeparam name="TEntity">The type of the class representing a entity.</typeparam>
/// <typeparam name="TKey">The type used for the primary key for the entity.</typeparam>
public interface IEntityValidator<TEntity, TKey> : IEntityValidator
    where TEntity : WarehouseEntity<TKey>
    where TKey : IEquatable<TKey> 
{
    /// <summary>
    /// Validates an entity as an asynchronous operation.
    /// </summary>
    /// <param name="entity">The entity to validate.</param>
    /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="WarehouseResult"/> of the asynchronous validation.</returns>
    Task<WarehouseResult> ValidateAsync(TEntity entity);
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
    Task<WarehouseResult> ValidateAsync(object entity);
}
