using Microsoft.AspNetCore.Mvc;
using WebShop.Dtos;
using WebShop.Services;
using static WebShop.Dtos.CategoryDtos;

namespace WebShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoriesController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryResponseDto>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpPost]
        public async Task<ActionResult<CategoryResponseDto>> Create(CreateCategoryDto dto)
        {
            var category = await _service.CreateAsync(dto);
            return Created("", category);
        }
    }
}