using System;
using System.Text.Json.Serialization;

namespace Compradon.Warehouse
{
    /// <summary>
    /// Provides the type of entity which uses a <see cref="short" /> as a primary key in the warehouse system.
    /// </summary>
    public class WarehouseType : WarehouseType<short>
    {

    }

    /// <summary>
    /// Provides the type of entity in the warehouse system.
    /// </summary>
    /// <typeparam name="TKey">The type used for the primary key for the entity type.</typeparam>
    public class WarehouseType<TKey>
        where TKey : IEquatable<TKey>
    {
        #region Properties

        /// <summary>
        /// Gets the primary key of the entity.
        /// </summary>
        [JsonPropertyName("entity_type_id")]
        public TKey Key { get; set; }

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
        /// Gets or sets the class name of the entity type.
        /// </summary>
        [JsonPropertyName("class")]
        public string Class { get; set; }

        /// <summary>
        /// Gets or sets the summary of the entity type.
        /// </summary>
        [JsonPropertyName("summary")]
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
        /// 
        /// </summary>
        [JsonPropertyName("attributes")]
        public AttributeCollection Attributes { get; set; } = new AttributeCollection();

        #endregion
    }
}
