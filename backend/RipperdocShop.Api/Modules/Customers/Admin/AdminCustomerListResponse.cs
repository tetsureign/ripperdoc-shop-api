using RipperdocShop.Shared.DTOs;

namespace RipperdocShop.Api.Modules.Customers.Admin;

public class AdminCustomerListResponse
{
    public IEnumerable<UserDto> Customers { get; init; } = null!;
    public int TotalCount { get; init; }
    public int TotalPages { get; init; }
}
