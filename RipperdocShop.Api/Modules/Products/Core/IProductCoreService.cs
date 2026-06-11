using RipperdocShop.Api.Models.Entities;

namespace RipperdocShop.Api.Modules.Products.Core;

public interface IProductCoreService
{
    Task<Product?> GetByIdAsync(Guid id);
    Task<Product?> GetByIdWithDetailsAsync(Guid id);
    Task<Product?> GetBySlugWithDetailsAsync(string slug);

    Task<(IEnumerable<Product> Products, int TotalCount, int TotalPages)> GetByCategorySlugAsync(
        string slug, bool includeDeleted, int page, int pageSize);

    Task<(IEnumerable<Product> Products, int TotalCount, int TotalPages)> GetByBrandSlugAsync(
        string slug, bool includeDeleted, int page, int pageSize);

    Task<(IEnumerable<Product> Products, int TotalCount, int TotalPages)> GetAllAsync(
        bool includeDeleted, int page, int pageSize);

    Task<IEnumerable<Product>> GetFeaturedAsync();
}
