namespace RipperdocShop.Api.Modules.Auth.Commands;

public class LogoutCommand
{
    public object Execute()
    {
        return new { message = "Wiped clean" };
    }
}
