using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Howl.Core.Extensions
{
    public static partial class Extensions
    {
        /// <summary>
        /// 根据条件决定是否应用Where筛选。
        /// </summary>
        /// <typeparam name="T">元素类型。</typeparam>
        /// <param name="source">数据源。</param>
        /// <param name="predicate">筛选条件表达式。</param>
        /// <param name="condition">是否应用筛选的条件。</param>
        /// <returns>如果条件为true则返回筛选后的结果，否则返回原数据源。</returns>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate,
            bool condition)
        {
            return condition ? source.Where(predicate) : source;
        }

        /// <summary>
        /// 根据条件决定是否应用带索引的Where筛选。
        /// </summary>
        /// <typeparam name="T">元素类型。</typeparam>
        /// <param name="source">数据源。</param>
        /// <param name="predicate">带索引的筛选条件表达式。</param>
        /// <param name="condition">是否应用筛选的条件。</param>
        /// <returns>如果条件为true则返回筛选后的结果，否则返回原数据源。</returns>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, Expression<Func<T, int, bool>> predicate,
            bool condition)
        {
            return condition ? source.Where(predicate) : source;
        }

        /// <summary>
        /// 根据条件决定是否应用Where筛选。
        /// </summary>
        /// <typeparam name="T">元素类型。</typeparam>
        /// <param name="source">数据源。</param>
        /// <param name="predicate">筛选条件。</param>
        /// <param name="condition">是否应用筛选的条件。</param>
        /// <returns>如果条件为true则返回筛选后的结果，否则返回原数据源。</returns>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, Func<T, bool> predicate, bool condition)
        {
            return condition ? source.Where(predicate) : source;
        }

        /// <summary>
        /// 如果筛选条件不为空则应用Where筛选。
        /// </summary>
        /// <typeparam name="T">元素类型。</typeparam>
        /// <param name="source">数据源。</param>
        /// <param name="predicate">筛选条件，为null时不应用筛选。</param>
        /// <returns>如果条件不为null则返回筛选后的结果，否则返回原数据源。</returns>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            return predicate != null ? source.Where(predicate) : source;
        }

        /// <summary>
        /// 根据条件决定是否应用带索引的Where筛选。
        /// </summary>
        /// <typeparam name="T">元素类型。</typeparam>
        /// <param name="source">数据源。</param>
        /// <param name="predicate">带索引的筛选条件。</param>
        /// <param name="condition">是否应用筛选的条件。</param>
        /// <returns>如果条件为true则返回筛选后的结果，否则返回原数据源。</returns>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, Func<T, int, bool> predicate, bool condition)
        {
            return condition ? source.Where(predicate) : source;
        }

        /// <summary>
        ///     如果value的值不为null、空
        /// </summary>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate,
            string value)
        {
            return !string.IsNullOrWhiteSpace(value) ? source.Where(predicate) : source;
        }

        /// <summary>
        ///     如果value的值不为null、空
        /// </summary>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, Func<T, bool> predicate, string value)
        {
            return !string.IsNullOrWhiteSpace(value) ? source.Where(predicate) : source;
        }
    }
}
