using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Entities.Parking;
using Common.Entities.Statistics;
using Common.Entities.WX;
using Common.Entities.Order;
namespace Common.Entities.Other
{
    public class Pagination
    {
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get;
            set;
        }
        /// <summary>
        /// 总条目数
        /// </summary>
        public int Total
        {
            get;
            set;
        }
        /// <summary>
        /// 月卡信息
        /// </summary>
        public List<MonthCardInfoModel> MonthCardList
        {
            get;
            set;
        }
        /// <summary>
        /// 进出记录集合
        /// </summary>
        public List<ParkIORecord> IORecordsList
        {
            get;
            set;
        }
        /// <summary>
        /// 通道事件集合
        /// </summary>
        public List<ParkEvent> GateEventList
        {
            get;
            set;
        }
        /// <summary>
        /// 订单集合
        /// </summary>
        public List<ParkOrder> OrderList
        {
            get;
            set;
        }
        /// <summary>
        /// 线上支付集合
        /// </summary>
        public List<OnlineOrder> OnlineOrderList
        {
            get;
            set;
        }
        /// <summary>
        /// 进出场集合
        /// </summary>
        public List<ParkIORecord> ParkIOList
        {
            get;
            set;
        }
        /// <summary>
        /// 当班统计
        /// </summary>
        public List<Statistics_ChangeShift> OnDutyList
        {
            get;
            set;
        }
        /// <summary>
        /// 商家优免
        /// </summary>
        public List<ParkCarDerate> CarDerateList
        {
            get;
            set;
        }
        /// <summary>
        /// 统计集合
        /// </summary>
        public List<Statistics_Gather> StatisticsGatherList
        {
            get;
            set;
        }
        /// <summary>
        /// 二维码推广人员集合
        /// </summary>
        public List<tgPerson> StatisticsPersonList
        {
            get;
            set;
        }
        /// <summary>
        /// 二维码推广统计集合
        /// </summary>
        public List<TgOrder> StatisticsTgOrderList
        {
            get;
            set;
        }
        /// <summary>
        /// 微信帐户
        /// </summary>
        public List<WX_Account> WXAccountList
        {
            get;
            set;
        }
        public List<ParkVisitorReportModel> VisitorList { get; set; }


        /// <summary>
        /// 客户端通知集合
        /// </summary>
        public List<Message> MessageList
        {
            get;
            set;
        }
    }
}
