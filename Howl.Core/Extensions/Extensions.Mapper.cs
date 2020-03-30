using AutoMapper;
using Howl.Core.Reflection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Howl.Core.Extensions
{
    public static partial class  MapperExtensions
    {
        private static ConcurrentDictionary<Type, ConcurrentDictionary<Type, IMapper>> TypeMappers { get; } = new ConcurrentDictionary<Type, ConcurrentDictionary<Type, IMapper>>();
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

        public static T1 Inherit<T1, T2>(this T1 me, T2 source)
        {
            if (source == null) return me;

            var properties = typeof(T2).GetProperties().Where(el => el.CanRead && el.CanWrite).ToArray();
            foreach (var property in properties)
            {
                var oldValue = me.Get(property.Name);
                if (oldValue != null) continue;

                var newValue = source.Get(property);
                if (newValue == null) continue;

                me.Set(property, newValue);
            }

            return me;
        }
    }
}
