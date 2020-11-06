using System;
using System.Collections;
using System.Collections.Generic;

namespace Compradon.Warehouse
{
    /// <summary>
    /// Represents a read-only collection of elements with pagination.
    /// </summary>
    public class WarehousePagination<T> : IEnumerable<T>
        where T : class
    {
        #region Variables

        private IList<T> _items;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of elements in the collection.
        /// </summary>
        public int Count { get; }

        /// <summary>
        /// Gets the page number of the collection.
        /// </summary>
        public int Page { get; }

        /// <summary>
        /// Gets the page size of the collection.
        /// </summary>
        public int Size { get; }

        /// <summary>
        /// Gets the total number of pages in the collection.
        /// </summary>
        public int Pages { get { return Size == int.MaxValue || Count == 0 ? 1 : (Count + Size - 1) / Size; } }

        /// <summary>
        /// Gets a value indicating whether the has previous page in the collection.
        /// </summary>
        public bool HasPrevious => Page > 1;

        /// <summary>
        /// Gets a value indicating whether the has next page in the collection.
        /// </summary>
        public bool HasNext => Page < Pages;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new instance of <see cref="WarehousePagination{T}"/> with information about paging.
        /// </summary>
        /// <param name="items">Collection of elements on the page.</param>
        /// <param name="count">The number of elements in the collection.</param>
        /// <param name="size">The page size of the collection.</param>
        /// <param name="page">The page number of the collection.</param>
        public WarehousePagination(T[] items, int count, int size, int page)
        {
            if (items == null) throw new ArgumentNullException("{0}", nameof(items));
            if (size < 1) throw new ArgumentOutOfRangeException("{0}", nameof(size));
            if (page < 1) throw new ArgumentOutOfRangeException("{0}", nameof(page));
            if (items.Length > size) throw new ArgumentOutOfRangeException("{0}", nameof(items));
            if (items.Length > count) throw new ArgumentOutOfRangeException("{0}", nameof(items));

            _items = items;

            Count = count;
            Size = size;
            Page = page;

            if (page > Pages) throw new ArgumentOutOfRangeException("{0}", nameof(page));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the element at the specified index in the read-only list.
        /// </summary>
        public T this[int index] { get { return _items[index]; } }

        #endregion

        #region IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }
}
