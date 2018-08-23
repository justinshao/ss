using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Reflection;
using System.Runtime.Serialization;

namespace Common.Core
{
    /// <summary>
    /// 外接口序列化器
    /// </summary>
    public class InterfaceXMLSerializer : XMLSerializer
    {
        protected override PropertyInfo FindProperty(string name, PropertyInfo[] pros)
        {
            return FindPropertyByPropertyName(name, pros);
        }

        protected override void NodeNameError(string name)
        {
            //throw new SerializationException("参数名称错误：" + name);
        }

        protected override void DealAttributes(PropertyInfo[] pros, XmlNode item, object child)
        {
            
        }
    }
}
