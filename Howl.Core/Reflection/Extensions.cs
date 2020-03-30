using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Howl.Core.Reflection
{
    public static partial class Extensions
    {
        public static ConcurrentDictionary<MemberInfo, Func<object, object>> Getters { get; } = new ConcurrentDictionary<MemberInfo, Func<object, object>>();

        public static T Get<T>(this object me, LambdaExpression expr)
        {
            return (T)Get(me, expr);
        }
        public static T Get<T>(this object me, MemberInfo member)
        {
            return (T)Get(me, member);
        }
        public static T Get<T>(this object me, string propName)
        {
            return (T)me.Get(propName);
        }
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
        public static object Get(this object me, string propName)
        {
            var prop = me?.GetType().GetProperty(propName);
            if (prop == null) return null;

            return me.Get(prop);
        }
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
        public static object Set(this object me, string name, object value)
        {
            var pi = me?.GetType().GetProperty(name);
            pi?.SetValue(me, value);

            return me;
        }

        public static IDictionary<string, object> ToDictionary(this object me)
        {
            if (me == null) return null;
            if (me is IDictionary<string, object> dict) return dict;

            var props = me.GetType().GetProperties().Where(prop => prop.CanRead).ToArray();
            var values = props.ToDictionary(prop => prop.Name, me.Get, StringComparer.OrdinalIgnoreCase);

            return values;
        }
        public static string Path(this LambdaExpression me)
        {
            var props = me.Members();
            if (props == null) throw new ArgumentException("Expression does not contain any member access.");

            return string.Join(".", props.Select(el => el.Name));
        }
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

        public static bool IsNullableType(this Type me)
        {
            return me.IsGenericType
                && me.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }
    }
}
