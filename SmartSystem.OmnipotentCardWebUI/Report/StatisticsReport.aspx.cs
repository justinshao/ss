using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;

using Common.Entities;
using Common.Entities.Statistics;
using Common.Entities.Parking;
using Common.Entities.Other;
using Common.Services.Statistics;
using Common.Entities.Enum;
using Common.Entities.Condition;

namespace SmartSystem.OmnipotentCardWebUI.Report
{
    public partial class StatisticsReport : System.Web.UI.Page
    {
        /// <summary>
        /// 在场车辆报表
        /// </summary>
        const string RptPresence = "ReportViewer/ReportView_Presence.rpt";
        /// <summary>
        /// 无牌车辆报表
        /// </summary>
        const string RptNoPlateNumber = "ReportViewer/ReportView_NoPlateNumber.rpt";
        /// <summary>
        /// 进出记录报表
        /// </summary>
        const string RptInOut = "ReportViewer/ReportView_InOut.rpt";
        /// <summary>
        /// 异常放行报表
        /// </summary>
        const string RptExceptionRelease = "ReportViewer/ReportView_ExceptionRelease.rpt";
        /// <summary>
        /// 通道事件报表
        /// </summary>
        const string RptGateEvent = "ReportViewer/ReportView_GateEvents.rpt";
        /// <summary>
        /// 月卡续期报表
        /// </summary>
        const string RptCardExtension = "ReportViewer/ReportView.CardExtension.rpt";
        /// <summary>
        /// 当班统计报表
        /// </summary>
        const string RptOnDuty = "ReportViewer/ReportView_OnDuty.rpt";
        /// <summary>
        /// 订单明细报表
        /// </summary>
        const string RptOrder = "ReportViewer/ReportView_Orders.rpt";
        /// <summary>
        /// 商家优免报表
        /// </summary>
        const string RptCoupon = "ReportViewer/ReportView.Coupon.rpt";
        /// <summary>
        /// 临停缴费报表
        /// </summary>
        const string RptTempPay = "ReportViewer/ReportView_TempPay.rpt";
        /// <summary>
        /// 日统计报表
        /// </summary>
        const string RptStatisticsDaily = "ReportViewer/ReportView_Daily.rpt";
        /// <summary>
        /// 月统计报表
        /// </summary>
        const string RptStatisticsMonth = "ReportViewer/ReportView_Month.rpt";
        /// <summary>
        /// 日汇总报表
        /// </summary>
        const string RptStatisticsDailyGather = "ReportViewer/ReportView_DailyGather.rpt";
        /// <summary>
        /// 月汇总报表
        /// </summary>
        const string RptStatisticsMonthGather = "ReportViewer/ReportView_MonthGather.rpt";
        /// <summary>
        /// 月卡报表
        /// </summary>
        const string RptMonthCardInfo = "ReportViewer/ReportView_MonthCardInfo.rpt";
        /// <summary>
        /// 月卡报表
        /// </summary>
        const string RptParkVisitorInfo = "ReportViewer/ReportView_ParkVisitorInfo.rpt";

