using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Entities.Enum;
namespace Common.Entities.Statistics
{
    public class InParams
    {
        /// <summary>
        /// 车场编号
        /// </summary>
        public string ParkingID
        {
            get;
            set;
        }
      
        /// <summary>
        /// 进场通道
        /// </summary>
        public string InGateID
        {
            get;
            set;
        }
        /// <summary>
        /// 出场通道
        /// </summary>
        public string OutGateID
        {
            get;
            set;
        }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime
        {
            get;
            set;
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime
        {
            get;
            set;
        }
        /// <summary>
        /// 车牌号码
        /// </summary>
        public string PlateNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 卡片类型
        /// </summary>
        public string CardType
        {
            get;
            set;
        }
        /// <summary>
        /// 支付方式
        /// </summary>
        public int PayWay
        {
            get;
            set;
        }
        /// <summary>
        /// 车类型
        /// </summary>
        public string CarType
        {
            get;
            set;
        }
        /// <summary>
        /// 放行类型
        /// </summary>
        public int ReleaseType
        {
            get;
            set;
        }
        /// <summary>
        /// 区域编号
        /// </summary>
        public string AreaID
        {
            get;
            set;
        }
        /// <summary>
        /// 车主
        /// </summary>
        public string Owner
        {
            get;
            set;
        }
        /// <summary>
        /// 进操作员
        /// </summary>
        public string InOperator
        {
            get;
            set;
        }
        /// <summary>
        /// 出操作员
        /// </summary>
        public string OutOperator
        {
            get;
            set;
        }
        /// <summary>
        /// 是否0元订单
        /// </summary>
        public bool Zero
        {
            get;
            set;
        }
        /// <summary>
        /// 是否存在差异金额
        /// </summary>
        public bool DiffAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 是否即将到期
        /// </summary>
        public bool Due
        {
            get;
            set;
        }
        /// <summary>
        /// 线上或线下
        /// </summary>
        public int OnLineOffLine
        {
            get;
            set;
        }
        /// <summary>
        /// 订单来源
        /// </summary>
        public int OrderSource
        {
            get;
            set;
        }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string BoxID
        {
            get;
            set;
        }
        /// <summary>
        /// 当班人编号
        /// </summary>
        public string AdminID
        {
            get;
            set;
        }
        /// <summary>
        /// 未确认
        /// </summary>
        public bool IsNoConfirm
        {
            get;
            set;
        }

      
        /// <summary>
        /// 优免券编号
        /// </summary>
        public string CouponNo
        {
            get;
            set;
        }
        /// <summary>
        /// 商家编号
        /// </summary>
        public string SellerID
        {
            get;
            set;
        }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status
        {
            get;
            set;
        }
        /// <summary>
        /// 是否出场
        /// </summary>
        public int IsExit
        {
            get;
            set;
        }
        /// <summary>
        /// 报表类型1:在场车辆  2:无牌车辆  3:进出记录 4:异常放行 5:通道事件  6: 当班统计 7:订单明细
        /// 8: 月卡续期  9: 临停缴费 10: 商家优免  11: 日报表   12: 月报表  13: 日汇总报表  14: 月汇总报表
        /// </summary>
        public ReportType ReportType
        {
            get;
            set;
        }
        /// <summary>
        /// 1: 打印当前页  2: 打印所有页
        /// </summary>
        public int PrintType
        {
            get;
            set;
        }
        /// <summary>
        /// 每页显示数
        /// </summary>
        public int PageSize
        {
            get;
            set;
        }
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex
        {
            get;
            set;
        }
        /// <summary>
        /// 事件类型
        /// </summary>
        public int EventID
        {
            get;
            set;
        }
        /// <summary>
        /// 进或出
        /// </summary>
        public int InOrOut
        {
            get;
            set;
        }
        /// <summary>
        /// 订单类型
        /// </summary>
        public int OrderType
        {
            get;
            set;
        }
        private bool _IsInTime = true;
        /// <summary>
        /// 是否已进场时间查询
        /// </summary>
        public bool IsInTime
        {
            get { return _IsInTime; }
            set { _IsInTime = value; }
        }

        public string VID { set; get; }

        public string Addr
        {
            get;
            set;
        }

        public string Mobile
        {
            get;
            set;
        }
    }
}
