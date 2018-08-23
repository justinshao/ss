using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Parking
{
    public class ParkDerateConfig
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int ID { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string RecordID { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 所属车场
        /// </summary>
        public string PKID { get; set; }
        /// <summary>
        /// 消费开始金额
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal ConsumeStartAmount { get; set; }
        /// <summary>
        /// 消费结束金额
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal ConsumeEndAmount { get; set; }
        /// <summary>
        /// 优免类型 0-按折扣 1-按金额 2-按时间（分钟）
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int DerateType { get; set; }
        /// <summary>
        /// 优免值 按折扣（0到1） 按金额（具体的金额 保留1位小数） 按时间（分钟）
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal DerateValue { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime LastUpdateTime { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int HaveUpdate { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DataStatus DataStatus { get; set; }
    }
}
