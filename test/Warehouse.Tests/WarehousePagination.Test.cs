using System;
using Xunit;

namespace Compradon.Warehouse.Test
{
    public class WarehousePaginationTest
    {
        [Fact]
        public void Constructor_WithNullPageItems_ThrowsArgumentNullException()
        {
            string[] items = null;

            Action actual = () => new WarehousePagination<string>(items: items, count: 0, size: 10, page: 1);

            Assert.Throws<ArgumentNullException>(actual);
        }

        [Fact]
        public void Constructor_WithEmptyPageItems_ReturnsEmptyCollection()
        {
            string[] items = new string[] { };

            var values = new WarehousePagination<string>(items: items, count: 0, size: 10, page: 1);

            Assert.Empty(values);
        }

        [Fact]
        public void Constructor_WithPageItems_ReturnsNotEmptyCollection()
        {
            string[] items = new string[] { "one", "two", "three" };

            var values = new WarehousePagination<string>(items: items, count: items.Length, size: 10, page: 1);

            Assert.NotEmpty(values);
        }

        [Fact]
        public void Constructor_PageItemsMoreThanCount_ThrowsArgumentOutOfRangeException()
        {
            string[] items = new string[] { "one", "two", "three" };

            Action actual = () => new WarehousePagination<string>(items: items, count: 1, size: 10, page: 1);
            Assert.Throws<ArgumentOutOfRangeException>(actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Constructor_PageSizeIncorrect_ThrowsArgumentOutOfRangeException(int size)
        {
            string[] items = new string[] { "one", "two", "three" };

            Action actual = () => new WarehousePagination<string>(items: items, count: items.Length, size: size, page: 1);
            Assert.Throws<ArgumentOutOfRangeException>(actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Constructor_PageNumberIncorrect_ThrowsArgumentOutOfRangeException(int page)
        {
            string[] items = new string[] { "one", "two", "three" };

            Action actual = () => new WarehousePagination<string>(items: items, count: items.Length, size: 3, page: page);
            Assert.Throws<ArgumentOutOfRangeException>(actual);
        }

        [Fact]
        public void Constructor_PageItemsMoreThanPageSize_ThrowsArgumentOutOfRangeException()
        {
            string[] items = new string[] { "one", "two", "three" };

            Action actual = () => new WarehousePagination<string>(items: items, count: items.Length, size: 1, page: 1);
            Assert.Throws<ArgumentOutOfRangeException>(actual);
        }

        [Fact]
        public void Constructor_PageNumberMoreThanTotalPages_ThrowsArgumentOutOfRangeException()
        {
            string[] items = new string[] { "one", "two", "three" };

            Action actual = () => new WarehousePagination<string>(items: items, count: 9, size: 3, page: 4);
            Assert.Throws<ArgumentOutOfRangeException>(actual);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void PageNumber_Property_ReturnsCorrectValue(int page)
        {
            string[] items = new string[] { "one", "two", "three" };

            var values = new WarehousePagination<string>(items: items, count: 9, size: 3, page: page);

            var expected = page;
            var actual = values.Page;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(10)]
        public void PageSize_Property_ReturnsCorrectValue(int size)
        {
            string[] items = new string[] { "one", "two", "three" };

            var values = new WarehousePagination<string>(items: items, count: items.Length, size: size, page: 1);

            var expected = size;
            var actual = values.Size;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(6)]
        [InlineData(9)]
        public void TotalPages_Property_ReturnsCorrectValue(int count)
        {
            string[] items = new string[] { "one", "two", "three" };

            var values = new WarehousePagination<string>(items: items, count: count, size: 3, page: 1);

            var expected = count / 3;
            var actual = values.Pages;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(6)]
        [InlineData(9)]
        public void Count_Property_ReturnsCorrectValue(int count)
        {
            string[] items = new string[] { "one", "two", "three" };

            var values = new WarehousePagination<string>(items: items, count: count, size: 3, page: 1);

            var expected = count;
            var actual = values.Count;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void Item_GetByIndex_ReturnsCorrectValue(int index)
        {
            string[] items = new string[] { "one", "two", "three" };

            var values = new WarehousePagination<string>(items: items, count: items.Length, size: 3, page: 1);

            var expected = items[index];
            var actual = values[index];

            Assert.Equal(expected, actual);
        }
    }
}