        List<BaseVillage> GetCurrLoginVillages
        {
            get
            {
                if (Session["SmartSystem_LoginUser_ValidVillage"] != null)
                {
                    return (List<BaseVillage>)Session["SmartSystem_LoginUser_ValidVillage"];
                }
                return new List<BaseVillage>();
            }
        }
        List<ReportParkEvent> EmptyTransferEvent(List<ParkEvent> eventlist)
        {
            List<ReportParkEvent> reportparkeventlist = new List<ReportParkEvent> ();
            if (eventlist == null || eventlist.Count == 0)
                return reportparkeventlist;
            foreach (ParkEvent pevent in eventlist)
            {
                ReportParkEvent reportparkorder = new ReportParkEvent
                {
                    CardNo = (string.IsNullOrEmpty(pevent.CardNo) ? "-" : pevent.CardNo),
                    CardNum = (string.IsNullOrEmpty(pevent.CardNum) ? "-" : pevent.CardNum),
                    CarModelID = (string.IsNullOrEmpty(pevent.CarModelID) ? "-" : pevent.CarModelID),
                    CarModelName = (string.IsNullOrEmpty(pevent.CarModelName) ? "-" : pevent.CarModelName),
                    CarTypeID = (string.IsNullOrEmpty(pevent.CarTypeID) ? "-" : pevent.CarTypeID),
                    CarTypeName = (string.IsNullOrEmpty(pevent.CarTypeName) ? "-" : pevent.CarTypeName),
                    DataStatus = (pevent.DataStatus == null ? 0 : pevent.DataStatus),
                    EmployeeName = (string.IsNullOrEmpty(pevent.EmployeeName) ? "-" : pevent.EmployeeName),
                    LastUpdateTime = pevent.LastUpdateTime,
                    EventID = (pevent.EventID == null ? 1 : pevent.EventID),
                    EventName = (string.IsNullOrEmpty(pevent.EventName) ? "-" : pevent.EventName),
                    GateID = (string.IsNullOrEmpty(pevent.GateID) ? "-" : pevent.GateID),
                    GateName = (string.IsNullOrEmpty(pevent.GateName) ? "-" : pevent.GateName),
                    HaveUpdate = (pevent.HaveUpdate == null ? 0 : pevent.HaveUpdate),
                    ID = pevent.ID,
                    IORecordID = (string.IsNullOrEmpty(pevent.IORecordID) ? "-" : pevent.IORecordID),
                    IOState = (pevent.IOState == null ? 1 : pevent.IOState),
                    IOStateName = (string.IsNullOrEmpty(pevent.IOStateName) ? "-" : pevent.IOStateName),
                    Operator = (string.IsNullOrEmpty(pevent.Operator) ? "-" : pevent.Operator),
                    OperatorID = (string.IsNullOrEmpty(pevent.OperatorID) ? "-" : pevent.OperatorID),
                    ParkingID = (string.IsNullOrEmpty(pevent.ParkingID) ? "-" : pevent.ParkingID),
                    ParkingName = (string.IsNullOrEmpty(pevent.ParkingName) ? "-" : pevent.ParkingName),
                    PictureName = (string.IsNullOrEmpty(pevent.PictureName) ? "-" : pevent.PictureName),
                    PlateColor = pevent.PlateColor,
                    PlateNumber = (string.IsNullOrEmpty(pevent.PlateNumber) ? "-" : pevent.PlateNumber),
                    RecordID = (string.IsNullOrEmpty(pevent.RecordID) ? "-" : pevent.RecordID),
                    RecTime = pevent.RecTime,
                    Remark = (string.IsNullOrEmpty(pevent.Remark) ? "-" : pevent.Remark),
                };
                reportparkeventlist.Add(reportparkorder);
            }
            return reportparkeventlist;
        }

