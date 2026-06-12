using RipperdocShop.Shared.DTOs;

namespace RipperdocShop.Web.Models.ViewModels;

public class HomeViewModel
{
    public PaginatedCategoryResponse? Categories { get; init; }
    public List<ProductDto>? FeaturedProducts { get; init; } = [];
}
