using Core.Helpers.Errors;

namespace Core.Helpers.Erros;

public class AuthError : BaseError
{
    private AuthError(string code, string message)
        : base(code, message) { }

    public static BaseError InvalidCredentials = new AuthError("InvalidCredentials", "Email or password is invalid.");
    public static BaseError Blocked = new AuthError("Blocked", "User is blocked by excessive attempts.");
    public static BaseError PasswordsDontMatch = new AuthError("PasswordsDontMatch", "Passwords don't match.");
}