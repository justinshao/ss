using System;
using System.Reflection;

namespace Common.Utilities
{
    public class CloneUtil
    {
        public static bool Clone(object destination, object source)
        {
            if (destination == null || source == null)
            {
                return false;
            }

            return Clone(destination, source, source.GetType());
        }

        public static bool Clone(object destination, object source, Type type)
        {
            if (destination == null || source == null)
            {
                return false;
            }
            Type desType = destination.GetType();
            foreach (FieldInfo mi in type.GetFields())
            {
                try
                {
                    FieldInfo des = desType.GetField(mi.Name);
                    if (des != null && des.FieldType == mi.FieldType)
                    {
                        des.SetValue(destination, mi.GetValue(source));
                    }
                }
                catch
                {}
            }

            foreach (PropertyInfo pi in type.GetProperties())
            {
                try
                {
                    PropertyInfo des = desType.GetProperty(pi.Name);
                    if (des != null && des.PropertyType == pi.PropertyType && des.CanWrite && pi.CanRead)
                    {
                        des.SetValue(destination, pi.GetValue(source, null), null);
                    }
                }
                catch
                {}
            }
            return true;
        }
    }
}
