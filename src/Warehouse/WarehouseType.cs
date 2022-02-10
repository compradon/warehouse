using System;
using System.Text.Json.Serialization;

namespace Compradon.Warehouse
{
    /// <summary>
    /// Provides the type of stock keeping unit which uses a <see cref="string" /> as a primary key in the warehouse system.
    /// </summary>
    public class WarehouseType : WarehouseType<string>
    {
        /// <summary>
        /// Constructs a new instance of <see cref="WarehouseType"/>.
        /// </summary>
        public WarehouseType(string name, string key)
        {
            Name = name;
            Key = key;
        }
    }

    /// <summary>
    /// Provides the type of stock keeping unit in the warehouse system.
    /// </summary>
    /// <typeparam name="TKey">The type used for the primary key for the entity type.</typeparam>
    public class WarehouseType<TKey>
        where TKey : IEquatable<TKey>
    {
        #region Properties

        /// <summary>
        /// Gets the primary key of the entity.
        /// </summary>
        [JsonPropertyName("sku")]
        public TKey Key { get; set; }

        /// <summary>
        /// Gets or sets the program class name of the entity type.
        /// </summary>
        [JsonPropertyName("program_class")]
        public string Class { get; set; }

        /// <summary>
        /// Gets or sets the display name of the entity type.
        /// </summary>
        [JsonPropertyName("unit_name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the summary of the entity type.
        /// </summary>
        [JsonPropertyName("unit_summary")]
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets the flag indicating if the warehouse type has removed.
        /// </summary>
        [JsonPropertyName("is_removed")]
        public bool Removed { get; set; }

        /// <summary>
        /// Gets or sets the flag indicating if the warehouse type has privated.
        /// </summary>
        [JsonPropertyName("is_private")]
        public bool Privated { get; set; }

        /// <summary>
        /// Gets or sets the atttributes of the entity type.
        /// </summary>
        [JsonPropertyName("attributes")]
        public AttributeCollection Attributes { get; set; } = new AttributeCollection();

        #endregion
    }
}
