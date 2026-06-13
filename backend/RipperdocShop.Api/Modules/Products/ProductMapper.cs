using RipperdocShop.Api.Models.Entities;
using RipperdocShop.Api.Modules.Brands;
using RipperdocShop.Api.Modules.Categories;
using RipperdocShop.Shared.DTOs;
using RipperdocShop.Shared.DTOs.Products;

namespace RipperdocShop.Api.Modules.Products;

internal static class ProductMapper
{
    internal static ProductDto? ToDto(this Product? product)
    {
        if (product is null) return null;

        return new ProductDto
        {
            Name = product.Name,
            Slug = product.Slug,
            Description = product.Description,
            ImageUrl = product.ImageUrl,
            Price = product.Price,
            IsFeatured = product.IsFeatured,
            Category = product.Category.ToDto()!,
            Brand = product.Brand.ToCreateDto()
        };
    }

    internal static IEnumerable<ProductDto> ToDtos(this IEnumerable<Product> products)
    {
        return products.Select(product => product.ToDto()!);
    }
}
