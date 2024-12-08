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

    public static (int Page, int PageSize) GetQueryPagination(this HttpContext context, int defaultPage = 1, int defaultPageSize = 10)
    {
        var query = context.Request.Query;

        int page = int.TryParse(query["page"], out var parsedPage) && parsedPage > 0 ? parsedPage : defaultPage;
        int pageSize = int.TryParse(query["pageSize"], out var parsedPageSize) && parsedPageSize > 0 ? parsedPageSize : defaultPageSize;

        return (page, pageSize);
    }
}