using System.ComponentModel.DataAnnotations;

namespace WebShop.Dtos
{
    public class OrderItemDto
    {
        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }
        [Range(1, 1000)]
        public int Quantity { get; set; }
    }

    public class CreateOrderDto
    {
        [Required, MinLength(1)]
        public List<OrderItemDto> Items { get; set; } = new();
    }

    public class OrderItemResponseDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class OrderResponseDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } 
        public string Status { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public List<OrderItemResponseDto> Items { get; set; } = new();
    }
}
