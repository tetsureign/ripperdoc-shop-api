using Microsoft.EntityFrameworkCore;
using RipperdocShop.Api.Data;
using RipperdocShop.Api.Models.Entities;
using RipperdocShop.Api.Modules.Products.Core;
using RipperdocShop.Shared.DTOs;

namespace RipperdocShop.Api.Modules.Products.Admin;

public class AdminProductService(
    ApplicationDbContext context,
    IProductCoreService productCoreService)
    : IAdminProductService
{
    public async Task<Product?> CreateAsync(ProductCreateDto createDto)
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
            brand
        );

        context.Products.Add(product);
        await context.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> UpdateAsync(Guid id, ProductCreateDto createDto)
    {
        var product = await productCoreService.GetByIdAsync(id);
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
            brand
        );

        await context.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> SetFeaturedAsync(Guid id)
    {
        var product = await productCoreService.GetByIdAsync(id);
        if (product == null)
            return null;

        product.SetFeatured();
        await context.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> RemoveFeaturedAsync(Guid id)
    {
        var product = await productCoreService.GetByIdAsync(id);
        if (product == null)
            return null;

        product.RemoveFeatured();
        await context.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> SoftDeleteAsync(Guid id)
    {
        var product = await productCoreService.GetByIdAsync(id);
        if (product == null)
            return null;

        product.SoftDelete();
        await context.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> RestoreAsync(Guid id)
    {
        var product = await productCoreService.GetByIdAsync(id);
        if (product == null)
            return null;

        product.Restore();
        await context.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> DeletePermanentlyAsync(Guid id)
    {
        var product = await context.Products
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            return null;

        context.Products.Remove(product);
        await context.SaveChangesAsync();
        return product;
    }
}
