using System.Linq.Expressions;
using System.Text.Json;

namespace Nexora.Core.Common.Extensions
{
    public static class JsonExtension
    {
        public static string ToJson<T>(this T obj, bool includeRawString = true)
            => SerializeToJson(obj, includeRawString: includeRawString) ?? string.Empty;

        public static string ToJson<T>(this T obj, bool indentFormatting, bool includeRawString = true)
            => SerializeToJson(obj, new JsonSerializerOptions { WriteIndented = indentFormatting }, includeRawString) ?? string.Empty;

        public static string ToJson<T>(this T obj, JsonSerializerOptions jsonSerializerOptions, bool includeRawString = true)
            => SerializeToJson(obj, jsonSerializerOptions, includeRawString) ?? string.Empty;

        public static T? FromJson<T>(this string json, JsonSerializerOptions jsonSerializerOptions)
        {
            if (!string.IsNullOrEmpty(json))
            {
                if (typeof(T) == typeof(string) || Nullable.GetUnderlyingType(typeof(T)) != null)
                {
                    return (T)Convert.ChangeType(json, Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T));
                }
                else
                {
                    return JsonSerializer.Deserialize<T>(json, jsonSerializerOptions);
                }
            }

            return default;
        }

        public static object? FromJson(this string json, Type destinationType, JsonSerializerOptions jsonSerializerOptions)
        {

            if (!string.IsNullOrEmpty(json) && destinationType != null)
            {
                if (destinationType == typeof(string) || Nullable.GetUnderlyingType(destinationType) != null)
                    return Convert.ChangeType(json, Nullable.GetUnderlyingType(destinationType) ?? destinationType);
                else
                    return JsonSerializer.Deserialize(json, destinationType, jsonSerializerOptions);
            }

            return default;
        }

        public static T FromJson<T>(this string json)
        {
            if (!string.IsNullOrEmpty(json))
            {
                if (typeof(T) == typeof(string) || Nullable.GetUnderlyingType(typeof(T)) != null)
                    return (T)Convert.ChangeType(json, Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T));
                else
                    return JsonSerializer.Deserialize<T>(json);
            }

            return default;
        }

        public static string CreateJsonPathQuery<T>(this Expression<Func<T, bool>> expression)
        {
            return $"$.[?({expression.ConvertExpressionToJsonPath()})]";
        }

        /// <summary>
        /// Converts expression to json path. Supports and, or, equal, greaterthan, lessthan and contains(int,string)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static string ConvertExpressionToJsonPath<T>(this Expression<Func<T, bool>> expression)
        {
            if (expression.Body is BinaryExpression binaryExpression)
            {
                if (binaryExpression.NodeType == ExpressionType.AndAlso || binaryExpression.NodeType == ExpressionType.OrElse)
                {
                    var leftPath = ConvertExpressionToJsonPath(Expression.Lambda<Func<T, bool>>(binaryExpression.Left, expression.Parameters));

                    var rightPath = ConvertExpressionToJsonPath(Expression.Lambda<Func<T, bool>>(binaryExpression.Right, expression.Parameters));

                    return $"{leftPath} {binaryExpression.NodeType.GetOperatorByExpressionType()} {rightPath}";
                }
                else if (binaryExpression.NodeType == ExpressionType.Equal || binaryExpression.NodeType == ExpressionType.GreaterThan || binaryExpression.NodeType == ExpressionType.LessThan)
                {
                    var memberExpression = binaryExpression.Left as MemberExpression;

                    var constantExpression = binaryExpression.Right as ConstantExpression;

                    if (memberExpression != null && constantExpression != null)
                    {
                        var propertyName = memberExpression.Member.Name;

                        var propertyValue = constantExpression.Value;

                        return $"@.{propertyName} {binaryExpression.NodeType.GetOperatorByExpressionType()} {propertyValue}";
                    }
                }
            }
            else if (expression.Body is MethodCallExpression methodCallExpression)
            {
                if (methodCallExpression.Method.Name == "Contains")
                {
                    var methodExpression = (MemberExpression)(expression.Body as MethodCallExpression).Object;

                    var listValues = ((ConstantExpression)methodExpression.Expression).Value.GetType().GetField(methodExpression.Member.Name).GetValue(((ConstantExpression)methodExpression.Expression).Value);

                    if (listValues != null)
                    {
                        if (listValues is IEnumerable<int> intList)
                        {
                            var propertyName = (methodCallExpression.Arguments[0] as MemberExpression).Member.Name;

                            return $"({string.Join(" || ", intList.Select(x => $"@.{propertyName} == {x}"))})";
                        }
                        else if (listValues is IEnumerable<string> stringList)
                        {
                            var propertyName = (methodCallExpression.Arguments[0] as MemberExpression).Member.Name;

                            return $"({string.Join(" || ", stringList.Select(x => $"@.{propertyName} == \"{x}\""))})";
                        }
                    }

                    throw new ArgumentException("List values are empty.");
                }
            }

            throw new ArgumentException("Unsupported expression.");
        }

        private static string GetOperatorByExpressionType(this ExpressionType nodeType)
        {
            switch (nodeType)
            {
                case ExpressionType.AndAlso:
                    return "&&";
                case ExpressionType.OrElse:
                    return "||";
                case ExpressionType.Equal:
                    return "==";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.LessThan:
                    return "<";
                default:
                    throw new ArgumentException($"Unsupported operator: {nodeType}");
            }
        }

        private static string SerializeToJson<T>(this T obj, JsonSerializerOptions? options = null, bool includeRawString = true)
        {
            if (!includeRawString && obj is string str)
            {
                return str;
            }

            return JsonSerializer.Serialize(obj, options);
        }
    }
}