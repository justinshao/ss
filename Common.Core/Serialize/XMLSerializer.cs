using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Runtime.Serialization;
using System.Reflection;
using System.Xml;

namespace Common.Core
{
    /// <summary>
    /// 自定义序列化器，将对象序列化为标准xml，以string形式返回
    /// </summary>
    public abstract class XMLSerializer
    {
        const string ARRAY_MASK = "ArrayOf";

        public static string Serialize<T>(T obj)
        {
            StringBuilder result = new StringBuilder();
            Serialize<T>(obj, result);
            return result.ToString();
        }

        public static void Serialize<T>(T obj, StringBuilder result)
        {
            if (obj == null) return;
            Serialize<T>(obj, result, true);
        }

        public static string Serialize<T>(IEnumerable<T> objList)
        {
            StringBuilder result = new StringBuilder();
            Serialize<T>(objList, result);
            return result.ToString();
        }

        public static void Serialize<T>(IEnumerable<T> objList, StringBuilder result)
        {
            if (objList == null) return;
            Type type = typeof(T);
            result.AppendFormat("<{0}{1}>", ARRAY_MASK, type.Name);
            Serialize(objList, result, type);
            result.AppendFormat("</{0}{1}>", ARRAY_MASK, type.Name);
        }

        public object DeSerialize<T>(string str)
        {
            if (string.IsNullOrEmpty(str)) return null;
            object result = null;
            XmlDocument xml = LoadXML(str);
            result = DeSerialize<T>(xml);
            return result;
        }

        protected static XmlDocument LoadXML(string str)
        {
            XmlDocument xml = new XmlDocument();
            try
            {
                xml.LoadXml(str);
            }
            catch (Exception ex)
            {
                throw new SerializationException("格式不正确，" + ex.Message);
            }
            return xml;
        }

        public object DeSerialize<T>(XmlDocument xml)
        {
            object result = null;
            var baseNode = xml.DocumentElement;
            var type = typeof(T);
            if (baseNode.Name.ToUpper().StartsWith(ARRAY_MASK.ToUpper()))//数组
            {
                T array = Activator.CreateInstance<T>();//创建数组实例
                result = DeSerialize(array, baseNode, type);
            }
            else
            { //对象
                result = DeSerialize(type, type.GetProperties(), baseNode);
            }
            return result;
        }

        /// <summary>
        /// 反序列化数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="baseNode"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private IList DeSerialize(object array, XmlNode baseNode, Type type)
        {
            Type childType = GetChildType(type);
            IList list = array as IList;//转换为数组
            var pros = childType.GetProperties();//获取数组内对象的属性
            foreach (XmlNode item in baseNode.ChildNodes)//遍历反序列化数组中的每个对象
            {
                var child = DeSerialize(childType, pros, item);
                list.Add(child);
            }
            return list;
        }
        /// <summary>
        /// 解析数组或可空类型中的对象类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Type GetChildType(Type type)
        {
            try
            {
                var fullName = type.FullName;
                Type childType;
                string childTypeName;//解析数组内对象的类型名称
                if (HasChildType(fullName))//IEnumerable
                {
                    childTypeName = fullName.Substring(fullName.IndexOf("[["));
                    childTypeName = childTypeName.Replace("[[", string.Empty);
                    childTypeName = childTypeName.Replace("]]", string.Empty);
                    childType = Type.GetType(childTypeName);//用全称可取到Type
                }
                else//Array
                {
                    childTypeName = fullName.Replace("[]", string.Empty);
                    var assembly = type.Module.ToString();
                    assembly = assembly.Replace("{", string.Empty).Replace("}", string.Empty).Replace(".dll", string.Empty);
                    childType = Assembly.Load(assembly).GetType(childTypeName);//先加载程序集才能取到Type
                }
                return childType;
            }
            catch (Exception ex)
            {
                throw new SerializationException("解析子类型失败，父类型" + type.ToString() + "，原因" + ex.Message);
            }
        }

