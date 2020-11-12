using Compradon.Warehouse;

namespace Application.Entities
{
    public class Order : WarehouseEntity
    {
        public long Number { get; set; }

        public decimal Price { get; set; }

        public string Comment { get; set; }
    }
}
