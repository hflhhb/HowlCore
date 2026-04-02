using System;
using System.Collections.Generic;
using System.Text;

namespace Howl.Core
{
    /// <summary>
    /// 基于委托的工厂实现，通过委托创建指定类型的实例。
    /// </summary>
    /// <typeparam name="T">要创建的实例类型。</typeparam>
    public class DelegateFactory<T> : IFactory<T>
    {
        /// <summary>
        /// 初始化 <see cref="DelegateFactory{T}"/> 类的新实例。
        /// </summary>
        /// <param name="handler">用于创建实例的委托。</param>
        public DelegateFactory(Func<T> handler)
        {
            Handler = handler;
        }

        /// <summary>
        /// 获取用于创建实例的委托。
        /// </summary>
        protected Func<T> Handler { get; }

        /// <summary>
        /// 通过委托创建一个类型为T的实例。
        /// </summary>
        /// <returns>委托返回的实例。</returns>
        T IFactory<T>.Create()
        {
            return Handler.Invoke();
        }
    }
}
