using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Parking
{
    /// <summary>
    /// 结算单
    /// </summary>
    public class ParkSettlementModel
    {
        /// <summary>
        /// 记录编号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string RecordID
        {
            get;
            set;
        }
        /// <summary>
        /// 车场编号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PKID
        {
            get;
            set;
        }
        /// <summary>
        /// 车场名称
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ParkName
        {
            get;
            set;
        }
        /// <summary>
        /// 帐期名称
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Priod
        {
            get;
            set;
        }
        /// <summary>
        /// 结算状态
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int SettleStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 结算状态名称
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int SettleStatusName
        {
            get;
            set;
        }
        /// <summary>
        /// 结算总额(元)
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
        public decimal HandlingFeeAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 应结帐款
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal ReceivableAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 开始时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime StartTime
        {
            get;
            set;
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime EndTime
        {
            get;
            set;
        }
        /// <summary>
        /// 付款时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime PayTime
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
        /// <summary>
        /// 创建人
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CreateUser
        {
            get;
            set;
        }
        /// <summary>
        /// 开始时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string StartTimeName
        {
            get;
            set;
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EndTimeName
        {
            get;
            set;
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EndTimeName1
        {
            get;
            set;
        }
        /// <summary>
        /// 付款时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PayTimeName
        {
            get;
            set;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CreateTimeName
        {
            get;
            set;
        }
        /// <summary>
        /// 转款回执
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Receipt
        {
            get;
            set;
        }
        /// <summary>
        /// 隐藏
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool IsHide
        {
            get;
            set;
        }
        /// <summary>
        /// 用户名
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UserName
        {
            get;
            set;
        }
        /// <summary>
        /// 备注
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Remark
        {
            get;
            set;
        }
    }
}
