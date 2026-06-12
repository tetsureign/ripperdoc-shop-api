using RipperdocShop.Api.Models.Entities;
using RipperdocShop.Shared.DTOs;

namespace RipperdocShop.Api.Modules.Brands;

internal static class BrandMapper
{
    internal static BrandDto? ToDto(this Brand? brand)
    {
        if (brand is null) return null;

        return new BrandDto
        {
            Name = brand.Name,
            Slug = brand.Slug,
            Description = brand.Description
        };
    }

    internal static BrandCreateDto? ToCreateDto(this Brand? brand)
    {
        if (brand is null) return null;

        return new BrandCreateDto
        {
            Name = brand.Name,
            Description = brand.Description
        };
    }
}
