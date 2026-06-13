namespace RipperdocShop.Shared.DTOs.Categories;

public class PaginatedCategoryResponse
{
    public IEnumerable<CategoryDto> Categories { get; set; } = null!;
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}
