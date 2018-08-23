using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Parking
{
    public class ParkSettlementDetailsModel
    {
        /// <summary>
        /// 结算单明细编号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DetailsCode
        {
            get;
            set;
        }
        /// <summary>
        /// 结算单编号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SettlementCode
        {
            get;
            set;
        }
        /// <summary>
        /// 订单编号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OrderNo
        {
            get;
            set;
        }
        /// <summary>
        /// 订单类型
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int OrderType
        {
            get;
            set;
        }
        /// <summary>
        /// 订单时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime OrderTime
        {
            get;
            set;
        }
        /// <summary>
        /// 订单金额
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal Amount
        {
            get;
            set;
        }
        /// <summary>
        /// 已付款金额
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal PayAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 未付款金额
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal OutStandingAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 折扣金额
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal DiscountAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 创建时间 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime CreateTime
        {
            get;
            set;
        }
    }
}
