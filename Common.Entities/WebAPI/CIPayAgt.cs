using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;

namespace Common.Entities.WebAPI
{
    public class CIPayAgt
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        public string PlateNumber { set; get; }

        /// <summary>
        /// 车场ID
        /// </summary>
        public string PKID { set; get; }

        /// <summary>
        /// 订单信息
        /// </summary>
        public CIOrder Pkorder { set; get; }

        /// <summary>
        /// 线上订单编号
        /// </summary>
        public string OnlineOrderID { set; get; }

        /// <summary>
        /// 车场描述
        /// </summary>
        public string PKName { set; get; }

        /// <summary>
        /// 进场时间
        /// </summary>
        public string EntranceTime { set; get; }

        /// <summary>
        /// 剩余出场时间(分钟)
        /// </summary>
        public int OutTime { set; get; }

        /// <summary>
        /// 最后缴费时间(yyyyMMddHHmmss)
        /// </summary>
        public string PayDate { set; get; }

        /// <summary>
        /// 结果描述
        /// </summary>
        public APPResult Result { set; get; }
    }
}
