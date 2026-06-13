using RipperdocShop.Shared.DTOs;
using RipperdocShop.Shared.DTOs.Auth;

namespace RipperdocShop.Api.Modules.Customers.Admin;

public class AdminCustomerListResponse
{
    public IEnumerable<UserDto> Customers { get; init; } = null!;
    public int TotalCount { get; init; }
    public int TotalPages { get; init; }
}
