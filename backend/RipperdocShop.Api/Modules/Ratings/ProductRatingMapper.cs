using RipperdocShop.Api.Models.Entities;
using RipperdocShop.Shared.DTOs;
using RipperdocShop.Shared.DTOs.Ratings;

namespace RipperdocShop.Api.Modules.Ratings;

internal static class ProductRatingMapper
{
    internal static ProductRatingDto? ToDto(this ProductRating? rating)
    {
        if (rating is null) return null;

        return new ProductRatingDto
        {
            Id = rating.Id,
            Score = rating.Score,
            Comment = rating.Comment,
            ProductSlug = rating.Product.Slug,
            UserId = rating.UserId
        };
    }

    internal static IEnumerable<ProductRatingDto> ToDtos(this IEnumerable<ProductRating> ratings)
    {
        return ratings.Select(rating => rating.ToDto()!);
    }
}
