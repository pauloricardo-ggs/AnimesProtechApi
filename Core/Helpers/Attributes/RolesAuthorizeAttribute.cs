using Application.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Core.Helpers.Attributes;

public class RolesAuthorizeAttribute(params string[] roles) : Attribute, IAuthorizationFilter
{
    private readonly string[] _roles = roles;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        if (user?.Identity == null || !user.Identity.IsAuthenticated)
        {
            context.Result = BaseError.NotAuthenticated.Result;
            return;
        }

        if (_roles.Any() && !_roles.Any(user.IsInRole))
        {
            context.Result = BaseError.AccessDenied.Result;
            return;
        }
    }
}