using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Howl.Core.Extensions
{
    /// <summary>
    /// 提供JSON序列化和反序列化相关的扩展方法。
    /// </summary>
    public static partial class JsonExtensions
    {
        private static readonly JsonSerializerSettings _settings;
        private static readonly JsonSerializer _serializer;
        static JsonExtensions()
        {
            _settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
            };
            _settings.Converters.Add(new StringEnumConverter());
            //
            _serializer = JsonSerializer.Create(_settings);
        }

        /// <summary>
        /// 尝试将JSON字符串反序列化为指定类型的对象。
        /// </summary>
        /// <typeparam name="T">目标类型。</typeparam>
        /// <param name="data">要反序列化的JSON字符串。</param>
        /// <returns>反序列化后的对象，如果反序列化失败则返回类型的默认值。</returns>
        public static T TryDeserialize<T>(this string data)
        {
            try
            {
                using (var reader = new JsonTextReader(new StringReader(data)))
                {
                    return _serializer.Deserialize<T>(reader);
                }
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// 尝试将对象序列化为JSON字符串。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="value">要序列化的对象。</param>
        /// <returns>序列化后的JSON字符串，如果序列化失败则返回空字符串。</returns>
        public static string TrySerialize<T>(this T value)
        {
            try
            {
                //var ms = new MemoryStream();
                //using (var writer = new StreamWriter(ms))
                //{
                //    using (var jsonWriter = new JsonTextWriter(writer))
                //    {
                //        _serializer.Serialize(jsonWriter, value);
                //    }
                //}
                ////
                //using (var reader = new StreamReader(ms))
                //{
                //    ms.Seek(0, SeekOrigin.Begin);
                //    return reader.ReadToEnd();
                //}

                return JsonConvert.SerializeObject(value, _settings);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
