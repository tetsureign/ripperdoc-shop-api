using RipperdocShop.Shared.DTOs;

namespace RipperdocShop.Api.Modules.Products.Customer;

public interface ICustomerProductService
{
    Task<PaginatedProductResponse> GetAllAsync(
        bool includeDeleted, int page, int pageSize);

    Task<ProductDto?> GetBySlugAsync(string slug);
    Task<IEnumerable<ProductDto>> GetFeaturedProductsAsync();

    Task<PaginatedProductResponse>
        GetByCategorySlugAsync(string slug, bool includeDeleted, int page, int pageSize);

    Task<PaginatedProductResponse>
        GetByBrandSlugAsync(string slug, bool includeDeleted, int page, int pageSize);
}
