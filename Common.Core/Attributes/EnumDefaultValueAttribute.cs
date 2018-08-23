using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Core.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class EnumDefaultValueAttribute : Attribute
    {
        public string DefaultValue { get; set; }

        public EnumDefaultValueAttribute(string value)
        {
            this.DefaultValue = value;
        }
    }
}
