using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WX
{
    public class AdvanceParking
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int ID { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 订单编号
        /// </summary>
        public decimal OrderId { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 车牌号
        /// </summary>
        public string PlateNo { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime StartTime { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string StartTimeToString
        {
            get
            {
                return StartTime.ToyyyyMMddHHmmss();
            }
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime EndTime { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EndTimeToString
        {
            get
            {
                return EndTime.ToyyyyMMddHHmmss();
            }
        }
        /// <summary>
        /// 支付金额
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal Amount { get; set; }
        /// <summary>
        /// 支付人
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string WxOpenId { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? PayTime { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PayTimeToString
        {
            get
            {
                if (PayTime != null)
                {
                    return PayTime.ToString();
                }
                else
                {
                    return DateTime.MinValue.ToString();
                }
            }
        }
        /// <summary>
        /// 订单状态
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int OrderState { get; set; }
        /// <summary>
        /// 预支付编号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PrepayId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SerialNumber { get; set; }
        /// <summary>
        /// 单位编号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CompanyID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime CreateTime { get; set; }
        public string CreateTimeToString
        {
            get
            {
                return CreateTime.ToyyyyMMddHHmmss();
            }
        }
    }
}
