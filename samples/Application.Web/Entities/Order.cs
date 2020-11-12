using Compradon.Warehouse;

namespace Application.Entities
{
    [Entity("order")]
    public class Order : WarehouseEntity
    {
        [Attribute]
        public long Number { get; set; }

        [Attribute("price")]
        [Value(ValueTypes.Money)]
        public decimal Price { get; set; }

        [Attribute]
        public string Comment { get; set; }
    }
}
