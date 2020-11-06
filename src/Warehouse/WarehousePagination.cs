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
        public int PageNumber { get; }

        /// <summary>
        /// Gets the page size of the collection.
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        /// Gets the total number of pages in the collection.
        /// </summary>
        public int TotalPages { get { return PageSize == int.MaxValue ? 1 : (Count + PageSize - 1) / PageSize; } }

        /// <summary>
        /// Gets a value indicating whether the has previous page in the collection.
        /// </summary>
        public bool HasPreviousPage => PageNumber > 1;

        /// <summary>
        /// Gets a value indicating whether the has next page in the collection.
        /// </summary>
        public bool HasNextPage => PageNumber < TotalPages;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new instance of <see cref="WarehousePagination{T}"/> without information about paging.
        /// </summary>
        /// <param name="pageItems">Collection of elements on the page.</param>
        public WarehousePagination(T[] pageItems) : this(pageItems, pageItems?.Length ?? 0, int.MaxValue, 1)
        {

        }

        /// <summary>
        /// Constructs a new instance of <see cref="WarehousePagination{T}"/> with information about paging.
        /// </summary>
        /// <param name="pageItems">Collection of elements on the page.</param>
        /// <param name="count">The number of elements in the collection.</param>
        /// <param name="pageSize">The page size of the collection.</param>
        /// <param name="pageNumber">The page number of the collection.</param>
        public WarehousePagination(T[] pageItems, int count, int pageSize, int pageNumber)
        {
            if (pageItems == null) throw new ArgumentNullException(nameof(pageItems));
            if (pageSize < 1) throw new ArgumentException("{0}", nameof(pageSize));

            _items = pageItems;

            Count = count;
            PageSize = pageSize;
            PageNumber = pageNumber;

            if (PageNumber > TotalPages) throw new ArgumentException("{0}", nameof(pageNumber));
            if (_items.Count < count) throw new ArgumentException("{0}", nameof(count));
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
