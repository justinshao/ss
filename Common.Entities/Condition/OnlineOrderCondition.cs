using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Enum;
using Newtonsoft.Json;

namespace Common.Entities.Condition
{
    public class OnlineOrderCondition
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OrderId { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Start { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime End { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PlateNo { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PaymentChannel PaymentChannel { get; set; }
        public OnlineOrderStatus? Status { get; set; }
        public OnlineOrderType? Ordertype { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CompanyId { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ParkingId { get; set; }
        /// <summary>
        /// 外部车场编号
        /// </summary>
        public string ExternalPKID { get; set; }
    }
}
