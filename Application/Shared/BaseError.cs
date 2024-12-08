using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Application.Shared;

public class BaseError
{
    protected BaseError(ObjectResult result) => Result = result;

    public ObjectResult Result { get; private set; }

    public static BaseError AccessDenied = new(new ForbidObjectResult("Access Denied"));
    public static BaseError UnexpectedBehavior = new(new InternalServerErrorObjectResult("The server encountered an unexpected behavior.") {});
    public static BaseError NotAuthenticated = new(new UnauthorizedObjectResult("Not authenticated."));
    public static BaseError ResourceNotFound = new(new NotFoundObjectResult("This resource was not found."));
    public static BaseError RuleNotMet = new(new BadRequestObjectResult("One or more rules were not met."));
    public static BaseError InvalidArguments(List<KeyValuePair<string, string>> errors) 
        => new (new BadRequestObjectResult(string.Join("\n", errors.Select(error => error.Value))));
    public static BaseError InvalidArguments(IEnumerable<IdentityError> errors) 
        => new (new BadRequestObjectResult(string.Join("\n", errors.Select(e => e.Description))));
}

public static class BaseErrorExtensions
{
    public static BaseError SaveLog(this BaseError error, IRequestLogger logger, string requestType, string url, object? requestData = null)
    {
        logger.SaveRequest(requestType, url, requestData, error.Result, error.Result.StatusCode);
        return error;
    }
}