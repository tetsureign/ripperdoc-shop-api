using RipperdocShop.Api.Data;
using RipperdocShop.Api.Models.Entities;
using RipperdocShop.Shared.DTOs;
using RipperdocShop.Shared.DTOs.Brands;

namespace RipperdocShop.Api.Modules.Brands.Commands;

public class CreateBrandCommand(ApplicationDbContext context)
{
    public async Task<Brand> ExecuteAsync(BrandCreateDto createDto)
    {
        var brand = new Brand(createDto.Name, createDto.Description);
        context.Brands.Add(brand);
        await context.SaveChangesAsync();
        return brand;
    }
}
