using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Reflection;
using System.Collections.ObjectModel;
using Common.Core.Expands.DataContract;

/// <summary>
/// Object扩展类
/// </summary>
public static class ObjectExtension
{
    /// <summary>
    /// 将object对象转化为整型
    /// </summary>
    /// <remarks>扩展方法</remarks>
    /// <returns>整型值，转化失败默认返回0</returns>
    public static int ToInt(this object obj)
    {
        if (obj is double)
        {
            return (int)obj.ToDouble();
        }
        if (obj is decimal)
        {
            return (int)obj.ToDecimal();
        }
        if (obj is float)
        {
            return (int)obj.ToFloat();
        }
        if (obj == null)
        {
            return 0;
        }
        int ret = 0;
        Int32.TryParse(obj.ToString(), out ret);
        return ret;
    }
    /// <summary>
    /// 将object对象转化为整型
    /// </summary>
    /// <remarks>扩展方法</remarks>
    /// <returns>整型值，转化失败默认返回0</returns>
    public static uint ToUInt(this object obj)
    {
        if (obj is double)
        {
            return (uint)obj.ToDouble();
        }
        if (obj is decimal)
        {
            return (uint)obj.ToDecimal();
        }
        if (obj is float)
        {
            return (uint)obj.ToFloat();
        }
        if (obj == null)
        {
            return 0;
        }
        uint ret = 0;
        uint.TryParse(obj.ToString(), out ret);
        return ret;
    }
    /// <summary>
    /// 将object对象转化为大整型
    /// </summary>
    /// <remarks>扩展方法</remarks>
    /// <returns>整型值，转化失败默认返回0</returns>
    public static Int64 ToInt64(this object obj)
    {
        Int64 ret;
        Int64.TryParse(obj.ToString(), out ret);
        return ret;
    }

    /// <summary>
    /// 将object对象转化为Double类型
    /// </summary>
    /// <remarks>扩展方法</remarks>
    /// <returns>Double值，转化失败默认返回0</returns>
    public static double ToDouble(this object obj)
    {
        double ret = 0;
        Double.TryParse(obj.ToString(), out ret);
        return ret;
    }

    public static byte[] ToBytes(this object obj)
    {
        byte[] bytes = { };
        try
        {
            bytes = (byte[])obj;
        }
        catch
        {
        }
        return bytes;
    }
    /// <summary>
    /// 将object对象转化为时间类型
    /// </summary>
    /// <remarks>扩展方法</remarks>
    /// <returns>时间，转化失败默认返回最小时间</returns>
    public static DateTime ToDateTime(this object obj)
    {
        DateTime ret = new DateTime(1899, 12, 30);
        if (!DateTime.TryParse(obj.ToString(), out ret))
        {
            ret = DateTime.MinValue;
        }
        return ret;
    }
    public static DateTime? GetDateTimeDefaultNull(this object obj)
    {
        DateTime date;
        if (!DateTime.TryParse(obj.ToString(), out date))
            return null;
        return date;
    }
    /// <summary>
    /// 将object对象转化为浮点型
    /// </summary>
    /// <remarks>扩展方法</remarks>
    /// <returns>浮点值，转化失败默认返回0</returns>
    public static float ToFloat(this object obj)
    {
        float ret = 0;
        float.TryParse(obj.ToString(), out ret);
        return ret;
    }

    /// <summary>
    /// 将object对象转化为布尔型
    /// </summary>
    /// <remarks>扩展方法</remarks>
    /// <returns>布尔值，转化失败默认返回false</returns>
    public static bool ToBoolean(this object obj)
    {
        bool ret = false;
        if (obj.ToString().IsInteger())//大于零都为true
        {
            ret = obj.ToInt() == 0 ? false : true;
        }
        else
        {
            Boolean.TryParse(obj.ToString(), out ret);
        }
        return ret;
    }

    public static decimal ToDecimal(this object obj)
    {
        decimal ret = 0;
        decimal.TryParse(obj.ToString(), out ret);
        return ret;
    }

    /// <summary>
    /// 将Datatble转换成Silverlight可识别的数据集合
    /// </summary>
    /// <param name="dataTable">要转换的Datatble</param>
    /// <returns></returns>
    public static ObservableCollection<List<Common.Core.Expands.DataContract.DataItem>> ToList(this DataTable dataTable)
    {
        ObservableCollection<List<DataItem>> itemCollect =
            new ObservableCollection<List<DataItem>>();

        foreach (DataRow row in dataTable.Rows)
        {
            List<DataItem> items = new List<DataItem>(dataTable.Columns.Count);
            foreach (DataColumn column in dataTable.Columns)
            {
                items.Add(new DataItem()
                {
                    Name = column.ColumnName,
                    Value = Convert.ToString(row[column]),
                    ValueType = column.DataType.FullName
                });
            }
            itemCollect.Add(items);
        }
        return itemCollect;
    }

    /// <summary>把对象转换成Dictionary</summary>
    /// <param name="obj">要转换的对象</param>
    /// <param name="withoutProperties">要排除转换的属性，可选参数。</param>
    /// <returns></returns>
    public static Dictionary<string, T> ToDictionary<T>(this object obj, List<string> withoutProperties = null)
    {
        if (obj == null) return null;
        PropertyInfo[] properties = obj.GetType().GetProperties();
        Dictionary<string, T> valueList = new Dictionary<string, T>(properties.Count());
        foreach (var item in properties)
        {
            if (withoutProperties != null && withoutProperties.Contains(item.Name))
                valueList.Add(item.Name, (T)item.GetValue(obj, null));
        }
        return valueList;
    }

    /// <summary>将Dictionary转换成对象</summary>
    /// <typeparam name="T">要转换的对象类型</typeparam>
    /// <param name="valueList">要转换的值字典</param>
    /// <param name="withoutProperties">要排除的字典值。可选参数。</param>
    /// <returns></returns>
    public static T ToClass<T>(this Dictionary<string, object> valueList, List<string> withoutProperties = null)
    {
        if (valueList == null) return default(T);
        Type type = typeof(T);
        object obj = Activator.CreateInstance(type);
        foreach (var item in valueList)
        {
            if (withoutProperties == null || withoutProperties.Contains(item.Key) == false)
            {
                PropertyInfo property = type.GetProperty(item.Key);
                if (property != null)
                {
                    object value = ChangeType(item.Value, property.PropertyType);
                    property.SetValue(obj, value, null);
                }
            }
        }

        return (T)obj;
    }

    private static object ChangeType(object value, Type targetType)
    {
        if (targetType.Name == "Nullable`1")
            targetType = Nullable.GetUnderlyingType(targetType);
        try
        {
            return Convert.ChangeType(value, targetType, System.Threading.Thread.CurrentThread.CurrentCulture);
        }
        catch
        {
            return null;
        }

    }
}
