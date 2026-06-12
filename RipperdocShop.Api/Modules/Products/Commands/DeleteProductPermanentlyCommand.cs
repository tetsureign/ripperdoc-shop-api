using Microsoft.EntityFrameworkCore;
using RipperdocShop.Api.Data;
using RipperdocShop.Api.Models.Entities;

namespace RipperdocShop.Api.Modules.Products.Commands;

public class DeleteProductPermanentlyCommand(ApplicationDbContext context)
{
    public async Task<Product?> ExecuteAsync(Guid id)
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
