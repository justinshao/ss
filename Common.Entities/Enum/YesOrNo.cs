using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities
{
    public enum YesOrNo
    {
        /// <summary>
        /// 否
        /// </summary>
        [Description("否")]
        No = 0,
        /// <summary>
        /// 是
        /// </summary>
        [Description("是")]
        Yes = 1,
    }
}
