using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;

namespace Howl.Core.Contracts
{
    /// <summary>
    /// 定义分页查询的基本接口。
    /// </summary>
    public interface IQuery
    {
        /// <summary>
        /// 获取或设置每页获取的记录数。
        /// </summary>
        int? Take { get; set; }

        /// <summary>
        /// 获取跳过的记录数。
        /// </summary>
        int? Skip { get; }

        /// <summary>
        /// 获取或设置当前页码。
        /// </summary>
        int? Page { get; set; }
    }

    /// <summary>
    /// 定义针对特定实体类型的分页查询接口。
    /// </summary>
    /// <typeparam name="TEntity">实体类型。</typeparam>
    public interface IQuery<TEntity> : IQuery
    {
    }

    /// <summary>
    /// 分页查询参数类，用于构建分页请求。
    /// </summary>
    /// <typeparam name="T">实体类型。</typeparam>
    public class Query<T> : IQuery<T>
    {
        /// <summary>
        /// 获取或设置每页获取的记录数。
        /// </summary>
        [NotMapped]
        public int? Take { get; set; }

        /// <summary>
        /// 获取跳过的记录数，计算公式为 Page * Take。
        /// </summary>
        [NotMapped]
        public int? Skip => Page * Take;

        /// <summary>
        /// 获取或设置当前页码（从0开始）。
        /// </summary>
        [NotMapped]
        public int? Page { get; set; }

        /// <summary>
        /// 获取或设置要包含的导航属性名称数组。
        /// </summary>
        [NotMapped]
        public string[] Includes { get; set; }

        /// <summary>
        /// 获取或设置排序字符串，格式为"属性,升降序;属性,升降序"。
        /// </summary>
        [NotMapped]
        public string SortString { get; set; }
    }
}
