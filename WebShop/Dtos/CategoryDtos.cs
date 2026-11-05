using System.ComponentModel.DataAnnotations;
namespace WebShop.Dtos
{
    public class CategoryDtos
    {
        public class  CreateCategoryDto
        {
            [Required, StringLength(100)]
            public string Name { get; set; } = string.Empty;

        }

        public class CategoryResponseDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }
    }
}
