using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Statistics
{
    /// <summary>
    /// 统计基础信息
    /// </summary>
    public class Statistics_Base
    {
        /// <summary>
        /// 应收金额
        /// </summary>
        public decimal Receivable_Amount
        {
            get;
            set;
        }
        /// <summary>
        /// 实收金额
        /// </summary>
        public decimal Real_Amount
        {
            get;
            set;
        }
        /// <summary>
        /// 差异金额
        /// </summary>
        public decimal Diff_Amount
        {
            get;
            set;
        }
        /// <summary>
        /// 现金收费
        /// </summary>
        public decimal Cash_Amount
        {
            get;
            set;
        }
        /// <summary>
        /// 现金收费次数
        /// </summary>
        public int Cash_Count
        {
            get;
            set;
        }
        /// <summary>
        /// 线下临停缴费金额
        /// </summary>
        public decimal Temp_Amount
        {
            get;
            set;
        }
        /// <summary>
        /// 线下临停缴费次数
        /// </summary>
        public int Temp_Count
        {
            get;
            set;
        }
        /// <summary>
        /// 线下临停缴费金额
        /// </summary>
        public decimal OnLineTemp_Amount
        {
            get;
            set;
        }
        /// <summary>
        /// 线上临停缴费次数
        /// </summary>
        public decimal OnLineTemp_Count
        {
            get;
            set;
        }
        /// <summary>
        /// 储值卡扣费金额
        /// </summary>
        public decimal StordCard_Amount
        {
            get;
            set;
        }
        /// <summary>
        /// 储值卡扣费次数
        /// </summary>
        public int StordCard_Count
        {
            get;
            set;
        }
        /// <summary>
        /// 线上缴费金额
        /// </summary>
        public decimal OnLine_Amount
        {
            get;
            set;
        }
        /// <summary>
        /// 线上缴费次数
        /// </summary>
        public int OnLine_Count
        {
            get;
            set;
        }
        /// <summary>
        /// 优免抵扣金额
        /// </summary>
        public decimal Discount_Amount
        {
            get;
            set;
        }
        /// <summary>
        /// 优免抵扣次数
        /// </summary>
        public int Discount_Count
        {
            get;
            set;
        }
        /// <summary>
        /// 正常放行次数
        /// </summary>
        public int ReleaseType_Normal
        {
            get;
            set;
        }
        /// <summary>
        /// 收费放行次数
        /// </summary>
        public int ReleaseType_Charge
        {
            get;
            set;
        }
        /// <summary>
        /// 免费放行次数
        /// </summary>
        public int ReleaseType_Free
        {
            get;
            set;
        }
        /// <summary>
        /// 异常放行次数
        /// </summary>
        public int ReleaseType_Catch
        {
            get;
            set;
        }
        /// <summary>
        /// VIP续期数
        /// </summary>
        public int VIPExtend_Count
        {
            get;
            set;
        }
        /// <summary>
        /// 进场数
        /// </summary>
        public int Entrance_Count
        {
            get;
            set;
        }
        /// <summary>
        /// 出场数
        /// </summary>
        public int Exit_Count
        {
            get;
            set;
        }
        /// <summary>
        /// VIP卡进场数
        /// </summary>
        public int VIPCard
        {
            get;
            set;
        }
        /// <summary>
        /// 储值卡进场数
        /// </summary>
        public int StordCard
        {
            get;
            set;
        }
        /// <summary>
        /// 月卡进场数
        /// </summary>
        public int MonthCard
        {
            get;
            set;
        }
        /// <summary>
        /// 工作卡进场数
        /// </summary>
        public int JobCard
        {
            get;
            set;
        }
        /// <summary>
        /// 临时卡进场数
        /// </summary>
        public int TempCard
        {
            get;
            set;
        }
        /// <summary>
        /// 线上月卡续期次数
        /// </summary>
        public int OnLineMonthCardExtend_Count
        {
            get;
            set;
        }
        /// <summary>
        /// 线上月卡续期金额
        /// </summary>
        public decimal OnLineMonthCardExtend_Amount
        {
            get;
            set;
        }
        /// <summary>
        /// 线下月卡续期次数
        /// </summary>
        public int MonthCardExtend_Count
        {
            get;
            set;
        }
        /// <summary>
        /// 线下月卡续期金额
        /// </summary>
        public decimal MonthCardExtend_Amount
        {
            get;
            set;
        }
        /// <summary>
        /// 线上储值卡充值次数
        /// </summary>
        public int OnLineStordCard_Count
        {
            get;
            set;
        }
        /// <summary>
        /// 线上储值卡充值金额
        /// </summary>
        public decimal OnLineStordCard_Amount
        {
            get;
            set;
        }
        /// <summary>
        /// 线下储值卡充值次数
        /// </summary>
        public int StordCardRecharge_Count
        {
            get;
            set;
        }
        /// <summary>
        /// 线下储值卡充值金额
        /// </summary>
        public decimal StordCardRecharge_Amount
        {
            get;
            set;
        }
        /// <summary>
        /// 现金收费方式折扣金额
        /// </summary>
        public decimal CashDiscount_Amount
        {
            get;
            set;
        }
        /// <summary>
        /// 现金收费方式折扣次数
        /// </summary>
        public int CashDiscount_Count
        {
            get;
            set;
        }
        /// <summary>
        /// 手机收费方式折扣金额
        /// </summary>
        public decimal OnLineDiscount_Amount
        {
            get;
            set;
        }
        /// <summary>
        /// 手机收费方式折扣次数
        /// </summary>
        public int OnLineDiscount_Count
        {
            get;
            set;
        }
        /// <summary>
        /// 是否已更新
        /// </summary>
        public int HaveUpdate
        {
            get;
            set;
        }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime LastUpdateTime
        {
            get;
            set;
        }
    }
}
