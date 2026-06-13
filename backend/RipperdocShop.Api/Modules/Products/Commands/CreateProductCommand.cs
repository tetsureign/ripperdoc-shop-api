using RipperdocShop.Api.Data;
using RipperdocShop.Api.Models.Entities;
using RipperdocShop.Shared.DTOs;
using RipperdocShop.Shared.DTOs.Products;

namespace RipperdocShop.Api.Modules.Products.Commands;

public class CreateProductCommand(ApplicationDbContext context)
{
    public async Task<Product?> ExecuteAsync(ProductCreateDto createDto)
    {
        var category = await context.Categories.FindAsync(createDto.CategoryId);
        if (category == null) return null;

        Brand? brand = null;
        if (createDto.BrandId != null)
        {
            brand = await context.Brands.FindAsync(createDto.BrandId);
            if (brand == null) return null;
        }

        var product = new Product(
            createDto.Name,
            createDto.Description,
            createDto.ImageUrl,
            createDto.Price,
            category,
            createDto.IsFeatured,
            brand);

        context.Products.Add(product);
        await context.SaveChangesAsync();
        return product;
    }
}
