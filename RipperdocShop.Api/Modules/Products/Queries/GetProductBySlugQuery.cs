using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RipperdocShop.Api.Data;
using RipperdocShop.Shared.DTOs;

namespace RipperdocShop.Api.Modules.Products.Queries;

public class GetProductBySlugQuery(ApplicationDbContext context, IMapper mapper)
{
    public async Task<ProductDto?> ExecuteAsync(string slug)
    {
        var product = await context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .FirstOrDefaultAsync(p => p.Slug == slug);

        return mapper.Map<ProductDto>(product);
    }
}
