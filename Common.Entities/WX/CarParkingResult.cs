using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WX
{
    public class CarParkingResult
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PlateNo { get; set; }
        /// <summary>
        /// 车场名称
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ParkingName { get; set; }
        /// <summary>
        /// 小区名称
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string VillageName { get; set; }
        /// <summary>
        /// 车位号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ParkLot { get; set; }
        /// <summary>
        /// 车类型
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CarTypeName { get; set; }
        /// <summary>
        /// 有效开始时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 有效结束时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime EndTime { get; set; }
    }
}
