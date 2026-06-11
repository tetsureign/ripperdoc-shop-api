using Microsoft.EntityFrameworkCore;
using RipperdocShop.Api.Data;
using RipperdocShop.Api.Models.Entities;

namespace RipperdocShop.Api.Modules.Categories.Core;

public class CategoryCoreService(ApplicationDbContext context) : ICategoryCoreService
{
    public async Task<Category?> GetByIdAsync(Guid id)
    {
        return await context.Categories.FindAsync(id);
    }

    public async Task<(IEnumerable<Category> Categories, int TotalCount, int TotalPages)> GetAllAsync(
        bool includeDeleted, int page, int pageSize)
    {
        var query = context.Categories
            .Where(c => includeDeleted || c.DeletedAt == null);

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var categories = await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (categories, totalCount, totalPages);
    }
    
    public async Task<Category?> GetBySlugWithDetailsAsync(string slug)
    {
        return await context.Categories
            .FirstOrDefaultAsync(p => p.Slug == slug);
    }
}
