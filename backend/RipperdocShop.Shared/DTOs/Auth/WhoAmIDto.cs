namespace RipperdocShop.Shared.DTOs.Auth;

public class WhoAmIDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public IEnumerable<string> Roles { get; set; } = null!;
}
