using Microsoft.EntityFrameworkCore;
using WebShop.Data;
using WebShop.Dtos;
using WebShop.Models;
using static WebShop.Dtos.CategoryDtos;


namespace WebShop.Services
{
   
        public interface ICategoryService
        {
            Task<List<CategoryResponseDto>> GetAllAsync();
            Task<CategoryResponseDto> CreateAsync(CreateCategoryDto dto);
        }

        public class CategoryService : ICategoryService
        {
            private readonly AppDbContext _context;

            public CategoryService(AppDbContext context)
            {
                _context = context;
            }

            public async Task<List<CategoryResponseDto>> GetAllAsync()
            {
                return await _context.Categories
                    .Select(c => new CategoryResponseDto { Id = c.Id, Name = c.Name } )
                    .ToListAsync();
            }

            public async Task<CategoryResponseDto> CreateAsync(CreateCategoryDto dto)
            {
                   var newCategory = new Category
                   {
                       Name = dto.Name
                   };

                   _context.Categories.Add(newCategory);
                   await _context.SaveChangesAsync();

                   return new CategoryResponseDto { Id = newCategory.Id, Name = newCategory.Name };
            }
         }
    
}
