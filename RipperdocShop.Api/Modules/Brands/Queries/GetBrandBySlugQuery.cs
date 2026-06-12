using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RipperdocShop.Api.Data;
using RipperdocShop.Shared.DTOs;

namespace RipperdocShop.Api.Modules.Brands.Queries;

public class GetBrandBySlugQuery(ApplicationDbContext context, IMapper mapper)
{
    public async Task<BrandDto?> ExecuteAsync(string slug)
    {
        var brand = await context.Brands
            .FirstOrDefaultAsync(b => b.Slug == slug);

        return mapper.Map<BrandDto>(brand);
    }
}
