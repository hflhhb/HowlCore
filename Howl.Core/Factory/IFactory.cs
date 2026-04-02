using System;
using System.Collections.Generic;
using System.Text;

namespace Howl.Core
{
    /// <summary>
    /// 定义工厂模式的基础接口。
    /// </summary>
    public interface IFactory
    {

    }

    /// <summary>
    /// 定义创建指定类型实例的工厂接口。
    /// </summary>
    /// <typeparam name="T">要创建的实例类型。</typeparam>
    public interface IFactory<T> : IFactory
    {
        /// <summary>
        /// 创建一个类型为T的实例。
        /// </summary>
        /// <returns>新创建的实例。</returns>
        T Create();
    }
}
