using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Parking;
using Common.Entities.ZHDZ;
using Newtonsoft.Json;

namespace Common.Entities.WX
{
    public class WXReserveBitResult
    {
        /// <summary>
        /// 结果0成功、1重复预约、-1失败
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int code { set; get; }
        /// <summary>
        /// 失败信息
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string message { set; get; }
        /// <summary>
        /// 车位号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BitNo { set; get; }
        /// <summary>
        /// 订单
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ZHParkOrder Order { set; get; }
    }
}
