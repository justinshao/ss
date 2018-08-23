using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Reflection;
using Common.Core.Attributes;
 
    public static class EnumExtensions
    {
        /// <summary>
        /// 获取32位整形值
        /// </summary>
        public static int GetIntegerValue(this Enum enumValue)
        {
            return Convert.ToInt32(enumValue);
        }

        /// <summary>
        /// 获取byte值
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static byte GetByteValue(this Enum enumValue)
        {
            return Convert.ToByte(enumValue);
        }

        public static string GetString(this Enum enumValue)
        {
            return enumValue.ToString();
        }

        /// <summary>
        /// 获取Description的值
        /// </summary>
        public static string GetDescription(this Enum enumValue)
        {
            var description = enumValue.GetType()
                .GetMember(enumValue.ToString())
                .Select(x => x.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault())
                .FirstOrDefault() as DescriptionAttribute;
            return description == null ? enumValue.ToString() : description.Description;
        }

        /// <summary>
        /// 获取有属性说明的枚举
        /// </summary>
        /// <returns>key为int类型值，value为属性说明</returns>
        public static Hashtable GetEnumHash(this Enum en)
        {
            var ht = new Hashtable();
            var fieldinfo = en.GetType().GetFields();
            foreach (var item in fieldinfo)
            {
                var objAttr = item.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (objAttr.Length <= 0) continue;
                var value = ((DescriptionAttribute)objAttr[0]).Description;
                ht.Add((int)item.GetValue(item.Name), value);
            }
            return ht;
        }
        public static string GetEnumDefaultValue(this Enum value)
        {
            if (value == null) throw new ArgumentException("value");
            return GetEnumDefaultValueEnum(value);
        }
        public static string GetEnumDefaultValueEnum(this object value)
        {
            Type type = value.GetType();
            FieldInfo field = type.GetField(Enum.GetName(type, value));
            if (field != null)
            {
                if (Attribute.IsDefined(field, typeof(EnumDefaultValueAttribute)))
                {
                    EnumDefaultValueAttribute des = Attribute.GetCustomAttribute(field, typeof(EnumDefaultValueAttribute)) as EnumDefaultValueAttribute;
                    if (des != null)
                    {
                        return des.DefaultValue;
                    }
                }
            }
            return value.ToString();
        }


    public static string GetDescriptionForEnum(this object value)
    {
        Type type = value.GetType();
        FieldInfo field = type.GetField(Enum.GetName(type, value));
        if (field != null)
        {
            if (Attribute.IsDefined(field, typeof(DescriptionAttribute)))
            {
                DescriptionAttribute des = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (des != null)
                {
                    return des.Description;
                }
            }
        }
        return value.ToString();
    }

    public static string GetValueToString(this Enum value)
    {
        return value.GetValueToString(true);
    }
    public static string GetValueToString(this Enum value, bool withSplit)
    {
        if (value == null) throw new ArgumentException("value");
        string str = Convert.ToInt32(value).ToString();
        return withSplit ? "'" + str + "'" : str;
    }

    /// <summary>
    /// 判断枚举是否包含一个标识(通常是位与运算)
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="value">值</param>
    /// <param name="flag">标识</param>
    /// <returns>是否包含</returns>
    public static bool IsFlagSet<T>(this Enum value, T flag)
    {
        if (Type.GetTypeHandle(value).Value != Type.GetTypeHandle(flag).Value)
            throw new ArgumentException("value flag必须是同一类枚举");
        if (!typeof(T).IsEnum) throw new ArgumentException("T必须是枚举");

        var v = Convert.ToInt64(value);
        var f = Convert.ToInt64(flag);
        return (v & f) == f;
    }
}
 
