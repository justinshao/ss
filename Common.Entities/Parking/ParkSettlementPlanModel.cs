using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Parking
{
    public class ParkSettlementPlanModel
    {
        /// <summary>
        /// 结算计划编号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PlanCode
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
        public string PKName
        {
            get;
            set;
        }
        /// <summary>
        /// 状态 0: 停用  1:启用
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int Status
        {
            get;
            set;
        }

        /// <summary>
        /// 结算天数 -1:自然月   否则为实际天数
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int SettlementDays
        {
            get;
            set;
        }

        /// <summary>
        /// 下次结算时的开始时间 每次生成结算单后都需要更新此时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime StartTime
        {
            get;
            set;
        }
        /// <summary>
        /// 联系人
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Contact
        {
            get;
            set;
        }
        /// <summary>
        /// 联系电话
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Tel
        {
            get;
            set;
        }
        /// <summary>
        /// 电子邮件
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Mail
        {
            get;
            set;
        }
        /// <summary>
        /// 结算银行帐号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SettlementAccount
        {
            get;
            set;
        }
        /// <summary>
        /// 结算帐号名
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SettlementAccountName
        {
            get;
            set;
        }
        /// <summary>
        /// 结算银行
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SettlementBank
        {
            get;
            set;
        }
    }
}
