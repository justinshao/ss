using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using System.Xml;
using System.Xml.Linq;
using System.Diagnostics;
using System.Dynamic;
using System.Collections;
using System.Reflection;
namespace System.Runtime.CompilerServices
{
    public class ExtensionAttribute : Attribute { }
}
namespace Common.Utilities.Helpers
{
    public class JsonHelper : DynamicObject
    {
        /// <summary>
        /// 对象转换为Json字符串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetJsonString(object data)
        {
            return JsonConvert.SerializeObject(data);
        }

        /// <summary>
        /// Json字符串转换为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static T GetJson<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
        private enum JsonType
        {
            String,
            Number,
            Boolean,
            Object,
            Array,
            Null
        }

        /// <summary>
        /// JsonString转换为Dynamic
        /// 默认编码格式UTF-8
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static dynamic Parse(string json)
        {
            return Parse(json, Encoding.UTF8);
        }

        /// <summary>
        /// 序列化Dictionary 返回 {"Key":"key","Value":"value"}。value值 可为多维，对象，或对象数组
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static string GetJsonDictionary<TKey, TValue>(Dictionary<TKey, TValue> dic)
        {
            var sb = new StringBuilder();
            var type = GetJsonType(typeof(TValue));
            if (type != JsonType.Number && type != JsonType.String)
            {
                foreach (var d in dic)
                {
                    sb.Append("{\"Key\":\"" + d.Key + "\",\"Value\":" + GetJsonString(d.Value) + "},");
                }
            }
            else
            {
                foreach (var d in dic)
                {
                    sb.Append("{\"Key\":\"" + d.Key + "\",\"Value\":\"" + d.Value + "\"},");
                }
            }
            return string.Format("[{0}]", sb.ToString().TrimEnd(','));
        }

        /// <summary>
        /// JsonString转换为Dynamic
        /// </summary>
        /// <param name="json"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static dynamic Parse(string json, Encoding encoding)
        {
            using (var reader = JsonReaderWriterFactory.CreateJsonReader(encoding.GetBytes(json), XmlDictionaryReaderQuotas.Max))
            {
                return ToValue(XElement.Load(reader));
            }
        }

        /// <summary>from JsonSringStream to DynamicJson</summary>
        public static dynamic Parse(Stream stream)
        {
            using (var reader = JsonReaderWriterFactory.CreateJsonReader(stream, XmlDictionaryReaderQuotas.Max))
            {
                return ToValue(XElement.Load(reader));
            }
        }

        /// <summary>from JsonSringStream to DynamicJson</summary>
        public static dynamic Parse(Stream stream, Encoding encoding)
        {
            using (var reader = JsonReaderWriterFactory.CreateJsonReader(stream, encoding, XmlDictionaryReaderQuotas.Max, _ => { }))
            {
                return ToValue(XElement.Load(reader));
            }
        }

        /// <summary>create JsonSring from primitive or IEnumerable or Object({public property name:property value})</summary>
        public static string Serialize(object obj)
        {
            return CreateJsonString(new XStreamingElement("root", CreateTypeAttr(GetJsonType(obj)), CreateJsonNode(obj)));
        }

        // private static methods

        private static dynamic ToValue(XElement element)
        {
            var type = (JsonType)Enum.Parse(typeof(JsonType), element.Attribute("type").Value, true);
            switch (type)
            {
                case JsonType.Boolean:
                    return (bool)element;
                case JsonType.Number:
                    return (double)element;
                case JsonType.String:
                    return (string)element;
                case JsonType.Object:
                case JsonType.Array:
                    return new JsonHelper(element, type);
                case JsonType.Null:
                    return null;
                default:
                    return null;
            }
        }

        private static JsonType GetJsonType(object obj)
        {
            if (obj == null) return JsonType.Null;

            switch (Type.GetTypeCode(obj.GetType()))
            {
                case TypeCode.Boolean:
                    return JsonType.Boolean;
                case TypeCode.String:
                case TypeCode.Char:
                case TypeCode.DateTime:
                    return JsonType.String;
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                case TypeCode.SByte:
                case TypeCode.Byte:
                    return JsonType.Number;
                case TypeCode.Object:
                    return (obj is IEnumerable) ? JsonType.Array : JsonType.Object;
                case TypeCode.DBNull:
                case TypeCode.Empty:
                    return JsonType.Null;
                default:
                    return JsonType.Null;
            }
        }

