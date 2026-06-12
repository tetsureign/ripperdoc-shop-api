using Microsoft.EntityFrameworkCore;
using RipperdocShop.Api.Data;
using RipperdocShop.Api.Models.Entities;

namespace RipperdocShop.Api.Modules.Brands.Commands;

public class DeleteBrandPermanentlyCommand(ApplicationDbContext context)
{
    public async Task<Brand?> ExecuteAsync(Guid id)
    {
        var brand = await context.Brands
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(b => b.Id == id);

        if (brand == null)
            return null;

        context.Brands.Remove(brand);
        await context.SaveChangesAsync();
        return brand;
    }
}
