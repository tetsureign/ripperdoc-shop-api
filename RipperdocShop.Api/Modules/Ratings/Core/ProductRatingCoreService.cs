using Microsoft.EntityFrameworkCore;
using RipperdocShop.Api.Data;
using RipperdocShop.Api.Models.Entities;

namespace RipperdocShop.Api.Modules.Ratings.Core;

public class ProductRatingCoreService(ApplicationDbContext context) : IProductRatingCoreService
{
    public async Task<ProductRating?> GetByIdAsync(Guid id)
    {
        return await context.ProductRatings.FindAsync(id);
    }

    public async Task<ProductRating?> GetByIdWithDetailsAsync(Guid id)
    {
        return await context.ProductRatings
            .Include(r => r.Product)
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<(IEnumerable<ProductRating> Ratings, int TotalCount, int TotalPages)> GetByProductAsync(
        Guid productId, bool includeDeleted, int page, int pageSize)
    {
        var query = context.ProductRatings
            .Include(r => r.User)
            .Where(r => r.ProductId == productId)
            .Where(r => includeDeleted || r.DeletedAt == null);

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var ratings = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (ratings, totalCount, totalPages);
    }
    
    public async Task<(IEnumerable<ProductRating> Ratings, int TotalCount, int TotalPages)> GetByProductSlugAsync(
        string slug, bool includeDeleted, int page, int pageSize)
    {
        var query = context.ProductRatings
            .Include(r => r.User)
            .Include(r => r.Product)
            .Where(r => r.Product.Slug == slug)
            .Where(r => includeDeleted || r.DeletedAt == null);

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var ratings = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (ratings, totalCount, totalPages);
    }

    public async Task<(IEnumerable<ProductRating> Ratings, int TotalCount, int TotalPages)> GetByUserAsync(Guid userId, bool includeDeleted, int page, int pageSize)
    {
        var query = context.ProductRatings
            .Include(r => r.Product)
            .Where(r => r.UserId == userId)
            .Where(r => includeDeleted || r.DeletedAt == null);

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var ratings = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (ratings, totalCount, totalPages);
    }
}
