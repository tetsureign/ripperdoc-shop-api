using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RipperdocShop.Api.Data;
using RipperdocShop.Shared.DTOs;

namespace RipperdocShop.Api.Modules.Categories.Queries;

public class GetCategoryBySlugQuery(ApplicationDbContext context, IMapper mapper)
{
    public async Task<CategoryDto?> ExecuteAsync(string slug)
    {
        var category = await context.Categories
            .FirstOrDefaultAsync(c => c.Slug == slug);

        return mapper.Map<CategoryDto>(category);
    }
}
