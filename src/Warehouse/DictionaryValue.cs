using System;

namespace Compradon.Warehouse
{
    /// <summary>
    /// Provides the type of entity which uses a <see cref="int" /> as a primary key in the warehouse system.
    /// </summary>
    public class DictionaryValue : DictionaryValue<int>
    {

    }

    /// <summary>
    /// Provides the type of entity in the warehouse system.
    /// </summary>
    /// <typeparam name="TKey">The type used for the primary key for the entity type.</typeparam>
    public class DictionaryValue<TKey>
        where TKey : IEquatable<TKey>
    {
        #region Properties

        /// <summary>
        /// Gets the primary key of the dictionary value.
        /// </summary>
        public TKey Key { get; protected set; }

        /// <summary>
        /// Gets or sets the unique alias of the dictionary value.
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the display name of the dictionary value.
        /// </summary>
        public string Display { get; set; }

        /// <summary>
        /// Gets or sets the summary of the dictionary value.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets the summary of the dictionary value.
        /// </summary>
        public DictionaryValue<TKey> Items { get; set; }

        #endregion
    }
}
