// Licensed to the Compradon Inc. under one or more agreements.
// The Compradon Inc. licenses this file to you under the MIT license.

using System;

namespace Compradon.Warehouse;

/// <summary>
/// Associates a class with a specific warehouse entity by alias.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class EntityAttribute : Attribute
{
    #region Properties

    /// <summary>
    /// Gets the unique alias of the entity type.
    /// </summary>
    public string Alias { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityAttribute"/> class.
    /// </summary>
    /// <param name="alias">The unique alias of the entity type.</param>
    public EntityAttribute(string alias)
    {
        Alias = alias;
    }

    #endregion
}
