using RipperdocShop.Shared.DTOs;

namespace RipperdocShop.Web.Services;

public interface IProductRatingService
{
    Task<PaginatedProductRatingResponse?> GetByProductSlug(string slug, int page = 1, int pageSize = 10);
}
