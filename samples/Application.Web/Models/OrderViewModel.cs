using Application.Entities;

namespace Application.Models
{
    public class OrderViewModel
    {
        public long Number { get; set; }

        public decimal Price { get; set; }
        
        public string Comment { get; set; }

        public OrderViewModel()
        {

        }

        public OrderViewModel(Order order)
        {
            Number = order.Number;
            Price = order.Price;
            Comment = order.Comment;
        }
    }
}
