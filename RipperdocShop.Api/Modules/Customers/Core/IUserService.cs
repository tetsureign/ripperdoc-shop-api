using RipperdocShop.Api.Models.Identities;

namespace RipperdocShop.Api.Modules.Customers.Core;

public interface IUserService
{
    Task<AppUser?> GetByIdAsync(Guid id);
}