        List<ReportParkOrder> EmptyTransferOrder(List<ParkOrder> orderlist)
        {
            List<ReportParkOrder> reportparkorderlist = new List<ReportParkOrder>();
            if (orderlist == null || orderlist.Count == 0)
                return reportparkorderlist;
            
            foreach (ParkOrder order in orderlist)
            {
                ReportParkOrder reportparkorder = new ReportParkOrder
                {
                    Amount = order.Amount,
                    CarderateID = (string.IsNullOrEmpty(order.CarderateID) ? "-" : order.CarderateID),
                    CardNo = (string.IsNullOrEmpty(order.CardNo) ? "-" : order.CardNo),
                    CashMoney = order.CashMoney,
                    CashTime = order.CashTime,
                    DataStatus = order.DataStatus,
                    DiscountAmount = order.DiscountAmount,
                    EmployeeName = (string.IsNullOrEmpty(order.EmployeeName) ? "-" : order.EmployeeName),
                    FeeRuleID = order.FeeRuleID,
                    HaveUpdate = order.HaveUpdate,
                    ID = order.ID,
                    LastUpdateTime = order.LastUpdateTime,
                    MobilePhone = order.MobilePhone,
                    NewMoney = order.NewMoney,
                    OldUserBegin = order.OldUserBegin,
                    NewUsefulDate = order.NewUsefulDate,
                    NewUserBegin = order.NewUserBegin,
                    OldMoney = order.OldMoney,
                    OldUserulDate = order.OldUserulDate,
                    OnlineOrderNo = (string.IsNullOrEmpty(order.OnlineOrderNo) ? "-" : order.OnlineOrderNo),
                    PKID = (string.IsNullOrEmpty(order.PKID) ? "-" : order.PKID),
                    PKName = (string.IsNullOrEmpty(order.PKName) ? "-" : order.PKName),
                    OnlineUserID = (string.IsNullOrEmpty(order.OnlineUserID) ? "-" : order.OnlineUserID),
                    RecordID = (string.IsNullOrEmpty(order.RecordID) ? "-" : order.RecordID),
                    TagID = (string.IsNullOrEmpty(order.TagID) ? "-" : order.TagID),
                    UserID = (string.IsNullOrEmpty(order.UserID) ? "-" : order.UserID),
                    Operator = (string.IsNullOrEmpty(order.Operator) ? "-" : order.Operator),
                    OrderNo = (string.IsNullOrEmpty(order.OrderNo) ? "-" : order.OrderNo),
                    OrderSource = order.OrderSource,
                    OrderSourceName = (string.IsNullOrEmpty(order.OrderSourceName) ? "-" : order.OrderSourceName),
                    OrderType = order.OrderType,
                    PlateNumber = (string.IsNullOrEmpty(order.PlateNumber) ? "-" : order.PlateNumber),
                    Remark = (string.IsNullOrEmpty(order.Remark) ? "-" : order.Remark),
                    PayWayName = (string.IsNullOrEmpty(order.PayWayName) ? "-" : order.PayWayName),
                    PayWay = order.PayWay,
                    UnPayAmount = order.UnPayAmount,
                    Status = order.Status,
                    OrderTime = order.OrderTime,
                    OrderTypeName = order.OrderTypeName,
                    PayAmount = order.PayAmount,
                    PayTime = order.PayTime,
                    EntranceTime = order.EntranceTime,
                    ExitTime = order.ExitTime,
                    LongTime = order.LongTime,
                    ZZWeight = (order.ZZWeight == null ? "0" : order.ZZWeight),
                    NetWeight = order.NetWeight == null ? "0" : order.NetWeight,
                    Tare = order.Tare == null ? "0" : order.Tare
                };
                reportparkorderlist.Add(reportparkorder);
            }
            return reportparkorderlist;
        }
        void EmptyTransferCarDerate(List<ParkCarDerate> carderate)
        {
            if (carderate == null && carderate.Count == 0)
                return;

            foreach (var c in carderate)
            {
                c.AreaID = (string.IsNullOrEmpty(c.AreaID) ? "-" : c.AreaID);
                c.CarDerateID = (string.IsNullOrEmpty(c.CarDerateID) ? "-" : c.CarDerateID);
                c.CarDerateNo = (string.IsNullOrEmpty(c.CarDerateNo) ? "-" : c.CarDerateNo);
                c.CardNo = (string.IsNullOrEmpty(c.CardNo) ? "-" : c.CardNo);
                c.DerateID = (string.IsNullOrEmpty(c.DerateID) ? "-" : c.DerateID);
                c.PKID = (string.IsNullOrEmpty(c.PKID) ? "-" : c.PKID);
                c.PlateNumber = (string.IsNullOrEmpty(c.PlateNumber) ? "-" : c.PlateNumber);
                c.RuleName = (string.IsNullOrEmpty(c.RuleName) ? "-" : c.RuleName);
                c.SellerName = (string.IsNullOrEmpty(c.SellerName) ? "-" : c.SellerName);
                c.StatusName = (string.IsNullOrEmpty(c.StatusName) ? "-" : c.StatusName);
            }
        }

