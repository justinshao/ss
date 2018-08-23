using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Services.BaiDu
{
    public class BaiDuParkingDetail
    {        /// <summary>
        /// 车场名称
        /// </summary>
        public string ParkName { get; set; }
        /// <summary>
        /// 车场地址
        /// </summary>
        public string ParkAddress { get; set; }
        /// <summary>
        /// 车场图片
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// 总车位数
        /// </summary>
        public string TotalParkQuantity { get; set; }
        /// <summary>
        /// 剩余车位数
        /// </summary>
        public string SurplusParkQuantity { get; set; }
        /// <summary>
        /// 车场类型
        /// </summary>
        public string ParkType { get; set; }
        /// <summary>
        /// 价格信息
        /// </summary>
        public string PriceInfo { get; set; }
        /// <summary>
        /// 车场经度
        /// </summary>
        public string ParkLongitude { get; set; }
        /// <summary>
        /// 车场纬度
        /// </summary>
        public string ParkLatitude { get; set; }
        /// <summary>
        /// 当前所在经度
        /// </summary>
        public string CurrLongitude { get; set; }
        /// <summary>
        /// 当前所在车场纬度
        /// </summary>
        public string CurrLatitude { get; set; }
        /// <summary>
        /// 距离描述
        /// </summary>
        public string DistanceDes { get; set; }
    }
}