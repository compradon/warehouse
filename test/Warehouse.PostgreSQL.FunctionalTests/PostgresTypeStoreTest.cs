using System.Threading.Tasks;
using Xunit;

namespace Compradon.Warehouse.PostgreSQL.FunctionalTests
{
    public class PostgresTypeStoreTest : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        private readonly PostgresTypeStore _store;

        public PostgresTypeStoreTest(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _fixture.Build().GetAwaiter().GetResult();
            _store = new PostgresTypeStore(_fixture.Connector, new WarehouseErrorDescriber());
        }

        [SkippableFact]
        public async Task FindById_ItemExists_ReturnsSuccessResult()
        {
            Skip.IfNot(_fixture.Succeeded);

            var result = await _store.FindByIdAsync(1);

            Assert.Null(result.Errors);
            Assert.Null(result.Exception);
            Assert.True(result.Succeeded);
            Assert.NotNull(result.Value);
        }

        [SkippableFact]
        public async Task FindById_ItemExists_ReturnsSuccess()
        {
            Skip.IfNot(_fixture.Succeeded);

            var result = await _store.FindByIdAsync(1);

            Assert.True(result.Succeeded);
        }

        [SkippableFact]
        public async Task FindById_ItemExists_ReturnsNullErrors()
        {
            Skip.IfNot(_fixture.Succeeded);

            var result = await _store.FindByIdAsync(1);

            Assert.Null(result.Errors);
        }

        [SkippableFact]
        public async Task FindById_ItemNotExists_ReturnsNotSuccessResult()
        {
            Skip.IfNot(_fixture.Succeeded);

            var result = await _store.FindByIdAsync(short.MaxValue);

            Assert.Null(result.Errors);
            Assert.Null(result.Exception);
            Assert.True(result.Succeeded);
            Assert.NotNull(result.Value);
        }

        [SkippableFact]
        public async Task Create_ValidTypeWithoutAttributes_ReturnsSuccessResult()
        {
            Skip.IfNot(_fixture.Succeeded);

            var warehouseType = new WarehouseType("Самолёт", "AIRCRAFT_WITHOUT_ATTRIBUTES");

            var result = await _store.CreateAsync(warehouseType);

            Assert.True(result.Succeeded);
        }

        [SkippableFact]
        public async Task Create_ValidTypeWithValidAttributes_ReturnsSuccessResult()
        {
            Skip.IfNot(_fixture.Succeeded);

            var warehouseType = new WarehouseType("Самолёт", "AIRCRAFT");

            warehouseType.Attributes.Add("Код самолёта", "AIRCRAFT_CODE", AttributeTypes.String);
            warehouseType.Attributes.Add("Модель самолёта", "AIRCRAFT_MODEL", AttributeTypes.String);
            warehouseType.Attributes.Add("Максимальная дальность полёта", "AIRCRAFT_RANGE", AttributeTypes.Integer);

            var result = await _store.CreateAsync(warehouseType);

            Assert.True(result.Succeeded);
        }
    }
}
