using Microsoft.EntityFrameworkCore;
using RipperdocShop.Api.Data;
using RipperdocShop.Api.Models.Entities;
using RipperdocShop.Api.Modules.Ratings.Core;

namespace RipperdocShop.Api.Modules.Ratings.Admin;

public class AdminProductRatingService(
    ApplicationDbContext context,
    IProductRatingCoreService productRatingCoreService)
    : IAdminProductRatingService
{
    public async Task<IEnumerable<ProductRating>> GetRecentAsync(int count, bool includeDeleted)
    {
        return await context.ProductRatings
            .Where(r => includeDeleted || r.DeletedAt == null)
            .OrderByDescending(r => r.CreatedAt)
            .Take(count)
            .Include(r => r.Product)
            .Include(r => r.User)
            .ToListAsync();
    }
    
    public async Task<ProductRating?> SoftDeleteAsync(Guid id)
    {
        var rating = await productRatingCoreService.GetByIdAsync(id);
        if (rating == null)
            return null;

        rating.SoftDelete();
        await context.SaveChangesAsync();
        return rating;
    }

    public async Task<ProductRating?> RestoreAsync(Guid id)
    {
        var rating = await productRatingCoreService.GetByIdAsync(id);
        if (rating == null)
            return null;

        rating.Restore();
        await context.SaveChangesAsync();
        return rating;
    }

    public async Task<ProductRating?> DeletePermanentlyAsync(Guid id)
    {
        var rating = await context.ProductRatings
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(r => r.Id == id);
    
        if (rating == null)
            return null;

        context.ProductRatings.Remove(rating);
        await context.SaveChangesAsync();
        return rating;
    }
}
