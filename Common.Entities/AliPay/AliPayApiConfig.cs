using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.AliPay
{
    public class AliPayApiConfig
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int ID { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string RecordId { get; set; }
        /// <summary>
        /// 系统名称
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SystemName { get; set; }
        /// <summary>
        /// 系统域名
        /// </summary>
        private string _systemdomain;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SystemDomain {
            set { _systemdomain = value; }
            get
            {
                return SystemDefaultConfig.SystemDomain;
            }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 支付宝APPID
        /// </summary>
        public string AppId { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 公钥
        /// </summary>
        public string PublicKey { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 私钥
        /// </summary>
        public string PrivateKey { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 收款账号
        /// </summary>
        public string PayeeAccount { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 单位编号
        /// </summary>
        public string CompanyID { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool Status { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 支持上级账号支付
        /// </summary>
        public bool SupportSuperiorPay { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 签名类型 0-RSA 1-RSA2
        /// </summary>
        public int AliPaySignType { get; set; }
    }
}