        private static bool HasChildType(string fullName)
        {
            return fullName.IndexOf("[[") > 0;
        }

        /// <summary>
        /// 反序列化对象和属性
        /// </summary>
        /// <param name="childType"></param>
        /// <param name="pros"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        protected object DeSerialize(Type childType, PropertyInfo[] pros, XmlNode item)
        {
            if (childType.IsValueType || childType.FullName.Equals("System.String"))
            {
                return ChangeType(item.Name, item.InnerText, childType);
            }
            var child = Activator.CreateInstance(childType);
            foreach (XmlNode node in item.ChildNodes)//遍历对象的属性
            {
                var pro = SetBasicValue(pros, child, node.Name, node.InnerText);
                if (pro == null) continue;
                var attr = pro.GetAttribute(typeof(SerializeAttribute)) as SerializeAttribute;
                if (attr == null) continue;//没有设置序列化描述的属性直接跳过
                if (attr.SerializeType == SerializeType.Object)//对象，递归调用反序列化
                {
                    var newChildType = pro.PropertyType;//对象类型取该属性的类型
                    var newChild = DeSerialize(newChildType, newChildType.GetProperties(), node);//递归
                    pro.SetValue(child, newChild, null);
                }
                else if (attr.SerializeType == SerializeType.List)//数组，递归调用反序列化
                {
                    var newChildType = pro.PropertyType;//对象类型取该属性的类型
                    var array = Activator.CreateInstance(newChildType);//创建数组实例
                    var newChild = DeSerialize(array, node, newChildType);//递归
                    pro.SetValue(child, newChild, null);
                }
            }
            DealAttributes(pros, item, child);
            return child;
        }

        protected abstract void DealAttributes(PropertyInfo[] pros, XmlNode item, object child);

        protected PropertyInfo SetBasicValue(PropertyInfo[] pros, object child, string name, string nodeValue)
        {
            var pro = FindProperty(name, pros);//找到反射出的属性集合中对应的属性
            if (pro == null)
            {
                NodeNameError(name);
                return null;
            }
            var attr = pro.GetAttribute(typeof(SerializeAttribute)) as SerializeAttribute;
            if (attr == null) return pro;//没有设置序列化描述的属性直接跳过
            //根据属性设置的序列化类型进行反序列化
            if (attr.SerializeType == SerializeType.Basic)//基元类型直接赋值
            {
                object value = ChangeType(name, nodeValue, pro.PropertyType);//强制转换类型，否则值类型会抛异常
                pro.SetValue(child, value, null);
            }
            return pro;
        }

        protected abstract void NodeNameError(string name);

        protected abstract PropertyInfo FindProperty(string name, PropertyInfo[] pros);

        protected static PropertyInfo FindPropertyByPropertyName(string name, PropertyInfo[] pros)
        {
            return pros.FirstOrDefault(p => p.Name.ToUpper() == name.ToUpper());
        }
        /// <summary>
        /// 类型转换
        /// </summary>
        /// <param name="node"></param>
        /// <param name="pro"></param>
        /// <returns></returns>
        private static object ChangeType(string name, string value, Type type)
        {
            if (Nullable(type) && value == string.Empty) return null;//可空值类型，值为空时返回null
            if (type.IsEnum)
            {
                try
                {
                    return Int32.Parse(value);//枚举必须先转换为Int
                }
                catch
                {
                    throw new SerializationException("参数" + name + "只允许为数字，而实际值为" + value);
                }
            }
            var realType = GetRealType(type);
            if (realType.IsEnum)//可空枚举，无法转换类型，循环枚举找出对应值
            {
                StringBuilder values = new StringBuilder();
                foreach (var v in Enum.GetValues(realType))
                {
                    if (((int)v).ToString() == value) return v;
                    values.Append(((int)v).ToString() + ",");
                }
                throw new SerializationException("参数" + name + "只允许为" + values.ToString() + "，而实际值为" + value);
            }
            try
            {
                return Convert.ChangeType(value, realType);//强制转换类型，否则值类型会抛异常
            }
            catch (Exception ex)
            {
                throw new SerializationException("参数" + name + "验证失败，实际值为" + value + "，请检查格式，" + ex.Message);
            }
        }
        /// <summary>
        /// 可空值类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool Nullable(Type type)
        {
            return type.Name.IndexOf("Nullable") >= 0;
        }

