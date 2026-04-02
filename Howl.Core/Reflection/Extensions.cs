using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Howl.Core.Reflection
{
    /// <summary>
    /// 提供反射相关的扩展方法。
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 获取成员的getter委托缓存。
        /// </summary>
        public static ConcurrentDictionary<MemberInfo, Func<object, object>> Getters { get; } = new ConcurrentDictionary<MemberInfo, Func<object, object>>();

        /// <summary>
        /// 通过Lambda表达式获取对象的属性值。
        /// </summary>
        /// <typeparam name="T">属性值类型。</typeparam>
        /// <param name="me">目标对象。</param>
        /// <param name="expr">属性访问表达式。</param>
        /// <returns>属性值。</returns>
        public static T Get<T>(this object me, LambdaExpression expr)
        {
            return (T)Get(me, expr);
        }

        /// <summary>
        /// 通过成员信息获取对象的属性或字段值。
        /// </summary>
        /// <typeparam name="T">成员值类型。</typeparam>
        /// <param name="me">目标对象。</param>
        /// <param name="member">成员信息。</param>
        /// <returns>成员值。</returns>
        public static T Get<T>(this object me, MemberInfo member)
        {
            return (T)Get(me, member);
        }

        /// <summary>
        /// 通过属性名获取对象的属性值。
        /// </summary>
        /// <typeparam name="T">属性值类型。</typeparam>
        /// <param name="me">目标对象。</param>
        /// <param name="propName">属性名。</param>
        /// <returns>属性值。</returns>
        public static T Get<T>(this object me, string propName)
        {
            return (T)me.Get(propName);
        }

        /// <summary>
        /// 通过Lambda表达式获取对象的属性值。
        /// </summary>
        /// <param name="me">目标对象。</param>
        /// <param name="expr">属性访问表达式。</param>
        /// <returns>属性值。</returns>
        public static object Get(this object me, LambdaExpression expr)
        {
            var path = new Stack<PropertyInfo>();
            var memberAccess = expr.Body as MemberExpression ?? (expr.Body as UnaryExpression)?.Operand as MemberExpression;
            if (memberAccess == null) return null;

            path.Push(memberAccess.Member as PropertyInfo);
            while (memberAccess.Expression is MemberExpression)
            {
                memberAccess = (MemberExpression)memberAccess.Expression;
                path.Push(memberAccess.Member as PropertyInfo);
            }

            var value = me;
            while (value != null && path.Any())
                value = value.Get(path.Pop());

            return value;
        }

        /// <summary>
        /// 通过成员信息获取对象的属性或字段值。使用表达式树缓存以提高性能。
        /// </summary>
        /// <param name="me">目标对象。</param>
        /// <param name="member">成员信息。</param>
        /// <returns>成员值。</returns>
        public static object Get(this object me, MemberInfo member)
        {
            var type = member.DeclaringType;
            var getters = Getters;

            if (getters.TryGetValue(member, out var getter) == false)
            {
                getters.AddOrUpdate(member, key =>
                {
                    var parameter = Expression.Parameter(typeof(object));
                    var memberAccess = member.MemberType == MemberTypes.Property ? Expression.Property(Expression.Convert(parameter, type), member as PropertyInfo) : Expression.Field(Expression.Convert(parameter, type), member as FieldInfo);
                    var function = Expression.Lambda<Func<object, object>>(Expression.Convert(memberAccess, typeof(object)), parameter);

                    return getter = function.Compile();
                }, (key, value) => getter = value);
            }

            return getter.Invoke(me);

            throw new ArgumentException("Only property and field are supported.");
        }

        /// <summary>
        /// 通过属性名获取对象的属性值。
        /// </summary>
        /// <param name="me">目标对象。</param>
        /// <param name="propName">属性名。</param>
        /// <returns>属性值。</returns>
        public static object Get(this object me, string propName)
        {
            var prop = me?.GetType().GetProperty(propName);
            if (prop == null) return null;

            return me.Get(prop);
        }

        /// <summary>
        /// 通过Lambda表达式设置对象的属性值。
        /// </summary>
        /// <param name="me">目标对象。</param>
        /// <param name="accessPath">属性访问表达式。</param>
        /// <param name="value">要设置的值。</param>
        /// <returns>目标对象。</returns>
        public static object Set(this object me, LambdaExpression accessPath, object value)
        {
            var current = me;
            var members = accessPath.Members();
            if (members == null || members.Any() == false) throw new ArgumentException("LambdaExpression should only contain member access expression.");
            for (var i = 0; i < members.Length - 1 && me != null; i++)
            {
                current = current.Get(members[i]);
            }
            if (current != null)
            {
                var memberToSet = members.Last();
                current.Set(memberToSet, value);
            }

            return me;
        }

        /// <summary>
        /// 通过成员信息设置对象的属性或字段值。
        /// </summary>
        /// <param name="me">目标对象。</param>
        /// <param name="member">成员信息。</param>
        /// <param name="value">要设置的值。</param>
        /// <returns>设置的值。</returns>
        public static object Set(this object me, MemberInfo member, object value)
        {
            if (member is PropertyInfo property)
                property.SetValue(me, value);
            else if (member is FieldInfo field)
                field.SetValue(me, value);
            else
                throw new ArgumentException("Only property and field are supported.");

            return value;
        }

        /// <summary>
        /// 通过属性名设置对象的属性值。
        /// </summary>
        /// <param name="me">目标对象。</param>
        /// <param name="name">属性名。</param>
        /// <param name="value">要设置的值。</param>
        /// <returns>设置的值。</returns>
        public static object Set(this object me, string name, object value)
        {
            var pi = me?.GetType().GetProperty(name);
            pi?.SetValue(me, value);

            return me;
        }

        /// <summary>
        /// 将对象的属性转换为字典。
        /// </summary>
        /// <param name="me">目标对象。</param>
        /// <returns>属性名和值的字典。</returns>
        public static IDictionary<string, object> ToDictionary(this object me)
        {
            if (me == null) return null;
            if (me is IDictionary<string, object> dict) return dict;

            var props = me.GetType().GetProperties().Where(prop => prop.CanRead).ToArray();
            var values = props.ToDictionary(prop => prop.Name, me.Get, StringComparer.OrdinalIgnoreCase);

            return values;
        }

        /// <summary>
        /// 获取Lambda表达式中属性访问路径的字符串表示。
        /// </summary>
        /// <param name="me">Lambda表达式。</param>
        /// <returns>属性访问路径，如"User.Address.City"。</returns>
        public static string Path(this LambdaExpression me)
        {
            var props = me.Members();
            if (props == null || props.Length == 0) throw new ArgumentException("Expression does not contain any member access.");

            return string.Join(".", props.Select(el => el.Name));
        }

        /// <summary>
        /// 获取Lambda表达式中所有成员访问的成员信息数组。
        /// </summary>
        /// <param name="me">Lambda表达式。</param>
        /// <returns>成员信息数组。</returns>
        public static MemberInfo[] Members(this LambdaExpression me)
        {
            var members = new Stack<MemberInfo>();
            var root = me.Body;
            while (root is MemberExpression memberExpression)
            {
                members.Push(memberExpression.Member);
                root = memberExpression.Expression;
            }

            return members.ToArray();
        }

        /// <summary>
        /// 获取Lambda表达式中属性访问路径的属性信息数组。
        /// </summary>
        /// <param name="me">Lambda表达式。</param>
        /// <returns>属性信息数组。</returns>
        public static PropertyInfo[] Properties(this LambdaExpression me)
        {
            var path = new Stack<PropertyInfo>();
            var memberAccess = me.Body as MemberExpression ?? (me.Body as UnaryExpression)?.Operand as MemberExpression;
            if (memberAccess == null) return null;

            path.Push(memberAccess.Member as PropertyInfo);
            while (memberAccess.Expression is MemberExpression)
            {
                memberAccess = (MemberExpression)memberAccess.Expression;
                path.Push(memberAccess.Member as PropertyInfo);
            }

            return path.ToArray();
        }

        /// <summary>
        /// 判断类型是否为可空类型。
        /// </summary>
        /// <param name="me">要判断的类型。</param>
        /// <returns>如果是可空类型则返回true，否则返回false。</returns>
        public static bool IsNullableType(this Type me)
        {
            return me.IsGenericType
                && me.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }
    }
}
