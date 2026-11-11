using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebShop.Dtos;
using WebShop.Services;
namespace WebShop.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrdersController(IOrderService service)
        {
            _service = service;
        }

        private int CurrentCustomerId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpPost]
        public async Task<ActionResult<OrderResponseDto>> Create(CreateOrderDto dto)
        {
            var (order, error) = await _service.CreateAsync(CurrentCustomerId, dto);
            if (order == null)
                return BadRequest(error);

            return Created("", order);
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderResponseDto>>> GetMine()
        {
            var orders = await _service.GetForCustomerAsync(CurrentCustomerId);
            return Ok(orders);
        }
    }
}
