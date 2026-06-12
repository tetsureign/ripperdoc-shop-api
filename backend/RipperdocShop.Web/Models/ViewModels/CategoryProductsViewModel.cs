using RipperdocShop.Shared.DTOs;

namespace RipperdocShop.Web.Models.ViewModels;

public class CategoryProductsViewModel
{
    public CategoryDto? Category { get; init; }
    public PaginatedProductResponse? Products { get; init; }
}
