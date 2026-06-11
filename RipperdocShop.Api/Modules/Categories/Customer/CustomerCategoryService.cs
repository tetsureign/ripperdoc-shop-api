using AutoMapper;
using RipperdocShop.Api.Modules.Categories.Core;
using RipperdocShop.Shared.DTOs;

namespace RipperdocShop.Api.Modules.Categories.Customer;

public class CustomerCategoryService(ICategoryCoreService categoryCoreService, IMapper mapper) : ICustomerCategoryService
{
    public async Task<PaginatedCategoryResponse> GetAllAsync(
        bool includeDeleted, int page, int pageSize)
    {
        var (categories, totalCount, totalPages) = await categoryCoreService.GetAllAsync(includeDeleted, page, pageSize);
        var categoriesDto = mapper.Map<IEnumerable<CategoryDto>>(categories);
        return new PaginatedCategoryResponse()
        {
            Categories = categoriesDto,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }
    
    public async Task<CategoryDto?> GetBySlugAsync(string slug)
    {
        var category = await categoryCoreService.GetBySlugWithDetailsAsync(slug);
        var categoryDto = mapper.Map<CategoryDto>(category);
        return categoryDto;
    }
}
