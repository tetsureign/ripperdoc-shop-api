using RipperdocShop.Shared.DTOs;

namespace RipperdocShop.Web.Services;

public class CategoryService(IHttpClientFactory factory) : BaseApiService(factory), ICategoryService
{
    public Task<CategoryDto?> GetBySlugAsync(string slug) =>
        GetAsync<CategoryDto>($"/api/categories/{slug}");

    public Task<PaginatedCategoryResponse?> GetAllAsync(int page = 1, int pageSize = 10) =>
        GetAsync<PaginatedCategoryResponse>($"/api/categories",
            new Dictionary<string, string>
            {
                { "page", page.ToString() },
                { "pageSize", pageSize.ToString() }
            });
}
