using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Condition
{
    public class ParkGrantCondition
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 车场编号
        /// </summary>
        public string ParkingId { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 业主电话或姓名
        /// </summary>
        public string EmployeeNameOrMoblie { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 车牌号
        /// </summary>
        public string PlateNumber { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 卡号
        /// </summary>
        public string CardNo { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 车型
        /// </summary>
        public string CarModelId { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 车类
        /// </summary>
        public string CarTypeId { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int? State { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 车位号
        /// </summary>
        public string ParkingLot { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 是否仅查询有车位数据
        /// </summary>
        public bool QueryHasParkingLot { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 地址
        /// </summary>
        public string HomeAddress { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 即将到期
        /// </summary>
        public bool Due { get; set; }
    }
}
