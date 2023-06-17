namespace NLayerApp.Core.DTOs
{
    public class ProductWithCategoryDto : BaseDto
    {
        public string? Name { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public CategoryDto? Category { get; set; }
    }
}
