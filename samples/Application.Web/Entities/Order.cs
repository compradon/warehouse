using Compradon.Warehouse;

namespace Application.Entities
{
    public class Order : Entity
    {
        public long Number { get; set; }

        public decimal Price { get; set; }

        public string Comment { get; set; }
    }
}
