namespace RipperdocShop.Api.Modules.Auth.Commands;

public enum AuthFailureReason
{
    UserMissingOrRevoked,
    WrongCredentials,
    IdentityErrors
}

public record AuthCommandResult(
    bool Succeeded,
    string Message,
    string? Token = null,
    AuthFailureReason? FailureReason = null,
    IEnumerable<object>? Errors = null);
