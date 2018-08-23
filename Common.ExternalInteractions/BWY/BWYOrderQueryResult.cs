using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.ExternalInteractions.BWY
{
    public class BWYOrderQueryResult
    {
        public int Result { get; set; }
        public string Desc { get; set; }
        public List<OrderQueryResultReference> Reference { get; set; }
    }
    public class OrderQueryResultReference
    {
        /// <summary>
        /// 停车场信息
        /// </summary>
        public OrderResultReferenceParkingLot ParkingLot { get; set; }
        /// <summary>
        /// 入口信息
        /// </summary>
        public OrderResultPassAgeway EnterEntrance { get; set; }
        /// <summary>
        /// 出口信息
        /// </summary>
        public OrderResultPassAgeway LeaveEntrance { get; set; }
        /// <summary>
        /// 停车费用
        /// </summary>
        public OrderResultCostDetail CostDetail { get; set; }
        /// <summary>
        /// 订单信息
        /// </summary>
        public OrderResultBill Bill { get; set; }
    }
    /// <summary>
    /// 停车场信息
    /// </summary>
    public class OrderResultReferenceParkingLot
    {
        /// <summary>
        /// 停车场索引
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 停车场名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 停车场所在的省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 停车场所在的市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 停车场所在的区
        /// </summary>
        public string Area { get; set; }
        /// <summary>
        /// 具体位置
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// 停车场 Logo 图片地址
        /// </summary>
        public string Logo { get; set; }
        /// <summary>
        /// 总车位数
        /// </summary>
        public int NumOfSpace { get; set; }
        /// <summary>
        /// 空闲车位数
        /// </summary>
        public int IdleSpaceNum { get; set; }
    }
    /// <summary>
    /// 通道信息
    /// </summary>
    public class OrderResultPassAgeway
    {
        /// <summary>
        /// 出入口索引
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 出入口名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 出入口方向，1 入，2 出
        /// </summary>
        public int Direction { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double Latitude { get; set; }
    }
    /// <summary>
    /// 停车费用
    /// </summary>
    public class OrderResultCostDetail
    {
        /// <summary>
        /// 停车时长：天
        /// </summary>
        public int Days { get; set; }
        /// <summary>
        /// 停车时长：小时
        /// </summary>
        public int Hours { get; set; }
        /// <summary>
        /// 停车时长：分钟
        /// </summary>
        public int Minutes { get; set; }
        /// <summary>
        /// 总停车时长：秒
        /// </summary>
        public int TotalSeconds { get; set; }
        /// <summary>
        /// 停车总费用
        /// </summary>
        public int Sums { get; set; }
        /// <summary>
        /// 折扣信息
        /// </summary>
        public OrderResultCostDetailDiscount Discount { get; set; }
        /// <summary>
        /// 原始停车信息
        /// </summary>
        public OrderResultCostDetailSrcCost SrcCost { get; set; }
    }
    /// <summary>
    /// 折扣信息
    /// </summary>
    public class OrderResultCostDetailDiscount
    {
        /// <summary>
        /// 折扣天数
        /// </summary>
        public int Days { get; set; }
        /// <summary>
        /// 折扣小时数
        /// </summary>
        public int Hours { get; set; }
        /// <summary>
        /// 折扣分钟数
        /// </summary>
        public int Minutes { get; set; }
        /// <summary>
        /// 折扣总时长：秒
        /// </summary>
        public int TotalSeconds { get; set; }
        /// <summary>
        /// 折扣费用
        /// </summary>
        public int Sums { get; set; }
    }
    /// <summary>
    /// 原始停车信息
    /// </summary>
    public class OrderResultCostDetailSrcCost
    {
        /// <summary>
        /// 原始天数
        /// </summary>
        public int Days { get; set; }
        /// <summary>
        /// 原始小时数
        /// </summary>
        public int Hours { get; set; }
        /// <summary>
        /// 原始分钟数
        /// </summary>
        public int Minutes { get; set; }
        /// <summary>
        /// 原始总时长：秒
        /// </summary>
        public int TotalSeconds { get; set; }
        /// <summary>
        /// 原始费用
        /// </summary>
        public int Sums { get; set; }
    }
    /// <summary>
    /// 订单信息
    /// </summary>
    public class OrderResultBill
    {
        /// <summary>
        /// 订单的索引
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 订单所在的停车场索引
        /// </summary>
        public int ParkingLotIndex { get; set; }
        /// <summary>
        /// 订单的开始时间
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 订单的结束时间
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 应缴费用
        /// </summary>
        public int FeeOfPayable { get; set; }
        /// <summary>
        /// 实缴费用
        /// </summary>
        public int FeeOfPaid { get; set; }
        /// <summary>
        /// 状态：1 未缴费；2 正在缴费；3 已缴费
        /// </summary>
        public int PayState { get; set; }
    }
}
