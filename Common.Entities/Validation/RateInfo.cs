using System;

namespace Common.Entities.Validation
{
    public class RateInfo
    {
        /// <summary>
        /// 应付金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 还需支付金额
        /// </summary>
        public decimal UnPayAmount { get; set; }

        /// <summary>
        /// 线上缴费金额
        /// </summary>
        public decimal OnlinePayAmount { get; set; }

        /// <summary>
        /// 储值卡余额
        /// </summary>
        public decimal CardTransationsBalance { get; set; }

        /// <summary>
        /// 储值卡最终余额（余额-为上传订单）
        /// </summary>
        public decimal AccountSurplus { get; set; }

        /// <summary>
        /// 储值卡扣除金额
        /// </summary>
        public decimal CardTransactionsAmount { get; set; }
        /// <summary>
        /// 储值卡本次余额
        /// </summary>
        public decimal AvailableTotal
        {
            get
            {
                return (AccountSurplus - CardTransactionsAmount) < 0 ? 0 : (AccountSurplus - CardTransactionsAmount);
            }
        }

        /// <summary>
        /// 折扣金额 减免金额 应该比商家订单金额大
        /// </summary>
        public decimal DiscountAmount { get; set; }


        /// <summary>
        /// 结余金额
        /// </summary>
        public decimal CashMoney { get; set; }
        /// <summary>
        /// 结余时间
        /// </summary>
        public DateTime CashTime { get; set; }

        /// <summary>
        ///支付方式
        /// </summary>
        public OrderPayWay OrderPayWay = OrderPayWay.Cash;
    }
}
