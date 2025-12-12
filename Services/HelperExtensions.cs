using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
 
namespace WMS_WEBAPI.Services
{
    public class Filter
    {
        public string Column { get; set; } = string.Empty;
        public string Operator { get; set; } = "Equals";
        public string Value { get; set; } = string.Empty;
    }

    public class PagedRequest
    {
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? SortBy { get; set; } = "Id";
    public string? SortDirection { get; set; } = "desc";
    public List<Filter>? Filters { get; set; } = new();
    }

    public static class QueryHelper
    {
        public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, System.Collections.Generic.List<Filter>? filters)
        {
            if (filters == null || filters.Count == 0) return query;

            ParameterExpression param = Expression.Parameter(typeof(T), "x");
            Expression? predicate = null;

            foreach (var filter in filters)
            {
                if (string.IsNullOrEmpty(filter.Value)) continue;
                var property = typeof(T).GetProperty(filter.Column);
                if (property == null) continue;
                var left = Expression.Property(param, property);
                Expression? exp = null;

                if (property.PropertyType == typeof(string))
                {
                    var method = filter.Operator switch
                    {
                        "Contains" => typeof(string).GetMethod("Contains", new[] { typeof(string) }),
                        "StartsWith" => typeof(string).GetMethod("StartsWith", new[] { typeof(string) }),
                        "EndsWith" => typeof(string).GetMethod("EndsWith", new[] { typeof(string) }),
                        _ => null
                    };
                    if (method != null) exp = Expression.Call(left, method, Expression.Constant(filter.Value));
                    else exp = Expression.Equal(left, Expression.Constant(filter.Value));
                }
                else if (property.PropertyType == typeof(int))
                {
                    int val = int.Parse(filter.Value);
                    exp = filter.Operator switch
                    {
                        ">=" => Expression.GreaterThanOrEqual(left, Expression.Constant(val)),
                        "<=" => Expression.LessThanOrEqual(left, Expression.Constant(val)),
                        _ => Expression.Equal(left, Expression.Constant(val))
                    };
                }
                else if (property.PropertyType == typeof(decimal))
                {
                    decimal val = decimal.Parse(filter.Value);
                    exp = filter.Operator switch
                    {
                        ">=" => Expression.GreaterThanOrEqual(left, Expression.Constant(val)),
                        "<=" => Expression.LessThanOrEqual(left, Expression.Constant(val)),
                        _ => Expression.Equal(left, Expression.Constant(val))
                    };
                }
                else if (property.PropertyType == typeof(DateTime))
                {
                    DateTime val = DateTime.Parse(filter.Value);
                    exp = filter.Operator switch
                    {
                        ">=" => Expression.GreaterThanOrEqual(left, Expression.Constant(val)),
                        "<=" => Expression.LessThanOrEqual(left, Expression.Constant(val)),
                        _ => Expression.Equal(left, Expression.Constant(val))
                    };
                }
                else if (property.PropertyType.IsEnum)
                {
                    var enumVal = Enum.Parse(property.PropertyType, filter.Value);
                    exp = Expression.Equal(left, Expression.Constant(enumVal));
                }

                if (exp != null) predicate = predicate == null ? exp : Expression.AndAlso(predicate, exp);
            }

            if (predicate == null) return query;
            var lambda = Expression.Lambda<Func<T, bool>>(predicate, param);
            return query.Where(lambda);
        }

        public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, string sortBy, bool desc)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var member = Expression.PropertyOrField(parameter, sortBy);
            var keySelector = Expression.Lambda(
                typeof(Func<,>).MakeGenericType(typeof(T), member.Type),
                member,
                parameter
            );
            var methodName = desc ? "OrderByDescending" : "OrderBy";
            var call = Expression.Call(
                typeof(Queryable),
                methodName,
                new[] { typeof(T), member.Type },
                query.Expression,
                keySelector
            );
            return query.Provider.CreateQuery<T>(call);
        }

        public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, int page, int pageSize)
        {
            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}
