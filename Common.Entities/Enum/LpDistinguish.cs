using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities
{
    /// <summary>
    /// 进出方式
    /// </summary>
    public enum LpDistinguish
    {
        /// <summary>
        /// 刷卡
        /// </summary>
        [Description("刷卡")]
        Card = 1,
        /// <summary>
        /// 刷卡+车牌
        /// </summary>
        [Description("刷卡+车牌")]
        CardAndPlate = 2,
        /// <summary>
        /// 车牌
        /// </summary>
        [Description("车牌")]
        Plate = 3
    }
}
