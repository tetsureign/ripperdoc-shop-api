using Microsoft.EntityFrameworkCore;
using RipperdocShop.Api.Data;
using RipperdocShop.Api.Models.Entities;
using RipperdocShop.Api.Modules.Customers.Queries;
using RipperdocShop.Shared.DTOs;

namespace RipperdocShop.Api.Modules.Ratings.Commands;

public class UpdateProductRatingCommand(ApplicationDbContext context, GetUserByIdQuery getUserById)
{
    public async Task<ProductRating?> ExecuteAsync(Guid id, ProductRatingCreateDto createDto, Guid userId)
    {
        var rating = await context.ProductRatings.FindAsync(id);
        if (rating == null)
            return null;

        if (rating.DeletedAt != null)
            throw new InvalidOperationException("Cannot update a deleted rating.");

        var product = await context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .FirstOrDefaultAsync(p => p.Slug == createDto.ProductSlug);
        if (product is not { DeletedAt: null }) return null;

        var user = await getUserById.ExecuteAsync(userId);
        if (user is not { DeletedAt: null }) return null;

        rating.UpdateDetails(createDto.Score, createDto.Comment, product, user);
        await context.SaveChangesAsync();
        return rating;
    }
}
