using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.NoninductivePay
{
    public class NoninductivePayOutModel
    {
        /// <summary>
        /// 状态码  0000: 成功    其它失败
        /// </summary>
        public string Status
        {
            get;
            set;
        }
        /// <summary>
        /// 失败或成功信息
        /// </summary>
        public string Msg
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
        /// 车牌号码
        /// </summary>
        public string PlateNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 线上订单编号
        /// </summary>
        public string OnLineOrderID
        {
            get;
            set;
        }
    }
}
