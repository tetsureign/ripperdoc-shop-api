using RipperdocShop.Shared.DTOs;

namespace RipperdocShop.Web.Services;

public interface IProductService
{
    Task<ProductDto?> GetBySlugAsync(string slug);
    Task<PaginatedProductResponse?> GetAllAsync(int page = 1, int pageSize = 10);
    Task<PaginatedProductResponse?> GetByCategorySlugAsync(string slug);
    Task<PaginatedProductResponse?> GetByBrandSlugAsync(string slug);
    Task<List<ProductDto>?> GetFeaturedAsync();
}
