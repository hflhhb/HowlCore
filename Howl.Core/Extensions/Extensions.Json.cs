using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Howl.Core.Extensions
{
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
