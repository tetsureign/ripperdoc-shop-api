using RipperdocShop.Shared.DTOs;
using RipperdocShop.Api.Models.Entities;

namespace RipperdocShop.Api.Modules.Categories.Admin;

public interface IAdminCategoryService
{
    Task<Category> CreateAsync(CategoryCreateDto createDto);
    Task<Category?> UpdateAsync(Guid id, CategoryCreateDto createDto);
    Task<Category?> SoftDeleteAsync(Guid id);
    Task<Category?> RestoreAsync(Guid id);
    Task<Category?> DeletePermanentlyAsync(Guid id);
}
