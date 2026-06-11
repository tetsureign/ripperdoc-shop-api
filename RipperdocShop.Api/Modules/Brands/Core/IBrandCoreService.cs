using RipperdocShop.Api.Models.Entities;

namespace RipperdocShop.Api.Modules.Brands.Core;

public interface IBrandCoreService
{
    Task<Brand?> GetByIdAsync(Guid id);
    Task<(IEnumerable<Brand> Brands, int TotalCount, int TotalPages)> GetAllAsync(bool includeDeleted,
        int page, int pageSize);

    Task<Brand?> GetBySlugWithDetailsAsync(string slug);
    // Task<bool> ExistsAsync(Guid id);
    // Task<Brand?> GetByIdWithProductsAsync(Guid id);
}
