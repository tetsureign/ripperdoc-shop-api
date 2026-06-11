using RipperdocShop.Api.Models.Entities;

namespace RipperdocShop.Api.Modules.Ratings.Admin;

public class AdminProductRatingResponse
{
    public IEnumerable<ProductRating> Ratings { get; init; } = null!;
    public int TotalCount { get; init; }
    public int TotalPages { get; init; }
}
