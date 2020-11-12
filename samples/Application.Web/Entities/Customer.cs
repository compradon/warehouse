using Compradon.Warehouse;

namespace Application.Entities
{
    public class Customer : WarehouseEntity
    {
        [Attribute]
        public string Name { get; set; }

        [Attribute]
        public int Age { get; set; }

        [Attribute]
        public decimal Balance { get; set; }

        [Attribute]
        public CustomerSettings Settings { get; set; }
    }

    public class CustomerSettings
    {
        public bool Autoplay { get; set; }
    }
}
