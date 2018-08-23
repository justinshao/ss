using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.ExternalInteractions.SFM
{
    public class SFMResult
    {
        /// <summary>
        /// 返回状态
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 代码200为正确  400为获取价格失败
        /// </summary>
        public string Code { get; set; }
    }
    public class PlateQueryResult : SFMResult
    {
        public PlateNumberInfo Data { get; set; }
    }
    public class PlateNumberInfo
    {
        /// <summary>
        /// 停车场的编号
        /// </summary>
        public string parkKey { get; set; }
        /// <summary>
        /// 车牌号码
        /// </summary>
        public string carNo { get; set; }
        /// <summary>
        /// 锁车状态 0或者null为没有锁车  1为车辆锁定
        /// </summary>
        public string carLock { get; set; }
        /// <summary>
        /// 内部停车订单号
        /// </summary>
        public string orderNo { get; set; }
        /// <summary>
        /// 收费类型 :3651 临时车 代码:3652 月租车A 代码:3653 月租车B 代码:3654 月租车C 代码:3655 月租车D
        /// </summary>
        public string CarType { get; set; }
        /// <summary>
        /// 入场时间
        /// </summary>
        public string enterTime { get; set; }
        /// <summary>
        /// 入口车道
        /// </summary>
        public string enterGateName { get; set; }
        /// <summary>
        /// 入口车道执勤操作员
        /// </summary>
        public string enterOperatorName { get; set; }
        public string enterImg { get; set; }
        /// <summary>
        /// 停车场免费分钟 停车场固定的设置可以不显示
        /// </summary>
        public int? freeTime { get; set; }
        /// <summary>
        /// 停车场超时免费分钟  停车场固定的设置可以不显示
        /// </summary>
        public int? freeTimeout { get; set; }
        /// <summary>
        /// 应缴费用
        /// </summary>
        public decimal? totalAmount { get; set; }
        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal? couponAmount { get; set; }
        /// <summary>
        /// 实际应付
        /// </summary>
        public decimal? payAmount { get; set; }
    }
    public class SFMParkInfoResult : SFMResult
    {
        public SFMParkInfoData Data { get; set; }
    }

    public class SFMParkInfoData
    {
        public bool AllowPaging { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalRecord { get; set; }
        public int TotalPage { get; set; }
        public List<SFMParkInfo> ResultList { get; set; }
    }
    public class SFMParkInfo
    {
        public string parkKey { get; set; }
        public string parkName { get; set; }
        /// <summary>
        /// 停车场岗亭列表
        /// </summary>
        public List<ParkSentry> parkSentry { get; set; }
    }
    public class ParkSentry
    {
        /// <summary>
        /// 岗亭编号
        /// </summary>
        public string sentryNo { get; set; }
        /// <summary>
        /// 岗亭名称
        /// </summary>
        public string sentryName { get; set; }
        /// <summary>
        /// 停车场车道列表
        /// </summary>
        public List<ParkLane> parkLane { get; set; }
    }
    public class ParkLane
    {
        /// <summary>
        /// 车道编号
        /// </summary>
        public string vehicleNo { get; set; }
        /// <summary>
        /// 车道类型(0为入口车道 1 为出口车道)
        /// </summary>
        public string vehicleType { get; set; }
        /// <summary>
        /// 车道名称
        /// </summary>
        public string vehicleName { get; set; }
    }
    public class OutCarInfoResult : SFMResult
    {
        public SMFOutCarInfo Data { get; set; }
    }
    public class SMFOutCarInfo
    {
        /// <summary>
        /// 车场编号
        /// </summary>
        public string Parking_Key { get; set; }
        /// <summary>
        /// 车场订单号
        /// </summary>
        public string ParkOrder_OrderNo { get; set; }
        /// <summary>
        /// 入场时间
        /// </summary>
        public string enterTime { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public string carNo { get; set; }
        /// <summary>
        /// 停车场免费分钟 停车场固定的设置可以不显示
        /// </summary>
        public int? freeTime { get; set; }
        /// <summary>
        /// 停车场超时免费分钟  停车场固定的设置可以不显示
        /// </summary>
        public int? freeTimeout { get; set; }
        /// <summary>
        /// 应缴费用
        /// </summary>
        public decimal? totalAmount { get; set; }
        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal? couponAmount { get; set; }
        /// <summary>
        /// 实际应付
        /// </summary>
        public decimal? payAmount { get; set; }
    }
}
