using RipperdocShop.Api.Data;
using RipperdocShop.Api.Models.Identities;

namespace RipperdocShop.Api.Modules.Customers.Core;

public class UserService(ApplicationDbContext context) : IUserService
{
    public async Task<AppUser?> GetByIdAsync(Guid id)
    {
        return await context.Users.FindAsync(id);
    }
}
