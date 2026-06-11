using RipperdocShop.Api.Models.Entities;

namespace RipperdocShop.Api.Modules.Products.Admin;

public class AdminProductResponse
{
    public IEnumerable<Product> Products { get; init; } = null!;
    public int TotalCount { get; init; }
    public int TotalPages { get; init; }
}
