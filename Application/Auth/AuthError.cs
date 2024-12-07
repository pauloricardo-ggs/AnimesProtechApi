using Microsoft.AspNetCore.Mvc;

namespace Application.Shared.Errors;

public class AuthError : BaseError
{
    private AuthError(ObjectResult result) : base(result) { }

    public static AuthError InvalidCredentials = new (new UnauthorizedObjectResult(InvalidCredentialsMessage));
    public static AuthError Blocked = new (new ForbidObjectResult(BlockedMessage));

    public const string InvalidCredentialsMessage = "Email or password is invalid.";
    public const string BlockedMessage = "User is blocked by excessive attempts.";
}