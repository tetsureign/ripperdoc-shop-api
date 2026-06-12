using RipperdocShop.Api.Data;
using RipperdocShop.Api.Models.Entities;

namespace RipperdocShop.Api.Modules.Products.Commands;

public class RestoreProductCommand(ApplicationDbContext context)
{
    public async Task<Product?> ExecuteAsync(Guid id)
    {
        var product = await context.Products.FindAsync(id);
        if (product == null)
            return null;

        product.Restore();
        await context.SaveChangesAsync();
        return product;
    }
}
