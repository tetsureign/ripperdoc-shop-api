using RipperdocShop.Shared.DTOs;

namespace RipperdocShop.Api.Modules.Customers.Admin;

public interface IAdminCustomerListService
{
    Task<(IEnumerable<UserDto> Users, int TotalCount, int TotalPages)> GetAllAsync(bool includeDeleted,
        int page, int pageSize);
}
