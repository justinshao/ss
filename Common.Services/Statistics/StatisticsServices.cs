using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.Entities.Other;
using Common.Entities.Parking;
using Common.Entities.Statistics;
using Common.IRepository.Statistics;
using Common.Factory.Statistics;
using Common.Core;
using Common.Entities.Condition;
using Common.Entities.Order;
using Common.DataAccess;



namespace Common.Services.Statistics
{
    /// <summary>
    /// 车场统计相关报表
    /// </summary>
    public class StatisticsServices
    {
        #region 私有方法
        static void IORecordFormat(List<ParkIORecord> iorecordlist)
        {
            if (iorecordlist != null && iorecordlist.Count > 0)
            {
                foreach (ParkIORecord record in iorecordlist)
                {
                    record.LongTime = Functions.CalLongTime(record.EntranceTime, record.ExitTime);
                }
            }
        }

        static void ParkOrderFormat(List<ParkOrder> parkorderlist)
        {
            if (parkorderlist != null && parkorderlist.Count > 0)
            {
                foreach (ParkOrder order in parkorderlist)
                {
                    order.LongTime = Functions.CalLongTime(order.EntranceTime, order.ExitTime);
                }
            }
        }

        static void ParkOderMonthTimeLong(List<ParkOrder> orderlist)
        {
            if (orderlist != null && orderlist.Count > 0)
            {
                foreach (var order in orderlist)
                {
                    order.MonthLongTime = Functions.CalMonthLongTime(order.OldUserulDate, order.NewUsefulDate);
                }
            }
        }
        #endregion

        #region 在场车辆
        /// <summary>
        /// 在场车辆记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <param name="PageSize">每页显示数</param>
        /// <param name="PageIndex">当前页</param>
        /// <returns></returns>
        public static Pagination Search_Presence(InParams paras, int PageSize, int PageIndex)
        {
            Pagination _pagination = new Pagination();
            IStatistics factory = StatisticsFactory.GetFactory();
            _pagination.Total = factory.Search_PresenceCount(paras);
            List<ParkIORecord> iorecordlist = factory.Search_Presence(paras, PageSize, PageIndex);
            IORecordFormat(iorecordlist);
            _pagination.IORecordsList = iorecordlist;
            return _pagination;
        }

        /// <summary>
        /// 获取小车场在场车辆记录
        /// </summary>
        /// <param name="paras"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public static Pagination Search_PresenceSmall(InParams paras, int PageSize, int PageIndex)
        {
            Pagination _pagination = new Pagination();
            IStatistics factory = StatisticsFactory.GetFactory();
            _pagination.Total = factory.Search_PresenceCountSmall(paras);
            List<ParkIORecord> iorecordlist = factory.Search_PresenceSmall(paras, PageSize, PageIndex);
            IORecordFormat(iorecordlist);
            _pagination.IORecordsList = iorecordlist;
            return _pagination;
        }
        /// <summary>
        /// 获取在场车辆记录
        /// </summary>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static List<ParkIORecord> Search_Presence(InParams paras)
        {
            List<ParkIORecord> iorecordlist = new List<ParkIORecord>();
            IStatistics factory = StatisticsFactory.GetFactory();
            iorecordlist = factory.Search_Presence(paras);
            IORecordFormat(iorecordlist);
            return iorecordlist;
        }
        /// <summary>
        /// 手动出场车辆
        /// </summary>
        /// <param name="Id">车场通道事件表主键ID</param>
        /// <returns></returns>
        public static bool SetExit(string Id)
        {
            if (Id.IsEmpty()) throw new ArgumentNullException("Id");
            IStatistics factory = StatisticsFactory.GetFactory();
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    if (factory.QueryByIORecordId(Id))
                        throw new MyException("不存在在场车辆" + Id);
                    dbOperator.BeginTransaction();
                    bool result = factory.SetExit(Id, dbOperator);
                    if (!result)
                        throw new MyException("手动出场车辆失败");
                    dbOperator.CommitTransaction();
                    if (result)
                        OperateLogServices.AddOperateLog(OperateType.Update, string.Format("手动出场车辆id:{0}", Id));
                    return result;
                }
                catch
                {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
        }


        #endregion

        #region 在场无牌车辆
        /// <summary>
        /// 在场无牌车辆
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <param name="PageSize">每页显示数</param>
        /// <param name="PageIndex">当前页</param>
        /// <returns></returns>
        public static Pagination Search_NoPlateNumber(InParams paras, int PageSize, int PageIndex)
        {
            Pagination _pagination = new Pagination ();
            IStatistics factory = StatisticsFactory.GetFactory();
            _pagination.Total = factory.Search_NoPlateNumberCount(paras);
            List<ParkIORecord> iorecordlist = factory.Search_NoPlateNumber(paras,PageSize,PageIndex);
            IORecordFormat(iorecordlist);
            _pagination.IORecordsList = iorecordlist;
            return _pagination;
        }
        /// <summary>
        /// 在场无牌车辆
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <returns></returns>
        public static List<ParkIORecord> Search_NoPlateNumber(InParams paras)
        {
            List<ParkIORecord> iorecordlist = new List<ParkIORecord>();
            IStatistics factory = StatisticsFactory.GetFactory();
            iorecordlist = factory.Search_NoPlateNumber(paras);
            IORecordFormat(iorecordlist);
            return iorecordlist;
        }
        #endregion

