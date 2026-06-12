using RipperdocShop.Shared.DTOs;

namespace RipperdocShop.Web.Models.ViewModels;

public class ProductDetailsViewModel
{
    public ProductDto? Product { get; init; }
    public PaginatedProductRatingResponse? Ratings { get; init; }
}
