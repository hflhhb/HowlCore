using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;

namespace Howl.Core.Contracts
{
    public interface IQuery
    {
        int? Take { get; set; }
        int? Skip { get; }
        //
        int? Page { get; set; }
    }
    public interface IQuery<TEntity> : IQuery
    {
    }

	public class Query<T> : IQuery<T>
	{
        //private int _take = 10;
        //private int _skip = 0;
        //private int _page = 0;

        //[NotMapped]
        //public int? Take { get { return _take <= 0 ? 10 : _take; } set { _take = value ?? 10; } }
        //[NotMapped]
        //public int? Skip { get { return _skip <= 0 ? 0 : _skip; } set { _skip = value ?? 0; } }
        //[NotMapped]
        //public int? Page { get { return _page <= 0 ? 0 : _page; } set { _page = value ?? 0; } }

        [NotMapped]
        public int? Take { get; set; }
        [NotMapped]
        public int? Skip => Page * Take;
        //
        [NotMapped]
        public int? Page { get; set; }
        [NotMapped]
        public string[] Includes { get; set; }
        //[NotMapped]
        //public OrderBy[] OrderBies { get; set; }
        /// <summary>
        /// 排序字符串(属性,升降序;属性,升降序)
        /// </summary>
        [NotMapped]
        public string SortString { get; set; }
    }
}
