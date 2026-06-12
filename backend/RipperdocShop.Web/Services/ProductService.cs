using RipperdocShop.Shared.DTOs;

namespace RipperdocShop.Web.Services;

public class ProductService(IHttpClientFactory factory) : BaseApiService(factory), IProductService
{
    public Task<ProductDto?> GetBySlugAsync(string slug) =>
        GetAsync<ProductDto>($"/api/products/{slug}");

    public Task<PaginatedProductResponse?> GetAllAsync(int page = 1, int pageSize = 10) =>
        GetAsync<PaginatedProductResponse?>($"/api/products/", new Dictionary<string, string>
        {
            { "page", page.ToString() },
            { "pageSize", pageSize.ToString() }
        });

    public Task<PaginatedProductResponse?> GetByCategorySlugAsync(string slug) =>
        GetAsync<PaginatedProductResponse?>($"/api/products/category/{slug}");

    public Task<PaginatedProductResponse?> GetByBrandSlugAsync(string slug) =>
        GetAsync<PaginatedProductResponse?>($"/api/products/brand/{slug}");

    public Task<List<ProductDto>?> GetFeaturedAsync() =>
        GetAsync<List<ProductDto>>($"/api/products/featured");
}
