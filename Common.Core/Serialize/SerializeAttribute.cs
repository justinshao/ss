using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Core
{
    /// <summary>
    /// 自定义序列化用，需要序列化的属性需设置此描述，仅限属性，因为序列化时只取属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SerializeAttribute : Attribute
    {
        /// <summary>
        /// 默认为基元类型
        /// </summary>
        public SerializeType SerializeType { get; set; }
        /// <summary>
        /// IEnumerable类型的必须设置此属性
        /// </summary>
        public Type ListType { get; set; }
        /// <summary>
        /// 反序列化时重命名
        /// </summary>
        public string Name { get; set; }

        public SerializeAttribute() { }

        public SerializeAttribute(SerializeType serializeType)
        {
            SerializeType = serializeType;
        }
    }
}
