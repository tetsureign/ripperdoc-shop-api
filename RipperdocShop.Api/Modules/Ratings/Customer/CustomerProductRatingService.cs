using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RipperdocShop.Api.Data;
using RipperdocShop.Api.Models.Entities;
using RipperdocShop.Api.Modules.Customers.Core;
using RipperdocShop.Api.Modules.Products.Core;
using RipperdocShop.Api.Modules.Ratings.Core;
using RipperdocShop.Shared.DTOs;


namespace RipperdocShop.Api.Modules.Ratings.Customer;

public class CustomerProductRatingService(
    IProductRatingCoreService ratingCoreService,
    IProductCoreService productCoreService,
    IUserService userService,
    ApplicationDbContext context,
    IMapper mapper)
    : ICustomerProductRatingService
{
    public async Task<ProductRatingDto?> CreateAsync(ProductRatingCreateDto createDto, Guid userId)
    {
        var product = await productCoreService.GetBySlugWithDetailsAsync(createDto.ProductSlug);
        if (product is not { DeletedAt: null }) return null;

        var user = await userService.GetByIdAsync(userId);
        if (user is not { DeletedAt: null }) return null;

        if (await context.ProductRatings.AnyAsync(r =>
                r.ProductId == product.Id && r.UserId == user.Id))
            throw new InvalidOperationException("You have already reviewed this product.");
        
        var rating = new ProductRating(createDto.Score, createDto.Comment, product, user);
        context.ProductRatings.Add(rating);
        await context.SaveChangesAsync();

        var ratingDto = mapper.Map<ProductRatingDto>(rating);

        return ratingDto;
    }
    
    public async Task<ProductRatingDto?> GetByIdAsync(Guid id)
    {
        var rating = await ratingCoreService.GetByIdWithDetailsAsync(id);
        var ratingDto = mapper.Map<ProductRatingDto>(rating);
        return ratingDto;
    }

    public async Task<PaginatedProductRatingResponse> GetByProductSlugAsync(string slug, bool includeDeleted, int page,
        int pageSize)
    {
        var (ratings, totalCount, totalPages) = await
            ratingCoreService.GetByProductSlugAsync(slug, includeDeleted, page, pageSize);
        var ratingsDto = mapper.Map<IEnumerable<ProductRatingDto>>(ratings);
        return new PaginatedProductRatingResponse()
        {
            Ratings = ratingsDto,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }

    public async Task<ProductRating?> UpdateAsync(Guid id, ProductRatingCreateDto createDto, Guid userId)
    {
        var rating = await ratingCoreService.GetByIdAsync(id);
        if (rating == null)
            return null;

        if (rating.DeletedAt != null)
            throw new InvalidOperationException("Cannot update a deleted rating.");

        var product = await productCoreService.GetBySlugWithDetailsAsync(createDto.ProductSlug);
        if (product is not { DeletedAt: null }) return null;

        var user = await userService.GetByIdAsync(userId);
        if (user is not { DeletedAt: null }) return null;

        rating.UpdateDetails(createDto.Score, createDto.Comment, product, user);
        await context.SaveChangesAsync();
        return rating;
    }
}