        private static XAttribute CreateTypeAttr(JsonType type)
        {
            return new XAttribute("type", type.ToString());
        }

        private static object CreateJsonNode(object obj)
        {
            var type = GetJsonType(obj);
            switch (type)
            {
                case JsonType.String:
                case JsonType.Number:
                    return obj;
                case JsonType.Boolean:
                    return obj.ToString().ToLower();
                case JsonType.Object:
                    return CreateXObject(obj);
                case JsonType.Array:
                    return CreateXArray(obj as IEnumerable);
                case JsonType.Null:
                    return null;
                default:
                    return null;
            }
        }

        private static IEnumerable<XStreamingElement> CreateXArray<T>(T obj) where T : IEnumerable
        {
            return obj.Cast<object>()
                .Select(o => new XStreamingElement("item", CreateTypeAttr(GetJsonType(o)), CreateJsonNode(o)));
        }

        private static IEnumerable<XStreamingElement> CreateXObject(object obj)
        {
            return obj.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(pi => new { Name = pi.Name, Value = pi.GetValue(obj, null) })
                .Select(a => new XStreamingElement(a.Name, CreateTypeAttr(GetJsonType(a.Value)), CreateJsonNode(a.Value)));
        }

        private static string CreateJsonString(XStreamingElement element)
        {
            using (var ms = new MemoryStream())
            {
                using (var writer = JsonReaderWriterFactory.CreateJsonWriter(ms, Encoding.Unicode))
                {
                    element.WriteTo(writer);
                    return Encoding.Unicode.GetString(ms.ToArray());
                }
            }
        }

        // dynamic structure represents JavaScript Object/Array

        readonly XElement _xml;
        readonly JsonType _jsonType;

        /// <summary>create blank JSObject</summary>
        public JsonHelper()
        {
            _xml = new XElement("root", CreateTypeAttr(JsonType.Object));
            _jsonType = JsonType.Object;
        }

        private JsonHelper(XElement element, JsonType type)
        {
            Debug.Assert(type == JsonType.Array || type == JsonType.Object);

            _xml = element;
            _jsonType = type;
        }

        public bool IsObject { get { return _jsonType == JsonType.Object; } }

        public bool IsArray { get { return _jsonType == JsonType.Array; } }

        /// <summary>has property or not</summary>
        public bool IsDefined(string name)
        {
            return IsObject && (_xml.Element(name) != null);
        }

        /// <summary>has property or not</summary>
        public bool IsDefined(int index)
        {
            return IsArray && (_xml.Elements().ElementAtOrDefault(index) != null);
        }

        /// <summary>delete property</summary>
        public bool Delete(string name)
        {
            var elem = _xml.Element(name);
            if (elem != null)
            {
                elem.Remove();
                return true;
            }
            return false;
        }

        /// <summary>delete property</summary>
        public bool Delete(int index)
        {
            var elem = _xml.Elements().ElementAtOrDefault(index);
            if (elem != null)
            {
                elem.Remove();
                return true;
            }
            return false;
        }

        public T Deserialize<T>()
        {
            return (T)Deserialize(typeof(T));
        }

        private object Deserialize(Type type)
        {
            return (IsArray) ? DeserializeArray(type) : DeserializeObject(type);
        }

        private dynamic DeserializeValue(XElement element, Type elementType)
        {
            var value = ToValue(element);
            if (value is JsonHelper)
            {
                value = ((JsonHelper)value).Deserialize(elementType);
            }
            return Convert.ChangeType(value, elementType);
        }

