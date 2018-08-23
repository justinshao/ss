using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ClassLibrary1
{
    public class ParkingDetail
    {
        /// <summary>
        /// Status
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Result
        /// </summary>
        public FHResult Result { get; set; }
    }

    public class FHResult
    {
        /// <summary>
        /// 车牌
        /// </summary>
        public string LicensePlate { get; set; }
        /// <summary>
        /// 车场名
        /// </summary>
        public string ParkingName { get; set; }
        /// <summary>
        /// 预约时间
        /// </summary>
        public string AppointmentTime { get; set; }
        /// <summary>
        /// 预计到达时间
        /// </summary>
        public string ArriveTime { get; set; }
        /// <summary>
        /// 预约时长 
        /// </summary>
        public string AppointmentLength { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string LockNo { get; set; }
        /// <summary>
        /// 驶入时间
        /// </summary>
        public DateTime IntoTime { get; set; }
        /// <summary>
        /// 驶出时间
        /// </summary>
        public DateTime OutTime { get; set; }
        /// <summary>
        /// 停车时长
        /// </summary>
        public string ParkingTime { get; set; }
        /// <summary>
        /// 积分奖励 
        /// </summary>
        public int Integral { get; set; }
        /// <summary>
        /// 停车类型(0临时停车1预约停车)
        /// </summary>
        public int ParkType { get; set; }
        /// <summary>
        /// 订单流水号 
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 预约费用
        /// </summary>
        public decimal AppointmentPrice { get; set; }
        /// <summary>
        /// 停车费用
        /// </summary>
        public decimal TotalPrice { get; set; }
        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal Cut { get; set; }
        /// <summary>
        /// 实际支付金额
        /// </summary>
        public decimal OnlinePrice { get; set; }
        /// <summary>
        /// 0 支付宝 1 微信 2 银联 3月卡 4平台帐户 5一网通
        /// </summary>
        public int PayType { get; set; }
        /// <summary>
        /// Longitude
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// Latitude
        /// </summary>
        public double Latitude { get; set; }
    }

   
}