        #region 进出记录
        /// <summary>
        /// 进出记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <param name="PageSize">每页显示数</param>
        /// <param name="PageIndex">当前页</param>
        /// <returns></returns>
        public static Pagination Search_InOutRecords(InParams paras, int PageSize, int PageIndex)
        {
            Pagination _pagination = new Pagination();
            IStatistics factory = StatisticsFactory.GetFactory();
            _pagination.Total = factory.Search_InOutRecordsCount(paras);
            List<ParkIORecord> iorecordlist = factory.Search_InOutRecords(paras, PageSize, PageIndex);
            IORecordFormat(iorecordlist);
            _pagination.IORecordsList = iorecordlist;
            return _pagination;
        }
        /// <summary>
        /// 获取进出记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <returns></returns>
        public static List<ParkIORecord> Search_InOutRecords(InParams paras)
        {
            List<ParkIORecord> iorecordlist = new List<ParkIORecord>();
            IStatistics factory = StatisticsFactory.GetFactory();
            iorecordlist = factory.Search_InOutRecords(paras);
            IORecordFormat(iorecordlist);
            return iorecordlist;
        }
        #endregion

        #region 月卡信息  
        public static Pagination Search_MonthCardInfo(InParams paras, int PageSize, int PageIndex)
        {
            Pagination _pagination = new Pagination();
            IStatistics factory = StatisticsFactory.GetFactory();
            _pagination.Total = factory.Search_MonthCardInfoCount(paras);
            List<MonthCardInfoModel> iorecordlist = factory.Search_MonthCardInfo(paras, PageSize, PageIndex);
            if (iorecordlist != null && iorecordlist.Count > 0)
            {
                foreach (var m in iorecordlist)
                {
                    if (m.StartTime > DateTime.MinValue)
                    {
                        m.strStartTime = m.StartTime.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    if (m.EndTime > DateTime.MinValue)
                    {
                        m.strEndTime = m.EndTime.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }
            }
            _pagination.MonthCardList = iorecordlist;
            return _pagination;
        }
        /// <summary>
        /// 获得临停缴费记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <returns></returns>
        public static List<MonthCardInfoModel> Search_MonthCardInfo(InParams paras)
        {
            IStatistics factory = StatisticsFactory.GetFactory();
            List<MonthCardInfoModel> gatherlist = factory.Search_MonthCardInfo(paras);
            if (gatherlist != null && gatherlist.Count > 0)
            {
                foreach (var m in gatherlist)
                {
                    if (m.StartTime > DateTime.MinValue)
                    {
                        m.strStartTime = m.StartTime.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    if (m.EndTime > DateTime.MinValue)
                    {
                        m.strEndTime = m.EndTime.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }
            }
            return gatherlist;
        }
       
        #endregion

        #region 车牌前缀分析
        /// <summary>
        /// 车牌前缀查询
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <returns></returns>
        public static List<PlateNumberPrefixModel> Search_PlateNumberPrefix(InParams paras)
        {
            List<PlateNumberPrefixModel> iorecordlist = new List<PlateNumberPrefixModel>();
            IStatistics factory = StatisticsFactory.GetFactory();
            iorecordlist = factory.Search_PlateNumberPrefix(paras);

            BaseParkinfo park = Common.Services.ParkingServices.QueryParkingByParkingID(paras.ParkingID);
            int total = 0;
            //处理数据
            if (iorecordlist != null && iorecordlist.Count > 0)
            {
                total = iorecordlist.Select(u => u.Number).Sum();
                foreach (var record in iorecordlist)
                {
                    if (park != null)
                    {
                        record.ParkingName = park.PKName;
                    }
                    record.Rate = ((record.Number / (total * 1.0)) * 100).ToString("0.00") + "%";
                }
            }
            return iorecordlist;
        }
        #endregion

        #region 异常放行
        /// <summary>
        /// 获取异常放行记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <param name="PageSize">每页显示数</param>
        /// <param name="PageIndex">当前页</param>
        /// <returns></returns>
        public static Pagination Search_ExceptionRelease(InParams paras, int PageSize, int PageIndex)
        {
            Pagination _pagination = new Pagination();
            IStatistics factory = StatisticsFactory.GetFactory();
            _pagination.Total = factory.Search_ExceptionReleaseCount(paras);
            List<ParkIORecord> iorecordlist = factory.Search_ExceptionRelease(paras, PageSize, PageIndex);
            IORecordFormat(iorecordlist);
            _pagination.IORecordsList = iorecordlist;
            return _pagination;
        }
        /// <summary>
        /// 获取异常放行记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <returns></returns>
        public static List<ParkIORecord> Search_ExceptionRelease(InParams paras)
        {
            List<ParkIORecord> iorecordlist = new List<ParkIORecord>();
            IStatistics factory = StatisticsFactory.GetFactory();
            iorecordlist = factory.Search_ExceptionRelease(paras);
            IORecordFormat(iorecordlist);
            return iorecordlist;
        }
        #endregion

        #region 通道事件
        /// <summary>
        /// 获取通道事件记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <param name="PageSize">每页显示数</param>
        /// <param name="PageIndex">当前页</param>
        /// <returns></returns>
        public static Pagination Search_GateEvents(InParams paras, int PageSize, int PageIndex)
        {
            Pagination _pagination = new Pagination();
            IStatistics factory = StatisticsFactory.GetFactory();
            _pagination.Total = factory.Search_GateEventsCount(paras);
            _pagination.GateEventList = factory.Search_GateEvents(paras, PageSize, PageIndex);
            return _pagination;
        }
        public static Pagination Search_DevConnctions(InParams paras, int PageSize, int PageIndex)
        {
            Pagination _pagination = new Pagination();
            IStatistics factory = StatisticsFactory.GetFactory();
            _pagination.Total = factory.Search_DevConnectionCount(paras);
            _pagination.GateEventList = factory.Search_DevConnection(paras, PageSize, PageIndex);
            return _pagination;
        }
        
        /// <summary>
        /// 获取通道事件记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <returns></returns>
        public static List<ParkEvent> Search_GateEvents(InParams paras)
        {
            IStatistics factory = StatisticsFactory.GetFactory();
            return factory.Search_GateEvents(paras);
        }
        #endregion

        #region 当班统计
        /// <summary>
        /// 获取当班统计记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <param name="PageSize">每页显示数</param>
        /// <param name="PageIndex">当前页</param>
        /// <returns></returns>
        public static Pagination Search_OnDuty(InParams paras, int PageSize, int PageIndex)
        {
            Pagination _pagination = new Pagination();
            IStatistics factory = StatisticsFactory.GetFactory();
            _pagination.Total = factory.Search_OnDutyCount(paras);
            _pagination.OnDutyList = factory.Search_OnDuty(paras, PageSize, PageIndex);
            return _pagination;
        }
        /// <summary>
        /// 获取当班统计记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <returns></returns>
        public static List<Statistics_ChangeShift> Search_OnDuty(InParams paras)
        {
            IStatistics factory = StatisticsFactory.GetFactory();
            return factory.Search_OnDuty(paras);
        }
        #endregion

        #region 订单明细
        /// <summary>
        /// 获取订单明细记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <param name="PageSize">每页显示数</param>
        /// <param name="PageIndex">当前页</param>
        /// <returns></returns>
        public static Pagination Search_Orders(InParams paras, int PageSize, int PageIndex)
        {
            Pagination _pagination = new Pagination();
            IStatistics factory = StatisticsFactory.GetFactory();
            _pagination.Total = factory.Search_OrdersCount(paras);
            _pagination.OrderList = factory.Search_Orders(paras, PageSize, PageIndex);
            return _pagination;
        }
        /// <summary>
        /// 获取订单明细记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <returns></returns>
        public static List<ParkOrder> Search_Orders(InParams paras)
        {
            IStatistics factory = StatisticsFactory.GetFactory();
            return factory.Search_Orders(paras);
        }
        #endregion

        #region 线上支付
        /// <summary>
        /// 获取线上支付记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <param name="PageSize">每页显示数</param>
        /// <param name="PageIndex">当前页</param>
        /// <returns></returns>
        public static Pagination Search_OnlinePays(InParams paras, int PageSize, int PageIndex)
        {
            Pagination _pagination = new Pagination();
            IStatistics factory = StatisticsFactory.GetFactory();
            _pagination.Total = factory.Search_OnlinePaysCount(paras);
            _pagination.OnlineOrderList = factory.Search_OnlinePays(paras, PageSize, PageIndex);
            return _pagination;
        }
        /// <summary>
        /// 获取线上支付记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <returns></returns>
        public static List<OnlineOrder> Search_OnlinePays(InParams paras)
        {
            IStatistics factory = StatisticsFactory.GetFactory();
            return factory.Search_OnlinePays(paras);
        }
        #endregion

        #region 车辆进出场数量
        /// <summary>
        /// 获取车辆进出场数量
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <param name="PageSize">每页显示数</param>
        /// <param name="PageIndex">当前页</param>
        /// <returns></returns>
        public static Pagination Search_ParkIOs(InParams paras, int PageSize, int PageIndex)
        {
            Pagination _pagination = new Pagination();
            IStatistics factory = StatisticsFactory.GetFactory();
            _pagination.Total = factory.Search_ParkIOsCount(paras);
            _pagination.ParkIOList = factory.Search_ParkIOs(paras, PageSize, PageIndex);
            return _pagination;
        }
        /// <summary>
        /// 获取车辆进出场数量
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <returns></returns>
        public static List<ParkIORecord> Search_ParkIOs(InParams paras)
        {
            IStatistics factory = StatisticsFactory.GetFactory();
            return factory.Search_ParkIOs(paras);
        }
        #endregion


        #region 月卡续期
        /// <summary>
        /// 获得月卡续期记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <param name="PageSize">每页显示数</param>
        /// <param name="PageIndex">当前页</param>
        /// <returns></returns>
        public static Pagination Search_CardExtension(InParams paras, int PageSize, int PageIndex)
        {
            Pagination _pagination = new Pagination();
            IStatistics factory = StatisticsFactory.GetFactory();
            _pagination.Total = factory.Search_CardExtensionCount(paras);
            _pagination.OrderList = factory.Search_CardExtension(paras, PageSize, PageIndex);
            ParkOderMonthTimeLong(_pagination.OrderList);
            return _pagination;
        }
        /// <summary>
        /// 获得月卡续期记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <returns></returns>
        public static List<ParkOrder> Search_CardExtension(InParams paras)
        {
            IStatistics factory = StatisticsFactory.GetFactory();
            return factory.Search_CardExtension(paras);
        }
        #endregion

        #region 储值卡充值
        /// <summary>
        /// 获得月卡续期记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <param name="PageSize">每页显示数</param>
        /// <param name="PageIndex">当前页</param>
        /// <returns></returns>
        public static Pagination Search_CardRecharge(InParams paras, int PageSize, int PageIndex)
        {
            Pagination _pagination = new Pagination();
            IStatistics factory = StatisticsFactory.GetFactory();
            _pagination.Total = factory.Search_CardRechargeCount(paras);
            _pagination.OrderList = factory.Search_CardRecharge(paras, PageSize, PageIndex);
            return _pagination;
        }
   
        #endregion

        #region 临停缴费
        /// <summary>
        /// 获得临停缴费记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <param name="PageSize">每页显示数</param>
        /// <param name="PageIndex">当前页</param>
        /// <returns></returns>
        public static Pagination Search_TempPays(InParams paras, int PageSize, int PageIndex)
        {
            Pagination _pagination = new Pagination();
            IStatistics factory = StatisticsFactory.GetFactory();
            _pagination.Total = factory.Search_TempPaysCount(paras);
            List<ParkOrder> temp = factory.Search_TempPays(paras, PageSize, PageIndex);
            ParkOrderFormat(temp);
            _pagination.OrderList = temp;
            return _pagination;
        }

        public static AmoutModel Search_TempPayCount(InParams paras)
        {
            IStatistics factory = StatisticsFactory.GetFactory();
            return factory.Search_TempPayCount(paras); 
        }
        /// <summary>
        /// 获得临停缴费记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <returns></returns>
        public static List<ParkOrder> Search_TempPays(InParams paras)
        {
            IStatistics factory = StatisticsFactory.GetFactory();
            List<ParkOrder> temp = factory.Search_TempPays(paras);
            ParkOrderFormat(temp);
            return temp;
        }

        /// <summary>
        /// 获得临停缴费记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <returns></returns>
        public static List<ReportPoundNoteModel> Search_TempPaysPound(InParams paras)
        {
            IStatistics factory = StatisticsFactory.GetFactory();
            List<ReportPoundNoteModel> temp = factory.Search_TempPaysPound(paras);

            if (temp != null && temp.Count > 0)
            {
                foreach (var p in temp)
                {
                    p.LongTime = Functions.CalLongTime(p.EntranceTime, p.ExitTime);
                    p.OrderTimeName = p.OrderTime.ToString("yyyy-MM-dd HH:mm:ss");
                    p.EntranceTimeName = p.EntranceTime.ToString("yyyy-MM-dd HH:mm:ss");
                    p.ExitTimeName = p.ExitTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }

            return temp;
        }
        #endregion

        #region 储值卡缴费
        /// <summary>
        /// 获得临停缴费记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <param name="PageSize">每页显示数</param>
        /// <param name="PageIndex">当前页</param>
        /// <returns></returns>
        public static Pagination Search_RechargePays(InParams paras, int PageSize, int PageIndex)
        {
            Pagination _pagination = new Pagination();
            IStatistics factory = StatisticsFactory.GetFactory();
            _pagination.Total = factory.Search_RechargePaysCount(paras);
            List<ParkOrder> temp = factory.Search_RechargePays(paras, PageSize, PageIndex);
            ParkOrderFormat(temp);
            _pagination.OrderList = temp;
            return _pagination;
        }
        
        #endregion

        #region 车辆优免
        /// <summary>
        /// 获得临停缴费记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <param name="PageSize">每页显示数</param>
        /// <param name="PageIndex">当前页</param>
        /// <returns></returns>
        public static Pagination Search_CarDerates(InParams paras, int PageSize, int PageIndex)
        {
            Pagination _pagination = new Pagination();
            IStatistics factory = StatisticsFactory.GetFactory();
            _pagination.Total = factory.Search_CarDeratesCount(paras);
            _pagination.CarDerateList = factory.Search_CarDerates(paras, PageSize, PageIndex);
            return _pagination;
        }
        /// <summary>
        /// 获得临停缴费记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <returns></returns>
        public static List<ParkCarDerate> Search_CarDerates(InParams paras)
        {
            IStatistics factory = StatisticsFactory.GetFactory();
            return factory.Search_CarDerates(paras);
        }
        #endregion

        #region 商家充值
        /// <summary>
        /// 获得临停缴费记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <param name="PageSize">每页显示数</param>
        /// <param name="PageIndex">当前页</param>
        /// <returns></returns>
        public static Pagination Search_SellerRecharge(InParams paras, int PageSize, int PageIndex)
        {
            Pagination _pagination = new Pagination();
            IStatistics factory = StatisticsFactory.GetFactory();
            _pagination.Total = factory.Search_SellerRechargeCount(paras);
            _pagination.OrderList = factory.Search_SellerRecharges(paras, PageSize, PageIndex);
            return _pagination;
        }
        #endregion

        public static List<InOutNum> Search_InOutNum(DateTime starttime, DateTime startend)
        {
            IStatistics factory = StatisticsFactory.GetFactory();
           return factory.Search_InOutNum(starttime, startend);
           
        }
        public static List<ParkEvent> Search_Event(DateTime timedate)
        {
            IStatistics factory = StatisticsFactory.GetFactory();
            return factory.Search_Event(timedate);
           
        }

        /// <summary>
        /// 按地址统计停车时长
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="isExit"></param>
        /// <returns></returns>
        public static List<Statistics_SumTime> QueryParkSumTime(DateTime starttime, DateTime endtime, bool isExit)
        {
            IStatistics factory = StatisticsFactory.GetFactory();
            return factory.QueryParkSumTime(starttime, endtime, isExit);
        }


        #region 日收费报表
        /// <summary>
        /// 获得临停缴费记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <param name="PageSize">每页显示数</param>
        /// <param name="PageIndex">当前页</param>
        /// <returns></returns>
        public static Pagination Search_DailyStatistics(InParams paras, int PageSize, int PageIndex)
        {
            Pagination _pagination = new Pagination();
            IStatistics factory = StatisticsFactory.GetFactory();
            _pagination.Total = factory.Search_DailyStatisticsCount(paras);
            _pagination.StatisticsGatherList = factory.Search_DailyStatistics(paras, PageSize, PageIndex);
            if (!string.IsNullOrEmpty(paras.ParkingID) && _pagination.StatisticsGatherList != null && _pagination.StatisticsGatherList.Count > 0)
            {
                BaseParkinfo parkinfo = ParkingServices.QueryParkingByParkingID(paras.ParkingID);
                if (parkinfo != null)
                {
                    foreach (var v in _pagination.StatisticsGatherList)
                    {
                        v.ParkingName = parkinfo.PKName;
                    }
                }
            }
            return _pagination;
        }
        /// <summary>
        /// 获得临停缴费记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <returns></returns>
        public static List<Statistics_Gather> Search_DailyStatistics(InParams paras)
        {
            IStatistics factory = StatisticsFactory.GetFactory();
            List<Statistics_Gather> gatherlist = factory.Search_DailyStatistics(paras);
            if (!string.IsNullOrEmpty(paras.ParkingID) && gatherlist != null && gatherlist.Count > 0)
            {
                BaseParkinfo parkinfo = ParkingServices.QueryParkingByParkingID(paras.ParkingID);
                if (parkinfo != null)
                {
                    foreach (var v in gatherlist)
                    {
                        v.ParkingName = parkinfo.PKName;
                    }
                }
            }
            return gatherlist;
        }
        #endregion


        #region 访客报表
        public static Pagination QueryParkVisitorReport(VisitorReportCondition paras, int PageSize, int PageIndex)
        {
            Pagination _pagination = new Pagination();
            IStatistics factory = StatisticsFactory.GetFactory();
            int recordTotalCount = 0;
            _pagination.VisitorList = factory.QueryParkVisitorReport(paras, PageSize, PageIndex, out recordTotalCount);
            _pagination.Total = recordTotalCount;
            return _pagination;
        }
        #endregion

        #region 月收费报表
        /// <summary>
        /// 获得临停缴费记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <param name="PageSize">每页显示数</param>
        /// <param name="PageIndex">当前页</param>
        /// <returns></returns>
        public static Pagination Search_MonthStatistics(InParams paras, int PageSize, int PageIndex)
        {
            Pagination _pagination = new Pagination();
            IStatistics factory = StatisticsFactory.GetFactory();
            _pagination.Total = factory.Search_MonthStatisticsCount(paras);
            _pagination.StatisticsGatherList = factory.Search_MonthStatistics(paras, PageSize, PageIndex);
            if (!string.IsNullOrEmpty(paras.ParkingID) && _pagination.StatisticsGatherList != null && _pagination.StatisticsGatherList.Count > 0)
            {
                BaseParkinfo parkinfo = ParkingServices.QueryParkingByParkingID(paras.ParkingID);
                if (parkinfo != null )
                {
                    foreach (var v in _pagination.StatisticsGatherList)
                    {
                        v.ParkingName = parkinfo.PKName;
                    }
                }
            }
            return _pagination;
        }
        /// <summary>
        /// 获得临停缴费记录
        /// </summary>
        /// <param name="paras">输入参数</param>
        /// <returns></returns>
        public static List<Statistics_Gather> Search_MonthStatistics(InParams paras)
        {
            IStatistics factory = StatisticsFactory.GetFactory();
            List<Statistics_Gather> gatherlist = factory.Search_MonthStatistics(paras);
            if (!string.IsNullOrEmpty(paras.ParkingID) && gatherlist != null && gatherlist.Count > 0)
            {
                BaseParkinfo parkinfo = ParkingServices.QueryParkingByParkingID(paras.ParkingID);
                if (parkinfo != null)
                {
                    foreach (var v in gatherlist)
                    {
                        v.ParkingName = parkinfo.PKName;
                    }
                }
            }
            return gatherlist;
        }
        #endregion

        #region 进出分析
        /// <summary>
        /// 进出分析
        /// </summary>
        /// <param name="ParkingID">车场编号</param>
        /// <returns></returns>
        public static InOutResult Analysis_InOut(string ParkingID)
        {
            InOutResult inoutmodel = new InOutResult();
            IStatistics factory = StatisticsFactory.GetFactory();
            DateTime dtNow = DateTime.Now;
            DateTime StartTime = DateTime.Parse(dtNow.AddDays(-14).ToString("yyyy-MM-dd 00:00:00"));
            DateTime EndTime = DateTime.Parse(dtNow.ToString("yyyy-MM-dd 23:59:59"));
            List<string> parkings = new List<string>();
            parkings.Add(ParkingID);
            List<Statistics_Gather> _gatherday15 = factory.GetStatisticsGroupByDay(parkings, StartTime, EndTime);
            for (int i = 0; i < 10; i++)
            {
                string temp = DateTime.Now.AddDays(-(i + 1)).ToString("yyyy-MM-dd");
                if (!_gatherday15.Select(u => u.KeyName).Contains(temp))
                {
                    _gatherday15.Add(new Statistics_Gather
                    {
                        KeyName = temp,
                        Entrance_Count = 0,
                        Cash_Amount = 0,
                        Real_Amount = 0,
                        OnLine_Amount = 0,
                        Receivable_Amount = 0
                    });
                }
            }
            if (_gatherday15 != null && _gatherday15.Count > 0)
            {
                for (int i = 0; i < 15; i++)
                {
                    var v = _gatherday15.Find(u => u.KeyName == dtNow.AddDays(-i).ToString("yyyy-MM-dd"));
                    if (v == null)
                    {
                        _gatherday15.Add(new Statistics_Gather { KeyName = dtNow.AddDays(-i).ToString("yyyy-MM-dd"), GatherTime = dtNow.AddDays(-i) });
                    }
                }
                _gatherday15 = _gatherday15.OrderBy(u => u.GatherTime).ToList();
                foreach (Statistics_Gather g in _gatherday15)
                {
                    g.KeyName = g.KeyName.Split('-')[1] + "月" + g.KeyName.Split('-')[2] + "日";
                }
            }
           
            //进出高峰
            StartTime = DateTime.Parse(dtNow.ToString("yyyy-MM-dd 00:00:00"));
            inoutmodel.InOutPeak = factory.Search_InOutPeak(ParkingID, StartTime, EndTime);
            //30天进场数
            inoutmodel.DailyTemp = _gatherday15.OrderByDescending(u => u.KeyName).ToList();
            //近10个月的入场数据
            StartTime = DateTime.Parse(dtNow.AddMonths(-9).ToString("yyyy-MM-01 00:00:00"));
            List<string> templist = new List<string>();
            templist.Add(ParkingID);
            List<Statistics_Gather> sg =factory.GetStatisticsGroupByMonth(templist, StartTime, EndTime);
            if (sg != null)
            {
                if (sg.Count < 10)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        var v = sg.Find(u => u.KeyName == dtNow.AddMonths(-i).ToString("yyyy-MM"));
                        if (v == null)
                        {
                            sg.Add(new Statistics_Gather { KeyName = DateTime.Parse(dtNow.ToString("yyyy-MM")).AddMonths(-i).ToString("yyyy-MM"), GatherTime = DateTime.Parse(dtNow.ToString("yyyy-MM-01")).AddMonths(-i) });
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    sg.Add(new Statistics_Gather { KeyName = DateTime.Parse(dtNow.ToString("yyyy-MM")).AddMonths(-i).ToString("yyyy-MM"), GatherTime = DateTime.Parse(dtNow.ToString("yyyy-MM-01")).AddMonths(-i) });
                }
            }
            inoutmodel.MonthTemp = sg.OrderByDescending(u => u.KeyName).ToList();
            //近30天的放行数据
            decimal sumnormal = _gatherday15.Select(u => u.ReleaseType_Normal).Sum();
            decimal sumcatch = _gatherday15.Select(u => u.ReleaseType_Catch).Sum();
            decimal sumfree = _gatherday15.Select(u => u.ReleaseType_Free).Sum();
            decimal sumcharge = _gatherday15.Select(u => u.ReleaseType_Charge).Sum();
            decimal releasetypesum = sumnormal + sumcatch + sumfree + sumcharge;
            if (releasetypesum > 0)
            {
                inoutmodel.ReleaseType.Add(new KeyValue
                {
                    KeyName = "正常放行",
                    KeyValue1 = sumnormal,
                    KeyValue2 = Math.Round(sumnormal * 100 / (sumnormal + sumcatch + sumfree + sumcharge), 2)
                });
                inoutmodel.ReleaseType.Add(new KeyValue
                {
                    KeyName = "免费放行",
                    KeyValue1 = sumfree,
                    KeyValue2 = Math.Round(sumfree * 100 / (sumnormal + sumcatch + sumfree + sumcharge), 2)
                });
                inoutmodel.ReleaseType.Add(new KeyValue
                {
                    KeyName = "收费放行",
                    KeyValue1 = sumcharge,
                    KeyValue2 = Math.Round(sumcharge * 100 / (sumnormal + sumcatch + sumfree + sumcharge), 2)
                });
                inoutmodel.ReleaseType.Add(new KeyValue
                {
                    KeyName = "异常放行",
                    KeyValue1 = sumcatch,
                    KeyValue2 = Math.Round(sumcatch * 100 / (sumnormal + sumcatch + sumfree + sumcharge), 2)
                });
            }
            else
            {
                inoutmodel.ReleaseType.Add(new KeyValue { KeyName = "正常放行", KeyValue1 = sumnormal, KeyValue2 = 100 });
                inoutmodel.ReleaseType.Add(new KeyValue { KeyName = "免费放行", KeyValue1 = sumfree, KeyValue2 = 0 });
                inoutmodel.ReleaseType.Add(new KeyValue { KeyName = "收费放行", KeyValue1 = sumcharge, KeyValue2 = 0 });
                inoutmodel.ReleaseType.Add(new KeyValue { KeyName = "异常放行", KeyValue1 = sumcatch, KeyValue2 = 0 });
            }
            //近30天的停车类型           
            int vipcard = _gatherday15.Select(u => u.VIPCard).Sum();
            int stordcard = _gatherday15.Select(u => u.StordCard).Sum();
            int monthcard = _gatherday15.Select(u => u.MonthCard).Sum();
            int jobcard = _gatherday15.Select(u => u.JobCard).Sum();
            int tempcard = _gatherday15.Select(u => u.TempCard).Sum();
            int sum = _gatherday15.Select(u => u.VIPCard).Sum() + _gatherday15.Select(u => u.StordCard).Sum() + _gatherday15.Select(u => u.MonthCard).Sum() + _gatherday15.Select(u => u.JobCard).Sum() + _gatherday15.Select(u => u.TempCard).Sum();
            if (sum > 0)
            {
                inoutmodel.CardType.Add(new KeyValue { KeyName = "贵宾卡", Key_Value = vipcard, KeyValue2 = Math.Round(System.Convert.ToDecimal(vipcard * 1.0 * 100 / sum), 2) });
                inoutmodel.CardType.Add(new KeyValue { KeyName = "储值卡", Key_Value = stordcard, KeyValue2 = Math.Round(System.Convert.ToDecimal(stordcard * 1.0 * 100 / sum), 2) });
                inoutmodel.CardType.Add(new KeyValue { KeyName = "月卡", Key_Value = monthcard, KeyValue2 = Math.Round(System.Convert.ToDecimal(monthcard * 1.0 * 100 / sum), 2) });
                inoutmodel.CardType.Add(new KeyValue { KeyName = "临时卡", Key_Value = tempcard, KeyValue2 = Math.Round(System.Convert.ToDecimal(tempcard * 1.0 * 100 / sum), 2) });
                inoutmodel.CardType.Add(new KeyValue { KeyName = "工作卡", Key_Value = jobcard, KeyValue2 = Math.Round(System.Convert.ToDecimal(jobcard * 1.0 * 100 / sum), 2) });
            }
            else
            {
                inoutmodel.CardType.Add(new KeyValue { KeyName = "贵宾卡", Key_Value = 0, KeyValue2 = 0 });
                inoutmodel.CardType.Add(new KeyValue { KeyName = "储值卡", Key_Value = 0, KeyValue2 = 0 });
                inoutmodel.CardType.Add(new KeyValue { KeyName = "月卡", Key_Value = 0, KeyValue2 = 0 });
                inoutmodel.CardType.Add(new KeyValue { KeyName = "临时卡", Key_Value = 0, KeyValue2 = 0 });
                inoutmodel.CardType.Add(new KeyValue { KeyName = "工作卡", Key_Value = 0, KeyValue2 = 100 });
            }
            return inoutmodel;
        }

        #endregion

        #region 收入分析
        /// <summary>
        /// 车场收入分析
        /// </summary>
        /// <param name="ParkingID">车场编号</param>
        /// <returns></returns>
        public static InComeResult Analysis_InCome(string ParkingID)
        {
            InComeResult incomeresult = new InComeResult();
            IStatistics factory = StatisticsFactory.GetFactory();
            DateTime dtNow = DateTime.Now;
            DateTime StartTime = DateTime.Parse(dtNow.ToString("yyyy-MM-01 00:00:00")).AddMonths(-11);
            DateTime EndTime = DateTime.Parse(dtNow.ToString("yyyy-MM-dd 23:59:59"));

            #region 近12个月的统计数据
            List<string> templist = new List<string>();
            templist.Add(ParkingID);
            List<Statistics_Gather> gatherlist = factory.GetStatisticsGroupByMonth(templist, StartTime, EndTime);
            if (gatherlist != null && gatherlist.Count > 0)
            {
                if (gatherlist.Count < 12)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        var isexists = gatherlist.Find(u => u.KeyName == dtNow.AddMonths(-i).ToString("yyyy-MM"));
                        if (isexists == null)
                        {
                            gatherlist.Add(new Statistics_Gather
                            {
                                KeyName = dtNow.AddMonths(-i).ToString("yyyy-MM"),
                                GatherTime = dtNow.Date.AddMonths(-i)
                            });
                        }
                    }
                }
            }
            else
            {
                gatherlist = new List<Statistics_Gather>();
                for (int i = 0; i < 12; i++)
                {
                    gatherlist.Add(new Statistics_Gather
                    {
                        KeyName = dtNow.AddMonths(-i).ToString("yyyy-MM"),
                        GatherTime = dtNow.Date.AddMonths(-i)
                    });
                }
            }
            #endregion
            incomeresult.GatherMonth12 = gatherlist;
            #region 环比数据
            var mom1 = gatherlist.Find(u => u.KeyName == dtNow.ToString("yyyy-MM"));
            if (mom1 != null)
            {
                incomeresult.MOM.Add(mom1);
            }
            else
            {
                incomeresult.MOM.Add(new Statistics_Gather
                {
                    KeyName = dtNow.ToString("yyyy-MM"),
                    GatherTime = dtNow.Date
                });
            }

            var mom2 = gatherlist.Find(u => u.KeyName == dtNow.AddMonths(-1).ToString("yyyy-MM"));
            if (mom2 != null)
            {
                incomeresult.MOM.Add(mom2);
            }
            else
            {
                incomeresult.MOM.Add(new Statistics_Gather
                {
                    KeyName = dtNow.AddMonths(-1).ToString("yyyy-MM"),
                    GatherTime = dtNow.Date.AddMonths(-1)
                });
            }
            #endregion

            #region 同比数据
            var yoy1 = gatherlist.Find(u => u.KeyName == dtNow.ToString("yyyy-MM"));
            if (yoy1 != null)
            {
                incomeresult.YOY.Add(yoy1);
            }
            else
            {
                incomeresult.YOY.Add(new Statistics_Gather
                {
                    KeyName = dtNow.ToString("yyyy-MM"),
                    GatherTime = dtNow.Date
                });
            }
            StartTime = DateTime.Parse(dtNow.Date.AddYears(-1).ToString("yyyy-MM-01 00:00:00"));
            EndTime = StartTime.AddSeconds(-1).AddMonths(1);
            //获取去年同月的统计数据
            List<Statistics_Gather> yoygather = factory.GetStatisticsGroupByMonth(templist, StartTime, EndTime);
            if (yoygather != null && yoygather.Count == 1)
            {
                incomeresult.YOY.Add(yoygather[0]);
            }
            else
            {
                incomeresult.YOY.Add(new Statistics_Gather
                {
                    KeyName = dtNow.Date.AddYears(-1).AddHours(1).AddSeconds(-1).ToString("yyyy-MM"),
                    GatherTime = dtNow.Date
                });
            }
            #endregion

            return incomeresult;
        }
        #endregion

        #region 首页数据
        /// <summary>
        /// 获得首页数据
        /// </summary>
        /// <returns></returns>
        public static HomeResult GetHomeData(List<string> villages)
        {
            HomeResult homeresult = new HomeResult();
            try
            {
                IStatistics factory = StatisticsFactory.GetFactory();
                DateTime dtNow = DateTime.Now;
                DateTime StartTime = dtNow.Date.AddDays(-14);
                DateTime EndTime = dtNow;
                List<BaseParkinfo> parkinfos = ParkingServices.QueryParkingByVillageIds(villages);
                List<Statistics_Gather> gatherlistdaily15 = null;
                if (parkinfos != null && parkinfos.Count > 0)
                {
                    gatherlistdaily15 = factory.GetStatisticsGroupByDay(parkinfos.Select(u=>u.PKID).ToList(), StartTime, EndTime);
                }
                //如果少于15天,则补足15天
                if (gatherlistdaily15 != null)
                {
                    if (gatherlistdaily15.Count <= 15)
                    {
                        for (int i = 0; i < 15; i++)
                        {
                            var v = gatherlistdaily15.Find(u => u.KeyName == dtNow.AddDays(-i).ToString("yyyy-MM-dd"));
                            if (v == null)
                            {
                                gatherlistdaily15.Add(new Statistics_Gather { KeyName = dtNow.AddDays(-i).ToString("MM-dd"), GatherTime = dtNow.AddDays(-i) });
                            }
                            else
                                v.KeyName = dtNow.AddDays(-i).ToString("MM-dd");
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 15; i++)
                    {
                        gatherlistdaily15.Add(new Statistics_Gather { KeyName = dtNow.AddDays(-i).ToString("MM-dd"), GatherTime = dtNow.AddDays(-i) });
                    }
                }
                gatherlistdaily15 = gatherlistdaily15.OrderBy(u => u.KeyName).ToList();
               
                homeresult.GatherDaily30 = gatherlistdaily15;
                int vipcard = gatherlistdaily15.Select(u => u.VIPCard).Sum();
                int stordcard = gatherlistdaily15.Select(u => u.StordCard).Sum();
                int monthcard = gatherlistdaily15.Select(u => u.MonthCard).Sum();
                int jobcard = gatherlistdaily15.Select(u => u.JobCard).Sum();
                int tempcard = gatherlistdaily15.Select(u => u.TempCard).Sum();

                int sumnormal = gatherlistdaily15.Select(u => u.ReleaseType_Normal).Sum();
                int sumcatch = gatherlistdaily15.Select(u => u.ReleaseType_Catch).Sum();
                int sumfree = gatherlistdaily15.Select(u => u.ReleaseType_Free).Sum();
                int sumcharge = gatherlistdaily15.Select(u => u.ReleaseType_Charge).Sum();
                //进场类型
                homeresult.EntranceCardType.Add(new KeyValue { KeyName = "贵宾卡", Key_Value = vipcard, KeyValue2 = vipcard });
                homeresult.EntranceCardType.Add(new KeyValue { KeyName = "储值卡", Key_Value = stordcard, KeyValue2 = stordcard });
                homeresult.EntranceCardType.Add(new KeyValue { KeyName = "月卡", Key_Value = monthcard, KeyValue2 = monthcard });
                homeresult.EntranceCardType.Add(new KeyValue { KeyName = "临时卡", Key_Value = tempcard, KeyValue2 = tempcard });
                homeresult.EntranceCardType.Add(new KeyValue { KeyName = "工作卡", Key_Value = jobcard, KeyValue2 = jobcard });
                //放行类型
                homeresult.ReleaseType.Add(new KeyValue { KeyName = "正常放行", Key_Value = sumnormal, KeyValue2 = sumnormal });
                homeresult.ReleaseType.Add(new KeyValue { KeyName = "免费放行", Key_Value = sumfree, KeyValue2 = sumfree });
                homeresult.ReleaseType.Add(new KeyValue { KeyName = "收费放行", Key_Value = sumcharge, KeyValue2 = sumcharge });
                homeresult.ReleaseType.Add(new KeyValue { KeyName = "异常放行", Key_Value = sumcatch, KeyValue2 = sumcatch });

                List<Statistics_Gather> gathertop5 = null;
                if (parkinfos != null && parkinfos.Count > 0)
                {
                    gathertop5 = factory.GetParkingTempTop5(parkinfos.Select(u => u.PKID).ToList(), StartTime, EndTime);
                }
                List<KeyValue> parkingtop5 = new List<KeyValue> ();

                if (gathertop5 != null)
                {
                    foreach (var v in gathertop5)
                    {
                        parkingtop5.Add(new KeyValue
                        {
                            Key_Value = v.TempCard,
                            KeyName = v.ParkingName
                        });
                    }

                    int num = gathertop5.Count;
                    if (num < 5)
                    {
                        for (int i = 0; i < 5 - num; i++)
                        {
                            parkingtop5.Add(new KeyValue
                            {
                                KeyName = "",
                                Key_Value = 0
                            });
                        }
                    }
                }
                else
                {
                    if (gathertop5.Count < 5)
                    {
                        int n = gathertop5.Count;
                        for (int i = 0; i < 5 - n; i++)
                        {
                            parkingtop5.Add(new KeyValue
                            {
                                KeyName = "",
                                Key_Value = 0
                            });
                        }
                    }
                }
                homeresult.ParkTempTop5 = parkingtop5.OrderBy(u => u.Key_Value).ToList();
                StartTime = DateTime.Parse(dtNow.ToString("yyyy-MM-01")).AddMonths(-5);
                List<Statistics_Gather> gatherlistmonth12 = null;
                if (parkinfos != null && parkinfos.Count > 0)
                {
                    gatherlistmonth12 = factory.GetStatisticsGroupByMonth(parkinfos.Select(u => u.PKID).ToList(), StartTime, EndTime);
                }

                if (gatherlistmonth12 != null)
                {
                    if (gatherlistmonth12.Count < 6)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            var v = gatherlistmonth12.Find(u => u.KeyName == dtNow.AddMonths(-i).ToString("yyyy-MM"));
                            if (v == null)
                            {
                                gatherlistmonth12.Add(new Statistics_Gather { KeyName = DateTime.Parse(dtNow.ToString("yyyy-MM")).AddMonths(-i).ToString("yyyy-MM"), GatherTime = DateTime.Parse(dtNow.ToString("yyyy-MM-01")).AddMonths(-i) });
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 6; i++)
                    {
                        gatherlistmonth12.Add(new Statistics_Gather { KeyName = DateTime.Parse(dtNow.ToString("yyyy-MM")).AddMonths(-i).ToString("yyyy-MM"), GatherTime = DateTime.Parse(dtNow.ToString("yyyy-MM-01")).AddMonths(-i) });
                    }
                }
                homeresult.GatherMonth12 = gatherlistmonth12.OrderBy(u => u.KeyName).ToList();
            }
            catch(Exception ex)
            {
                Common.Services.TxtLogServices.WriteTxtLog("打开首页异常 异常信息:", ex.Message);
                
            }
            return homeresult;
        }
        #endregion
    }
}
