using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Utilities
{
    public class ObjectHelper
    {
        //获取实体类里面所有的名称、值、DESCRIPTION值
        public static string getProperties<T>(T t)
        {
            StringBuilder str = new StringBuilder();
            try
            {
                if (t == null)
                {
                    return str.ToString();
                }
                System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

                if (properties.Length <= 0)
                {
                    return str.ToString();
                }
                foreach (System.Reflection.PropertyInfo item in properties)
                {
                    string name = item.Name; //名称
                    object value = item.GetValue(t, null);  //值

                    if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                    {
                        str.AppendFormat("{0}：{1}；", name, value);
                    }
                    else
                    {
                        getProperties(value);
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            return str.ToString();
        }
    }
}
