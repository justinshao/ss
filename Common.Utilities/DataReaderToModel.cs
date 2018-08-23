using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.Common;
using Common.Utilities.Helpers;

namespace Common.Utilities
{
    public class DataReaderToModel<T> where T:new()
    { 
        public static T ToModel(DbDataReader dr)
        {
            if (dr == null || dr.FieldCount == 0)
                return default(T);
            Type modelType = typeof(T);
            T t = new T();
            for (int count = 0; count < dr.FieldCount; count++)
            {
                PropertyInfo[] ps = modelType.GetProperties();
                foreach (PropertyInfo p in ps)
                {
                    string propertyname = dr.GetName(count);
                    if (propertyname != p.Name)
                        continue;
                    if (!p.CanWrite)
                        break;
                    object v = dr[count];
                    if (v != DBNull.Value)
                    {
                        if (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                        {
                            Type pbasetype = Nullable.GetUnderlyingType(p.PropertyType);
                            //p.SetValue(t, Convert.ChangeType(v, pbasetype), null);
                            if (v != null)
                            {
                                p.SetValue(t, Convert.ChangeType(v, pbasetype), null);
                            }
                            else
                            {
                                p.SetValue(t, System.DBNull.Value, null);
                            }
                        }
                        else if (p.PropertyType.FullName == "System.Boolean")
                        {
                            p.SetValue(t, v.ToString().ToBoolean() ? true : false, null);
                            //p.SetValue(t, v.ToString() == "1" ? true : false, null);

                        }
                        else
                        {
                            if (dr[p.Name].GetType().FullName == "System.Guid")
                            {
                                p.SetValue(t,v.ToString(), null);
                            }
                            else
                            {
                                ChangeType(p, t, v, p.PropertyType);
                                //p.SetValue(t, Convert.ChangeType(v, p.PropertyType), null);
                            }
                        }
                    }                    
                    break;
                }
            }
            return t;
        }

        static void ChangeType(PropertyInfo p, object t, object ConversionValue, Type ConversionType)
        {
            if (ConversionType.IsEnum)
            { 
                p.SetValue(t,EnumHelper.GetEnumObject(p.PropertyType, (int)ConversionValue), null);
                //switch (ConversionType.FullName)
                //{ 
                ////case "Common.Entities.YesOrNo":
                ////    p.SetValue(t, (Common.Entities.YesOrNo)ConversionValue, null);
                //// break;
                //default:
                //        break;
                //}
            }
            else
            {
                p.SetValue(t, Convert.ChangeType(ConversionValue, p.PropertyType), null);
            }
        }
    }
}
