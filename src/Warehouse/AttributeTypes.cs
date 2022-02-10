// Licensed to the Compradon Inc. under one or more agreements.
// The Compradon Inc. licenses this file to you under the MIT license.

namespace Compradon.Warehouse;

/// <summary>
/// Represents a Warehouse data type that can be written or read to the database.
/// </summary>
public enum AttributeTypes
{
    /// <summary>
    /// Represents a boolean (true or false) value.
    /// </summary>
    Boolean = 1,
    /// <summary>
    /// Represents an instant in time, typically expressed as a date and time of day.
    /// </summary>
    DateTime = 7,
    /// <summary>
    /// Represents a decimal floating-point number.
    /// </summary>
    Decimal = 3,
    /// <summary>
    /// Represents a reference to a dictionary item.
    /// </summary>
    Dictionary = 9,
    /// <summary>
    /// Represents a reference to a dictionary items.
    /// </summary>
    DictionarySet = 10,
    /// <summary>
    /// Represents a reference to an entity instance.
    /// </summary>
    Entity = 11,
    /// <summary>
    /// Represents a reference to an entity instances.
    /// </summary>
    EntitySet = 12,
    /// <summary>
    /// Represents a JSON data.
    /// </summary>
    Json = 8,
    /// <summary>
    /// Represents a integer number.
    /// </summary>
    Integer = 2,
    /// <summary>
    /// Represents a money.
    /// </summary>
    Money = 4,
    /// <summary>
    /// Represents a string.
    /// </summary>
    String = 5,
    /// <summary>
    /// Represents a big text.
    /// </summary>
    Text = 6
}
