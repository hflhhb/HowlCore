using System;
using System.Collections.Generic;
using System.Text;

namespace Howl.Core.Contracts
{
    /// <summary>
    /// 表示分页结果的数据容器。
    /// </summary>
    /// <typeparam name="T">数据项类型。</typeparam>
    public class Paged<T>
    {
        /// <summary>
        /// 获取或设置总记录数。
        /// </summary>
        public long? Total { get; set; }

        /// <summary>
        /// 获取或设置当前页的数据项集合。
        /// </summary>
        public IEnumerable<T> Items { get; set; }

        /// <summary>
        /// 获取一个空的分页结果实例。
        /// </summary>
        public static Paged<T> Empty { get; } = new Paged<T> { Total = 0, Items = new T[] { } };

        /// <summary>
        /// 创建一个包含指定项和总数的分页结果实例。
        /// </summary>
        /// <param name="items">数据项集合。</param>
        /// <param name="count">总记录数。</param>
        /// <returns>新的分页结果实例。</returns>
        public static Paged<T> Create(IEnumerable<T> items, long count) => new Paged<T> { Total = count, Items = items };
    }
}
