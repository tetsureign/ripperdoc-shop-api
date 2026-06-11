using RipperdocShop.Shared.DTOs;
using RipperdocShop.Api.Models.Entities;

namespace RipperdocShop.Api.Modules.Products.Admin;

public interface IAdminProductService
{
    Task<Product?> CreateAsync(ProductCreateDto createDto);
    Task<Product?> UpdateAsync(Guid id, ProductCreateDto createDto);
    Task<Product?> SetFeaturedAsync(Guid id);
    Task<Product?> RemoveFeaturedAsync(Guid id);
    Task<Product?> SoftDeleteAsync(Guid id);
    Task<Product?> RestoreAsync(Guid id);
    Task<Product?> DeletePermanentlyAsync(Guid id);
}
