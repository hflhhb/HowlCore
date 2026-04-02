using AutoMapper;
using Howl.Core.Reflection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Howl.Core.Extensions
{
    /// <summary>
    /// 提供对象映射相关的扩展方法。
    /// </summary>
    public static partial class  MapperExtensions
    {
        private static ConcurrentDictionary<Type, ConcurrentDictionary<Type, IMapper>> TypeMappers { get; } = new ConcurrentDictionary<Type, ConcurrentDictionary<Type, IMapper>>();
        /// <summary>
        /// 使用AutoMapper将源对象映射为目标类型。
        /// </summary>
        /// <typeparam name="T">目标类型。</typeparam>
        /// <param name="me">源对象。</param>
        /// <param name="target">可选的目标对象，如果提供则映射到该对象。</param>
        /// <param name="afterMap">映射完成后的回调操作。</param>
        /// <returns>映射后的目标对象。</returns>
        public static T Map<T>(this object me, T target = default(T), Action<T> afterMap = null)
        {
            if (me == null) return default;
            var typeMappers = TypeMappers;
            var sourceType = me.GetType();
            var destinationType = typeof(T);
            if (typeMappers.TryGetValue(sourceType, out var mappers) == false)
            {
                typeMappers.AddOrUpdate(sourceType, key => mappers = new ConcurrentDictionary<Type, IMapper>(), (key, value) => mappers = value);
            }

            if (mappers.TryGetValue(destinationType, out var mapper) == false)
            {
                mappers.AddOrUpdate(destinationType, key =>
                {
                    var config = new MapperConfiguration(expression =>
                    {
                        //expression.CreateMissingTypeMaps = true;
                        expression.AllowNullCollections = true;
                        expression.CreateMap(sourceType, destinationType).ForAllMembers(opts =>
                        {
                            opts.Condition((source, destination, sourceMember, destinationMember, context) =>
                                sourceMember != null
                            );
                        });
                    });

                    return mapper = config.CreateMapper();
                }, (key, value) => mapper = value);
            }
            T returnValue;
            if (target == null)
            {
                returnValue = (T)mapper.Map(me, sourceType, destinationType);
            }
            else
            {
                returnValue = (T)mapper.Map(me, target, sourceType, destinationType);
            }

            afterMap?.Invoke(returnValue);

            return returnValue;
        }

        /// <summary>
        /// 将源对象的属性值继承到目标对象。
        /// 只有当目标对象的属性为空且源对象的对应属性不为空时才会进行赋值。
        /// </summary>
        /// <typeparam name="T1">目标对象类型。</typeparam>
        /// <typeparam name="T2">源对象类型。</typeparam>
        /// <param name="me">目标对象。</param>
        /// <param name="source">源对象。</param>
        /// <returns>目标对象。</returns>
        public static T1 Inherit<T1, T2>(this T1 me, T2 source)
        {
            if (source == null) return me;

            var sourceProperties = typeof(T2).GetProperties().Where(el => el.CanRead).ToArray();
            var targetProperties = typeof(T1).GetProperties().Where(el => el.CanRead && el.CanWrite).ToArray();

            foreach (var sourceProperty in sourceProperties)
            {
                var targetProperty = targetProperties.FirstOrDefault(p => p.Name == sourceProperty.Name);
                if (targetProperty == null) continue;

                var oldValue = me.Get(targetProperty);
                // 需要被赋值的属性要为空才能被赋值，有值的时候退出
                if (oldValue != null) continue;

                var newValue = source.Get(sourceProperty);
                // 新的值要不为空，为空的时候退出
                if (newValue == null) continue;

                me.Set(targetProperty, newValue);
            }

            return me;
        }
    }
}
