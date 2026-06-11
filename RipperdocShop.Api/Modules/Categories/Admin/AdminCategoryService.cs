using Microsoft.EntityFrameworkCore;
using RipperdocShop.Api.Data;
using RipperdocShop.Api.Models.Entities;
using RipperdocShop.Api.Modules.Categories.Core;
using RipperdocShop.Shared.DTOs;

namespace RipperdocShop.Api.Modules.Categories.Admin;

public class AdminCategoryService(
    ApplicationDbContext context,
    ICategoryCoreService categoryCoreService)
    : IAdminCategoryService
{
    public async Task<Category> CreateAsync(CategoryCreateDto createDto)
    {
        var category = new Category(createDto.Name, createDto.Description);
        context.Categories.Add(category);
        await context.SaveChangesAsync();
        return category;
    }

    public async Task<Category?> UpdateAsync(Guid id, CategoryCreateDto createDto)
    {
        var category = await categoryCoreService.GetByIdAsync(id);
        if (category == null)
            return null;

        if (category.DeletedAt != null)
            throw new InvalidOperationException("Cannot update a deleted category. Restore it first, choom.");

        category.UpdateDetails(createDto.Name, createDto.Description);
        await context.SaveChangesAsync();
        return category;
    }

    public async Task<Category?> SoftDeleteAsync(Guid id)
    {
        var category = await categoryCoreService.GetByIdAsync(id);
        if (category == null)
            return null;

        category.SoftDelete();
        await context.SaveChangesAsync();
        return category;
    }

    public async Task<Category?> RestoreAsync(Guid id)
    {
        var category = await categoryCoreService.GetByIdAsync(id);
        if (category == null)
            return null;

        category.Restore();
        await context.SaveChangesAsync();
        return category;
    }

    public async Task<Category?> DeletePermanentlyAsync(Guid id)
    {
        var category = await context.Categories
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
            return null;

        context.Categories.Remove(category);
        await context.SaveChangesAsync();
        return category;
    }
}
