using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Domain.Helpers;
using Microsoft.EntityFrameworkCore;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, string[]? filters)
    {
        if (filters == null || filters.Length == 0)
            return query;

        foreach (var filter in filters)
        {
            var match = Regex.Match(filter, @"(?<key>[^\[]+)\[(?<operator>[^\]]+)\]=(?<value>.+)");
            if (!match.Success) continue;

            var key = match.Groups["key"].Value;
            var op = match.Groups["operator"].Value;
            var value = match.Groups["value"].Value;

            query = query.ApplyFilter(key, op, value);
        }

        return query;
    }

    private static IQueryable<T> ApplyFilter<T>(this IQueryable<T> query, string key, string op, string value)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.PropertyOrField(parameter, key);

        var likeMethod = typeof(DbFunctionsExtensions).GetMethod(nameof(DbFunctionsExtensions.Like), new[] { typeof(DbFunctions), typeof(string), typeof(string) });

        Expression? filterExpression = op switch
        {
            "like" => Expression.Call(
                typeof(DbFunctionsExtensions).GetMethod(nameof(DbFunctionsExtensions.Like), [typeof(DbFunctions), typeof(string), typeof(string)])!,
                Expression.Constant(EF.Functions),
                Expression.Call(property, typeof(string).GetMethod("ToLower", Type.EmptyTypes)!),
                Expression.Constant($"%{value.ToLower()}%")
            ),
            "eq" => Expression.Equal(property, Expression.Constant(value)),
            "ne" => Expression.NotEqual(property, Expression.Constant(value)),
            "lte" => Expression.LessThanOrEqual(property, Expression.Constant(Convert.ChangeType(value, property.Type))),
            "le" => Expression.LessThan(property, Expression.Constant(Convert.ChangeType(value, property.Type))),
            "gte" => Expression.GreaterThanOrEqual(property, Expression.Constant(Convert.ChangeType(value, property.Type))),
            "ge" => Expression.GreaterThan(property, Expression.Constant(Convert.ChangeType(value, property.Type))),
            _ => null
        };

        if (filterExpression == null)
            return query;

        var lambda = Expression.Lambda<Func<T, bool>>(filterExpression, parameter);
        return query.Where(lambda);
    }

    public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, int page, int pageSize)
    {
        return query.Skip((page - 1) * pageSize).Take(pageSize);
    }

    public static async Task<PagedList<T>> ToPagedList<T>(this IQueryable<T> query, int page, int pageSize)
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = 10;
        var totalCount = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PagedList<T>(items, page, pageSize, totalCount);
    }
}