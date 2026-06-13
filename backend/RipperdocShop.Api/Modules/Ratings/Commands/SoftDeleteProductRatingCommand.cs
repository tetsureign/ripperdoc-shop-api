using RipperdocShop.Api.Data;
using RipperdocShop.Api.Models.Entities;

namespace RipperdocShop.Api.Modules.Ratings.Commands;

public class SoftDeleteProductRatingCommand(ApplicationDbContext context)
{
    public async Task<ProductRating?> ExecuteAsync(Guid id)
    {
        var rating = await context.ProductRatings.FindAsync(id);
        if (rating == null)
            return null;

        rating.SoftDelete();
        await context.SaveChangesAsync();
        return rating;
    }
}
