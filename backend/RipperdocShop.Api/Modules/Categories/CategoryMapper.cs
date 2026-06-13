using RipperdocShop.Api.Models.Entities;
using RipperdocShop.Shared.DTOs;
using RipperdocShop.Shared.DTOs.Categories;

namespace RipperdocShop.Api.Modules.Categories;

internal static class CategoryMapper
{
    internal static CategoryDto? ToDto(this Category? category)
    {
        if (category is null) return null;

        return new CategoryDto
        {
            Name = category.Name,
            Slug = category.Slug,
            Description = category.Description
        };
    }
}
