using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WX
{
    public class ParkUserCarInfo
    {
        /// <summary>
        /// 卡片ID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CardID { set; get; }

        /// <summary>
        /// 车牌号码
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PlateNumber { set; get; }

        /// <summary>
        /// 开始有效期
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime BeginDate { set; get; }

        /// <summary>
        /// 结束日期
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime EndDate { set; get; }

        /// <summary>
        /// 余额
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal Balance { set; get; }

        /// <summary>
        /// 车类型
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CarTypeID { set; get; }

        /// <summary>
        /// 车场ID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PKID { set; get; }

        /// <summary>
        /// 车类型基础类型
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BaseTypeID { set; get; }

        /// <summary>
        /// 允许线上缴费
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool IsAllowOnlIne { set; get; }

        /// <summary>
        /// 续费月金额
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal Amount { set; get; }

        /// <summary>
        /// 线上续费最大月数
        /// </summary>
        public int MaxMonth { set; get; }

        /// <summary>
        /// 线上充值最大金额
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal MaxValue { set; get; }

        /// <summary>
        /// 车场描述
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PKName { set; get; }

        /// <summary>
        /// 小区ID
        /// </summary>
        public string VID { set; get; }

        /// <summary>
        /// 状态(0正常，1停止，2暂停)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int State { set; get; }

        /// <summary>
        /// 车场是否支持手机缴费
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int MobilePay { set; get; }

        /// <summary>
        /// 车场是否支持手机锁车
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int MobileLock { set; get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int OnlineUnit { set; get; }

    }
}
