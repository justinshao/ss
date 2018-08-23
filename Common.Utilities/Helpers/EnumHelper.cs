using System;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;
using System.Collections;

namespace Common.Utilities.Helpers
{
    /// <summary>
    /// 枚举助手类
    /// </summary>
    public sealed class EnumHelper
    {
        /// <summary>
        /// 将字符转换为枚举
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="value">传入的字符</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static T ParseEnum<T>(string value, T defaultValue)
            where T : struct, IComparable, IFormattable, IConvertible
        {
            if (!typeof(T).IsEnum)
                return defaultValue;
            if (string.IsNullOrWhiteSpace(value)) return defaultValue;
            T outPutValue;
            if (!Enum.TryParse(value, true, out outPutValue))
            {
                outPutValue = defaultValue;
            }
            return outPutValue;
        }

        /// <summary>
        /// 获取枚举描述
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool GetBindable(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            obj.ToString();
            return GetBindable(obj.GetType(), obj.ToString());
        }
        public static bool GetBindable(Type type, string filedName)
        {
            bool bindable = true;
            foreach (FieldInfo info in type.GetFields())
            {
                if (!info.IsSpecialName && !(info.Name != filedName))
                {
                    object[] customAttributes = info.GetCustomAttributes(typeof(BindableAttribute), false);
                    if (customAttributes.Length > 0)
                    {
                        bindable = ((BindableAttribute)customAttributes[0]).Bindable;
                    }
                }
            }
            return bindable;
        }

