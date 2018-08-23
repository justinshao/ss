using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Device
{
    public class IDCard
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 籍贯
        /// </summary>
        public string Nation { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 年
        /// </summary>
        public string Birthday { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IDNum { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 发证机关
        /// </summary>
        public string Issue { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 有效起始时间
        /// </summary>
        public string BeginDate { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 有效结束时间
        /// </summary>
        public string EndDate { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
         
        /// <summary>
        /// 照片
        /// </summary>
        public byte[] ImageData { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 照片
        /// </summary>
        public byte[] FaceData { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 验证结果
        /// </summary>
        public int ScoreRet { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 相似度
        /// </summary>
        public int Score { get; set; }
    }
}
