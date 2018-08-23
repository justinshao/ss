using System;
using System.ComponentModel;
using System.Reflection;

/// <summary>
/// 扩展 Type 类型。
/// </summary>
public static class TypeExtensions {

    /// <summary>
    /// 判断类型是否实现了指定的接口。
    /// </summary>
    /// <param name="type">判断的类型。</param>
    /// <param name="interfaceType">接口类型。</param>
    /// <returns>如果类型 type 实现了接口 interfaceType 返回 true,否则返回 false.</returns>
    public static bool Implemented(this Type type, Type interfaceType) {
        if (interfaceType == null) {
            throw new ArgumentNullException("interfaceType");
        }
        if (interfaceType.IsInterface) {
            return Implemented(type, interfaceType.Name);
        }

        throw new Exception(string.Format("{0} 不是接口。", interfaceType.Name));
    }

    /// <summary>
    /// 判断类型是否实现了指定的接口。
    /// </summary>
    /// <param name="type">判断的类型。</param>
    /// <param name="interfaceName">接口名称。</param>
    /// <returns>如果类型 type 实现了接口 interfaceType 返回 true,否则返回 false.</returns>
    public static bool Implemented(this Type type, string interfaceName) {
        if (string.IsNullOrEmpty(interfaceName)) {
            throw new ArgumentNullException("interfaceName");
        }
        return type.GetInterface(interfaceName) != null;
    }

    public static string GetDescription(this Type type) {
        DescriptionAttribute des = type.GetAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
        if (des != null) {
            return des.Description;
        }
        return string.Empty;
    }

    public static Attribute[] GetAttributes(this Type type, Type attributeType) {
        if (attributeType == null) {
            throw new ArgumentNullException("attributeType");
        }
        if (type.IsDefined(attributeType, true)) {
            return type.GetCustomAttributes(attributeType, true) as Attribute[];
        }
        return null;
    }
    public static Attribute GetAttribute(this Type type, Type attributeType) {
        Attribute[] attributes = GetAttributes(type, attributeType);
        if (attributes != null && attributes.Length > 0) {
            return attributes[0];
        }
        return null;
    }

    public static Attribute[] GetAttributes(this MemberInfo mem, Type attributeType) {
        if (attributeType == null) {
            throw new ArgumentNullException("attributeType");
        }
        if (mem.IsDefined(attributeType, true)) {
            return mem.GetCustomAttributes(attributeType, true) as Attribute[];
        }
        return null;
    }
    public static Attribute GetAttribute(this MemberInfo mem, Type attributeType) {
        Attribute[] attributes = GetAttributes(mem, attributeType);
        if (attributes != null && attributes.Length > 0) {
            return attributes[0];
        }
        return null;
    }
}
