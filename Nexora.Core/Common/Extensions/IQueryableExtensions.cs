using Nexora.Core.Common.Models;
using System.Linq.Expressions;

namespace Nexora.Core.Common.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> PaginationAsync<T>(this IQueryable<T> queryable, SearchRequestBaseModel searchModel)
        {
            queryable = queryable
                .Skip((searchModel.PageNumber - 1) * searchModel.PageSize)
                .Take(searchModel.PageSize);

            return queryable;
        }

        public static IQueryable<T> OrderedPaginationAsync<T>(this IQueryable<T> queryable, SearchRequestBaseModel searchModel)
        {
            queryable = queryable
                .OrderQuery(searchModel)
                .PaginationAsync(searchModel);

            return queryable;
        }

        public static IOrderedQueryable<T> OrderQuery<T>(this IQueryable<T> queryable, SearchRequestBaseModel searchModel)
        {
            if (searchModel.OrderType.ToLower() == "desc")
                return queryable.DynamicOrderByDescending(searchModel.OrderColumn);
            else
                return queryable.DynamicOrderBy(searchModel.OrderColumn);
        }

        private static IOrderedQueryable<T> DynamicOrderBy<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder(source, property, "OrderBy");
        }

        private static IOrderedQueryable<T> DynamicOrderByDescending<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder(source, property, "OrderByDescending");
        }

        private static IOrderedQueryable<T> DynamicThenBy<T>(this IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder(source, property, "ThenBy");
        }

        private static IOrderedQueryable<T> DynamicThenByDescending<T>(this IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder(source, property, "ThenByDescending");
        }

        private static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
        {
            var props = property.Split('.');

            var type = typeof(T);

            var arg = Expression.Parameter(type, "x");

            Expression expr = arg;

            foreach (var prop in props)
            {
                var pi = type.GetProperty(prop);

                expr = Expression.Property(expr, pi);

                type = pi.PropertyType;
            }

            var delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);

            var lambda = Expression.Lambda(delegateType, expr, arg);

            return (IOrderedQueryable<T>)typeof(Queryable).GetMethods().Single(
                    method => method.Name == methodName
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), type)
                    .Invoke(null, new object[] { source, lambda });
        }
    }
}