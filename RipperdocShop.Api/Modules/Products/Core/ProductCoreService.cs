using Microsoft.EntityFrameworkCore;
using RipperdocShop.Api.Data;
using RipperdocShop.Api.Models.Entities;

namespace RipperdocShop.Api.Modules.Products.Core;

public class ProductCoreService(ApplicationDbContext context) : IProductCoreService
{
    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await context.Products.FindAsync(id);
    }

    public async Task<Product?> GetByIdWithDetailsAsync(Guid id)
    {
        return await context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Product?> GetBySlugWithDetailsAsync(string slug)
    {
        return await context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .FirstOrDefaultAsync(p => p.Slug == slug);
    }

    public async Task<(IEnumerable<Product> Products, int TotalCount, int TotalPages)> GetByCategorySlugAsync(
        string slug, bool includeDeleted, int page, int pageSize)
    {
        var query = context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Where(p =>
                (p.Category.Slug == slug) &&
                // Product must not be soft-deleted (unless includeDeleted)
                (includeDeleted || p.DeletedAt == null) &&
                // Category must not be soft-deleted (unless includeDeleted)
                (includeDeleted || p.Category.DeletedAt == null) &&
                // Brand: if present, must not be soft-deleted (unless includeDeleted)
                (p.Brand == null || includeDeleted || p.Brand.DeletedAt == null)
            );

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var products = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (products, totalCount, totalPages);
    }

    public async Task<(IEnumerable<Product> Products, int TotalCount, int TotalPages)> GetByBrandSlugAsync(
        string slug, bool includeDeleted, int page, int pageSize)
    {
        var query = context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Where(p =>
                p.Brand != null && (p.Brand.Slug == slug) &&
                (includeDeleted || p.DeletedAt == null) &&
                (includeDeleted || p.Category.DeletedAt == null) &&
                (includeDeleted || p.Brand.DeletedAt == null)
            );

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var products = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (products, totalCount, totalPages);
    }

    public async Task<(IEnumerable<Product> Products, int TotalCount, int TotalPages)> GetAllAsync(
        bool includeDeleted, int page, int pageSize)
    {
        var query = context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Where(p =>
                // Product must not be soft-deleted (unless includeDeleted)
                (includeDeleted || p.DeletedAt == null) &&
                // Category must not be soft-deleted (unless includeDeleted)
                (includeDeleted || p.Category.DeletedAt == null) &&
                // Brand: if present, must not be soft-deleted (unless includeDeleted)
                (p.Brand == null || includeDeleted || p.Brand.DeletedAt == null)
            );

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var products = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (products, totalCount, totalPages);
    }

    public async Task<IEnumerable<Product>> GetFeaturedAsync()
    {
        var query = context.Products
            .Where(p => (p.IsFeatured && p.DeletedAt == null));

        return await query.ToListAsync();
    }
}
