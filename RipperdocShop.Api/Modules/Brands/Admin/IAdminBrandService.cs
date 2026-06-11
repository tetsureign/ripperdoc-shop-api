using RipperdocShop.Shared.DTOs;
using RipperdocShop.Api.Models.Entities;

namespace RipperdocShop.Api.Modules.Brands.Admin;

public interface IAdminBrandService
{
    Task<Brand> CreateAsync(BrandCreateDto createDto);
    Task<Brand?> UpdateAsync(Guid id, BrandCreateDto createDto);
    Task<Brand?> SoftDeleteAsync(Guid id);
    Task<Brand?> RestoreAsync(Guid id);
    Task<Brand?> DeletePermanentlyAsync(Guid id);
}
