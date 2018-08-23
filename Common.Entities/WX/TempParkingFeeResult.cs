using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Parking;
using Newtonsoft.Json;

namespace Common.Entities.WX
{
    public class TempParkingFeeResult
    {
        public TempParkingFeeResult()
        {
            EntranceDate = DateTime.Now;

        }
        /// <summary>
        /// 车牌号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PlateNumber { set; get; }

        /// <summary>
        /// 卡片编号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CardNo { set; get; }

        /// <summary>
        /// 车场ID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ParkingID { set; get; }

        /// <summary>
        /// 车场名称
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ParkName { set; get; }


        /// <summary>
        /// 进时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime EntranceDate { set; get; }

        /// <summary>
        /// 订单明细
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ParkOrder Pkorder { set; get; }

        /// <summary>
        /// 剩余出场时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int OutTime { set; get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool isAdd { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 结果描述0正常,1找不到入场记录,2不需要缴费,3不支持手机缴费,4代理网络断开，5其他异常,6,非临停卡，10已缴费
        /// </summary>
        public APPResult Result { set; get; }

        /// <summary>
        /// 最后缴费时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime PayDate { set; get; }
        /// <summary>
        /// 订单来源
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PayOrderSource OrderSource{set;get;}
        /// <summary>
        /// 外部车场接口编号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ExternalPKID { get; set; }
        /// <summary>
        /// 错误描述
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorDesc { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ImageUrl { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string GateID { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BoxID { set; get; }

    }
    public enum PayOrderSource {
        Platform=0,
        BWY=1,
        SFM=2,
    }
}
