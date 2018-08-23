using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities
{
    /// <summary>
    /// 卡类型
    /// </summary>
    public enum CardType
    {
        /// <summary>
        /// 车牌
        /// </summary>
        [Description("车牌")]
        Plate = 1,
        /// <summary>
        /// 卡片
        /// </summary>
        [Description("卡片")]
        Card = 2
    }
}
