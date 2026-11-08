using Microsoft.AspNetCore.Mvc;
using WebShop.Dtos;
using WebShop.Services;
using Microsoft.AspNetCore.Authorization;

namespace WebShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductResponseDto>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponseDto>> GetById(int id)
        {
            var product = await _service.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ProductResponseDto>> Create(CreateProductDto dto)
        {
            var product = await _service.CreateAsync(dto);
            if (product == null)
                return BadRequest("Kategorija ne postoji.");

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }
    }
}