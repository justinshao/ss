using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities.Enum
{
    public enum DerateQRCodeSource
    {
        /// <summary>
        /// 管理处
        /// </summary>
        [Description("管理处")]
        Platefrom = 0,
        /// <summary>
        /// 商家
        /// </summary>
        [Description("商家")]
        Seller = 1,
    }
}
