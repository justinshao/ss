using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Parking
{
    public class ParkSettlementApprovalFlowModel
    {
        /// <summary>
        /// 自增编号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int ID
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
            set;
            get;
        }
        /// <summary>
        /// 审批状态值
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int FlowID
        {
            get;
            set;
        }
        /// <summary>
        /// 审批状态名称
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string FlowName
        {
            get;
            set;
        }
        /// <summary>
        /// 审批人
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string FlowUser
        {
            get;
            set;
        }
        /// <summary>
        /// 审批人编号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string FlowUserName
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
