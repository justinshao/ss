using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities
{
    public enum PassRemarkType
    {
        /// <summary>
        /// 免费放行
        /// </summary>
        [Description("免费放行")]
        Gratis = 1,
        /// <summary>
        /// 异常放行
        /// </summary>
        [Description("异常放行")]
        Abnormal = 2,

        /// <summary>
        /// 手动抬杆放行
        /// </summary>
        [Description("手动抬杆放行")]
        ManualOpen = 3,


        /// <summary>
        /// 修改金额
        /// </summary>
        [Description("修改金额")]
        EditMoney=4,

        /// <summary>
        /// 物品类型
        /// </summary>
        [Description("物品类型")]
        EditGoods = 5,
    }
}
