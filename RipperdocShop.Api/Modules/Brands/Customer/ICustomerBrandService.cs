using RipperdocShop.Shared.DTOs;

namespace RipperdocShop.Api.Modules.Brands.Customer;

public interface ICustomerBrandService
{
    Task<PaginatedBrandResponse> GetAllAsync(
        bool includeDeleted, int page, int pageSize);

    Task<BrandDto?> GetBySlugAsync(string slug);
}
