using Microsoft.EntityFrameworkCore;
using RipperdocShop.Api.Data;
using RipperdocShop.Shared.DTOs;

namespace RipperdocShop.Api.Modules.Brands.Queries;

public class GetBrandBySlugQuery(ApplicationDbContext context)
{
    public async Task<BrandDto?> ExecuteAsync(string slug)
    {
        var brand = await context.Brands
            .FirstOrDefaultAsync(b => b.Slug == slug);

        return brand.ToDto();
    }
}
