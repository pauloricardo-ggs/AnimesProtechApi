namespace Core.Helpers.Errors;

public class BaseError
{
    protected BaseError(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public string Code { get; private set; }
    public string Message { get; private set; }

    public static BaseError AccessDenied = new("AccessDenied", "Access denied.");
    public static BaseError InvalidArguments = new("InvalidArguments", "One or more arguments are invalid.");
    public static BaseError UnexpectedBehavior = new("UnexpectedBehavior", "The server encountered an unexpected behavior.");
    public static BaseError NotAuthenticated = new("NotAuthenticated", "Not authenticated.");
    public static BaseError ResourceNotFound = new("ResourceNotFound", "This resource was not found.");
    public static BaseError RuleNotMet = new("RuleNotMet", "One or more rules were not met.");
}