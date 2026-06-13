using RipperdocShop.Api.Data;
using RipperdocShop.Api.Models.Entities;
using RipperdocShop.Shared.DTOs;
using RipperdocShop.Shared.DTOs.Products;

namespace RipperdocShop.Api.Modules.Products.Commands;

public class UpdateProductCommand(ApplicationDbContext context)
{
    public async Task<Product?> ExecuteAsync(Guid id, ProductCreateDto createDto)
    {
        var product = await context.Products.FindAsync(id);
        if (product == null)
            return null;

        if (product.DeletedAt != null)
            throw new InvalidOperationException("Cannot update a deleted product. Restore it first, choom.");

        var category = await context.Categories.FindAsync(createDto.CategoryId);
        if (category == null) return null;

        Brand? brand = null;
        if (createDto.BrandId != null)
        {
            brand = await context.Brands.FindAsync(createDto.BrandId);
            if (brand == null) return null;
        }

        product.UpdateDetails(
            createDto.Name,
            createDto.Description,
            createDto.ImageUrl,
            createDto.Price,
            category,
            brand);

        await context.SaveChangesAsync();
        return product;
    }
}
