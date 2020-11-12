using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Compradon.Warehouse
{
    /// <summary>
    /// Represents a collection of keys and values which uses a <see cref="int" /> as a primary key in a persistence store.
    /// </summary>
    public class WarehouseDictionary : WarehouseDictionary<int>
    {

    }

    /// <summary>
    /// Represents a collection of keys and values in a persistence store.
    /// </summary>
    public class WarehouseDictionary<TKey> : ICollection<DictionaryValue<TKey>>
        where TKey : IEquatable<TKey>
    {
        #region Variables

        private IList<DictionaryValue<TKey>> _items;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the primary key of the dictionary.
        /// </summary>
        public TKey Key { get; protected set; }

        /// <summary>
        /// Gets or sets the unique alias of the dictionary.
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the display name of the dictionary.
        /// </summary>
        public string Display { get; set; }

        /// <summary>
        /// Gets or sets the class name of the entity type.
        /// </summary>
        public string Enum { get; set; }

        /// <summary>
        /// Gets or sets the summary of the entity type.
        /// </summary>
        public string Summary { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new instance of <see cref="WarehouseDictionary{T}"/>.
        /// </summary>
        public WarehouseDictionary()
        {
            _items = new List<DictionaryValue<TKey>>();
        }

        /// <summary>
        /// Constructs a new instance of <see cref="WarehouseDictionary{T}"/> with values.
        /// </summary>
        /// <param name="values">Collection of elements on the dictionary.</param>
        public WarehouseDictionary(DictionaryValue<TKey>[] values)
        {
            _items = values;
        }

        #endregion

        #region IDictionary

        /// <summary>
        /// Gets or sets the dictionary value associated with the specified key.
        /// </summary>
        /// <returns>
        /// The dictionary value associated with the specified key. If the specified key is not found, a get operation throws a <see cref="KeyNotFoundException"/>, and a set operation creates a new element with the specified key.
        /// </returns>
        public DictionaryValue<TKey> this[TKey key]
        {
            get
            {
                return _items.First();
            }
        }

        /// <summary>
        /// Gets the number of key/value pairs contained in the <see cref="WarehouseDictionary{TKey}"/>.
        /// </summary>
        /// <returns>
        /// The number of key/value pairs contained in the <see cref="WarehouseDictionary{TKey}"/>.
        /// </returns>
        public int Count => _items.Count;

        /// <summary>
        /// Gets a value that indicates whether the dictionary is read-only.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Adds an <paramref name="item"/> to the <see cref="WarehouseDictionary{TKey}"/>.
        /// 
        /// </summary>
        /// <param name="item">The dictionary value to add to the dictionary.</param>
        public void Add(DictionaryValue<TKey> item)
        {
            if (IsReadOnly) throw new NotSupportedException();

            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes all items from the <see cref="WarehouseDictionary{TKey}"/>.
        /// </summary>
        public void Clear()
        {
            if (IsReadOnly) throw new NotSupportedException();

            _items.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="WarehouseDictionary{TKey}"/> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the dictionary.</param>
        /// <returns>
        /// true if item is found in the <see cref="WarehouseDictionary{TKey}"/>; otherwise, false.
        /// </returns>
        public bool Contains(DictionaryValue<TKey> item)
        {
            return _items.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="WarehouseDictionary{TKey}"/> to an <see cref="Array"/>, starting at a particular <see cref="Array"/> index.
        /// </summary>
        public void CopyTo(DictionaryValue<TKey>[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="WarehouseDictionary{TKey}"/>.
        /// </summary>
        /// <param name="item">The object to remove from the dictionary.</param>
        /// <returns>
        /// true if item was successfully removed from the <see cref="WarehouseDictionary{TKey}"/>; otherwise, false. This method also returns false if item is not found in the original <see cref="WarehouseDictionary{TKey}"/>.
        /// </returns>
        public bool Remove(DictionaryValue<TKey> item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        public IEnumerator<DictionaryValue<TKey>> GetEnumerator() => _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }
}
