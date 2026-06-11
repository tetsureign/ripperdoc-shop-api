using RipperdocShop.Api.Models.Entities;

namespace RipperdocShop.Api.Modules.Ratings.Core;

public interface IProductRatingCoreService
{
    Task<ProductRating?> GetByIdAsync(Guid id);
    Task<ProductRating?> GetByIdWithDetailsAsync(Guid id);

    Task<(IEnumerable<ProductRating> Ratings, int TotalCount, int TotalPages)> GetByProductAsync(Guid productId,
        bool includeDeleted, int page, int pageSize);

    Task<(IEnumerable<ProductRating> Ratings, int TotalCount, int TotalPages)> GetByProductSlugAsync(
        string slug, bool includeDeleted, int page, int pageSize);

    Task<(IEnumerable<ProductRating> Ratings, int TotalCount, int TotalPages)> GetByUserAsync(Guid userId,
        bool includeDeleted, int page, int pageSize);
}
