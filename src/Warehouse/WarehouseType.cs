using System;

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
        public TKey Key { get; protected set; }

        /// <summary>
        /// Gets or sets the unique alias of the entity type.
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the display name of the entity type.
        /// </summary>
        public string Display { get; set; }

        /// <summary>
        /// Gets or sets the class name of the entity type.
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// Gets or sets the summary of the entity type.
        /// </summary>
        public string Summary { get; set; }

        #endregion
    }
}
