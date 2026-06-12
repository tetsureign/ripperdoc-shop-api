using RipperdocShop.Shared.DTOs;

namespace RipperdocShop.Web.Services;

public class UserService(IHttpClientFactory factory) : BaseApiService(factory), IUserService
{
    public Task<bool> Login(string email, string password)
        => PostAsync("/api/auth/login", new LoginDto
        {
            Email = email,
            Password = password
        });
    
    public Task<bool> Register(string email, string password)
        => PostAsync("/api/auth/register", new LoginDto
        {
            Email = email,
            Password = password
        });
    
    public Task<bool> Logout() => PostAsync("/api/auth/logout", null!);

    public Task<WhoAmIDto?> WhoAmI() => GetAsync<WhoAmIDto>("/api/auth/whoami");
}
