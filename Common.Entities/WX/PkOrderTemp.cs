using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WX
{
    public class PkOrderTemp
    {
        /// <summary>
        /// 线上订单编号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OnlineOrderNo { set; get; }

        /// <summary>
        /// 车场名称
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PKName { set; get; }

        /// <summary>
        /// 车牌号码
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PlateNumber { set; get; }

        /// <summary>
        /// 入场时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime EntranceDate { set; get; }

        /// <summary>
        /// 停车时长
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int MinuteNum { set; get; }

        /// <summary>
        /// 缴费金额
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal Amount { set; get; }

        /// <summary>
        /// 缴费时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime PayTime { set; get; }
    }
}