        /// <summary>
        /// 数组需要用<ArrayOf对象类型名称>括起来
        /// </summary>
        /// <param name="objList"></param>
        /// <param name="result"></param>
        /// <param name="type"></param>
        private static void Serialize(IEnumerable objList, StringBuilder result, Type type)
        {
            var pros = type.GetProperties();
            if (objList == null) return;
            foreach (var obj in objList)
            {
                Serialize(obj, result, pros, type, true);
            }
        }

        /// <summary>
        /// 数组中的对象用对象类型名称括起来，子对象不括起来
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="result"></param>
        /// <param name="withTypeName"></param>
        private static void Serialize<T>(T obj, StringBuilder result, bool withTypeName)
        {
            Serialize(obj, result, (typeof(T)).GetProperties(), typeof(T), withTypeName);
        }

        /// <summary>
        /// 单个对象用对象类型名称括起来
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="result"></param>
        /// <param name="pros"></param>
        /// <param name="type"></param>
        private static void Serialize(object obj, StringBuilder result, PropertyInfo[] pros, Type type, bool withTypeName)
        {
            if (withTypeName) result.AppendFormat("<{0}>", GetTypeName(type));
            foreach (var item in pros)
            {
                var serializeAttr = item.GetAttribute(typeof(SerializeAttribute)) as SerializeAttribute;
                if (serializeAttr == null) continue;//没有设置序列化描述的属性直接跳过
                if (serializeAttr.SerializeType == SerializeType.Basic)//基元类型，直接ToString
                    result.AppendFormat("<{0}>{1}</{0}>", item.Name, GetValue(obj, item));
                else if (serializeAttr.SerializeType == SerializeType.Object)//对象，递归调用序列化
                {
                    result.AppendFormat("<{0}>", item.Name);//子对象用属性名称括起来
                    Serialize(item.GetValue(obj, null), result, item.PropertyType.GetProperties(), item.PropertyType, false);
                    result.AppendFormat("</{0}>", item.Name);
                }
                else if (serializeAttr.SerializeType == SerializeType.List)//数组，递归调用序列化
                {
                    result.AppendFormat("<{0}>", item.Name);//数组用属性名称括起来
                    Serialize(item.GetValue(obj, null) as IEnumerable, result, GetChildType(item.PropertyType));
                    result.AppendFormat("</{0}>", item.Name);
                }
            }
            if (withTypeName) result.AppendFormat("</{0}>", GetTypeName(type));
        }

        private static string GetTypeName(Type type)
        {
            if (!HasChildType(type.FullName))
                return type.Name;
            return type.Name.Split('`')[0] + "Of" + GetChildType(type).Name;
        }
        /// <summary>
        /// 枚举返回int，时间返回字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private static string GetValue(object obj, PropertyInfo item)
        {
            if (obj == null) return string.Empty;
            var result = item.GetValue(obj, null);
            if (result == null) return string.Empty;
            var type = GetRealType(item.PropertyType);
            if (type.IsEnum) return ((int)result).ToString();
            if (type == typeof(DateTime)) return ((DateTime)result).ToString();
            return result.ToString().Replace("<", "【").Replace(">", "】").Replace("【br/】","<br />").Replace("【br /】", "<br />");
        }
        /// <summary>
        /// 可空类型返回子类型
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private static Type GetRealType(Type type)
        {
            if (Nullable(type)) type = System.Nullable.GetUnderlyingType(type);
            return type;
        }
    }
}
