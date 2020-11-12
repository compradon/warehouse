using System;

namespace Compradon.Warehouse
{
    /// <summary>
    /// Associates a property or field with a specific attribute of the warehouse entity.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class AttributeAttribute : Attribute
    {
        #region Properties

        /// <summary>
        /// Gets the unique alias of the entity attribute.
        /// </summary>
        public string Alias { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeAttribute"/> class.
        /// </summary>
        public AttributeAttribute()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeAttribute"/> class.
        /// </summary>
        /// <param name="alias">The unique alias of the entity attribute.</param>
        public AttributeAttribute(string alias)
        {
            Alias = alias;
        }

        #endregion
    }
}
