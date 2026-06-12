using RipperdocShop.Api.Data;
using RipperdocShop.Api.Models.Identities;

namespace RipperdocShop.Api.Modules.Customers.Queries;

public class GetUserByIdQuery(ApplicationDbContext context)
{
    public async Task<AppUser?> ExecuteAsync(Guid id)
    {
        return await context.Users.FindAsync(id);
    }
}
