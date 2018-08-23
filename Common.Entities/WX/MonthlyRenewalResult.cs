using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Parking;
using Newtonsoft.Json;
using System.ComponentModel;

namespace Common.Entities.WX
{
    public class MonthlyRenewalResult
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PlateNumber { set; get; }

        /// <summary>
        /// 车场ID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PKID { set; get; }

        /// <summary>
        /// 订单信息
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ParkOrder Pkorder { set; get; }

        /// <summary>
        /// 线上订单编号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OnlineOrderID { set; get; }

        /// <summary>
        /// 结果描述
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public APPResult Result { set; get; }
    }

    public enum APPResult
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Normal = 0,
        /// <summary>
        /// 找不到入场记录
        /// </summary>
          [Description("找不到入场记录")]
        NotFindIn = 1,
        /// <summary>
        /// 不需要缴费
        /// </summary>
        [Description("不需要缴费")]
        NoNeedPay = 2,
        /// <summary>
        /// 不支持手机缴费
        /// </summary>
       [Description("不支持手机缴费")]
        NotSupportedPay = 3,
        /// <summary>
        /// 代理网络异常
        /// </summary>
        [Description("代理网络异常")]
        ProxyException = 4,
        /// <summary>
        /// 其他异常
        /// </summary>
          [Description("其他异常")]
        OtherException = 5,
        /// <summary>
        /// 非临时卡
        /// </summary>
         [Description("非临时卡")]
        NoTempCard = 6,
        /// <summary>
        /// 重复缴费
        /// </summary>
        [Description("重复缴费")]
        RepeatPay = 10,
        /// <summary>
        /// 金额不对
        /// </summary>
         [Description("金额不对")]
        AmountIsNot = 11,
        /// <summary>
        /// 找不到卡片
        /// </summary>
            [Description("找不到卡片")]
        NotFindCard = 7,

        /// <summary>
        /// 订单失效
        /// </summary>
           [Description("订单失效")]
        OrderSX = 12,

        /// <summary>
        /// 找不到岗亭信息
        /// </summary>
         [Description("找不到岗亭信息")]
        NoBox=13,

        /// <summary>
        /// 无车辆在岗亭
        /// </summary>
        [Description("无车辆在岗亭")]
        NoCarInBox=14,

        /// <summary>
        /// 人工缴费
        /// </summary>
         [Description("人工缴费")]
        ManualPay=15,

        /// <summary>
        /// 入口非无牌车扫码
        /// </summary>
       [Description("入口非无牌车扫码")]
        NotIn=16,

        /// <summary>
        /// 无牌车进出
        /// </summary>
         [Description("无牌车进出")]
        NoLp=17,
    }
}
