using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.JNAPI
{
    public class JNParkIORecord
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 时间
        /// </summary>
        public string time { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 微卡口编号
        /// </summary>
        public string dev { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        /// <summary>
        /// 通道名称
        /// </summary>
        public string channel { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        /// <summary>
        /// 入场|出场
        /// </summary>
        public string direction { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        /// <summary>
        /// 车牌号
        /// </summary>
        public string plate { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        /// <summary>
        /// 车牌颜色
        /// </summary>
        public string pcolor { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 车辆颜色
        /// </summary>
        public string color { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        /// <summary>
        /// 车辆品牌
        /// </summary>
        public string brand { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        /// <summary>
        /// 车型
        /// </summary>
        public string type { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string pagecount { set; get; }
    }
}
