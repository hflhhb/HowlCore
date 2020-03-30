using Howl.Core.Reflection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace Howl.Core.Extensions
{
    public  static partial class Extensions
    {
        public static TTarget To<TSource, TTarget>(this TSource me, TTarget defaultValue = default(TTarget)) where TTarget : struct
        {
            if (me == null)
            {
                return defaultValue;
            }

            try
            {
                var type = typeof(TTarget);
                if (type.IsEnum)
                {
                    if (Enum.TryParse<TTarget>(me.ToString(), true, out var returnValue))
                    {
                        return returnValue;
                    }

                    return defaultValue;
                }

                return (TTarget)Convert.ChangeType(me, type);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static TTarget To<TSource, TTarget>(this TSource? me, TTarget defaultValue = default(TTarget)) 
            where TSource : struct 
            where TTarget : struct
        {
            if (me == null) return defaultValue;
            //
            return To<TSource, TTarget>(me.Value, defaultValue);
        }

        public static T To<T>(this string me, T defaultValue = default(T)) where T : struct
        {
            return To<string, T>(me, defaultValue);
        }

        public static T? Nullable<T>(this object me, T? defaultValue = null) where T : struct
        {
            if (me == null)
            {
                return defaultValue;
            }
            try
            {
                var type = typeof(T);
                if (type.IsEnum)
                {
                    if (Enum.TryParse<T>(me.ToString(), true, out var returnValue))
                    {
                        return returnValue;
                    }

                    return defaultValue;
                }

                return (T)Convert.ChangeType(me, type);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static T As<T>(this object me)
        {
            if (me == null)
            {
                return default(T);
            }

            if (me is T converted)
            {
                return converted;
            }

            return default(T);
        }

        public static bool In<T>(this T? me, params T?[] values) where T : struct
        {
            if (values == null || !values.Any()) return false;

            return values.Contains(me);
        }

        public static bool In(this string me, params string[] values)
        {
            return values?.Contains(me) == true;
        }

        /// <summary>
        /// 序号排序规则(binary)并忽略被比较字符串的大小写 OrdinalIgnoreCase
        /// </summary>
        /// <param name="value"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool EqualsIgnoreCase(this string value, string other)
        {
            return string.Equals(value, other, StringComparison.OrdinalIgnoreCase);
            //return string.Compare(value, other, true) == 0;
        }
        /// <summary>
        /// 完全相等 必须有值并且区分大小写
        /// </summary>
        /// <param name="value"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool EqualsFully(this string value, string other)
        {
            return !string.IsNullOrWhiteSpace(value) && !string.IsNullOrWhiteSpace(other) && value == other;
        }

        public static string ToQueryString(this IDictionary<string, object> me)
        {
            if (me == null) return string.Empty;
            var qs = string.Join("&", me.Select(pair => $"{pair.Key}={HttpUtility.UrlEncode(pair.Value?.ToString() ?? string.Empty)}"));

            return qs;
        }
        public static string AppendQueries(this string url, object parameters)
        {
            if (string.IsNullOrWhiteSpace(url) || parameters == null) return url;
            var queryString = parameters.ToDictionary()?.ToQueryString();
            var indexOfHash = url.IndexOf("#", StringComparison.InvariantCultureIgnoreCase);
            var hash = string.Empty;
            var appendedUrl = url;
            if (indexOfHash >= 0)
            {
                appendedUrl = url.Substring(0, indexOfHash + 1);
                hash = url.Substring(indexOfHash);
            }

            var separator = "?";
            if (appendedUrl.Contains("?"))
                separator = appendedUrl.EndsWith("&") ? string.Empty : "&";

            appendedUrl = $"{appendedUrl}{separator}{queryString}{hash}";

            return appendedUrl;
        }

        /// <summary>
        /// 调用 HttpUtility.UrlEncode
        /// </summary>
        /// <param name="me"></param>
        /// <returns></returns>
        public static string ToUrlEncode(this string me)
        {
            return HttpUtility.UrlEncode(me);
        }

        public static bool IsAny<T>(this IEnumerable<T> t, Func<T, bool> predicate = null)
        {
            if (t == null) return false;
            if (predicate != null) return t.Any(predicate);
            return t.Any();
        }

        public static bool IsEmpty<T>(this IEnumerable<T> t)
        {
            if (t == null) return true;
            return !t.Any();
        }

        public static bool HasValue(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
        public static string StringJoin<T>(this IEnumerable<T> source, Func<T, string> selector, string separator = ",")
        {
            if (!source.IsAny()) return "";
            if (selector != null)
            {
                return string.Join(separator, source.Select(selector));
            }
            return string.Join(separator, source);
        }

        public static string StringJoin<T>(this IEnumerable<T> source, string separator = ",")
        {
            if (!source.IsAny()) return "";
            return string.Join(separator, source);
        }

        public static string Coalesce(this string me, params string[] defaultValues)
        {
            return !string.IsNullOrEmpty(me) ? me : (defaultValues.FirstOrDefault(v => !string.IsNullOrEmpty(v)));
        }

        /// <summary>
        /// equals string.Format
        /// </summary>
        /// <param name="me"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string Supplant(this string me, params object[] args)
        {
            return string.Format(me, args);
        }

        /// <summary>
        ///  获取DescriptionAttribute上指定的Description
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string OfDescription(this Enum value)
        {
            //return EnumHelper.GetEnumDescription(value);

            if (value == null) return string.Empty;
            var type = value.GetType();
            var field = type.GetField(Enum.GetName(type, value));
            var attr = field.GetCustomAttribute<DescriptionAttribute>();

            return attr?.Description ?? string.Empty;
        }

        public static string[] SplitString(this string me, char separator = ',')
        {
            if (string.IsNullOrEmpty(me)) return null;
            return me.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries).ToArray();
        }

        public static IEnumerable<T> SplitTo<T>(this string me, char separator = ',') where T : struct
        {
            if (string.IsNullOrEmpty(me)) return null;
            return me.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries)
                .Select(o => o.To<T>())
                .ToList();
        }

        public static T?[] SplitToNullable<T>(this string me, char separator = ',') where T : struct
        {
            if (string.IsNullOrEmpty(me)) return null;
            return me.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries)
                .Select(o => o.Nullable<T>())
                .ToArray();
        }

        public static List<T> AsList<T>(this IEnumerable<T> me)
        {
            if (me is List<T> list) return list;
            return me.ToList();
        }

        public static TValue Value<TKey, TValue>(this IDictionary<TKey, TValue> me, TKey key, TValue defaultValue = default(TValue))
        {
            if (me == null || key == null) return defaultValue;

            me.TryGetValue(key, out defaultValue);

            return defaultValue;
        }

    }
}
