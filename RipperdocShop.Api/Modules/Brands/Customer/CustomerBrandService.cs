using AutoMapper;
using RipperdocShop.Api.Modules.Brands.Core;
using RipperdocShop.Shared.DTOs;

namespace RipperdocShop.Api.Modules.Brands.Customer;

public class CustomerBrandService(IBrandCoreService brandCoreService, IMapper mapper) : ICustomerBrandService
{
    public async Task<PaginatedBrandResponse> GetAllAsync(
        bool includeDeleted, int page, int pageSize)
    {
        var (brands, totalCount, totalPages) = await brandCoreService.GetAllAsync(includeDeleted, page, pageSize);
        var brandsDto = mapper.Map<IEnumerable<BrandDto>>(brands);
        return new PaginatedBrandResponse
        {
            Brands = brandsDto,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }

    public async Task<BrandDto?> GetBySlugAsync(string slug)
    {
        var brand = await brandCoreService.GetBySlugWithDetailsAsync(slug);
        var brandDto = mapper.Map<BrandDto>(brand);
        return brandDto;
    }
}
