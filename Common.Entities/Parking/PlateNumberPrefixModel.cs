using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Parking
{
    public class PlateNumberPrefixModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 车场名称
        /// </summary>
        public string ParkingName
        {
            get;
            set;
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 车牌前缀
        /// </summary>
        public string PlateNumberPrefix
        {
            get;
            set;
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 数量
        /// </summary>
        public int Number
        {
            get;
            set;
        }
        /// <summary>
        /// 占比
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Rate
        {
            get;
            set;
        }
    }
}
