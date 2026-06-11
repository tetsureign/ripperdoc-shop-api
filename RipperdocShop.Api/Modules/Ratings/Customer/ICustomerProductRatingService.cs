using RipperdocShop.Api.Models.Entities;
using RipperdocShop.Shared.DTOs;

namespace RipperdocShop.Api.Modules.Ratings.Customer;

public interface ICustomerProductRatingService
{
    Task<ProductRatingDto?> CreateAsync(ProductRatingCreateDto createDto, Guid userId);

    Task<ProductRatingDto?> GetByIdAsync(Guid id);

    Task<PaginatedProductRatingResponse> GetByProductSlugAsync(string slug, bool includeDeleted, int page,
        int pageSize);

    Task<ProductRating?> UpdateAsync(Guid id, ProductRatingCreateDto createDto, Guid userId);
}
