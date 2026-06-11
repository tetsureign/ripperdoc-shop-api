using Microsoft.EntityFrameworkCore;
using RipperdocShop.Api.Data;
using RipperdocShop.Api.Models.Entities;

namespace RipperdocShop.Api.Modules.Brands.Core;

public class BrandCoreService(ApplicationDbContext context) : IBrandCoreService
{
    public async Task<Brand?> GetByIdAsync(Guid id)
    {
        return await context.Brands.FindAsync(id);
    }
    
    public async Task<(IEnumerable<Brand> Brands, int TotalCount, int TotalPages)> GetAllAsync(bool includeDeleted,
        int page, int pageSize)
    {
        var query = context.Brands
            .Where(b => includeDeleted || b.DeletedAt == null);
            
        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        
        var brands = await query
            .OrderByDescending(b => b.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
            
        return (brands, totalCount, totalPages);
    }
    
    public async Task<Brand?> GetBySlugWithDetailsAsync(string slug)
    {
        return await context.Brands
            .FirstOrDefaultAsync(p => p.Slug == slug);
    }
}
