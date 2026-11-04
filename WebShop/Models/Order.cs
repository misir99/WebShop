namespace WebShop.Models
{
    public enum OrderStatus
    {
        Pending,
        Paid,
        Shipped,
        Cancelled
    }
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public List<OrderItem> Items { get; set; } = new();
    }
}
