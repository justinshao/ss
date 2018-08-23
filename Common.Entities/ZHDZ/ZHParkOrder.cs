using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.ZHDZ
{
    public class ZHParkOrder
    {
        /// <summary>
        /// 预约记录ID
        /// </summary>
        public string ReserveID { set; get; }

        /// <summary>
        /// 线下订单编号
        /// </summary>
        public string OrderID { set; get; }

        /// <summary>
        /// 预约金额
        /// </summary>
        public decimal Amount { set; get; }

        /// <summary>
        /// 车场ID
        /// </summary>
        public string PKID { set; get; }
    }
}
