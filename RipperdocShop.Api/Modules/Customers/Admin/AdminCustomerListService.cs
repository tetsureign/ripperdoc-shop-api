using Microsoft.EntityFrameworkCore;
using RipperdocShop.Api.Data;
using RipperdocShop.Shared.DTOs;

namespace RipperdocShop.Api.Modules.Customers.Admin;

public class AdminCustomerListService(ApplicationDbContext context) : IAdminCustomerListService
{
    public async Task<(IEnumerable<UserDto> Users, int TotalCount, int TotalPages)> GetAllAsync(bool includeDeleted,
        int page, int pageSize)
    {
        var query = context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Where(u => u.UserRoles.Any(ur => ur.Role.Name == "Customer"))
            .Where(u => includeDeleted || u.DeletedAt == null)
            .Select(u => new UserDto
            {
                Id = u.Id,
                UserName = u.UserName!,
                Email = u.Email!,
                EmailConfirmed = u.EmailConfirmed,
                LockoutEnabled = u.LockoutEnabled,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt,
                DeletedAt = u.DeletedAt,
                IsDisabled = u.IsDisabled,
                Roles = u.UserRoles.Select(ur => ur.Role.Name).ToList()
            });

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var users = await query
            .OrderByDescending(u => u.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (users, totalCount, totalPages);
    }
}
