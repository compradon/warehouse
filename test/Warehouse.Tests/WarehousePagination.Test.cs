using System;
using Xunit;

namespace Compradon.Warehouse.Test
{
    public class WarehousePaginationTest
    {
        [Fact]
        public void Constructor_SingleItemsNull_ThrowsArgumentNullException()
        {
            Action actual = () => new WarehousePagination<string>(null);
            Assert.Throws<ArgumentNullException>(actual);
        }

        [Fact]
        public void Constructor_SingleItems_CollectionEmpty()
        {
            var values = new WarehousePagination<string>(new string[] { });

            Assert.Empty(values);
        }

        [Fact]
        public void Constructor_Single_CollectionNotEmpty()
        {
            var values = new WarehousePagination<string>(new string[] { "one", "two", "three" });

            Assert.NotEmpty(values);
        }

        [Fact]
        public void Constructor_Single_AssignsPageNumberProperty()
        {
            var values = new WarehousePagination<string>(new string[] { "one", "two", "three" });

            var expected = 1;
            var actual = values.PageNumber;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Constructor_Single_AssignsPageSizeProperty()
        {
            var values = new WarehousePagination<string>(new string[] { "one", "two", "three" });

            var expected = int.MaxValue;
            var actual = values.PageSize;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Constructor_Single_AssignsTotalPagesProperty()
        {
            var values = new WarehousePagination<string>(new string[] { "one", "two", "three" });

            var expected = 1;
            var actual = values.TotalPages;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Constructor_Single_AssignsCountProperty()
        {
            var values = new WarehousePagination<string>(new string[] { "one", "two", "three" });

            var expected = 3;
            var actual = values.Count;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Constructor_Full_ThrowsArgumentNullException()
        {
            Action actual = () => new WarehousePagination<string>(null, 0, 10, 1);
            Assert.Throws<ArgumentNullException>(actual);
        }

        [Fact]
        public void Constructor_Full_CollectionEmpty()
        {
            var values = new WarehousePagination<string>(new string[] { }, 0, 10, 1);

            Assert.Empty(values);
        }

        [Fact]
        public void Constructor_Full_CollectionNotEmpty()
        {
            var values = new WarehousePagination<string>(new string[] { "one", "two", "three" }, 3, 10, 1);

            Assert.NotEmpty(values);
        }

        [Fact]
        public void Constructor_Full_AssignsPageNumberProperty()
        {
            var values = new WarehousePagination<string>(new string[] { "one", "two", "three" }, 3, 10, 1);

            var expected = 1;
            var actual = values.PageNumber;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Constructor_Full_AssignsPageSizeProperty()
        {
            var values = new WarehousePagination<string>(new string[] { "one", "two", "three" }, 3, 10, 1);

            var expected = 10;
            var actual = values.PageSize;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Constructor_Full_AssignsTotalPagesProperty()
        {
            var values = new WarehousePagination<string>(new string[] { "one", "two", "three" }, 3, 10, 1);

            var expected = 1;
            var actual = values.TotalPages;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Constructor_Full_AssignsCountProperty()
        {
            var values = new WarehousePagination<string>(new string[] { "one", "two", "three" }, 3, 10, 1);

            var expected = 3;
            var actual = values.Count;

            Assert.Equal(expected, actual);
        }
    }
}
