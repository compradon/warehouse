// Licensed to the Compradon Inc. under one or more agreements.
// The Compradon Inc. licenses this file to you under the MIT license.

using System;
using System.Text.Json.Serialization;

namespace Compradon.Warehouse;

/// <summary>
/// Provides the attribute of entity which uses a <see cref="int" /> as a primary key in the warehouse system.
/// </summary>
public class WarehouseAttribute : WarehouseAttribute<int>
{
    
}

/// <summary>
/// Provides the attribute of entity in the warehouse system.
/// </summary>
/// <typeparam name="TKey">The type used for the primary key for the entity attribute.</typeparam>
public class WarehouseAttribute<TKey>
    where TKey : IEquatable<TKey>
{
    #region Properties

    /// <summary>
    /// Gets the primary key of the entity attribute.
    /// </summary>
    [JsonPropertyName("attribute_id")]
    public TKey Key { get; set; }

    /// <summary>
    /// Gets the type of the entity attribute.
    /// </summary>
    [JsonPropertyName("attribute_type_id")]
    public AttributeTypes AttributeType { get; set; }

    /// <summary>
    /// Gets or sets the unique alias of the entity type.
    /// </summary>
    [JsonPropertyName("alias")]
    public string Alias { get; set; }

    /// <summary>
    /// Gets or sets the display name of the entity type.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the summary of the entity type.
    /// </summary>
    [JsonPropertyName("summary")]
    public string Summary { get; set; }

    /// <summary>
    /// Gets or sets the summary of the entity type.
    /// </summary>
    [JsonPropertyName("default_value")]
    public string Default { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("is_required")]
    public bool Required { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("is_unique")]
    public bool Unique { get; set; }

    #endregion
}
