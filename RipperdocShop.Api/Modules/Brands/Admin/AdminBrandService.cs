using Microsoft.EntityFrameworkCore;
using RipperdocShop.Api.Data;
using RipperdocShop.Api.Models.Entities;
using RipperdocShop.Api.Modules.Brands.Core;
using RipperdocShop.Shared.DTOs;

namespace RipperdocShop.Api.Modules.Brands.Admin;

public class AdminBrandService(
    ApplicationDbContext context,
    IBrandCoreService brandCoreService)
    : IAdminBrandService
{
    public async Task<Brand> CreateAsync(BrandCreateDto createDto)
    {
        var brand = new Brand(createDto.Name, createDto.Description);
        context.Brands.Add(brand);
        await context.SaveChangesAsync();
        return brand;
    }

    public async Task<Brand?> UpdateAsync(Guid id, BrandCreateDto createDto)
    {
        var brand = await brandCoreService.GetByIdAsync(id);
        if (brand == null)
            return null;

        if (brand.DeletedAt != null)
            throw new InvalidOperationException("Cannot update a deleted brand. Restore it first, choom.");

        brand.UpdateDetails(createDto.Name, createDto.Description);
        await context.SaveChangesAsync();
        return brand;
    }

    public async Task<Brand?> SoftDeleteAsync(Guid id)
    {
        var brand = await brandCoreService.GetByIdAsync(id);
        if (brand == null)
            return null;

        brand.SoftDelete();
        await context.SaveChangesAsync();
        return brand;
    }


    public async Task<Brand?> RestoreAsync(Guid id)
    {
        var brand = await brandCoreService.GetByIdAsync(id);
        if (brand == null)
            return null;

        brand.Restore();
        await context.SaveChangesAsync();
        return brand;
    }


    public async Task<Brand?> DeletePermanentlyAsync(Guid id)
    {
        var brand = await context.Brands
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(p => p.Id == id);

        if (brand == null)
            return null;

        context.Brands.Remove(brand);
        await context.SaveChangesAsync();
        return brand;
    }
}
