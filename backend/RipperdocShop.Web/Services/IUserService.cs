using RipperdocShop.Shared.DTOs;

namespace RipperdocShop.Web.Services;

public interface IUserService
{
    Task<bool> Login(string email, string password);
    Task<bool> Logout();
    Task<bool> Register(string email, string password);
    Task<WhoAmIDto?> WhoAmI();
}
