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
        /// <para>适用于所有实现了 <see cref="IComparable{T}"/> 接口的值类型，如 int、long、decimal、DateTime 等。</para>
        /// </summary>
        /// <typeparam name="T">可比较的值类型，必须实现 <see cref="IComparable{T}"/> 接口。</typeparam>
        /// <param name="me">要判断的值。</param>
        /// <param name="from">范围下限（包含）。</param>
        /// <param name="to">范围上限（包含）。</param>
        /// <returns>如果值在范围内（from ≤ me ≤ to）则返回 true，否则返回 false。</returns>
        /// <example>
        /// <code>
        /// int age = 25;
        /// bool isValid = age.Between(18, 60); // 返回 true
        ///
        /// decimal price = 99.5m;
        /// bool inRange = price.Between(50m, 100m); // 返回 true
        /// </code>
        /// </example>
        public static bool Between<T>(this T me, T from, T to) where T : struct, IComparable<T>
        {
            return me.CompareTo(from) >= 0 && me.CompareTo(to) <= 0;
        }

        /// <summary>
        /// 判断当前可空值是否在指定范围内（包含边界）。
        /// <para>当边界值为 null 时，默认认为该边界条件满足。例如：from 为 null 时，认为下限条件满足。</para>
        /// </summary>
        /// <typeparam name="T">可比较的值类型，必须实现 <see cref="IComparable{T}"/> 接口。</typeparam>
        /// <param name="me">要判断的可空值。如果为 null，直接返回 false。</param>
        /// <param name="from">范围下限（可为空），为空时视为无下限限制。</param>
        /// <param name="to">范围上限（可为空），为空时视为无上限限制。</param>
        /// <returns>如果值在范围内则返回 true，否则返回 false。当 <paramref name="me"/> 为 null 时返回 false。</returns>
        /// <example>
        /// <code>
        /// int? age = 25;
        /// bool isValid = age.Between(18, null); // 无上限限制，返回 true
        /// bool isValid2 = age.Between(null, 60); // 无下限限制，返回 true
        /// </code>
        /// </example>
        public static bool Between<T>(this T? me, T? from, T? to)
            where T : struct, IComparable<T>
        {
            return Between(me, from, to, true, true);
        }

        /// <summary>
        /// 判断当前可空值是否在指定范围内，可自定义边界值为空时的行为。
        /// <para>此方法提供了更灵活的控制，可以指定当边界值为 null 时是否视为满足该边界条件。</para>
        /// </summary>
        /// <typeparam name="T">可比较的值类型，必须实现 <see cref="IComparable{T}"/> 接口。</typeparam>
        /// <param name="me">要判断的可空值。如果为 null，直接返回 false。</param>
        /// <param name="from">范围下限（可为空）。</param>
        /// <param name="to">范围上限（可为空）。</param>
        /// <param name="blfromIfnull">
        /// 当 <paramref name="from"/> 为 null 时是否满足下限判断条件。
        /// <para>true：视为满足下限条件（即无下限限制）；false：视为不满足下限条件。</para>
        /// </param>
        /// <param name="blToIfnull">
        /// 当 <paramref name="to"/> 为 null 时是否满足上限判断条件。
        /// <para>true：视为满足上限条件（即无上限限制）；false：视为不满足上限条件。</para>
        /// </param>
        /// <returns>如果值在范围内则返回 true，否则返回 false。</returns>
        /// <example>
        /// <code>
        /// int? value = 50;
        /// // 下限为空时不满足条件，相当于必须指定下限
        /// bool result = value.Between(null, 100, false, true); // 返回 false
        /// </code>
        /// </example>
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
        /// <para>适用于所有实现了 <see cref="IComparable{T}"/> 接口的类型。</para>
        /// </summary>
        /// <typeparam name="T">可比较的类型，必须实现 <see cref="IComparable{T}"/> 接口。</typeparam>
        /// <param name="me">当前值。</param>
        /// <param name="other">另一个值。</param>
        /// <returns>两个值中较大的那个值。</returns>
        /// <example>
        /// <code>
        /// int max = 5.Max(10); // 返回 10
        /// decimal price = 99.9m.Max(100m); // 返回 100m
        /// </code>
        /// </example>
        public static T Max<T>(this T me, T other) where T : IComparable<T>
        {
            return me.CompareTo(other) >= 0 ? me : other;
        }

        /// <summary>
        /// 返回当前值和另一个值中的较小值。
        /// <para>适用于所有实现了 <see cref="IComparable{T}"/> 接口的类型。</para>
        /// </summary>
        /// <typeparam name="T">可比较的类型，必须实现 <see cref="IComparable{T}"/> 接口。</typeparam>
        /// <param name="me">当前值。</param>
        /// <param name="other">另一个值。</param>
        /// <returns>两个值中较小的那个值。</returns>
        /// <example>
        /// <code>
        /// int min = 5.Min(10); // 返回 5
        /// decimal price = 99.9m.Min(100m); // 返回 99.9m
        /// </code>
        /// </example>
        public static T Min<T>(this T me, T other) where T : IComparable<T>
        {
            return me.CompareTo(other) <= 0 ? me : other;
        }

        /// <summary>
        /// 返回两个可空值中的较大值。
        /// <para>比较规则：</para>
        /// <list type="bullet">
        /// <item><description>如果两个值都不为空，返回较大的值。</description></item>
        /// <item><description>如果其中一个为空，返回非空的那个值。</description></item>
        /// <item><description>如果两个都为空，返回 null。</description></item>
        /// </list>
        /// </summary>
        /// <typeparam name="T">可比较的值类型，必须实现 <see cref="IComparable{T}"/> 接口。</typeparam>
        /// <param name="me">当前可空值。</param>
        /// <param name="other">另一个可空值。</param>
        /// <returns>较大的可空值。</returns>
        /// <example>
        /// <code>
        /// int? a = 10;
        /// int? b = null;
        /// int? max = a.Max(b); // 返回 10
        ///
        /// int? c = null;
        /// int? d = null;
        /// int? max2 = c.Max(d); // 返回 null
        /// </code>
        /// </example>
        public static T? Max<T>(this T? me, T? other) where T : struct, IComparable<T>
        {
            if (me.HasValue && other.HasValue)
            {
                return me.Value.CompareTo(other.Value) >= 0 ? me : other;
            }
            return me ?? other;
        }

        /// <summary>
        /// 返回两个可空值中的较小值。
        /// <para>比较规则：</para>
        /// <list type="bullet">
        /// <item><description>如果两个值都不为空，返回较小的值。</description></item>
        /// <item><description>如果其中一个为空，返回非空的那个值。</description></item>
        /// <item><description>如果两个都为空，返回 null。</description></item>
        /// </list>
        /// </summary>
        /// <typeparam name="T">可比较的值类型，必须实现 <see cref="IComparable{T}"/> 接口。</typeparam>
        /// <param name="me">当前可空值。</param>
        /// <param name="other">另一个可空值。</param>
        /// <returns>较小的可空值。</returns>
        /// <example>
        /// <code>
        /// int? a = 10;
        /// int? b = 20;
        /// int? min = a.Min(b); // 返回 10
        /// </code>
        /// </example>
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
        /// <para>这是空合并运算符 ?? 的扩展方法形式，提供更清晰的语义。</para>
        /// </summary>
        /// <typeparam name="T">值类型。</typeparam>
        /// <param name="me">可空值。</param>
        /// <param name="defval">默认值，当可空值为 null 时返回此值。</param>
        /// <returns>如果可空值有值则返回该值，否则返回默认值。</returns>
        /// <example>
        /// <code>
        /// int? count = null;
        /// int actual = count.Value(0); // 返回 0
        ///
        /// int? age = 25;
        /// int actualAge = age.Value(18); // 返回 25
        /// </code>
        /// </example>
        public static T Value<T>(this T? me, T defval = default(T)) where T : struct
        {
            return me ?? defval;
        }

        /// <summary>
        /// 截取指定小数位数，不进行四舍五入。
        /// <para>
        /// 通过精度的正负数控制小数位的舍入方向：
        /// </para>
        /// <list type="bullet">
        /// <item><description>精度为正数：向零方向舍入（截断）。例如 9.969 截取2位得 9.96。</description></item>
        /// <item><description>精度为负数：远离零方向舍入。例如 9.969 截取2位得 9.97。</description></item>
        /// <item><description>精度为0：直接返回整数位。</description></item>
        /// </list>
        /// </summary>
        /// <param name="d">要处理的 decimal 值。</param>
        /// <param name="s">精度（小数位数），正数向零舍入，负数远离零舍入，0返回整数位。</param>
        /// <returns>截取后的 decimal 值。</returns>
        /// <example>
        /// <code>
        /// decimal value = 9.969m;
        /// decimal result1 = value.ToFixed(2); // 返回 9.96（向零舍入）
        /// decimal result2 = value.ToFixed(-2); // 返回 9.97（远离零舍入）
        /// decimal result3 = value.ToFixed(0); // 返回 9（整数位）
        /// </code>
        /// </example>
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
        /// 截取可空 decimal 的指定小数位数。
        /// <para>如果值为 null，默认当作 0 处理。</para>
        /// </summary>
        /// <param name="d">要处理的可空 decimal 值。</param>
        /// <param name="s">精度（小数位数），正数向零舍入，负数远离零舍入，0返回整数位。</param>
        /// <returns>截取后的 decimal 值。如果输入为 null，返回 0。</returns>
        /// <example>
        /// <code>
        /// decimal? value = null;
        /// decimal result = value.ToFixed(2); // 返回 0
        ///
        /// decimal? value2 = 9.969m;
        /// decimal result2 = value2.ToFixed(2); // 返回 9.96
        /// </code>
        /// </example>
        public static decimal ToFixed(this decimal? d, int s)
        {
            decimal sp = d ?? 0;
            return ToFixed(sp, s);
        }

        /// <summary>
        /// 截取指定小数位数并格式化为字符串。
        /// <para>先调用 <see cref="ToFixed(decimal, int)"/> 截取小数位，再根据格式化器输出字符串。</para>
        /// </summary>
        /// <param name="d">要处理的 decimal 值。</param>
        /// <param name="s">精度（小数位数），默认为2。</param>
        /// <param name="formater">
        /// 格式化器类型：
        /// <list type="bullet">
        /// <item><description>"f"（默认）：固定小数位格式。如 1 => "1.00"，1.1 => "1.10"。</description></item>
        /// <item><description>"#"：可选小数位格式。如 1 => "1"，1.1 => "1.1"，1.123 => "1.12"。</description></item>
        /// </list>
        /// </param>
        /// <returns>格式化后的数字字符串。</returns>
        /// <example>
        /// <code>
        /// decimal value = 1234567.89m;
        /// string result1 = value.ToFixedAsString(2, "f"); // 返回 "1234567.89"
        /// string result2 = value.ToFixedAsString(2, "#"); // 返回 "1234567.89"
        ///
        /// decimal value2 = 1m;
        /// string result3 = value2.ToFixedAsString(2, "f"); // 返回 "1.00"
        /// string result4 = value2.ToFixedAsString(2, "#"); // 返回 "1"
        /// </code>
        /// </example>
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
        /// 截取可空 decimal 的指定小数位数并格式化为字符串。
        /// <para>如果值为 null，使用指定的默认值进行处理。</para>
        /// </summary>
        /// <param name="d">要处理的可空 decimal 值。</param>
        /// <param name="s">精度（小数位数），默认为2。</param>
        /// <param name="defval">当值为 null 时使用的默认值，默认为 0。</param>
        /// <param name="formater">
        /// 格式化器类型：
        /// <list type="bullet">
        /// <item><description>"f"（默认）：固定小数位格式。</description></item>
        /// <item><description>"#"：可选小数位格式。</description></item>
        /// </list>
        /// </param>
        /// <returns>格式化后的数字字符串。</returns>
        /// <example>
        /// <code>
        /// decimal? value = null;
        /// string result = value.ToFixedAsString(2, 0m, "f"); // 返回 "0.00"
        ///
        /// decimal? value2 = 9.969m;
        /// string result2 = value2.ToFixedAsString(2, 0m, "f"); // 返回 "9.96"
        /// </code>
        /// </example>
        public static string ToFixedAsString(this decimal? d, int s = 2, decimal defval = 0M, string formater = "f")
        {
            decimal sp = d ?? defval;
            return ToFixedAsString(sp, s, formater);
        }

        /// <summary>
        /// 截取可空 decimal 的指定小数位数并格式化为字符串。
        /// <para>如果值为 null，直接返回指定的替代字符串，不进行格式化。</para>
        /// </summary>
        /// <param name="d">要处理的可空 decimal 值。</param>
        /// <param name="valstr">当值为 null 时返回的替代字符串。</param>
        /// <param name="s">精度（小数位数），默认为2。</param>
        /// <param name="formater">
        /// 格式化器类型：
        /// <list type="bullet">
        /// <item><description>"f"（默认）：固定小数位格式。</description></item>
        /// <item><description>"#"：可选小数位格式。</description></item>
        /// </list>
        /// </param>
        /// <returns>如果值不为 null，返回格式化后的数字字符串；否则返回 <paramref name="valstr"/>。</returns>
        /// <example>
        /// <code>
        /// decimal? value = null;
        /// string result = value.ToFixedAsString("N/A", 2); // 返回 "N/A"
        ///
        /// decimal? value2 = 9.969m;
        /// string result2 = value2.ToFixedAsString("N/A", 2); // 返回 "9.96"
        /// </code>
        /// </example>
        public static string ToFixedAsString(this decimal? d, string valstr, int s = 2, string formater = "f")
        {
            if (!d.HasValue) return valstr;
            return ToFixedAsString(d.Value, s, formater);
        }

        /// <summary>
        /// 截取指定小数位数并格式化为带千分位的字符串。
        /// <para>先调用 <see cref="ToFixed(decimal, int)"/> 截取小数位，再添加千分位分隔符输出字符串。</para>
        /// </summary>
        /// <param name="d">要处理的 decimal 值。</param>
        /// <param name="s">精度（小数位数），默认为2。</param>
        /// <param name="formater">
        /// 格式化器类型：
        /// <list type="bullet">
        /// <item><description>"n"（默认）：带千分位的固定小数位格式。如 123456.789 => "123,456.78"。</description></item>
        /// <item><description>"#"：带千分位的可选小数位格式。如 123456 => "123,456"，123456.7 => "123,456.7"。</description></item>
        /// </list>
        /// </param>
        /// <returns>带千分位的格式化数字字符串。</returns>
        /// <example>
        /// <code>
        /// decimal value = 123456.789m;
        /// string result1 = value.ToFixedFormat(2, "n"); // 返回 "123,456.78"
        /// string result2 = value.ToFixedFormat(2, "#"); // 返回 "123,456.78"
        ///
        /// decimal value2 = 123456m;
        /// string result3 = value2.ToFixedFormat(2, "n"); // 返回 "123,456.00"
        /// string result4 = value2.ToFixedFormat(2, "#"); // 返回 "123,456"
        /// </code>
        /// </example>
        public static string ToFixedFormat(this decimal d, int s = 2, string formater = "n")
        {
            string formatStr = "n" + s;
            if(formater == "#")
            {
                formatStr = "#,##0" + Enumerable.Range(0, Math.Abs(s)).Aggregate(".", (x, y) => x + "#");
            }
            return d.ToFixed(s).ToString(formatStr);
        }

        /// <summary>
        /// 截取可空 decimal 的指定小数位数并格式化为带千分位的字符串。
        /// <para>如果值为 null，使用指定的默认值进行处理。</para>
        /// </summary>
        /// <param name="d">要处理的可空 decimal 值。</param>
        /// <param name="s">精度（小数位数），默认为2。</param>
        /// <param name="defval">当值为 null 时使用的默认值，默认为 0。</param>
        /// <param name="formater">
        /// 格式化器类型：
        /// <list type="bullet">
        /// <item><description>"n"（默认）：带千分位的固定小数位格式。</description></item>
        /// <item><description>"#"：带千分位的可选小数位格式。</description></item>
        /// </list>
        /// </param>
        /// <returns>带千分位的格式化数字字符串。</returns>
        /// <example>
        /// <code>
        /// decimal? value = null;
        /// string result = value.ToFixedFormat(2, 0m, "n"); // 返回 "0.00"
        ///
        /// decimal? value2 = 123456.789m;
        /// string result2 = value2.ToFixedFormat(2, 0m, "n"); // 返回 "123,456.78"
        /// </code>
        /// </example>
        public static string ToFixedFormat(this decimal? d, int s = 2, decimal defval = 0M, string formater = "n")
        {
            decimal sp = d ?? defval;
            return ToFixedFormat(sp, s, formater);
        }

        /// <summary>
        /// 截取可空 decimal 的指定小数位数并格式化为带千分位的字符串。
        /// <para>如果值为 null，直接返回指定的替代字符串，不进行格式化。</para>
        /// </summary>
        /// <param name="d">要处理的可空 decimal 值。</param>
        /// <param name="valstr">当值为 null 时返回的替代字符串。</param>
        /// <param name="s">精度（小数位数），默认为2。</param>
        /// <param name="formater">
        /// 格式化器类型：
        /// <list type="bullet">
        /// <item><description>"n"（默认）：带千分位的固定小数位格式。</description></item>
        /// <item><description>"#"：带千分位的可选小数位格式。</description></item>
        /// </list>
        /// </param>
        /// <returns>如果值不为 null，返回带千分位的格式化数字字符串；否则返回 <paramref name="valstr"/>。</returns>
        /// <example>
        /// <code>
        /// decimal? value = null;
        /// string result = value.ToFixedFormat("--", 2); // 返回 "--"
        ///
        /// decimal? value2 = 123456.789m;
        /// string result2 = value2.ToFixedFormat("--", 2); // 返回 "123,456.78"
        /// </code>
        /// </example>
        public static string ToFixedFormat(this decimal? d, string valstr, int s = 2, string formater = "n")
        {
            if (!d.HasValue) return valstr;
            return ToFixedFormat(d.Value, s, formater);
        }

    }
}
