using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Other
{
    public class CalSettleAmountModel
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool IsSuccess
        {
            get;
            set;
        }
        /// <summary>
        /// 失败时信息 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message
        {
            get;
            set;
        }
        /// <summary>
        /// 总金额
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal TotalAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 手续费
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal RateFeeAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 应结算金额
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal ReceiveAmount
        {
            get;
            set;
        }
    }
}
