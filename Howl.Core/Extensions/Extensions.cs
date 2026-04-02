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
    /// <summary>
    /// 提供通用扩展方法的静态类。
    /// </summary>
    public static partial class Extensions
    {
        #region 类型转换

        /// <summary>
        /// 将源对象转换为目标类型。
        /// </summary>
        /// <typeparam name="TSource">源类型。</typeparam>
        /// <typeparam name="TTarget">目标类型，必须是值类型。</typeparam>
        /// <param name="me">要转换的源对象。</param>
        /// <param name="defaultValue">转换失败时返回的默认值。</param>
        /// <returns>转换后的值或默认值。</returns>
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

        /// <summary>
        /// 将可空源对象转换为目标类型。
        /// </summary>
        /// <typeparam name="TSource">源类型，必须是值类型。</typeparam>
        /// <typeparam name="TTarget">目标类型，必须是值类型。</typeparam>
        /// <param name="me">要转换的可空源对象。</param>
        /// <param name="defaultValue">转换失败时返回的默认值。</param>
        /// <returns>转换后的值或默认值。</returns>
        public static TTarget To<TSource, TTarget>(this TSource? me, TTarget defaultValue = default(TTarget))
            where TSource : struct
            where TTarget : struct
        {
            if (me == null) return defaultValue;
            //
            return To<TSource, TTarget>(me.Value, defaultValue);
        }

        /// <summary>
        /// 将字符串转换为目标类型。
        /// </summary>
        /// <typeparam name="T">目标类型，必须是值类型。</typeparam>
        /// <param name="me">要转换的字符串。</param>
        /// <param name="defaultValue">转换失败时返回的默认值。</param>
        /// <returns>转换后的值或默认值。</returns>
        public static T To<T>(this string me, T defaultValue = default(T)) where T : struct
        {
                return To<string, T>(me, defaultValue);
            }

        /// <summary>
        /// 将对象转换为可空的指定类型。
        /// </summary>
        /// <typeparam name="T">目标类型，必须是值类型。</typeparam>
        /// <param name="me">要转换的对象。</param>
        /// <param name="defaultValue">转换失败时返回的默认值。</param>
        /// <returns>转换后的可空值或默认值。</returns>
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

        /// <summary>
        /// 尝试将对象转换为指定类型。
        /// </summary>
        /// <typeparam name="T">目标类型。</typeparam>
        /// <param name="me">要转换的对象。</param>
        /// <returns>转换后的值或类型默认值。</returns>
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

        #endregion

        #region 集合判断

        /// <summary>
        /// 判断可空值是否在指定的值集合中。
        /// </summary>
        /// <typeparam name="T">值类型，必须是值类型。</typeparam>
        /// <param name="me">要判断的可空值。</param>
        /// <param name="values">要匹配的值集合。</param>
        /// <returns>如果值在集合中则返回true，否则返回false。</returns>
        public static bool In<T>(this T? me, params T?[] values) where T : struct
        {
            if (values == null || !values.Any()) return false;

            return values.Contains(me);
        }

        /// <summary>
        /// 判断字符串是否在指定的字符串集合中。
        /// </summary>
        /// <param name="me">要判断的字符串。</param>
        /// <param name="values">要匹配的字符串集合。</param>
        /// <returns>如果字符串在集合中则返回true，否则返回false。</returns>
        public static bool In(this string me, params string[] values)
        {
            return values?.Contains(me) == true;
        }

        #endregion

        #region 字符串比较

        /// <summary>
        /// 使用序号排序规则并忽略大小写比较两个字符串是否相等。
        /// </summary>
        /// <param name="value">第一个字符串。</param>
        /// <param name="other">第二个字符串。</param>
        /// <returns>如果相等则返回true，否则返回false。</returns>
        public static bool EqualsIgnoreCase(this string value, string other)
        {
                return string.Equals(value, other, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 完全相等比较：两个字符串必须有值且完全相同（区分大小写）。
        /// </summary>
        /// <param name="value">第一个字符串。</param>
        /// <param name="other">第二个字符串。</param>
        /// <returns>如果完全相等则返回true，否则返回false。</returns>
        public static bool EqualsFully(this string value, string other)
        {
                return !string.IsNullOrWhiteSpace(value) && !string.IsNullOrWhiteSpace(other) && value == other;
        }

        #endregion

        #region URL处理

        /// <summary>
        /// 将字典转换为URL查询字符串。
        /// </summary>
        /// <param name="me">要转换的字典。</param>
        /// <returns>URL查询字符串。</returns>
        public static string ToQueryString(this IDictionary<string, object> me)
        {
            if (me == null) return string.Empty;
            var qs = string.Join("&", me.Select(pair => $"{pair.Key}={HttpUtility.UrlEncode(pair.Value?.ToString() ?? string.Empty)}"));

            return qs;
        }

        /// <summary>
        /// 将参数对象追加到URL作为查询字符串。
        /// </summary>
        /// <param name="url">原始URL。</param>
        /// <param name="parameters">要追加的参数对象。</param>
        /// <returns>追加查询字符串后的URL。</returns>
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
        /// 对字符串进行URL编码。
        /// </summary>
        /// <param name="me">要编码的字符串。</param>
        /// <returns>编码后的字符串。</returns>
        public static string ToUrlEncode(this string me)
        {
            return HttpUtility.UrlEncode(me);
        }

        #endregion

        #region 集合操作

        /// <summary>
        /// 判断集合是否有任何元素。
        /// </summary>
        /// <typeparam name="T">元素类型。</typeparam>
        /// <param name="t">要判断的集合。</param>
        /// <param name="predicate">可选的筛选条件。</param>
        /// <returns>如果集合有元素则返回true，否则返回false。</returns>
        public static bool IsAny<T>(this IEnumerable<T> t, Func<T, bool> predicate = null)
        {
            if (t == null) return false;
            if (predicate != null) return t.Any(predicate);
            return t.Any();
        }

        /// <summary>
        /// 判断集合是否为空。
        /// </summary>
        /// <typeparam name="T">元素类型。</typeparam>
        /// <param name="t">要判断的集合。</param>
        /// <returns>如果集合为空或null则返回true，否则返回false。</returns>
        public static bool IsEmpty<T>(this IEnumerable<T> t)
        {
                if (t == null) return true;
                return !t.Any();
        }

        #endregion

        #region 字符串判断

        /// <summary>
        /// 判断字符串是否有值（非空）。
        /// </summary>
        /// <param name="value">要判断的字符串。</param>
        /// <returns>如果字符串有值则返回true，否则返回false。</returns>
        public static bool HasValue(this string value)
        {
                return !string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// 判断字符串是否为空或null。
        /// </summary>
        /// <param name="value">要判断的字符串。</param>
        /// <returns>如果字符串为空或null则返回true，否则返回false。</returns>
        public static bool IsNullOrEmpty(this string value)
        {
                return string.IsNullOrEmpty(value);
        }

        #endregion

        #region 字符串连接

        /// <summary>
        /// 使用选择器将集合元素连接成字符串。
        /// </summary>
        /// <typeparam name="T">元素类型。</typeparam>
        /// <param name="source">要连接的集合。</param>
        /// <param name="selector">选择要连接的字符串的函数。</param>
        /// <param name="separator">分隔符，默认为逗号。</param>
        /// <returns>连接后的字符串。</returns>
        public static string StringJoin<T>(this IEnumerable<T> source, Func<T, string> selector, string separator = ",")
        {
            if (!source.IsAny()) return "";
            if (selector != null)
            {
                return string.Join(separator, source.Select(selector));
            }
            return string.Join(separator, source);
        }

        /// <summary>
        /// 将集合元素连接成字符串。
        /// </summary>
        /// <typeparam name="T">元素类型。</typeparam>
        /// <param name="source">要连接的集合。</param>
        /// <param name="separator">分隔符，默认为逗号。</param>
        /// <returns>连接后的字符串。</returns>
        public static string StringJoin<T>(this IEnumerable<T> source, string separator = ",")
        {
            if (!source.IsAny()) return "";
            return string.Join(separator, source);
        }

        /// <summary>
        /// 返回第一个非空的字符串值。
        /// </summary>
        /// <param name="me">当前字符串。</param>
        /// <param name="defaultValues">候选的默认值数组。</param>
        /// <returns>第一个非空的字符串，如果都为空则返回null。</returns>
        public static string Coalesce(this string me, params string[] defaultValues)
        {
                return !string.IsNullOrEmpty(me) ? me : (defaultValues.FirstOrDefault(v => !string.IsNullOrEmpty(v)));
        }

        /// <summary>
        /// 使用参数格式化字符串，等同于 string.Format。
        /// </summary>
        /// <param name="me">格式字符串。</param>
        /// <param name="args">格式化参数。</param>
        /// <returns>格式化后的字符串。</returns>
        public static string Supplant(this string me, params object[] args)
        {
                return string.Format(me, args);
        }

        #endregion

        #region 枚举描述

        /// <summary>
        /// 获取枚举值的 Description 特性描述。
        /// </summary>
        /// <param name="value">枚举值。</param>
        /// <returns>Description 特性的描述文本，如果没有则返回空字符串。</returns>
        public static string OfDescription(this Enum value)
        {
            if (value == null) return string.Empty;
            var type = value.GetType();
            var field = type.GetField(Enum.GetName(type, value));
            var attr = field.GetCustomAttribute<DescriptionAttribute>();

            return attr?.Description ?? string.Empty;
        }

        #endregion

        #region 字符串分割

        /// <summary>
        /// 将字符串按分隔符分割成字符串数组。
        /// </summary>
        /// <param name="me">要分割的字符串。</param>
        /// <param name="separator">分隔符，默认为逗号。</param>
        /// <returns>分割后的字符串数组，如果字符串为空则返回null。</returns>
        public static string[] SplitString(this string me, char separator = ',')
        {
            if (string.IsNullOrEmpty(me)) return null;
            return me.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries).ToArray();
        }

        /// <summary>
        /// 将字符串按分隔符分割并转换为指定类型的集合。
        /// </summary>
        /// <typeparam name="T">目标类型，必须是值类型。</typeparam>
        /// <param name="me">要分割的字符串。</param>
        /// <param name="separator">分隔符，默认为逗号。</param>
        /// <returns>转换后的集合，如果字符串为空则返回null。</returns>
        public static IEnumerable<T> SplitTo<T>(this string me, char separator = ',') where T : struct
        {
            if (string.IsNullOrEmpty(me)) return null;
            return me.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries)
                .Select(o => o.To<T>())
                .ToList();
        }

        /// <summary>
        /// 将字符串按分隔符分割并转换为可空类型的数组。
        /// </summary>
        /// <typeparam name="T">目标类型，必须是值类型。</typeparam>
        /// <param name="me">要分割的字符串。</param>
        /// <param name="separator">分隔符，默认为逗号。</param>
        /// <returns>转换后的可空类型数组，如果字符串为空则返回null。</returns>
        public static T?[] SplitToNullable<T>(this string me, char separator = ',') where T : struct
        {
            if (string.IsNullOrEmpty(me)) return null;
            return me.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries)
                .Select(o => o.Nullable<T>())
                .ToArray();
        }

        #endregion

        #region 集合转换

        /// <summary>
        /// 将集合转换为 List，如果已经是 List 则直接返回。
        /// </summary>
        /// <typeparam name="T">元素类型。</typeparam>
        /// <param name="me">要转换的集合。</param>
        /// <returns>List 实例。</returns>
        public static List<T> AsList<T>(this IEnumerable<T> me)
        {
            if (me is List<T> list) return list;
            return me.ToList();
        }

        #endregion

        #region 字典操作

        /// <summary>
        /// 从字典中获取指定键的值，如果键不存在则返回默认值。
        /// </summary>
        /// <typeparam name="TKey">键类型。</typeparam>
        /// <typeparam name="TValue">值类型。</typeparam>
        /// <param name="me">字典实例。</param>
        /// <param name="key">要获取的键。</param>
        /// <param name="defaultValue">键不存在时返回的默认值。</param>
        /// <returns>键对应的值或默认值。</returns>
        public static TValue Value<TKey, TValue>(this IDictionary<TKey, TValue> me, TKey key, TValue defaultValue = default(TValue))
        {
            if (me == null || key == null) return defaultValue;

            if (me.TryGetValue(key, out var result))
            {
                return result;
            }

            return defaultValue;
        }

        #endregion
    }
}
