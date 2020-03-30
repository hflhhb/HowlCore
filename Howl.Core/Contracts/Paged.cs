using System;
using System.Collections.Generic;
using System.Text;

namespace Howl.Core.Contracts
{
    public class Paged<T>
    {
        public long? Total { get; set; }
        public IEnumerable<T> Items { get; set; }
        public static Paged<T> Empty { get; } = new Paged<T> { Total = 0, Items = new T[] { } };
        public static Paged<T> Create(IEnumerable<T> items, long count) => new Paged<T> { Total = count, Items = items };
    }
}
