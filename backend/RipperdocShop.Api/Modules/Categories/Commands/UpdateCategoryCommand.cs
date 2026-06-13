using RipperdocShop.Api.Data;
using RipperdocShop.Api.Models.Entities;
using RipperdocShop.Shared.DTOs;
using RipperdocShop.Shared.DTOs.Categories;

namespace RipperdocShop.Api.Modules.Categories.Commands;

public class UpdateCategoryCommand(ApplicationDbContext context)
{
    public async Task<Category?> ExecuteAsync(Guid id, CategoryCreateDto createDto)
    {
        var category = await context.Categories.FindAsync(id);
        if (category == null)
            return null;

        if (category.DeletedAt != null)
            throw new InvalidOperationException("Cannot update a deleted category. Restore it first, choom.");

        category.UpdateDetails(createDto.Name, createDto.Description);
        await context.SaveChangesAsync();
        return category;
    }
}
