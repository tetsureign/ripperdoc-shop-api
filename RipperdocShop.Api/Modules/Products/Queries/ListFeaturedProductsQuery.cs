using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RipperdocShop.Api.Data;
using RipperdocShop.Shared.DTOs;

namespace RipperdocShop.Api.Modules.Products.Queries;

public class ListFeaturedProductsQuery(ApplicationDbContext context, IMapper mapper)
{
    public async Task<IEnumerable<ProductDto>> ExecuteAsync()
    {
        var products = await context.Products
            .Where(p => p.IsFeatured && p.DeletedAt == null)
            .ToListAsync();

        return mapper.Map<IEnumerable<ProductDto>>(products);
    }
}
