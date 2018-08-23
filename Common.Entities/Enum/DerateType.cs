using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities
{
    public enum DerateType
    {
        /// <summary>
        /// 按时缴费
        /// </summary>
        [Description("按时缴费")]
        PaymentOnTime = 0,
        /// <summary>
        /// 不收取费用
        /// </summary>
        [Description("不收取费用")]
        NoPayment = 1,
          /// <summary>
        /// 按次收费
        /// </summary>
        [Description("按次收费")]
        TimesPayment = 2,
        /// <summary>
        /// 按票收费
        /// </summary>
        [Description("按票收费")]
        VotePayment = 3,
        /// <summary>
        /// 分时段收费
        /// </summary>
        [Description("分时段收费")]
        TimePeriodPayment = 4,
        /// <summary>
        /// 按收费标准
        /// </summary>
        [Description("按收费标准")]
        StandardPayment = 5,
        /// <summary>
        /// 特定时段按次收费
        /// </summary>
        [Description("特定时段按次收费")]
        TimePeriodAndTimesPayment = 6,
        /// <summary>
        /// 按票收费（特殊规则）
        /// </summary>
        [Description("按票收费（特殊规则）")]
        VoteSpecialPayment = 7,
        /// <summary>
        /// 减免金额
        /// </summary>
        [Description("减免金额")]
        SpecialTimePeriodPayment = 8,
        /// <summary>
        /// 减免时间
        /// </summary>
        [Description("减免时间")]
        ReliefTime = 9,
        /// <summary>
        /// 按天减免
        /// </summary>
        [Description("按天减免")]
        DayFree=10

    }
}
