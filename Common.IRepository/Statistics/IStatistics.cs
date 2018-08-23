using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Entities.Statistics;
using Common.Entities.Parking;
using Common.Entities;
using Common.Entities.Condition;
using Common.Entities.Order;
using Common.DataAccess;

namespace Common.IRepository.Statistics
{
    public interface IStatistics
    {
        #region 在场车辆
        int Search_PresenceCount(InParams paras);
        List<ParkIORecord> Search_Presence(InParams paras);
        List<ParkIORecord> Search_Presence(InParams paras, int PageSize, int PageIndex);
        int Search_PresenceCountSmall(InParams paras);
        List<ParkIORecord> Search_PresenceSmall(InParams paras, int PageSize, int PageIndex);
        bool SetExit(string Id, DbOperator dbOperator);

        bool QueryByIORecordId(string Id);
        #endregion

        #region 在场无牌车辆
        int Search_NoPlateNumberCount(InParams paras);
        List<ParkIORecord> Search_NoPlateNumber(InParams paras);
        List<ParkIORecord> Search_NoPlateNumber(InParams paras, int PageSize, int PageIndex);
        #endregion

        #region 进出记录查询
        int Search_InOutRecordsCount(InParams paras);
        List<ParkIORecord> Search_InOutRecords(InParams paras);
        List<ParkIORecord> Search_InOutRecords(InParams paras, int PageSize, int PageIndex);
        #endregion

        #region 车牌前缀查询
        List<PlateNumberPrefixModel> Search_PlateNumberPrefix(InParams paras);
        #endregion

        #region 月卡信息
        int Search_MonthCardInfoCount(InParams paras);
        List<MonthCardInfoModel> Search_MonthCardInfo(InParams paras);
        List<MonthCardInfoModel> Search_MonthCardInfo(InParams paras, int PageSize, int PageIndex);
        #endregion

        #region 异常放行
        int Search_ExceptionReleaseCount(InParams paras);
        List<ParkIORecord> Search_ExceptionRelease(InParams paras);
        List<ParkIORecord> Search_ExceptionRelease(InParams paras, int PageSize, int PageIndex);
        #endregion

        #region 通道事件
        int Search_GateEventsCount(InParams paras);
        List<ParkEvent> Search_GateEvents(InParams paras);
        List<ParkEvent> Search_GateEvents(InParams paras, int PageSize, int PageIndex);
         /// <summary>
        /// 查询通道进出记录
        /// </summary>
        /// <param name="parkevent">通道事件对象模型</param>
        /// <param name="PageSize">页显示数</param>
        /// <param name="PageIndex">当前页</param>
        /// <returns></returns>
        List<ParkEvent> Search_DevConnection(InParams paras, int PageSize, int PageIndex);

        /// <summary>
        /// 通道事件数量
        /// </summary>
        /// <param name="parkevent">通道事件对象</param>
        /// <returns></returns>
        int Search_DevConnectionCount(InParams paras);
        #endregion

        #region 当班统计
        int Search_OnDutyCount(InParams paras);
        List<Statistics_ChangeShift> Search_OnDuty(InParams paras);
        List<Statistics_ChangeShift> Search_OnDuty(InParams paras, int PageSize, int PageIndex);
        #endregion

        #region 订单明细
        int Search_OrdersCount(InParams paras);
        List<ParkOrder> Search_Orders(InParams paras);
        List<ParkOrder> Search_Orders(InParams paras, int PageSize, int PageIndex);
        #endregion

        #region 线上支付
        int Search_OnlinePaysCount(InParams paras);
        List<OnlineOrder> Search_OnlinePays(InParams paras);
        List<OnlineOrder> Search_OnlinePays(InParams paras, int PageSize, int PageIndex);
        #endregion

        #region 车辆进出场数量
        int Search_ParkIOsCount(InParams paras);
        List<ParkIORecord> Search_ParkIOs(InParams paras);
        List<ParkIORecord> Search_ParkIOs(InParams paras, int PageSize, int PageIndex);
        #endregion

        #region 月卡续期
        int Search_CardExtensionCount(InParams paras);
        List<ParkOrder> Search_CardExtension(InParams paras);
        List<ParkOrder> Search_CardExtension(InParams paras, int PageSize, int PageIndex);
        #endregion

        #region 储值卡充值
        int Search_CardRechargeCount(InParams paras);
        List<ParkOrder> Search_CardRecharge(InParams paras, int PageSize, int PageIndex);
        #endregion

        #region 临停缴费
        int Search_TempPaysCount(InParams paras);
        List<ParkOrder> Search_TempPays(InParams paras);
        List<ParkOrder> Search_TempPays(InParams paras, int PageSize, int PageIndex);


        List<ReportPoundNoteModel> Search_TempPaysPound(InParams paras);
        #endregion

        #region 储值卡缴费
        int Search_RechargePaysCount(InParams paras);
        List<ParkOrder> Search_RechargePays(InParams paras, int PageSize, int PageIndex);
        
        #endregion
        #region 车辆优免
        int Search_CarDeratesCount(InParams paras);
        List<ParkCarDerate> Search_CarDerates(InParams paras);
        List<ParkCarDerate> Search_CarDerates(InParams paras, int PageSize, int PageIndex);
        #endregion
        #region 商家充值
        int Search_SellerRechargeCount(InParams paras);
        List<ParkOrder> Search_SellerRecharges(InParams paras, int PageSize, int PageIndex);
        #endregion
        #region 日收费 汇总报表
        int Search_DailyStatisticsCount(InParams paras);
        List<Statistics_Gather> Search_DailyStatistics(InParams paras);
        List<Statistics_Gather> Search_DailyStatistics(InParams paras, int PageSize, int PageIndex);
        #endregion

        #region 月收费 汇总报表
        int Search_MonthStatisticsCount(InParams paras);
        List<Statistics_Gather> Search_MonthStatistics(InParams paras);
        List<Statistics_Gather> Search_MonthStatistics(InParams paras, int PageSize, int PageIndex);
        #endregion



        List<Statistics_Gather> GetStatisticsGroupByMonth(List<string> Parkings, DateTime StartTime, DateTime EndTime);
        List<Statistics_Gather> GetStatisticsGroupByDay(List<string> Parkings, DateTime StartTime, DateTime EndTime);

        List<Statistics_Gather> Search_InOutPeak(string ParkingID, DateTime StartTime, DateTime EndTime);
        List<Statistics_Gather> GetParkingTempTop5(List<string> Parkings,DateTime starttime, DateTime endtime);
         AmoutModel Search_TempPayCount(InParams paras);

         List<InOutNum> Search_InOutNum(DateTime starttime, DateTime startend);
         List<ParkEvent> Search_Event(DateTime timedate);
         List<ParkVisitorReportModel> QueryParkVisitorReport(VisitorReportCondition paras, int PageSize, int PageIndex, out int recordTotalCount);
         /// <summary>
        /// 按地址统计停车时长
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="isExit"></param>
        /// <returns></returns>
        List<Statistics_SumTime> QueryParkSumTime(DateTime starttime, DateTime endtime, bool isExit);
    }
}
