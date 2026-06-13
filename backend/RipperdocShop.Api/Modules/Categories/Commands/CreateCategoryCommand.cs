using RipperdocShop.Api.Data;
using RipperdocShop.Api.Models.Entities;
using RipperdocShop.Shared.DTOs;
using RipperdocShop.Shared.DTOs.Categories;

namespace RipperdocShop.Api.Modules.Categories.Commands;

public class CreateCategoryCommand(ApplicationDbContext context)
{
    public async Task<Category> ExecuteAsync(CategoryCreateDto createDto)
    {
        var category = new Category(createDto.Name, createDto.Description);
        context.Categories.Add(category);
        await context.SaveChangesAsync();
        return category;
    }
}
