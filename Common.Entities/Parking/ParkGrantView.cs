using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Parking
{
    public class ParkGrantView
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int ID { get; set; }
        /// <summary>
        /// 授权ID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string GID{get;set;}
        /// <summary>
        /// 卡ID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CardID{get;set;}
        /// <summary>
        /// 车场ID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PKID{get;set;}
        /// <summary>
        /// 开始时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime BeginDate{get;set;}
        /// <summary>
        /// 结束时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime EndDate{get;set;}
        /// <summary>
        /// 车类型ID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CarTypeID{get;set;}

         [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int PKLotNum { set; get; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CarModelID{get;set;}
        /// <summary>
        /// 车位号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PKLot{get;set;}
        /// <summary>
        /// 车牌ID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PlateID{get;set;}
        /// <summary>
        /// 车牌号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PlateNo { get; set; }
        /// <summary>
        /// 区域权限(空为所有)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AreaIDS{get;set;}
        /// <summary>
        /// 通道权限(空为所有)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string GateID{get;set;}
        /// <summary>
        /// 状态(0正常，1停止，2暂停)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ParkGrantState State{get;set;}
        /// <summary>
        /// 人员姓名
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EmployeeName { get; set; }
        /// <summary>
        /// 人员编号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EmployeeID { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MobilePhone { get; set; }
        /// <summary>
        /// 家庭电话
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string HomePhone { get; set; }
        /// <summary>
        /// 卡号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CardNo { get; set; }
        /// <summary>
        /// 卡编号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CardNumber { get; set; }
        /// <summary>
        /// 余额
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal Balance { get; set; }
        /// <summary>
        /// 卡类型
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BaseCarType BaseCarType { get; set; }
        /// <summary>
        /// 车牌颜色
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PlateColor Color { get; set; }
        /// <summary>
        /// 获取转临停
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public OverdueToTemp OverdueToTemp { get; set; }
        /// <summary>
        /// 月卡过期允许进入天数
        /// </summary>
        public int MonthCardExpiredEnterDay { get; set; }
        /// <summary>
        /// 家庭地址
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string FamilyAddr { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Remark { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string CertifNo { set; get; }


        /// <summary>
        /// 是否即将到期
        /// </summary>
        public bool Due { get; set; }
        /// <summary>
        /// 线上续期按月数
        /// </summary>
        public int OnlineUnit { get; set; }
    }
}
