using RipperdocShop.Api.Data;
using RipperdocShop.Api.Models.Entities;

namespace RipperdocShop.Api.Modules.Categories.Commands;

public class RestoreCategoryCommand(ApplicationDbContext context)
{
    public async Task<Category?> ExecuteAsync(Guid id)
    {
        var category = await context.Categories.FindAsync(id);
        if (category == null)
            return null;

        category.Restore();
        await context.SaveChangesAsync();
        return category;
    }
}