        void EmptyTransfer(List<ParkIORecord> iorecordlist)
        {
            if (iorecordlist == null || iorecordlist.Count == 0)
                return;

            foreach (ParkIORecord iorecord in iorecordlist)
            {
                iorecord.Remark = (string.IsNullOrEmpty(iorecord.Remark) ? "-" : iorecord.Remark);
                iorecord.ReleaseTypeName = (string.IsNullOrEmpty(iorecord.ReleaseTypeName) ? "-" : iorecord.ReleaseTypeName);
                iorecord.RecordID = (string.IsNullOrEmpty(iorecord.RecordID) ? "-" : iorecord.RecordID);
                iorecord.PKName = (string.IsNullOrEmpty(iorecord.PKName) ? "-" : iorecord.PKName);
                iorecord.ParkingID = (string.IsNullOrEmpty(iorecord.ParkingID) ? "-" : iorecord.ParkingID);
                iorecord.OutOperatorName = (string.IsNullOrEmpty(iorecord.OutOperatorName) ? "-" : iorecord.OutOperatorName);
                iorecord.OutGateName = (string.IsNullOrEmpty(iorecord.OutGateName) ? "-" : iorecord.OutGateName);
                iorecord.MobilePhone = (string.IsNullOrEmpty(iorecord.MobilePhone) ? "-" : iorecord.MobilePhone);
                iorecord.LongTime = (string.IsNullOrEmpty(iorecord.LongTime) ? "-" : iorecord.LongTime);
                iorecord.InOperatorName = (string.IsNullOrEmpty(iorecord.InOperatorName) ? "-" : iorecord.InOperatorName);
                iorecord.InimgData = (string.IsNullOrEmpty(iorecord.InimgData) ? "-" : iorecord.InimgData);
                iorecord.InGateName = (string.IsNullOrEmpty(iorecord.InGateName) ? "-" : iorecord.InGateName);
                iorecord.ExitOperatorID = (string.IsNullOrEmpty(iorecord.ExitOperatorID) ? "-" : iorecord.ExitOperatorID);
                iorecord.ExitImage = (string.IsNullOrEmpty(iorecord.ExitImage) ? "-" : iorecord.ExitImage);
                iorecord.ExitGateID = (string.IsNullOrEmpty(iorecord.ExitGateID) ? "-" : iorecord.ExitGateID);
                iorecord.ExitCertificateNo = (string.IsNullOrEmpty(iorecord.ExitCertificateNo) ? "-" : iorecord.ExitCertificateNo);
                iorecord.ExitcertificateImage = (string.IsNullOrEmpty(iorecord.ExitcertificateImage) ? "-" : iorecord.ExitcertificateImage);
                iorecord.EntranceOperatorID = (string.IsNullOrEmpty(iorecord.EntranceOperatorID) ? "-" : iorecord.EntranceOperatorID);
                iorecord.EntranceImage = (string.IsNullOrEmpty(iorecord.EntranceImage) ? "-" : iorecord.EntranceImage);
                iorecord.EntranceGateID = (string.IsNullOrEmpty(iorecord.EntranceGateID) ? "-" : iorecord.EntranceGateID);
                iorecord.EntranceCertificateNo = (string.IsNullOrEmpty(iorecord.EntranceCertificateNo) ? "-" : iorecord.EntranceCertificateNo);
                iorecord.EntranceCertificateImage = (string.IsNullOrEmpty(iorecord.EntranceCertificateImage) ? "-" : iorecord.EntranceCertificateImage);
                iorecord.EmployeeName = (string.IsNullOrEmpty(iorecord.EmployeeName) ? "-" : iorecord.EmployeeName);
                iorecord.DerateID = (string.IsNullOrEmpty(iorecord.DerateID) ? "-" : iorecord.DerateID);
                iorecord.CarTypeName = (string.IsNullOrEmpty(iorecord.CarTypeName) ? "-" : iorecord.CarTypeName);
                iorecord.CarTypeID = (string.IsNullOrEmpty(iorecord.CarTypeID) ? "-" : iorecord.CarTypeID);
                iorecord.CarModelName = (string.IsNullOrEmpty(iorecord.CarModelName) ? "-" : iorecord.CarModelName);
                iorecord.CarModelID = (string.IsNullOrEmpty(iorecord.CarModelID) ? "-" : iorecord.CarModelID);
                iorecord.CardNumb = (string.IsNullOrEmpty(iorecord.CardNumb) ? "-" : iorecord.CardNumb);
                iorecord.CardNo = (string.IsNullOrEmpty(iorecord.CardNo) ? "-" : iorecord.CardNo);
                iorecord.CardID = (string.IsNullOrEmpty(iorecord.CardID) ? "-" : iorecord.CardID);
                iorecord.AreaName = (string.IsNullOrEmpty(iorecord.AreaName) ? "-" : iorecord.AreaName);
                iorecord.AreaID = (string.IsNullOrEmpty(iorecord.AreaID) ? "-" : iorecord.AreaID);
                iorecord.NetWeight=iorecord.NetWeight;
                iorecord.ZZWeight=iorecord.ZZWeight;
                iorecord.Tare=iorecord.Tare;
                if (string.IsNullOrEmpty(iorecord.PlateNumber))
                {
                    iorecord.PlateNumber = "-";
                }
                else
                {
                    if (iorecord.PlateNumber.Contains("无车牌"))
                    {
                        iorecord.PlateNumber = "无车牌";
                    }
                }
            }
        }