        private object DeserializeObject(Type targetType)
        {
            var result = Activator.CreateInstance(targetType);
            var dict = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite)
                .ToDictionary(pi => pi.Name, pi => pi);
            foreach (var item in _xml.Elements())
            {
                PropertyInfo propertyInfo;
                if (!dict.TryGetValue(item.Name.LocalName, out propertyInfo)) continue;
                var value = DeserializeValue(item, propertyInfo.PropertyType);
                propertyInfo.SetValue(result, value, null);
            }
            return result;
        }

        private object DeserializeArray(Type targetType)
        {
            if (targetType.IsArray) // Foo[]
            {
                var elemType = targetType.GetElementType();
                dynamic array = Array.CreateInstance(elemType, _xml.Elements().Count());
                var index = 0;
                foreach (var item in _xml.Elements())
                {
                    array[index++] = DeserializeValue(item, elemType);
                }
                return array;
            }
            else // List<Foo>
            {
                var elemType = targetType.GetGenericArguments()[0];
                dynamic list = Activator.CreateInstance(targetType);
                foreach (var item in _xml.Elements())
                {
                    list.Add(DeserializeValue(item, elemType));
                }
                return list;
            }
        }

        // Delete
        public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
        {
            result = (IsArray)
                ? Delete((int)args[0])
                : Delete((string)args[0]);
            return true;
        }

        // IsDefined, if has args then TryGetMember
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            if (args.Length > 0)
            {
                result = null;
                return false;
            }

            result = IsDefined(binder.Name);
            return true;
        }

        // Deserialize or foreach(IEnumerable)
        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            if (binder.Type == typeof(IEnumerable) || binder.Type == typeof(object[]))
            {
                var ie = (IsArray)
                    ? _xml.Elements().Select(x => ToValue(x))
                    : _xml.Elements().Select(x => (dynamic)new KeyValuePair<string, object>(x.Name.LocalName, ToValue(x)));
                result = (binder.Type == typeof(object[])) ? ie.ToArray() : ie;
            }
            else
            {
                result = Deserialize(binder.Type);
            }
            return true;
        }

        private bool TryGet(XElement element, out object result)
        {
            if (element == null)
            {
                result = null;
                return false;
            }

            result = ToValue(element);
            return true;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            return (IsArray)
                ? TryGet(_xml.Elements().ElementAtOrDefault((int)indexes[0]), out result)
                : TryGet(_xml.Element((string)indexes[0]), out result);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return (IsArray)
                ? TryGet(_xml.Elements().ElementAtOrDefault(int.Parse(binder.Name)), out result)
                : TryGet(_xml.Element(binder.Name), out result);
        }

        private bool TrySet(string name, object value)
        {
            var type = GetJsonType(value);
            var element = _xml.Element(name);
            if (element == null)
            {
                _xml.Add(new XElement(name, CreateTypeAttr(type), CreateJsonNode(value)));
            }
            else
            {
                element.Attribute("type").Value = type.ToString();
                element.ReplaceNodes(CreateJsonNode(value));
            }

            return true;
        }

        private bool TrySet(int index, object value)
        {
            var type = GetJsonType(value);
            var e = _xml.Elements().ElementAtOrDefault(index);
            if (e == null)
            {
                _xml.Add(new XElement("item", CreateTypeAttr(type), CreateJsonNode(value)));
            }
            else
            {
                e.Attribute("type").Value = type.ToString();
                e.ReplaceNodes(CreateJsonNode(value));
            }

            return true;
        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            return (IsArray)
                ? TrySet((int)indexes[0], value)
                : TrySet((string)indexes[0], value);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            return (IsArray)
                ? TrySet(int.Parse(binder.Name), value)
                : TrySet(binder.Name, value);
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return (IsArray)
                ? _xml.Elements().Select((x, i) => i.ToString())
                : _xml.Elements().Select(x => x.Name.LocalName);
        }

        /// <summary>
        /// 序列化为Json字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            foreach (var elem in _xml.Descendants().Where(x => x.Attribute("type").Value == "null"))
            {
                elem.RemoveNodes();
            }
            return CreateJsonString(new XStreamingElement("root", CreateTypeAttr(_jsonType), _xml.Elements()));
        }
    }
}
