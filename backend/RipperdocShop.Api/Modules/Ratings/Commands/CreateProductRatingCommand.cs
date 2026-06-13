using Microsoft.EntityFrameworkCore;
using RipperdocShop.Api.Data;
using RipperdocShop.Api.Models.Entities;
using RipperdocShop.Api.Modules.Customers.Queries;
using RipperdocShop.Api.Modules.Ratings;
using RipperdocShop.Shared.DTOs;
using RipperdocShop.Shared.DTOs.Ratings;

namespace RipperdocShop.Api.Modules.Ratings.Commands;

public class CreateProductRatingCommand(ApplicationDbContext context, GetUserByIdQuery getUserById)
{
    public async Task<ProductRatingDto?> ExecuteAsync(ProductRatingCreateDto createDto, Guid userId)
    {
        var product = await context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .FirstOrDefaultAsync(p => p.Slug == createDto.ProductSlug);
        if (product is not { DeletedAt: null }) return null;

        var user = await getUserById.ExecuteAsync(userId);
        if (user is not { DeletedAt: null }) return null;

        if (await context.ProductRatings.AnyAsync(r =>
                r.ProductId == product.Id && r.UserId == user.Id))
            throw new InvalidOperationException("You have already reviewed this product.");

        var rating = new ProductRating(createDto.Score, createDto.Comment, product, user);
        context.ProductRatings.Add(rating);
        await context.SaveChangesAsync();

        return rating.ToDto();
    }
}
