using RipperdocShop.Api.Data;
using RipperdocShop.Api.Models.Entities;

namespace RipperdocShop.Api.Modules.Brands.Commands;

public class SoftDeleteBrandCommand(ApplicationDbContext context)
{
    public async Task<Brand?> ExecuteAsync(Guid id)
    {
        var brand = await context.Brands.FindAsync(id);
        if (brand == null)
            return null;

        brand.SoftDelete();
        await context.SaveChangesAsync();
        return brand;
    }
}
