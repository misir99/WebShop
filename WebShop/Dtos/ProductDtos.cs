
using System.ComponentModel.DataAnnotations;

namespace WebShop.Dtos
{
    public class CreateProductDto
    {
        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(0.01, 1000000)]
        public decimal Price { get; set; }

        [Range(0, 1000000)]
        public int Stock { get; set; }
        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }

    }

    public class ProductResponseDto
    {
               public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}
