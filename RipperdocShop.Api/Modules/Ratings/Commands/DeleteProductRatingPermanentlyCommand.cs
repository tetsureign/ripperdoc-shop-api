using Microsoft.EntityFrameworkCore;
using RipperdocShop.Api.Data;
using RipperdocShop.Api.Models.Entities;

namespace RipperdocShop.Api.Modules.Ratings.Commands;

public class DeleteProductRatingPermanentlyCommand(ApplicationDbContext context)
{
    public async Task<ProductRating?> ExecuteAsync(Guid id)
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