        /// <summary>
        /// 返回指定枚举类型的指定值的描述
        /// </summary>
        /// <param name="t">枚举类型</param>
        /// <param name="v">枚举值</param>
        /// <returns></returns>
        public static bool GetBindable(Type t, object v)
        {
            try
            {
                FieldInfo oFieldInfo = t.GetField(GetName(t, v));
                BindableAttribute[] attributes = (BindableAttribute[])oFieldInfo.GetCustomAttributes(typeof(BindableAttribute), false);
                return (attributes.Length > 0) ? attributes[0].Bindable : false;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 获取枚举描述
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetDescription(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            obj.ToString();
            return GetDescription(obj.GetType(), obj.ToString());
        }

        /// <summary>
        /// 获取枚举描述
        /// </summary>
        /// <param name="type"></param>
        /// <param name="filedName"></param>
        /// <returns></returns>
        public static string GetDescription(Type type, string filedName)
        {
            string description = string.Empty;
            foreach (FieldInfo info in type.GetFields())
            {
                if (!info.IsSpecialName && !(info.Name != filedName))
                {
                    object[] customAttributes = info.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (customAttributes.Length > 0)
                    {
                        description = ((DescriptionAttribute)customAttributes[0]).Description;
                    }
                }
            }
            if (description == string.Empty)
            {
                description = filedName;
            }
            return description;
        }

        /// <summary>
        /// 获取枚举描述
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescriptionByValue(Type type, string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";
            string fieldName = string.Empty;
            fieldName = Enum.Parse(type, value).ToString();
            return GetDescription(type, fieldName);
        }


        /// <summary>
        /// 通过枚举描述获取枚举项名称
        /// </summary>
        /// <param name="type"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static object GetEnumValueByDescription(Type type, string description)
        {
            string[] arrDescription = GetDescriptions(type);
            string sName = string.Empty;
            foreach (string str in Enum.GetNames(type))
            {
                if (GetDescription(type, str) == description)
                {
                    sName = str;
                    break;
                }
            }

            if (string.IsNullOrEmpty(sName))
                return null;

            return Enum.Parse(type, sName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="des"></param>
        /// <returns></returns>
        public static object GetValueByDescription(Type type, string des)
        {
            object value = Enum.Parse(type, des, true);
            return value;
        }

        /// <summary>
        /// 根据枚举字符串 获取枚举值
        /// </summary>
        public static object GetEnumObjectByEnumStringName(Type type, string EnumStringName)
        {
            return Enum.Parse(type, EnumStringName);
        }

        /// <summary>
        /// 获取枚举描述列表 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string[] GetDescriptions(Type type)
        {
            FieldInfo[] fields = type.GetFields();
            ArrayList list = new ArrayList();
            string description = string.Empty;
            foreach (FieldInfo info in fields)
            {
                if (!info.IsSpecialName)
                {
                    object[] customAttributes = info.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (customAttributes.Length > 0)
                    {
                        description = ((DescriptionAttribute)customAttributes[0]).Description;
                        if (description == string.Empty)
                        {
                            description = customAttributes[0].ToString();
                        }
                        list.Add(description);
                    }
                }
            }
            string[] array = new string[list.Count];
            list.CopyTo(array);
            return array;
        }

        /// <summary>
        /// 获取枚举成员列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<EnumContext> GetEnumContextList(Type type)
        {
            List<EnumContext> list = new List<EnumContext>();
            foreach (string str in Enum.GetNames(type))
            {
                int x = (int)Enum.Parse(type, str);
                EnumContext item = new EnumContext
                {
                    EnumValue = x,
                    EnumString = str,
                    Description = GetDescription(type, str)
                };
                list.Add(item);
            }
            return list;
        }

        /// <summary>
        /// 获取枚举成员列表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="isGenerateAll">是否生成“全部”</param>
        /// <returns></returns>
        public static List<EnumContext> GetEnumContextList(Type type, bool isGenerateAll)
        {
            List<EnumContext> list = new List<EnumContext>();
            if (isGenerateAll)
                list.Add(new EnumContext() { EnumString = "all", EnumValue = -1, Description = "全部" });

            list.AddRange(GetEnumContextList(type));
            return list;
        }
        public static string GetDescriptionByEnumString(Type type, string enumString)
        {
            foreach (string str in Enum.GetNames(type))
            {
                if (enumString.Equals(str))
                {
                    return GetDescription(type, str);
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static EnumContext GetEnumContextByDescription(Type type, string description)
        {
            EnumContext context = null;
            foreach (string str in Enum.GetNames(type))
            {
                if (description.Equals(GetDescription(type, str)))
                {
                    EnumContext item = new EnumContext
                    {
                        EnumString = str,
                        Description = GetDescription(type, str)
                    };

                    context = item;
                }
            }

            return context;
        }

        /// <summary>
        /// 获取枚举值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static object GetEnumObject(Type type, int enumValue)
        {
            return Enum.ToObject(type, enumValue);
        }

        /// <summary>
        /// 获取枚举值
        /// </summary>
        public static object GetEnumObject(Type type, string enumName)
        {
            return Enum.ToObject(type, enumName);
        }

        /// <summary>
        /// 通过枚举描述获取枚举项名称
        /// </summary>
        /// <param name="type"></param>
        /// <param name="strDescription"></param>
        /// <returns></returns>
        public static string GetEnumNameByDescription(Type type, string strDescription)
        {
            string[] arrDescription = GetDescriptions(type);
            string sName = "";
            foreach (string str in Enum.GetNames(type))
            {
                if (GetDescription(type, str) == strDescription)
                {
                    sName = str;
                    break;
                }
            }
            return sName;
        }

        /// <summary>
        /// 获取枚举索引列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<int> GetEnumObjectIndexList(Type type)
        {
            List<int> list = new List<int>();
            string[] names = Enum.GetNames(type);
            if ((names != null) && (names.Length > 0))
            {
                foreach (string str in names)
                {
                    int item = (int)Enum.Parse(type, str);
                    list.Add(item);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取枚举列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<object> GetEnumObjectList(Type type)
        {
            List<object> list = new List<object>();
            string[] names = Enum.GetNames(type);
            if ((names != null) && (names.Length > 0))
            {
                foreach (string str in names)
                {
                    object item = Enum.Parse(type, str);
                    list.Add(item);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取枚举列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<int> GetEnumIntList(Type type)
        {
            List<int> list = new List<int>();
            string[] names = Enum.GetNames(type);
            if ((names != null) && (names.Length > 0))
            {
                foreach (string str in names)
                {
                    int item = (int)Enum.Parse(type, str);
                    list.Add(item);
                }
            }
            return list;
        }

        private static string GetName(Type t, object v)
        {
            try
            {
                return Enum.GetName(t, v);
            }
            catch
            {
                return "UNKNOWN";
            }
        }

        /// <summary>
        /// 返回指定枚举类型的指定值的描述
        /// </summary>
        /// <param name="t">枚举类型</param>
        /// <param name="v">枚举值</param>
        /// <returns></returns>
        public static string GetDescription(Type t, object v)
        {
            try
            {
                FieldInfo oFieldInfo = t.GetField(GetName(t, v));
                DescriptionAttribute[] attributes = (DescriptionAttribute[])oFieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                return (attributes.Length > 0) ? attributes[0].Description : GetName(t, v);
            }
            catch
            {
                return "UNKNOWN";
            }
        }
        /// <summary>
        /// 通过枚举字符串获取枚举类型
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="defaultType">默认枚举值</param>
        /// <param name="strObjectType">枚举字符串</param>
        /// <returns></returns>
        public static T GetEnumbyEnumTypeStr<T>(T defaultType, string strObjectType)
        {
            try
            {
                if (Enum.IsDefined(typeof(T), strObjectType))
                {
                    return (T)Enum.Parse(typeof(T), strObjectType, true);
                }
                return defaultType;
            }
            catch (Exception e)
            {
                throw new Exception("枚举转换异常：" + e.Message);
            }
        }

        /// <summary>
        /// GetDayOfWeekEnum
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static DayOfWeek GetDayOfWeekEnum(string strValue)
        {
            if (string.IsNullOrEmpty(strValue)) return DayOfWeek.Monday;
            DayOfWeek objType = DayOfWeek.Monday;
            if (Enum.IsDefined(typeof(DayOfWeek), strValue))
            {
                objType = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), strValue, true);
            }
            return objType;
        }
        /// <summary>
        /// 获取星期对应的中文名称
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static string GetDayOfWeekEnumChiName(string strValue)
        {
            string ret = "";
            if (string.IsNullOrEmpty(strValue)) return ret;
            DayOfWeek v = GetDayOfWeekEnum(strValue);
            switch (v)
            {
                case DayOfWeek.Friday:
                    ret = "星期五";
                    break;
                case DayOfWeek.Monday:
                    ret = "星期一";
                    break;
                case DayOfWeek.Saturday:
                    ret = "星期六";
                    break;
                case DayOfWeek.Sunday:
                    ret = "星期天";
                    break;
                case DayOfWeek.Thursday:
                    ret = "星期四";
                    break;
                case DayOfWeek.Tuesday:
                    ret = "星期二";
                    break;
                case DayOfWeek.Wednesday:
                    ret = "星期三";
                    break;
                default:
                    break;

            }
            return ret;
        }


        /// <summary>
        /// 获取枚举成员索引
        /// </summary>
        /// <param name="type"></param>
        /// <param name="enumName"></param>
        /// <returns></returns>
        public static int GetEnumObjectIndex(Type type, string enumName)
        {
            int result = -1;
            string[] names = Enum.GetNames(type);
            if ((names != null) && (names.Length > 0))
            {
                foreach (string str in names)
                {
                    if (str != enumName)
                        continue;
                    result = (int)Enum.Parse(type, str);
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 获取和枚举名称相等的name
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetEnumNameByTypeName(Type type, string name)
        {
            string[] names = Enum.GetNames(type);
            if (names != null)
            {
                foreach (var item in names)
                {
                    if (item.ToLower() == name.ToLower())
                    {
                        return item;
                    }
                }
            }
            return name;
        }
    }
}
