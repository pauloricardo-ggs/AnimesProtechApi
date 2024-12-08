using System.Text.RegularExpressions;

namespace Core.Helpers;

public static class HttpContextExtensions
{
    public static string[]? GetQueryFilters(this HttpContext context)
    {
        return (from param in context.Request.Query
                where Regex.IsMatch(param.Key, @"(.+)\[(.+)\]")
                select $"{param.Key}={param.Value}").ToArray();
    }
}