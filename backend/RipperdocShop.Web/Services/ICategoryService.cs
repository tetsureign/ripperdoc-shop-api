using RipperdocShop.Shared.DTOs;

namespace RipperdocShop.Web.Services;

public interface ICategoryService
{
    Task<CategoryDto?> GetBySlugAsync(string slug);

    Task<PaginatedCategoryResponse?> GetAllAsync(int page = 1, int pageSize = 10);
}
