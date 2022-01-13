using Xunit;

namespace Compradon.Warehouse.Tests
{
    public class WarehouseResultTest
    {
        [Fact]
        public void Success()
        {
            var result = WarehouseResult.Success;
            
            Assert.True(result.Succeeded);
            Assert.Null(result.Exception);
            Assert.Null(result.Errors);
        }

        [Fact]
        public void Failed()
        {
            var result = WarehouseResult.Failed();

            Assert.False(result.Succeeded);
            Assert.Null(result.Exception);
            Assert.Empty(result.Errors);
        }
    }
}
