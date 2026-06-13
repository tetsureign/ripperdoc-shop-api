using RipperdocShop.Api.Data;
using RipperdocShop.Api.Models.Entities;
using RipperdocShop.Shared.DTOs;
using RipperdocShop.Shared.DTOs.Brands;

namespace RipperdocShop.Api.Modules.Brands.Commands;

public class UpdateBrandCommand(ApplicationDbContext context)
{
    public async Task<Brand?> ExecuteAsync(Guid id, BrandCreateDto createDto)
    {
        var brand = await context.Brands.FindAsync(id);
        if (brand == null)
            return null;

        if (brand.DeletedAt != null)
            throw new InvalidOperationException("Cannot update a deleted brand. Restore it first, choom.");

        brand.UpdateDetails(createDto.Name, createDto.Description);
        await context.SaveChangesAsync();
        return brand;
    }
}
