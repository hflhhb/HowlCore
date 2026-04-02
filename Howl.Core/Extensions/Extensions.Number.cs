using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Howl.Core.Extensions
{
    public static partial class Extensions
    {
        /// <summary>
        /// 判断当前值是否在指定范围内（包含边界）。
        /// </summary>
        /// <typeparam name="T">可比较的值类型。</typeparam>
        /// <param name="me">要判断的值。</param>
        /// <param name="from">范围下限。</param>
        /// <param name="to">范围上限。</param>
        /// <returns>如果值在范围内则返回true，否则返回false。</returns>
        public static bool Between<T>(this T me, T from, T to) where T : struct, IComparable<T>
        {
            return me.CompareTo(from) >= 0 && me.CompareTo(to) <= 0;
        }

        /// <summary>
        /// 判断当前可空值是否在指定范围内（包含边界）。
        /// </summary>
        /// <typeparam name="T">可比较的值类型。</typeparam>
        /// <param name="me">要判断的可空值。</param>
        /// <param name="from">范围下限（可为空）。</param>
        /// <param name="to">范围上限（可为空）。</param>
        /// <returns>如果值在范围内则返回true，否则返回false。</returns>
        public static bool Between<T>(this T? me, T? from, T? to)
            where T : struct, IComparable<T>
        {
            return Between(me, from, to, true, true);
        }
        /// <summary>
        /// 判断当前可空值是否在指定范围内，可控制边界值为空时的行为。
        /// </summary>
        /// <typeparam name="T">可比较的值类型。</typeparam>
        /// <param name="me">要判断的可空值。</param>
        /// <param name="from">范围下限（可为空）。</param>
        /// <param name="to">范围上限（可为空）。</param>
        /// <param name="blfromIfnull">下限值为null时是否满足下限判断条件。</param>
        /// <param name="blToIfnull">上限值为null时是否满足上限判断条件。</param>
        /// <returns>如果值在范围内则返回true，否则返回false。</returns>
        public static bool Between<T>(this T? me, T? from, T? to, bool blfromIfnull, bool blToIfnull)
            where T : struct, IComparable<T>
        {
            if (!me.HasValue) return false;
            var blFrom = blfromIfnull; var blTo = blToIfnull;
            if (from.HasValue) blFrom = me.Value.CompareTo(from.Value) >= 0;
            if (to.HasValue) blTo = me.Value.CompareTo(to.Value) <= 0;

            return blFrom && blTo;
        }

        /// <summary>
        /// 返回当前值和另一个值中的较大值。
        /// </summary>
        /// <typeparam name="T">可比较的类型。</typeparam>
        /// <param name="me">当前值。</param>
        /// <param name="other">另一个值。</param>
        /// <returns>较大的值。</returns>
        public static T Max<T>(this T me, T other) where T : IComparable<T>
        {
            return me.CompareTo(other) >= 0 ? me : other;
        }

        /// <summary>
        /// 返回当前值和另一个值中的较小值。
        /// </summary>
        /// <typeparam name="T">可比较的类型。</typeparam>
        /// <param name="me">当前值。</param>
        /// <param name="other">另一个值。</param>
        /// <returns>较小的值。</returns>
        public static T Min<T>(this T me, T other) where T : IComparable<T>
        {
            return me.CompareTo(other) <= 0 ? me : other;
        }

        /// <summary>
        /// 返回两个可空值中的较大值。如果其中一个为空，返回非空值；如果都为空，返回空。
        /// </summary>
        /// <typeparam name="T">可比较的值类型。</typeparam>
        /// <param name="me">当前可空值。</param>
        /// <param name="other">另一个可空值。</param>
        /// <returns>较大的可空值。</returns>
        public static T? Max<T>(this T? me, T? other) where T : struct, IComparable<T>
        {
            if (me.HasValue && other.HasValue)
            {
                return me.Value.CompareTo(other.Value) >= 0 ? me : other;
            }
            return me ?? other;
        }

        /// <summary>
        /// 返回两个可空值中的较小值。如果其中一个为空，返回非空值；如果都为空，返回空。
        /// </summary>
        /// <typeparam name="T">可比较的值类型。</typeparam>
        /// <param name="me">当前可空值。</param>
        /// <param name="other">另一个可空值。</param>
        /// <returns>较小的可空值。</returns>
        public static T? Min<T>(this T? me, T? other) where T : struct, IComparable<T>
        {
            if (me.HasValue && other.HasValue)
            {
                return me.Value.CompareTo(other.Value) <= 0 ? me : other;
            }
            return me ?? other;
        }

        /// <summary>
        /// 获取可空值的实际值，如果为空则返回指定的默认值。
        /// </summary>
        /// <typeparam name="T">值类型。</typeparam>
        /// <param name="me">可空值。</param>
        /// <param name="defval">默认值。</param>
        /// <returns>实际值或默认值。</returns>
        public static T Value<T>(this T? me, T defval = default(T)) where T : struct
        {
            return me ?? defval;
        }

        /// <summary>
        /// 截取指定小数位(不进行四舍五入)
        /// <para>用精度的正负数控制小数位的直接舍入还是直接进位</para>
        /// <para>精度正数：直接舍入 精度负数：直接进位 0:直接返回整数位</para>
        /// </summary>
        /// <param name="d"></param>
        /// <param name="s">指定小数位</param>
        /// <returns></returns>
        public static decimal ToFixed(this decimal d, int s)
        {
            int precision = Math.Abs(s); //精度
            decimal sp = Convert.ToDecimal(Math.Pow(10, precision));
            var integerBit = Math.Truncate(d);  //整数位 9.969 => 9
            if (s == 0) return integerBit;
            var decimalBit = d - integerBit;    //小数位 9.969 => 0.969
            //decimal fixedDecimal = d;
            decimal extendDecimal = decimalBit * sp;    //小数位扩展 即 0.969 => 96.9
            decimal tempDecimal = 0;
            //
            if (s > 0)
            {
                tempDecimal = d < 0 ? Math.Ceiling(extendDecimal) : Math.Floor(extendDecimal);
            }
            else
            {
                tempDecimal = d > 0 ? Math.Ceiling(extendDecimal) : Math.Floor(extendDecimal);
            }

            return integerBit + tempDecimal / sp;
        }

        /// <summary>
        /// 计算精度 数字为空的时候默认为0
        /// </summary>
        /// <param name="d"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static decimal ToFixed(this decimal? d, int s)
        {
            decimal sp = d ?? 0;
            return ToFixed(sp, s);
        }

        /// <summary>
        /// 计算精度并且格式化成带数字字符串
        /// 1234567.89
        /// </summary>
        /// <param name="d"></param>
        /// <param name="s"></param>
        /// <param name="formater">f:1=>1.00, 1.1=>1.10, #:1=>1, 1.1=>1.1, 1.123=>1.12</param>
        /// <returns></returns>
        public static string ToFixedAsString(this decimal d, int s = 2, string formater = "f")
        {
            string formatStr = "f" + s;
            //string formatStr = "#.00";
            if (formater == "#")
            {
                formatStr = "#0" + Enumerable.Range(0, Math.Abs(s)).Aggregate(".", (x, y) => x + "#");
            }
            return d.ToFixed(s).ToString(formatStr);
        }
        /// <summary>
        /// 计算精度并且格式化成带数字字符串
        ///  1234567.89
        /// </summary>
        /// <param name="d"></param>
        /// <param name="s"></param>
        /// <param name="defval"></param>
        /// <returns></returns>
        public static string ToFixedAsString(this decimal? d, int s = 2, decimal defval = 0M, string formater = "f")
        {
            decimal sp = d ?? defval;
            return ToFixedAsString(sp, s, formater);
        }
        /// <summary>
        /// 计算精度并且格式化成带数字字符串
        /// 如果数值为空，则返回 valstr 否则 格式化d成字符串
        /// </summary>
        /// <param name="d"></param>
        /// <param name="valstr"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToFixedAsString(this decimal? d, string valstr, int s = 2, string formater = "f")
        {
            if (!d.HasValue) return valstr;
            return ToFixedAsString(d.Value, s, formater);
        }

        /// <summary>
        /// 计算精度并且格式化成带千分位的数字字符串
        /// 123456.789=>123,456.78
        /// 123456.789=>123,456.78
        /// </summary>
        /// <param name="d"></param>
        /// <param name="s"></param>
        /// <param name="formater">n:1=>1.00, 1.1=>1.10, #:1=>1, 1.1=>1.1, 1.123=>1.12</param>
        /// <returns></returns>
        public static string ToFixedFormat(this decimal d, int s = 2, string formater = "n")
        {
            string formatStr = "n" + s;
            if(formater == "#")
            {
                formatStr = "#,##0" + Enumerable.Range(0, Math.Abs(s)).Aggregate(".", (x, y) => x + "#");
            }
            return d.ToFixed(s).ToString(formatStr);
        }

        public static string ToFixedFormat(this decimal? d, int s = 2, decimal defval = 0M, string formater = "n")
        {
            decimal sp = d ?? defval;
            return ToFixedFormat(sp, s, formater);
        }

        public static string ToFixedFormat(this decimal? d, string valstr, int s = 2, string formater = "n")
        {
            if (!d.HasValue) return valstr;
            return ToFixedFormat(d.Value, s, formater);
        }

    }
}
