using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Statistics
{
    public class ReportPoundNoteModel
    {
        /// <summary>
        /// 车牌号码
        /// </summary>
        public string PlateNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 车场名称
        /// </summary>
        public string PKName
        {
            get;
            set;
        }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount
        {
            get;
            set;
        }
        /// <summary>
        /// 已付金额
        /// </summary>
        public decimal PayAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 未付金额
        /// </summary>
        public decimal OutStandingAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 折扣金额
        /// </summary>
        public decimal DiscountAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 订单时间
        /// </summary>
        public DateTime OrderTime
        {
            get;
            set;
        }
        /// <summary>
        /// 订单时间
        /// </summary>
        public string OrderTimeName
        {
            get;
            set;
        }
        /// <summary>
        /// 支付方式
        /// </summary>
        public int PayWay
        {
            get;
            set;
        }
        /// <summary>
        /// 支付方式名称
        /// </summary>
        public string PayWayName
        {
            get;
            set;
        }
        /// <summary>
        /// 进场时间
        /// </summary>
        public DateTime EntranceTime
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string EntranceTimeName
        {
            get;
            set;
        }
        public DateTime ExitTime
        {
            get;
            set;
        }
        public string ExitTimeName
        {
            get;
            set;
        }
        /// <summary>
        /// 停车时长
        /// </summary>
        public string LongTime
        {
            get;
            set;
        }
        /// <summary>
        /// 收费员
        /// </summary>
        public string Operator
        {
            get;
            set;
        }
        public string ZZWeight
        {
            get;
            set;
        }
        public string NetWeight
        {
            get;
            set;
        }
        /// <summary>
        /// 皮重
        /// </summary>
        public string Tare
        {
            get;
            set;
        }
        /// <summary>
        /// 物品
        /// </summary>
        public string Goods
        {
            get;
            set;
        }
        /// <summary>
        /// 货主
        /// </summary>
        public string Shipper
        {
            get;
            set;
        }
        /// <summary>
        /// 仓位号
        /// </summary>
        public string Shippingspace
        {
            get;
            set;
        }
    }
}
