using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.NoninductivePay
{
    /// <summary>
    /// 无感输入参数
    /// </summary>
    public class NoninductivePayInModel
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
        /// 车场编号
        /// </summary>
        public string PKID
        {
            get;
            set;
        }
        /// <summary>
        /// 进场时间
        /// </summary>
        public string EntranceTime
        {
            get;
            set;
        }
        /// <summary>
        /// 出场时间
        /// </summary>
        public string ExitTime
        {
            get;
            set;
        }
        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal Amount
        {
            get;
            set;
        }
        /// <summary>
        /// 出通道编号
        /// </summary>
        public string OutGateID
        {
            get;
            set;
        }
        /// <summary>
        /// 进通道编号
        /// </summary>
        public string InGateID
        {
            get;
            set;
        }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderID
        {
            get;
            set;
        }
        /// <summary>
        /// 未付金额
        /// </summary>
        public decimal UnPayAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 折扣金额
        /// </summary>
        public decimal CouponAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 无感支付类型
        /// </summary>
        public int NoninductivePayType
        {
            get;
            set;
        }
        /// <summary>
        /// 接口名称
        /// </summary>
        public string MethodName
        {
            get;
            set;
        }
        /// <summary>
        /// 进出记录编号
        /// </summary>
        public string RecordID
        {
            get;
            set;
        }
    }
}
