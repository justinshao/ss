using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WX
{
    public class TempStopPaymentResult
    {

        /// <summary>
        /// 0支付成功
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public APPResult Result { set; get; }

        /// <summary>
        /// 支付时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime PayDate { set; get; }

        /// <summary>
        /// 线上订单ID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OnlineOrderID { set; get; }
    }
}
