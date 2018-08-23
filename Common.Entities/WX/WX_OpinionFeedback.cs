using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WX
{
    /// <summary>
    /// 意见反馈
    /// </summary>
    public class WX_OpinionFeedback
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OpenId { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string FeedbackContent { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime CreateTime { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CompanyID { get; set; }
    }
}
