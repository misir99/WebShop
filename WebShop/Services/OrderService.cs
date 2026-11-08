using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShop.Data;
using WebShop.Dtos;
using WebShop.Models;

namespace WebShop.Services
{
    public interface IOrderService
    {
        Task<(OrderResponseDto? Order, string? Error)> CreateAsync(int customerId, CreateOrderDto dto);
        Task<List<OrderResponseDto>> GetForCustomerAsync(int customerId);
    }

    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(OrderResponseDto? Order, string? Error)> CreateAsync(int customerId, CreateOrderDto dto)
        {
            var lines = dto.Items
                .GroupBy(i => i.ProductId)
                .Select(g => new { ProductId = g.Key, Quantity = g.Sum(x => x.Quantity) })
                .ToList();


            var ids = lines.Select(l => l.ProductId).ToList();
            var products = await _context.Products
                .Where(p => ids.Contains(p.Id))
                .ToListAsync();

            var order = new Order { CustomerId = customerId };

            foreach(var line in lines)
            {
                var product = products.FirstOrDefault(p => p.Id == line.ProductId);
                if (product == null)
                    return (null, $"Proizvod {line.ProductId} ne postoji.");

                if (product.Stock < line.Quantity)
                    return (null, $"Nema dovoljno zalihe za '{product.Name}' (dostupno: {product.Stock}).");

                product.Stock -= line.Quantity;
                order.Items.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = line.Quantity,
                    UnitPrice = product.Price,
                });
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return (ToDto(order, products), null);
        
        }

        private static OrderResponseDto ToDto(Order order, IEnumerable<Product> products) => new()
        {
            Id = order.Id,
            CreatedAt = order.CreatedAt,
            Status = order.Status.ToString(),
            Total = order.Items.Sum(i => i.UnitPrice * i.Quantity),
            Items = order.Items.Select(i => new OrderItemResponseDto
            {
                ProductId = i.ProductId,
                ProductName = products.First(p => p.Id == i.ProductId).Name,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,

            }).ToList()
        };
        

        public async Task<List<OrderResponseDto>> GetForCustomerAsync(int customerId)
        {
            var orders = await _context.Orders
       .Where(o => o.CustomerId == customerId)
       .Include(o => o.Items)
           .ThenInclude(i => i.Product)
       .OrderByDescending(o => o.CreatedAt)
       .ToListAsync();

            return orders.Select(o => new OrderResponseDto
            {
                Id = o.Id,
                CreatedAt = o.CreatedAt,
                Status = o.Status.ToString(),
                Total = o.Items.Sum(i => i.UnitPrice * i.Quantity),
                Items = o.Items.Select(i => new OrderItemResponseDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product!.Name,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            }).ToList();
        }
    }
}