        void EmptyTransfer(List<MonthCardInfoModel> list)
        {
            if (list == null || list.Count == 0)
                return;
            foreach (var m in list)
            {
                if (string.IsNullOrWhiteSpace(m.PKName))
                {
                    m.PKName = "-";
                }
                if (string.IsNullOrWhiteSpace(m.Addr))
                {
                    m.Addr = "-";
                }
                if (string.IsNullOrWhiteSpace(m.CarTypeName))
                {
                    m.CarTypeName = "-";
                }
                if (string.IsNullOrWhiteSpace(m.EmployeeName))
                {
                    m.EmployeeName = "-";
                }
                if (string.IsNullOrWhiteSpace(m.Mobile))
                {
                    m.Mobile = "-";
                }
                if (string.IsNullOrWhiteSpace(m.PlateNumber))
                {
                    m.PlateNumber = "-";
                }
                if (string.IsNullOrWhiteSpace(m.strEndTime))
                {
                    m.strEndTime = "-";
                }
                if (string.IsNullOrWhiteSpace(m.strStartTime))
                {
                    m.strStartTime = "-";
                }
            }
        }
        ReportDocument rdocument = new ReportDocument();
        protected void Page_Load(object sender, EventArgs e)
        {
            InParams paras = (InParams)System.Web.HttpContext.Current.Session["QueryParams"];
            List<ParkIORecord> iorecordlist = new List<ParkIORecord> ();
            if (paras != null)
            {
                switch (paras.ReportType)
                {
                    //在场车辆
                    case ReportType.Presence:
                        if (paras.PageIndex > 0)
                        {
                            var v = StatisticsServices.Search_Presence(paras, paras.PageSize, paras.PageIndex);
                            if( v!= null)
                                iorecordlist = v.IORecordsList;
                        }
                        else
                        {
                            iorecordlist = StatisticsServices.Search_Presence(paras);
                        }
                        EmptyTransfer(iorecordlist);
                        rdocument.Load(Server.MapPath(RptPresence));
                        rdocument.SetDataSource(iorecordlist);
                        break;
                    //在场无牌车辆
                    case ReportType.NoPlateNumber:
                        if (paras.PageIndex > 0)
                        {
                            var v = StatisticsServices.Search_NoPlateNumber(paras, paras.PageSize, paras.PageIndex);
                            if( v!= null)
                                iorecordlist = v.IORecordsList;
                        }
                        else
                        {
                            iorecordlist = StatisticsServices.Search_NoPlateNumber(paras);
                        }
                        EmptyTransfer(iorecordlist);
                        rdocument.Load(Server.MapPath(RptNoPlateNumber));
                        rdocument.SetDataSource(iorecordlist);
                        break;
                    //进出记录
                    case ReportType.InOut:
                        if (paras.PageIndex > 0)
                        {
                            var v = StatisticsServices.Search_InOutRecords(paras, paras.PageSize, paras.PageIndex);
                            if( v!= null)
                                iorecordlist = v.IORecordsList;
                        }
                        else
                        {
                            iorecordlist = StatisticsServices.Search_InOutRecords(paras);
                        }
                        EmptyTransfer(iorecordlist);
                        rdocument.Load(Server.MapPath(RptInOut));
                        rdocument.SetDataSource(iorecordlist);
                        break;
                    //异常放行
                    case ReportType.ExceptionRelease:
                        if (paras.PageIndex > 0)
                        {
                            var v = StatisticsServices.Search_ExceptionRelease(paras, paras.PageSize, paras.PageIndex);
                            if( v!= null)
                                iorecordlist = v.IORecordsList;
                        }
                        else
                        {
                            iorecordlist = StatisticsServices.Search_ExceptionRelease(paras);
                        }
                        EmptyTransfer(iorecordlist);
                        rdocument.Load(Server.MapPath(RptExceptionRelease));
                        rdocument.SetDataSource(iorecordlist);
                        break;
                    //通道事件
                    case ReportType.GateEvent:
                        List<ParkEvent> parkeventlist = new List<ParkEvent> ();
                        if (paras.PageIndex > 0)
                        {
                            var v = StatisticsServices.Search_GateEvents(paras, paras.PageSize, paras.PageIndex);
                            if( v!= null)
                                parkeventlist = v.GateEventList;
                        }
                        else
                        {
                            parkeventlist = StatisticsServices.Search_GateEvents(paras);
                        }
                        rdocument.Load(Server.MapPath(RptGateEvent));
                        rdocument.SetDataSource(EmptyTransferEvent(parkeventlist));
                        break;
                    //当班统计
                    case ReportType.OnDuty:
                        List<Statistics_ChangeShift> changeshiftlist = new System.Collections.Generic.List<Statistics_ChangeShift> ();
                        if (paras.PageIndex > 0)
                        {
                            var v = StatisticsServices.Search_OnDuty(paras, paras.PageSize, paras.PageIndex);
                            if( v!= null)
                                changeshiftlist = v.OnDutyList;
                        }
                        else
                        {
                            changeshiftlist = StatisticsServices.Search_OnDuty(paras);
                        }
                        rdocument.Load(Server.MapPath(RptOnDuty));
                        rdocument.SetDataSource(changeshiftlist);
                        break;
                    //订单明细
                    case ReportType.Order:
                        List<ParkOrder> parkorderlist = new List<ParkOrder> ();
                        if (paras.PageIndex > 0)
                        {
                            var v = StatisticsServices.Search_Orders(paras, paras.PageSize, paras.PageIndex);
                            if( v!= null)
                                parkorderlist = v.OrderList;
                        }
                        else
                        {
                            parkorderlist = StatisticsServices.Search_Orders(paras);
                        }
                        rdocument.Load(Server.MapPath(RptOrder));
                        rdocument.SetDataSource(EmptyTransferOrder(parkorderlist));
                        break;
                    //月卡续期
                    case ReportType.CardExtension:
                        List<ParkOrder> cardextensionorderlist = new List<ParkOrder> ();
                        if (paras.PageIndex > 0)
                        {
                            var v = StatisticsServices.Search_CardExtension(paras, paras.PageSize, paras.PageIndex);
                            if( v!= null)
                                cardextensionorderlist = v.OrderList;
                        }
                        else
                        {
                            cardextensionorderlist = StatisticsServices.Search_CardExtension(paras);
                        }
                        rdocument.Load(Server.MapPath(RptCardExtension));
                        rdocument.SetDataSource(EmptyTransferOrder(cardextensionorderlist));
                        break;
                    //临停缴费
                    case ReportType.TempPay:
                        List<ParkOrder> temppayorderlist = new List<ParkOrder> ();
                        if (paras.PageIndex > 0)
                        {
                            var v = StatisticsServices.Search_TempPays(paras, paras.PageSize, paras.PageIndex);
                            if( v!= null)
                                temppayorderlist = v.OrderList;
                        }
                        else
                        {
                            temppayorderlist = StatisticsServices.Search_TempPays(paras);
                        }
                        rdocument.Load(Server.MapPath(RptTempPay));
                        rdocument.SetDataSource(EmptyTransferOrder(temppayorderlist));
                        break;
                    //商家优免
                    case ReportType.Coupon:
                        List<ParkCarDerate> parkcarderatelist = new List<ParkCarDerate> ();
                        if (paras.PageIndex > 0)
                        {
                            var v = StatisticsServices.Search_CarDerates(paras, paras.PageSize, paras.PageIndex);
                            if( v!= null)
                                parkcarderatelist = v.CarDerateList;
                        }
                        else
                        {
                            parkcarderatelist = StatisticsServices.Search_CarDerates(paras);
                        }
                        EmptyTransferCarDerate(parkcarderatelist);
                        rdocument.Load(Server.MapPath(RptCoupon));
                        rdocument.SetDataSource(parkcarderatelist);
                        break;
                    //日报表
                    case ReportType.DailyFee:
                        List<Statistics_Gather> dailystatisticslist = new List<Statistics_Gather> ();
                        if (paras.PageIndex > 0)
                        {
                            var v = StatisticsServices.Search_DailyStatistics(paras, paras.PageSize, paras.PageIndex);
                            if( v!= null)
                                dailystatisticslist = v.StatisticsGatherList;
                        }
                        else
                        {
                            dailystatisticslist = StatisticsServices.Search_DailyStatistics(paras);
                        }
                        rdocument.Load(Server.MapPath(RptStatisticsDaily));
                        rdocument.SetDataSource(dailystatisticslist);
                        break;
                    //月报表
                    case ReportType.MonthFee:
                        List<Statistics_Gather> monthstatisticslist = new List<Statistics_Gather> ();
                        if (paras.PageIndex > 0)
                        {
                            var v = StatisticsServices.Search_MonthStatistics(paras, paras.PageSize, paras.PageIndex);
                            if( v!= null)
                                monthstatisticslist = v.StatisticsGatherList;
                        }
                        else
                        {
                            monthstatisticslist = StatisticsServices.Search_MonthStatistics(paras);
                        }
                        rdocument.Load(Server.MapPath(RptStatisticsMonth));
                        rdocument.SetDataSource(monthstatisticslist);
                        break;
                    //日汇总报表
                    case ReportType.DailyGather:
                        List<Statistics_Gather> dailystatisticsgatherlist = new List<Statistics_Gather> ();
                        if (paras.PageIndex > 0)
                        {
                            var v = StatisticsServices.Search_DailyStatistics(paras, paras.PageSize, paras.PageIndex);
                            if( v!= null)
                                dailystatisticsgatherlist = v.StatisticsGatherList;
                        }
                        else
                        {
                            dailystatisticsgatherlist = StatisticsServices.Search_DailyStatistics(paras);
                        }
                        rdocument.Load(Server.MapPath(RptStatisticsDailyGather));
                        rdocument.SetDataSource(dailystatisticsgatherlist);
                        break;
                    //月汇总报表
                    case ReportType.MonthGather:
                        List<Statistics_Gather> monthstatisticsgatherlist = new List<Statistics_Gather> ();
                        if (paras.PageIndex > 0)
                        {
                            var v = StatisticsServices.Search_MonthStatistics(paras, paras.PageSize, paras.PageIndex);
                            if( v!= null)
                                monthstatisticsgatherlist = v.StatisticsGatherList;
                        }
                        else
                        {
                            monthstatisticsgatherlist = StatisticsServices.Search_MonthStatistics(paras);
                        }
                        rdocument.Load(Server.MapPath(RptStatisticsMonthGather));
                        rdocument.SetDataSource(monthstatisticsgatherlist);
                        break;
                    //月卡信息
                    case ReportType.MonthCardInfo:
                        List<MonthCardInfoModel> monthcardinfolist = new List<MonthCardInfoModel>();
                        if (paras.PageIndex > 0)
                        {
                            var v = StatisticsServices.Search_MonthCardInfo(paras, paras.PageSize, paras.PageIndex);
                            if (v != null)
                                monthcardinfolist = v.MonthCardList;
                        }
                        else
                        {
                            monthcardinfolist = StatisticsServices.Search_MonthCardInfo(paras);
                        }
                        EmptyTransfer(monthcardinfolist);
                        rdocument.Load(Server.MapPath(RptMonthCardInfo));
                        rdocument.SetDataSource(monthcardinfolist);
                        break;
                    //访客信息
                    case ReportType.ParkVisitorInfo:
                        List<ParkVisitorReportModel> models = new List<ParkVisitorReportModel>();
                        VisitorReportCondition condition = (VisitorReportCondition)System.Web.HttpContext.Current.Session["VisitorReportCondition"];
                        int pageIndex = paras.PageIndex < 0 ? 1 : paras.PageIndex;
                        int pageSize = paras.PageIndex < 0 ? int.MaxValue : paras.PageSize;
                        Pagination result = StatisticsServices.QueryParkVisitorReport(condition, pageSize, pageIndex);
                        if (result != null)
                        {
                            models = result.VisitorList;
                            foreach (var item in models) {
                                if (string.IsNullOrWhiteSpace(item.VisitorName)) {
                                    item.VisitorName = "-";
                                }
                            }
                        }

                        rdocument.Load(Server.MapPath(RptParkVisitorInfo));
                        rdocument.SetDataSource(models);
                        break;
                    default:
                        break;
                }
            }
            this.StatisticsReportViewer1.ReportSource = rdocument;
            this.StatisticsReportViewer1.DataBind();
          
        }
        protected void Page_UnLoad(object sender, EventArgs e)
        {
            rdocument.Close();
            this.Dispose();
            this.ClearChildState();
        }
    }
}