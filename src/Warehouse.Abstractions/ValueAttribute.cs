// Licensed to the Compradon Inc. under one or more agreements.
// The Compradon Inc. licenses this file to you under the MIT license.

using System;

namespace Compradon.Warehouse;

/// <summary>
/// Associates a property or field with a specific value type of warehouse system.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class ValueAttribute : Attribute
{
    #region Properties

    /// <summary>
    /// Gets the value type of the attribute.
    /// </summary>
    public ValueTypes ValueType { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ValueAttribute"/> class.
    /// </summary>
    /// <param name="valueType">The value type of the attribute.</param>
    public ValueAttribute(ValueTypes valueType)
    {
        ValueType = valueType;
    }

    #endregion
}
