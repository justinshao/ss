using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.OmnipotentCardWebUI.Controllers;
using System.Text;
using Common.Services;
using Common.Services.Statistics;
using Common.Entities.Other;
using Common.Utilities;
using Common.Entities;
using Common.Entities.Statistics;
using Common.Entities.Enum;
using Common.Services.Park;
using Common.Utilities.Helpers;
using CrystalDecisions.CrystalReports.Engine;
using Common.Entities.Parking;
using Common.Entities.Condition;
using Common.Entities.Order;
using Common.DataAccess;
using System.Data.Common;
using Common.Entities.PG;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;

namespace SmartSystem.OmnipotentCardWebUI.Areas.Statistics.Controllers
{
    [CheckPurview(Roles = "PK0104,PK0105")]
    public class StatisticsController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        #region 在场车辆
        /// <summary>
        /// 在场车辆
        /// </summary>
        /// <returns></returns>
        [CheckPurview(Roles = "PK010401")]
        public ActionResult Presence()
        {
            return View();
        }
        /// <summary>
        /// 获取在场车辆
        /// </summary>
        /// <returns></returns>
        public string Search_Presence()
        {
            try
            {
                int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int rows = string.IsNullOrEmpty(Request.Params["rows"]) ? 0 : int.Parse(Request.Params["rows"]);
                InParams paras = new InParams
                {
                    ParkingID = Request.Params["parkingid"],
                    CardType = Request.Params["cardtype"],
                    PlateNumber = Request.Params["platenumber"],
                    StartTime = DateTime.Parse(Request.Params["starttime"]),
                    EndTime = DateTime.Parse(Request.Params["endtime"]),
                    InGateID = Request.Params["entrancegateid"],
                    ReportType = ReportType.Presence
                };
                if (string.IsNullOrWhiteSpace(paras.ParkingID))
                    return string.Empty;
                Pagination pagination = StatisticsServices.Search_Presence(paras, rows, page);
                StringBuilder sb = new StringBuilder();
                string str = JsonHelper.GetJsonString(pagination.IORecordsList);
                sb.Append("{");
                sb.Append("\"total\":" + pagination.Total + ",");
                sb.Append("\"rows\":" + str + ",");
                sb.Append("\"index\":" + rows);
                sb.Append("}");
                return sb.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public string Export_Presence()
        {
            ReportDocument rdocument = new ReportDocument();
            List<ParkIORecord> iorecordlist = new List<ParkIORecord> ();
            InParams paras = new InParams
            {
                ParkingID = Request.Params["parkingid"],
                CardType = Request.Params["cardtype"],
                PlateNumber = Request.Params["platenumber"],
                StartTime = DateTime.Parse(Request.Params["starttime"]),
                EndTime = DateTime.Parse(Request.Params["endtime"]),
                InGateID = Request.Params["entrancegateid"],
                ReportType = ReportType.Presence
            };
            if (string.IsNullOrWhiteSpace(paras.ParkingID))
                return string.Empty;
            iorecordlist = StatisticsServices.Search_Presence(paras);
            EmptyTransfer(iorecordlist);
            rdocument.Load(Server.MapPath("../../Report/ReportViewer/ReportView_Presence.rpt"));
            rdocument.SetDataSource(iorecordlist);
            CrystalDecisions.Shared.DiskFileDestinationOptions DiskOpts = new CrystalDecisions.Shared.DiskFileDestinationOptions();
            rdocument.ExportOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;
            rdocument.ExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.Excel;
            string filename = "Presence_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            DiskOpts.DiskFileName = Server.MapPath("~/") + @"Report\\ReportFile\\" + filename;
            rdocument.ExportOptions.DestinationOptions = DiskOpts;
            rdocument.Export();
            return filename;
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

        [HttpPost]
        public JsonResult SetExit(string ID)
        {
            try
            {
                bool result = StatisticsServices.SetExit(ID);
                if (!result) throw new MyException("出场失败");
                return Json(MyResult.Success());

            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "出场失败");
                return Json(MyResult.Error("出场失败"));
            }
        }
        #endregion

        #region 在场无牌车辆
        /// <summary>
        /// 在场无牌车辆
        /// </summary>
        /// <returns></returns>
        [CheckPurview(Roles = "PK010402")]
        public ActionResult ReportNoPlateNumber()
        {
            return View();
        }
        /// <summary>
        /// 获取在场无牌车辆记录
        /// </summary>
        /// <returns></returns>
        public string Search_NoPlateNumber()
        {
            try
            {
                int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int rows = string.IsNullOrEmpty(Request.Params["rows"]) ? 0 : int.Parse(Request.Params["rows"]);
                InParams paras = new InParams
                {
                    ParkingID = Request.Params["parkingid"],
                    InGateID = Request.Params["ingateid"],
                    StartTime = DateTime.Parse(Request.Params["starttime"]),
                    EndTime = DateTime.Parse(Request.Params["endtime"]),
                    ReportType = ReportType.NoPlateNumber
                };
                if (string.IsNullOrWhiteSpace(paras.ParkingID))
                    return string.Empty;
                Pagination pagination = StatisticsServices.Search_NoPlateNumber(paras, rows, page);
                StringBuilder sb = new StringBuilder();
                string str = JsonHelper.GetJsonString(pagination.IORecordsList);
                sb.Append("{");
                sb.Append("\"total\":" + pagination.Total + ",");
                sb.Append("\"rows\":" + str + ",");
                sb.Append("\"index\":" + rows);
                sb.Append("}");
                return sb.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public string Export_NoPlateNumber()
        {
            ReportDocument rdocument = new ReportDocument();
            List<ParkIORecord> iorecordlist = new List<ParkIORecord>();
            InParams paras = new InParams
            {
                ParkingID = Request.Params["parkingid"],
                StartTime = DateTime.Parse(Request.Params["starttime"]),
                EndTime = DateTime.Parse(Request.Params["endtime"]),
                InGateID = Request.Params["entrancegateid"],
                ReportType = ReportType.NoPlateNumber
            };
            if (string.IsNullOrWhiteSpace(paras.ParkingID))
                return string.Empty;
            iorecordlist = StatisticsServices.Search_NoPlateNumber(paras);
            EmptyTransfer(iorecordlist);
            rdocument.Load(Server.MapPath("../../Report/ReportViewer/ReportView_NoPlateNumber.rpt"));
            rdocument.SetDataSource(iorecordlist);
            CrystalDecisions.Shared.DiskFileDestinationOptions DiskOpts = new CrystalDecisions.Shared.DiskFileDestinationOptions();
            rdocument.ExportOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;
            rdocument.ExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.Excel;
            string filename = "NoPlateNumber_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            DiskOpts.DiskFileName = Server.MapPath("~/") + @"Report\\ReportFile\\" + filename;
            rdocument.ExportOptions.DestinationOptions = DiskOpts;
            rdocument.Export();
            return filename;
        }
        #endregion

        #region 进出记录
        /// <summary>
        /// 进出记录
        /// </summary>
        /// <returns></returns>
        [CheckPurview(Roles = "PK010403")]
        public ActionResult ReportInOut()
        {
            return View();
        }
        /// <summary>
        /// 查询进出记录
        /// </summary>
        /// <returns></returns>
        public string Search_InOut()
        {
            try
            {
                int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int rows = string.IsNullOrEmpty(Request.Params["rows"]) ? 0 : int.Parse(Request.Params["rows"]);
                InParams paras = new InParams
                {
                    ParkingID = Request.Params["parkingid"],
                    CardType = Request.Params["cardtypeid"],
                    CarType = Request.Params["cartypeid"],
                    OutGateID = Request.Params["exitgateid"],
                    InGateID = Request.Params["entrancegateid"],
                    OutOperator = Request.Params["exitoperatorid"],
                    ReleaseType = int.Parse(Request.Params["releasetype"]),
                    AreaID = Request.Params["areaid"],
                    StartTime = DateTime.Parse(Request.Params["starttime"]),
                    EndTime = DateTime.Parse(Request.Params["endtime"]),
                    IsExit = int.Parse(Request.Params["isexit"]),
                    PlateNumber = Request.Params["platenumber"],
                    Owner = Request.Params["owner"],
                    ReportType = ReportType.InOut
                };
                if (string.IsNullOrWhiteSpace(paras.ParkingID))
                    return string.Empty;
                Pagination pagination = StatisticsServices.Search_InOutRecords(paras, rows, page);
                StringBuilder sb = new StringBuilder();
                string str = JsonHelper.GetJsonString(pagination.IORecordsList);
                sb.Append("{");
                sb.Append("\"total\":" + pagination.Total + ",");
                sb.Append("\"rows\":" + str + ",");
                sb.Append("\"index\":" + rows);
                sb.Append("}");
                return sb.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public string Export_InOut()
        {
            ReportDocument rdocument = new ReportDocument();
            List<ParkIORecord> iorecordlist = new List<ParkIORecord>();
            InParams paras = new InParams
            {
                ParkingID = Request.Params["parkingid"],
                CardType = Request.Params["cardtypeid"],
                CarType = Request.Params["cartypeid"],
                OutGateID = Request.Params["exitgateid"],
                InGateID = Request.Params["entrancegateid"],
                OutOperator = Request.Params["exitoperatorid"],
                ReleaseType = int.Parse(Request.Params["releasetype"]),
                AreaID = Request.Params["areaid"],
                StartTime = DateTime.Parse(Request.Params["starttime"]),
                EndTime = DateTime.Parse(Request.Params["endtime"]),
                IsExit = int.Parse(Request.Params["isexit"]),
                PlateNumber = Request.Params["platenumber"],
                Owner = Request.Params["owner"],
                ReportType = ReportType.InOut
            };
            if (string.IsNullOrWhiteSpace(paras.ParkingID))
                return string.Empty;
            iorecordlist = StatisticsServices.Search_InOutRecords(paras);
            EmptyTransfer(iorecordlist);
            rdocument.Load(Server.MapPath("../../Report/ReportViewer/ReportView_InOut.rpt"));
            rdocument.SetDataSource(iorecordlist);
            CrystalDecisions.Shared.DiskFileDestinationOptions DiskOpts = new CrystalDecisions.Shared.DiskFileDestinationOptions();
            rdocument.ExportOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;
            rdocument.ExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.Excel;
            string filename = "InOut_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            DiskOpts.DiskFileName = Server.MapPath("~/") + @"Report\\ReportFile\\" + filename;
            rdocument.ExportOptions.DestinationOptions = DiskOpts;
            rdocument.Export();
            return filename;
        }
        #endregion

        #region 异常放行
        /// <summary>
        /// 异常放行
        /// </summary>
        /// <returns></returns>
        [CheckPurview(Roles = "PK010404")]
        public ActionResult ReportExceptionRelease()
        {
            return View();
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        public string Search_ExceptionRelease()
        {
            try
            {
                int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int rows = string.IsNullOrEmpty(Request.Params["rows"]) ? 0 : int.Parse(Request.Params["rows"]);
                InParams paras = new InParams
                {
                    ParkingID = Request.Params["parkingid"],
                    PlateNumber = Request.Params["platenumber"],
                    OutGateID = Request.Params["exitgateid"],
                    StartTime = DateTime.Parse(Request.Params["starttime"]),
                    EndTime = DateTime.Parse(Request.Params["endtime"]),
                    ReportType = ReportType.ExceptionRelease
                };
                if (string.IsNullOrWhiteSpace(paras.ParkingID))
                    return string.Empty;
                Pagination pagination = StatisticsServices.Search_ExceptionRelease(paras, rows, page);
                StringBuilder sb = new StringBuilder();
                string str = JsonHelper.GetJsonString(pagination.IORecordsList);
                sb.Append("{");
                sb.Append("\"total\":" + pagination.Total + ",");
                sb.Append("\"rows\":" + str + ",");
                sb.Append("\"index\":" + rows);
                sb.Append("}");
                return sb.ToString();
            }
            catch 
            {
                return string.Empty;
            }
        }

        public string Export_Exception()
        {
            ReportDocument rdocument = new ReportDocument();
            List<ParkIORecord> iorecordlist = new List<ParkIORecord>();
            InParams paras = new InParams
            {
                ParkingID = Request.Params["parkingid"],
                PlateNumber = Request.Params["platenumber"],
                OutGateID = Request.Params["exitgateid"],
                StartTime = DateTime.Parse(Request.Params["starttime"]),
                EndTime = DateTime.Parse(Request.Params["endtime"]),
                ReportType = ReportType.ExceptionRelease
            };
            if (string.IsNullOrWhiteSpace(paras.ParkingID))
                return string.Empty;
            iorecordlist = StatisticsServices.Search_ExceptionRelease(paras);
            EmptyTransfer(iorecordlist);
            rdocument.Load(Server.MapPath("../../Report/ReportViewer/ReportView_ExceptionRelease.rpt"));
            rdocument.SetDataSource(iorecordlist);
            CrystalDecisions.Shared.DiskFileDestinationOptions DiskOpts = new CrystalDecisions.Shared.DiskFileDestinationOptions();
            rdocument.ExportOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;
            rdocument.ExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.Excel;
            string filename = "ExceptionRelease_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            DiskOpts.DiskFileName = Server.MapPath("~/") + @"Report\\ReportFile\\" + filename;
            rdocument.ExportOptions.DestinationOptions = DiskOpts;
            rdocument.Export();
            return filename;
        }
        #endregion

        #region 通道事件
        /// <summary>
        /// 通道事件
        /// </summary>
        /// <returns></returns>
        [CheckPurview(Roles = "PK010405")]
        public ActionResult ReportGateEvents()
        {
            return View();
        }
        /// <summary>
        /// 查询通道事件记录
        /// </summary>
        /// <returns></returns>
        public string Search_GateEvents()
        {
            try
            {
                int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int rows = string.IsNullOrEmpty(Request.Params["rows"]) ? 0 : int.Parse(Request.Params["rows"]);
                InParams paras = new InParams
                {
                    ParkingID = Request.Params["parkingid"],
                    EventID = int.Parse(Request.Params["eventtype"]),
                    CardType = Request.Params["cardtype"],
                    CarType = Request.Params["cartype"],
                    InOrOut = int.Parse(Request.Params["inorout"]),
                    PlateNumber = Request.Params["platenumber"],
                    InGateID = Request.Params["gateid"],
                    StartTime = DateTime.Parse(Request.Params["entrancedate"]),
                    EndTime = DateTime.Parse(Request.Params["exitdate"]),
                    ReportType = ReportType.GateEvent
                };
                if (string.IsNullOrWhiteSpace(paras.ParkingID))
                    return string.Empty;
                Pagination pagination = StatisticsServices.Search_GateEvents(paras, rows, page);
                StringBuilder sb = new StringBuilder();
                string str = JsonHelper.GetJsonString(pagination.GateEventList);
                sb.Append("{");
                sb.Append("\"total\":" + pagination.Total + ",");
                sb.Append("\"rows\":" + str + ",");
                sb.Append("\"index\":" + rows);
                sb.Append("}");
                return sb.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public string Export_GateEvent()
        {
            ReportDocument rdocument = new ReportDocument();
            List<ParkEvent> eventlist = new List<ParkEvent>();
            InParams paras = new InParams
            {
                ParkingID = Request.Params["parkingid"],
                EventID = int.Parse(Request.Params["eventtype"]),
                CardType = Request.Params["cardtype"],
                CarType = Request.Params["cartype"],
                InOrOut = int.Parse(Request.Params["inorout"]),
                PlateNumber = Request.Params["platenumber"],
                InGateID = Request.Params["gateid"],
                StartTime = DateTime.Parse(Request.Params["entrancedate"]),
                EndTime = DateTime.Parse(Request.Params["exitdate"]),
                ReportType = ReportType.GateEvent
            };
            if (string.IsNullOrWhiteSpace(paras.ParkingID))
                return string.Empty;
            eventlist = StatisticsServices.Search_GateEvents(paras);
            rdocument.Load(Server.MapPath("../../Report/ReportViewer/ReportView_GateEvents.rpt"));
            rdocument.SetDataSource(EmptyTransferEvent(eventlist));
            CrystalDecisions.Shared.DiskFileDestinationOptions DiskOpts = new CrystalDecisions.Shared.DiskFileDestinationOptions();
            rdocument.ExportOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;
            rdocument.ExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.Excel;
            string filename = "GateEvent" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            DiskOpts.DiskFileName = Server.MapPath("~/") + @"Report\\ReportFile\\" + filename;
            rdocument.ExportOptions.DestinationOptions = DiskOpts;
            rdocument.Export();
            return filename;
        }
        List<ReportParkEvent> EmptyTransferEvent(List<ParkEvent> eventlist)
        {
            List<ReportParkEvent> reportparkeventlist = new List<ReportParkEvent>();
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
                    DataStatus = pevent.DataStatus,
                    EmployeeName = (string.IsNullOrEmpty(pevent.EmployeeName) ? "-" : pevent.EmployeeName),
                    LastUpdateTime = pevent.LastUpdateTime,
                    EventID = (pevent.EventID == 0 ? 1 : pevent.EventID),
                    EventName = (string.IsNullOrEmpty(pevent.EventName) ? "-" : pevent.EventName),
                    GateID = (string.IsNullOrEmpty(pevent.GateID) ? "-" : pevent.GateID),
                    GateName = (string.IsNullOrEmpty(pevent.GateName) ? "-" : pevent.GateName),
                    HaveUpdate = pevent.HaveUpdate,
                    ID = pevent.ID,
                    IORecordID = (string.IsNullOrEmpty(pevent.IORecordID) ? "-" : pevent.IORecordID),
                    IOState = (pevent.IOState == 0 ? 1 : pevent.IOState),
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
        #endregion

        #region 当班统计
        /// <summary>
        /// 当班统计
        /// </summary>
        /// <returns></returns>
        [CheckPurview(Roles = "PK010406")]
        public ActionResult ReportOnDuty()
        {
            return View();
        }
        /// <summary>
        /// 获得当班统计记录
        /// </summary>
        /// <returns></returns>
        public string Search_OnDuty()
        {
            try
            {
                int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int rows = string.IsNullOrEmpty(Request.Params["rows"]) ? 0 : int.Parse(Request.Params["rows"]);
                InParams paras = new InParams
                {
                    ParkingID = Request.Params["parkingid"],
                    OutOperator = Request.Params["exitoperatorid"],
                    StartTime = DateTime.Parse(Request.Params["starttime"]),
                    EndTime = DateTime.Parse(Request.Params["endtime"]),
                    BoxID = Request.Params["boxid"],
                    AdminID = Request.Params["adminid"],
                    ReportType = ReportType.OnDuty
                };
                if (string.IsNullOrWhiteSpace(paras.ParkingID))
                    return string.Empty;
                Pagination pagination = StatisticsServices.Search_OnDuty(paras, rows, page);
                StringBuilder sb = new StringBuilder();
                string str = JsonHelper.GetJsonString(pagination.OnDutyList);
                sb.Append("{");
                sb.Append("\"total\":" + pagination.Total + ",");
                sb.Append("\"rows\":" + str + ",");
                sb.Append("\"index\":" + rows);
                sb.Append("}");
                return sb.ToString();
            }
            catch 
            {
                return string.Empty;
            }
        }
        public string Export_OnDuty()
        {
            ReportDocument rdocument = new ReportDocument();
            List<Statistics_ChangeShift> changeshiftlist = new List<Statistics_ChangeShift>();
            InParams paras = new InParams
            {
                ParkingID = Request.Params["parkingid"],
                OutOperator = Request.Params["exitoperatorid"],
                StartTime = DateTime.Parse(Request.Params["starttime"]),
                EndTime = DateTime.Parse(Request.Params["endtime"]),
                BoxID = Request.Params["boxid"],
                AdminID = Request.Params["adminid"],
                ReportType = ReportType.OnDuty
            };
            if (string.IsNullOrWhiteSpace(paras.ParkingID))
                return string.Empty;
            changeshiftlist = StatisticsServices.Search_OnDuty(paras);
            rdocument.Load(Server.MapPath("../../Report/ReportViewer/ReportView_OnDuty.rpt"));
            rdocument.SetDataSource(changeshiftlist);
            CrystalDecisions.Shared.DiskFileDestinationOptions DiskOpts = new CrystalDecisions.Shared.DiskFileDestinationOptions();
            rdocument.ExportOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;
            rdocument.ExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.Excel;
            string filename = "OnDuty" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            DiskOpts.DiskFileName = Server.MapPath("~/") + @"Report\\ReportFile\\" + filename;
            rdocument.ExportOptions.DestinationOptions = DiskOpts;
            rdocument.Export();
            return filename;
 
        }
        #endregion

        #region 订单明细
        /// <summary>
        /// 订单信息
        /// </summary>
        /// <returns></returns>
        [CheckPurview(Roles = "PK010407")]
        public ActionResult ReportOrders()
        {
            return View();
        }
        /// <summary>
        /// 获取订单明细
        /// </summary>
        /// <returns></returns>
        public string Search_Orders()
        {
            try
            {
                int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int rows = string.IsNullOrEmpty(Request.Params["rows"]) ? 0 : int.Parse(Request.Params["rows"]);
                InParams paras = new InParams
                {
                    ParkingID = Request.Params["parkingid"],
                    PlateNumber = Request.Params["platenumber"],
                    OnLineOffLine = int.Parse(Request.Params["onlineoffline"]),
                    OrderSource = int.Parse(Request.Params["ordersource"]),
                    StartTime = DateTime.Parse(Request.Params["starttime"]),
                    EndTime = DateTime.Parse(Request.Params["endtime"]),
                    Zero = bool.Parse(Request.Params["isshowzero"]),
                    DiffAmount = bool.Parse(Request.Params["isshowdiffamount"]),
                    BoxID = Request.Params["boxid"],
                    IsNoConfirm = bool.Parse(Request.Params["isnoconfirm"]),
                    OutOperator = Request.Params["exitoperatorid"],
                    ReportType = ReportType.Order
                };
                if (string.IsNullOrWhiteSpace(paras.ParkingID))
                    return string.Empty;
                Pagination pagination = StatisticsServices.Search_Orders(paras, rows, page);
                StringBuilder sb = new StringBuilder();
                string str = JsonHelper.GetJsonString(pagination.OrderList);
                sb.Append("{");
                sb.Append("\"total\":" + pagination.Total + ",");
                sb.Append("\"rows\":" + str + ",");
                sb.Append("\"index\":" + rows);
                sb.Append("}");
                return sb.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public string Export_Order()
        {
            ReportDocument rdocument = new ReportDocument();
            List<ParkOrder> orderlist = new List<ParkOrder>();
            InParams paras = new InParams
            {
                ParkingID = Request.Params["parkingid"],
                PlateNumber = Request.Params["platenumber"],
                OnLineOffLine = int.Parse(Request.Params["onlineoffline"]),
                OrderSource = int.Parse(Request.Params["ordersource"]),
                StartTime = DateTime.Parse(Request.Params["starttime"]),
                EndTime = DateTime.Parse(Request.Params["endtime"]),
                Zero = bool.Parse(Request.Params["isshowzero"]),
                DiffAmount = bool.Parse(Request.Params["isshowdiffamount"]),
                BoxID = Request.Params["boxid"],
                IsNoConfirm = bool.Parse(Request.Params["isnoconfirm"]),
                OutOperator = Request.Params["exitoperatorid"],
                ReportType = ReportType.Order
            };
            if (string.IsNullOrWhiteSpace(paras.ParkingID))
                return string.Empty;
            orderlist = StatisticsServices.Search_Orders(paras);
            rdocument.Load(Server.MapPath("../../Report/ReportViewer/ReportView_Orders.rpt"));
            rdocument.SetDataSource(EmptyTransferOrder(orderlist));
            CrystalDecisions.Shared.DiskFileDestinationOptions DiskOpts = new CrystalDecisions.Shared.DiskFileDestinationOptions();
            rdocument.ExportOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;
            rdocument.ExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.Excel;
            string filename = "Order" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            DiskOpts.DiskFileName = Server.MapPath("~/") + @"Report\\ReportFile\\" + filename;
            rdocument.ExportOptions.DestinationOptions = DiskOpts;
            rdocument.Export();
            return filename;
 
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
                    DiscountAmount =  order.DiscountAmount,
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
                    OldUserulDate =  order.OldUserulDate,
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
                    LongTime = order.LongTime
                };
                reportparkorderlist.Add(reportparkorder);
            }
            return reportparkorderlist;
        }
        #endregion

        #region 月卡续期
        /// <summary>
        /// 卡片续期
        /// </summary>
        /// <returns></returns>
        [CheckPurview(Roles = "PK010409")]
        public ActionResult CardExtension()
        {
            return View();
        }
        /// <summary>
        /// 获取卡片续期记录
        /// </summary>
        /// <returns></returns>
        public string Search_CardExtension()
        {
            try
            {
                int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int rows = string.IsNullOrEmpty(Request.Params["rows"]) ? 0 : int.Parse(Request.Params["rows"]);
                InParams paras = new InParams
                {
                    ParkingID = Request.Params["parkingid"],
                    PlateNumber = Request.Params["platenumber"],
                    Owner = Request.Params["owner"],
                    OnLineOffLine = int.Parse(Request.Params["onlineoffline"]),
                    StartTime = DateTime.Parse(Request.Params["starttime"]),
                    EndTime = DateTime.Parse(Request.Params["endtime"]),
                    ReportType = ReportType.CardExtension
                };
                if (string.IsNullOrWhiteSpace(paras.ParkingID))
                    return string.Empty;
                Pagination pagination = StatisticsServices.Search_CardExtension(paras, rows, page);
                StringBuilder sb = new StringBuilder();
                string str = JsonHelper.GetJsonString(pagination.OrderList);
                sb.Append("{");
                sb.Append("\"total\":" + pagination.Total + ",");
                sb.Append("\"rows\":" + str + ",");
                sb.Append("\"index\":" + rows);
                sb.Append("}");
                return sb.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
        public string Export_CardExtend()
        {
            ReportDocument rdocument = new ReportDocument();
            List<ParkOrder> orderlist = new List<ParkOrder>();
            InParams paras = new InParams
            {
                ParkingID = Request.Params["parkingid"],
                PlateNumber = Request.Params["platenumber"],
                Owner = Request.Params["owner"],
                OnLineOffLine = int.Parse(Request.Params["onlineoffline"]),
                StartTime = DateTime.Parse(Request.Params["starttime"]),
                EndTime = DateTime.Parse(Request.Params["endtime"]),
                ReportType = ReportType.CardExtension
            };
            if (string.IsNullOrWhiteSpace(paras.ParkingID))
                return string.Empty;
            orderlist = StatisticsServices.Search_CardExtension(paras);
            rdocument.Load(Server.MapPath("../../Report/ReportViewer/ReportView.CardExtension.rpt"));
            rdocument.SetDataSource(EmptyTransferOrder(orderlist));
            CrystalDecisions.Shared.DiskFileDestinationOptions DiskOpts = new CrystalDecisions.Shared.DiskFileDestinationOptions();
            rdocument.ExportOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;
            rdocument.ExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.Excel;
            string filename = "MonthExtend" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            DiskOpts.DiskFileName = Server.MapPath("~/") + @"Report\\ReportFile\\" + filename;
            rdocument.ExportOptions.DestinationOptions = DiskOpts;
            rdocument.Export();
            return filename;
        }
        #endregion 

        #region 临停缴费
        /// <summary>
        /// 临停缴费
        /// </summary>
        /// <returns></returns>
        [CheckPurview(Roles = "PK010410")]
        public ActionResult TempPay()
        {
            return View();
        }
        /// <summary>
        /// 取得临停缴费记录
        /// </summary>
        /// <returns></returns>
        public string Search_TempPay()
        {
            try
            {
                int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int rows = string.IsNullOrEmpty(Request.Params["rows"]) ? 0 : int.Parse(Request.Params["rows"]);
                InParams paras = new InParams
                {
                    ParkingID = Request.Params["parkingid"],
                    OnLineOffLine = int.Parse(Request.Params["onlineoffline"]),
                    OrderSource = int.Parse(Request.Params["ordersource"]),
                    StartTime = DateTime.Parse(Request.Params["starttime"]),
                    EndTime = DateTime.Parse(Request.Params["endtime"]),
                    Zero = bool.Parse(Request.Params["isshowzero"]),
                    BoxID = Request.Params["boxid"],
                    IsNoConfirm = bool.Parse(Request.Params["isnoconfirm"]),
                    DiffAmount = bool.Parse(Request.Params["isdiffamount"]),
                    PlateNumber = Request.Params["platenumber"],
                    OutOperator = Request.Params["exitoperatorid"],
                    ReleaseType = -1,
                    ReportType = ReportType.TempPay
                };
                if (string.IsNullOrWhiteSpace(paras.ParkingID))
                    return string.Empty;
                Pagination pagination = StatisticsServices.Search_TempPays(paras, rows, page);
                StringBuilder sb = new StringBuilder();
                string str = JsonHelper.GetJsonString(pagination.OrderList);
                sb.Append("{");
                sb.Append("\"total\":" + pagination.Total + ",");
                sb.Append("\"rows\":" + str + ",");
                sb.Append("\"index\":" + rows);
                sb.Append("}");
                return sb.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
        public string Export_TempPay()
        {
            ReportDocument rdocument = new ReportDocument();
            List<ParkOrder> orderlist = new List<ParkOrder>();
            InParams paras = new InParams
            {
                ParkingID = Request.Params["parkingid"],
                OnLineOffLine = int.Parse(Request.Params["onlineoffline"]),
                OrderSource = int.Parse(Request.Params["ordersource"]),
                StartTime = DateTime.Parse(Request.Params["starttime"]),
                EndTime = DateTime.Parse(Request.Params["endtime"]),
                Zero = bool.Parse(Request.Params["isshowzero"]),
                BoxID = Request.Params["boxid"],
                IsNoConfirm = bool.Parse(Request.Params["isnoconfirm"]),
                PlateNumber = Request.Params["platenumber"],
                OutOperator = Request.Params["exitoperatorid"],
                ReleaseType = -1,
                ReportType = ReportType.TempPay
            };
            if (string.IsNullOrWhiteSpace(paras.ParkingID))
                return string.Empty;
            orderlist = StatisticsServices.Search_TempPays(paras);
            rdocument.Load(Server.MapPath("../../Report/ReportViewer/ReportView_TempPay.rpt"));
            rdocument.SetDataSource(EmptyTransferOrder(orderlist));
            CrystalDecisions.Shared.DiskFileDestinationOptions DiskOpts = new CrystalDecisions.Shared.DiskFileDestinationOptions();
            rdocument.ExportOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;
            rdocument.ExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.Excel;
            string filename = "TempPay" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            DiskOpts.DiskFileName = Server.MapPath("~/") + @"Report\\ReportFile\\" + filename;
            rdocument.ExportOptions.DestinationOptions = DiskOpts;
            rdocument.Export();
            return filename;
        }
        #endregion

        #region 线上支付
        /// <summary>
        /// 线上支付
        /// </summary>
        /// <returns></returns>

        public ActionResult OnlinePay()
        {
            return View();
        }
        /// <summary>
        /// 获取线上支付
        /// </summary>
        /// <returns></returns>
        public string Search_OnlinePay()
        {
            try
            {
                int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int rows = string.IsNullOrEmpty(Request.Params["rows"]) ? 0 : int.Parse(Request.Params["rows"]);
                InParams paras = new InParams
                {
                    ParkingID = Request.Params["parkingid"],
                    Status = int.Parse(Request.Params["status"]),
                    PayWay = int.Parse(Request.Params["payway"]),
                    StartTime = DateTime.Parse(Request.Params["starttime"]),
                    EndTime = DateTime.Parse(Request.Params["endtime"]),

                    PlateNumber = Request.Params["platenumber"],

                    ReportType = ReportType.Order
                };
                if (string.IsNullOrWhiteSpace(paras.ParkingID))
                    return string.Empty;
                Pagination pagination = StatisticsServices.Search_OnlinePays(paras, rows, page);
                StringBuilder sb = new StringBuilder();
                string str = JsonHelper.GetJsonString(pagination.OnlineOrderList);
                sb.Append("{");
                sb.Append("\"total\":" + pagination.Total + ",");
                sb.Append("\"rows\":" + str + ",");
                sb.Append("\"index\":" + rows);
                sb.Append("}");
                return sb.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public string Export_OnlinePay()
        {
            ReportDocument rdocument = new ReportDocument();
            List<OnlineOrder> onlineorderlist = new List<OnlineOrder>();
            InParams paras = new InParams
            {
                ParkingID = Request.Params["parkingid"],
                Status = int.Parse(Request.Params["status"]),
                PayWay = int.Parse(Request.Params["payway"]),
                StartTime = DateTime.Parse(Request.Params["starttime"]),
                EndTime = DateTime.Parse(Request.Params["endtime"]),

                PlateNumber = Request.Params["platenumber"],

                ReportType = ReportType.Order
            };
            if (string.IsNullOrWhiteSpace(paras.ParkingID))
                return string.Empty;
            onlineorderlist = StatisticsServices.Search_OnlinePays(paras);
            rdocument.Load(Server.MapPath("../../Report/ReportViewer/ReportView_OnlinePay.rpt"));
            rdocument.SetDataSource(EmptyTransferOnlinePay(onlineorderlist));
            CrystalDecisions.Shared.DiskFileDestinationOptions DiskOpts = new CrystalDecisions.Shared.DiskFileDestinationOptions();
            rdocument.ExportOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;
            rdocument.ExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.Excel;
            string filename = "Order" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            DiskOpts.DiskFileName = Server.MapPath("~/") + @"Report\\ReportFile\\" + filename;
            rdocument.ExportOptions.DestinationOptions = DiskOpts;
            rdocument.Export();
            return filename;

        }
        List<OnlineOrder> EmptyTransferOnlinePay(List<OnlineOrder> onlineorderlist)
        {
            List<OnlineOrder> reportonlineorderlist = new List<OnlineOrder>();
            if (onlineorderlist == null || onlineorderlist.Count == 0)
                return reportonlineorderlist;

            foreach (OnlineOrder order in onlineorderlist)
            {
                OnlineOrder onlineorder = new OnlineOrder
                {
                    OrderNo = (string.IsNullOrEmpty(order.OrderNo) ? "-" : order.OrderNo),
                    PKName = (string.IsNullOrEmpty(order.PKName) ? "-" : order.PKName),
                    PlateNumber = (string.IsNullOrEmpty(order.PlateNumber) ? "-" : order.PlateNumber),
                    Amount = order.Amount,
                    MonthNum = order.MonthNum,
                    NickName = order.NickName,
                    SyncResultTimes = order.SyncResultTimes,
                    LastSyncResultTime = order.LastSyncResultTime,
                    RefundOrderId = order.RefundOrderId,
                    lx = order.lx,
                    zt = order.zt,
                    OrderTime = order.OrderTime,
                    RealPayTime = order.RealPayTime,
                };
                reportonlineorderlist.Add(onlineorder);
            }
            return reportonlineorderlist;
        }


        #endregion

        #region 商家优免
        /// <summary>
        /// 商家优免
        /// </summary>
        /// <returns></returns>
        [CheckPurview(Roles = "PK010411")]
        public ActionResult Coupon()
        {
            return View();
        }
        /// <summary>
        /// 获取商家优免数据
        /// </summary>
        /// <returns></returns>
        public string Search_Coupons()
        {
            try
            {
                int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int rows = string.IsNullOrEmpty(Request.Params["rows"]) ? 0 : int.Parse(Request.Params["rows"]);
                InParams paras = new InParams
                {
                    ParkingID = Request.Params["parkingid"],
                    Status = int.Parse(Request.Params["status"]),
                    CouponNo = Request.Params["couponno"],
                    StartTime = DateTime.Parse(Request.Params["starttime"]),
                    EndTime = DateTime.Parse(Request.Params["endtime"]),
                    SellerID = Request.Params["sellerid"],
                    ReportType = ReportType.Coupon
                };
                if (string.IsNullOrWhiteSpace(paras.ParkingID))
                    return string.Empty;
                Pagination pagination = StatisticsServices.Search_CarDerates(paras, rows, page);
                StringBuilder sb = new StringBuilder();
                string str = JsonHelper.GetJsonString(pagination.CarDerateList);
                sb.Append("{");
                sb.Append("\"total\":" + pagination.Total + ",");
                sb.Append("\"rows\":" + str + ",");
                sb.Append("\"index\":" + rows);
                sb.Append("}");
                return sb.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
        public string Export_Coupon()
        {
            ReportDocument rdocument = new ReportDocument();
            List<ParkCarDerate> orderlist = new List<ParkCarDerate>();
            InParams paras = new InParams
            {
                ParkingID = Request.Params["parkingid"],
                Status = int.Parse(Request.Params["status"]),
                CouponNo = Request.Params["couponno"],
                StartTime = DateTime.Parse(Request.Params["starttime"]),
                EndTime = DateTime.Parse(Request.Params["endtime"]),
                SellerID = Request.Params["sellerid"],
                ReportType = ReportType.Coupon
            };
            if (string.IsNullOrWhiteSpace(paras.ParkingID))
                return string.Empty;
            orderlist = StatisticsServices.Search_CarDerates(paras);
            rdocument.Load(Server.MapPath("../../Report/ReportViewer/ReportView.Coupon.rpt"));
            EmptyTransferCarDerate(orderlist);
            rdocument.SetDataSource(orderlist);
            CrystalDecisions.Shared.DiskFileDestinationOptions DiskOpts = new CrystalDecisions.Shared.DiskFileDestinationOptions();
            rdocument.ExportOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;
            rdocument.ExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.Excel;
            string filename = "Coupon" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            DiskOpts.DiskFileName = Server.MapPath("~/") + @"Report\\ReportFile\\" + filename;
            rdocument.ExportOptions.DestinationOptions = DiskOpts;
            rdocument.Export();
            return filename;
 
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
        #endregion

        #region 日报表
        /// <summary>
        /// 日报表
        /// </summary>
        /// <returns></returns>
        [CheckPurview(Roles = "PK010501")]
        public ActionResult StatisticsDaily()
        {
            return View();
        }
        /// <summary>
        /// 获取日报表数据
        /// </summary>
        /// <returns></returns>
        public string Search_DailyStatisticsData()
        {
            try
            {
                InParams paras = new InParams
                {
                    ParkingID = Request.Params["parkingid"],
                    //StartTime = DateTime.Parse(Request.Params["starttime"] + " 00:00:00"),
                    //EndTime = DateTime.Parse(Request.Params["endtime"] + " 23:59:59")
                    StartTime = DateTime.Parse(Request.Params["starttime"]),
                    EndTime = DateTime.Parse(Request.Params["endtime"]),
                };
                int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int rows = string.IsNullOrEmpty(Request.Params["rows"]) ? 0 : int.Parse(Request.Params["rows"]);
                Pagination pagination = StatisticsServices.Search_DailyStatistics(paras, rows, page);
                StringBuilder sb = new StringBuilder();
                sb.Append("{");
                sb.Append("\"total\":" + pagination.Total + ",");
                sb.Append("\"rows\":" + JsonHelper.GetJsonString(pagination.StatisticsGatherList) + ",");
                sb.Append("\"index\":" + rows);
                sb.Append("}");
                return sb.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
        public string Export_Daily()
        {
            ReportDocument rdocument = new ReportDocument();
            List<Statistics_Gather> orderlist = new List<Statistics_Gather>();
            InParams paras = new InParams
            {
                ParkingID = Request.Params["ParkingID"],
                //StartTime = DateTime.Parse(Request.Params["starttime"] + " 00:00:00"),
                //EndTime = DateTime.Parse(Request.Params["endtime"] + " 23:59:59")
                StartTime = DateTime.Parse(Request.Params["starttime"]),
                EndTime = DateTime.Parse(Request.Params["endtime"]),
            };
            if (string.IsNullOrWhiteSpace(paras.ParkingID))
                return string.Empty;
            orderlist = StatisticsServices.Search_MonthStatistics(paras);
            rdocument.Load(Server.MapPath("../../Report/ReportViewer/ReportView_Daily.rpt"));
            rdocument.SetDataSource(orderlist);
            CrystalDecisions.Shared.DiskFileDestinationOptions DiskOpts = new CrystalDecisions.Shared.DiskFileDestinationOptions();
            rdocument.ExportOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;
            rdocument.ExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.Excel;
            string filename = "Daily" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            DiskOpts.DiskFileName = Server.MapPath("~/") + @"Report\\ReportFile\\" + filename;
            rdocument.ExportOptions.DestinationOptions = DiskOpts;
            rdocument.Export();
            return filename;

        }
        #endregion

        #region 月报表
        /// <summary>
        /// 月报表
        /// </summary>
        /// <returns></returns>
        [CheckPurview(Roles = "PK010502")]
        public ActionResult StatisticsMonth()
        {
            return View();
        }
        /// <summary>
        /// 获取月报表数据
        /// </summary>
        /// <returns></returns>
        public string Search_MonthStatisticsData()
        {
            try
            {
                InParams paras = new InParams
                {
                    ParkingID = Request.Params["parkingid"],
                    StartTime = DateTime.Parse(Request.Params["starttime"] + "-01 00:00:00"),
                    EndTime = DateTime.Parse(Request.Params["endtime"] + "-01 00:00:00").AddSeconds(-1).AddMonths(1)
                };
                int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int rows = string.IsNullOrEmpty(Request.Params["rows"]) ? 0 : int.Parse(Request.Params["rows"]);
                Pagination pagination = StatisticsServices.Search_MonthStatistics(paras, rows, page);
                StringBuilder sb = new StringBuilder();
                sb.Append("{");
                sb.Append("\"total\":" + pagination.Total + ",");
                sb.Append("\"rows\":" + JsonHelper.GetJsonString(pagination.StatisticsGatherList) + ",");
                sb.Append("\"index\":" + rows);
                sb.Append("}");
                return sb.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
        public string Export_Month()
        {
            ReportDocument rdocument = new ReportDocument();
            List<Statistics_Gather> orderlist = new List<Statistics_Gather>();
            InParams paras = new InParams
            {
                ParkingID = Request.Params["parkingid"],
                StartTime = DateTime.Parse(Request.Params["starttime"] + "-01 00:00:00"),
                EndTime = DateTime.Parse(Request.Params["endtime"] + "-01 00:00:00").AddSeconds(-1).AddMonths(1)
            };
            if (string.IsNullOrWhiteSpace(paras.ParkingID))
                return string.Empty;
            orderlist = StatisticsServices.Search_MonthStatistics(paras);
            rdocument.Load(Server.MapPath("../../Report/ReportViewer/ReportView_Month.rpt"));
            rdocument.SetDataSource(orderlist);
            CrystalDecisions.Shared.DiskFileDestinationOptions DiskOpts = new CrystalDecisions.Shared.DiskFileDestinationOptions();
            rdocument.ExportOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;
            rdocument.ExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.Excel;
            string filename = "Month" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            DiskOpts.DiskFileName = Server.MapPath("~/") + @"Report\\ReportFile\\" + filename;
            rdocument.ExportOptions.DestinationOptions = DiskOpts;
            rdocument.Export();
            return filename;

        }
        #endregion

        #region 日汇总报表
        /// <summary>
        /// 日汇总报表
        /// </summary>
        /// <returns></returns>
        [CheckPurview(Roles = "PK010503")]
        public ActionResult StatisticsDailyGather()
        {
            return View();
        }
        /// <summary>
        /// 获取日汇总报表数据
        /// </summary>
        /// <returns></returns>
        public string Search_StatisticsDailyGather()
        {
            try
            {
                InParams paras = new InParams
                {
                    ParkingID = Request.Params["parkingid"],
                    StartTime = DateTime.Parse(Request.Params["starttime"] + " 00:00:00"),
                    EndTime = DateTime.Parse(Request.Params["endtime"] + " 23:59:59")
                };
                int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int rows = string.IsNullOrEmpty(Request.Params["rows"]) ? 0 : int.Parse(Request.Params["rows"]);
                Pagination pagination = StatisticsServices.Search_DailyStatistics(paras, rows, page);
                StringBuilder sb = new StringBuilder();
                sb.Append("{");
                sb.Append("\"total\":" + pagination.Total + ",");
                sb.Append("\"rows\":" + JsonHelper.GetJsonString(pagination.StatisticsGatherList) + ",");
                sb.Append("\"index\":" + rows);
                sb.Append("}");
                return sb.ToString();
            }
            catch { return string.Empty; }
        }
        public string Export_DailyGather()
        {
            ReportDocument rdocument = new ReportDocument();
            List<Statistics_Gather> orderlist = new List<Statistics_Gather>();
            InParams paras = new InParams
            {
                ParkingID = Request.Params["parkingid"],
                StartTime = DateTime.Parse(Request.Params["starttime"] + " 00:00:00"),
                EndTime = DateTime.Parse(Request.Params["endtime"] + " 23:59:59")
            };
            if (string.IsNullOrWhiteSpace(paras.ParkingID))
                return string.Empty;
            orderlist = StatisticsServices.Search_DailyStatistics(paras);
            rdocument.Load(Server.MapPath("../../Report/ReportViewer/ReportView_Daily.rpt"));
            rdocument.SetDataSource(orderlist);
            CrystalDecisions.Shared.DiskFileDestinationOptions DiskOpts = new CrystalDecisions.Shared.DiskFileDestinationOptions();
            rdocument.ExportOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;
            rdocument.ExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.Excel;
            string filename = "DailyGather" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            DiskOpts.DiskFileName = Server.MapPath("~/") + @"Report\\ReportFile\\" + filename;
            rdocument.ExportOptions.DestinationOptions = DiskOpts;
            rdocument.Export();
            return filename;

        }
        #endregion

        #region 月汇总报表
        /// <summary>
        /// 月汇总报表
        /// </summary>
        /// <returns></returns>
        [CheckPurview(Roles = "PK010504")]
        public ActionResult StatisticsMonthGather()
        {
            return View();
        }
        /// <summary>
        /// 获取月汇总报表数据
        /// </summary>
        /// <returns></returns>
        public string Search_StatisticsMonthGather()
        {
            InParams paras = new InParams
            {
                ParkingID = Request.Params["parkingid"],
                StartTime = DateTime.Parse(Request.Params["starttime"] + "-01 00:00:00"),
                EndTime = DateTime.Parse(Request.Params["endtime"] + "-01 00:00:00").AddSeconds(-1).AddMonths(1)
            };
            int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
            int rows = string.IsNullOrEmpty(Request.Params["rows"]) ? 0 : int.Parse(Request.Params["rows"]);
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            try
            {
                Pagination pagination = StatisticsServices.Search_MonthStatistics(paras, rows, page);
                if (pagination != null && pagination.StatisticsGatherList != null)
                {
                    sb.Append("\"total\":" + pagination.Total + ",");
                    sb.Append("\"rows\":" + JsonHelper.GetJsonString(pagination.StatisticsGatherList) + ",");
                }
                else
                {
                    sb.Append("\"total\":'0',");
                    sb.Append("\"rows\":" + JsonHelper.GetJsonString(new List<Statistics_Gather>()) + ",");
                }
            }
            catch 
            {
                sb.Append("\"total\":'0',");
                sb.Append("\"rows\":" + JsonHelper.GetJsonString(new List<Statistics_Gather>()) + ",");
            }
            sb.Append("\"index\":" + rows);
            sb.Append("}");
            return sb.ToString();
        }
        public string Export_MonthGather()
        {
            ReportDocument rdocument = new ReportDocument();
            List<Statistics_Gather> orderlist = new List<Statistics_Gather>();
            InParams paras = new InParams
            {
                ParkingID = Request.Params["parkingid"],
                StartTime = DateTime.Parse(Request.Params["starttime"] + "-01 00:00:00"),
                EndTime = DateTime.Parse(Request.Params["endtime"] + "-01 00:00:00").AddSeconds(-1).AddMonths(1)
            };
            if (string.IsNullOrWhiteSpace(paras.ParkingID))
                return string.Empty;
            orderlist = StatisticsServices.Search_MonthStatistics(paras);
            rdocument.Load(Server.MapPath("../../Report/ReportViewer/ReportView_MonthGather.rpt"));
            rdocument.SetDataSource(orderlist);
            CrystalDecisions.Shared.DiskFileDestinationOptions DiskOpts = new CrystalDecisions.Shared.DiskFileDestinationOptions();
            rdocument.ExportOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;
            rdocument.ExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.Excel;
            string filename = "MonthGather" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            DiskOpts.DiskFileName = Server.MapPath("~/") + @"Report\\ReportFile\\" + filename;
            rdocument.ExportOptions.DestinationOptions = DiskOpts;
            rdocument.Export();
            return filename;
 
        }
        #endregion

        #region 收入分析
        #endregion

        #region 进出分析
        #endregion

        #region 车辆进出场数量
        /// <summary>
        /// 车辆进出场数量分析
        /// </summary>
        /// <returns></returns>

        public ActionResult ParkIO()
        {
            return View();
        }
        /// <summary>
        /// 获取车辆进出场数量
        /// </summary>
        /// <returns></returns>
        public string Search_ParkIO()
        {
            try
            {
                int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int rows = string.IsNullOrEmpty(Request.Params["rows"]) ? 0 : int.Parse(Request.Params["rows"]);
                InParams paras = new InParams
                {
                    ParkingID = Request.Params["parkingid"],
                    
                    StartTime = DateTime.Parse(Request.Params["starttime"]),
                    EndTime = DateTime.Parse(Request.Params["endtime"]),


                    ReportType = ReportType.Order
                };
                
                Pagination pagination = StatisticsServices.Search_ParkIOs(paras, rows, page);
                StringBuilder sb = new StringBuilder();
                string str = JsonHelper.GetJsonString(pagination.ParkIOList);
                sb.Append("{");
                sb.Append("\"total\":" + pagination.Total + ",");
                sb.Append("\"rows\":" + str + ",");
                sb.Append("\"index\":" + rows);
                sb.Append("}");
                return sb.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 导出数据到Excel
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult DownLoadExcel(string data)
        {
            //获取前台post提交的数据  
            //定义生成文件的目录，获取绝对地址  
            string pathToFiles = Server.MapPath("/");
            //定义生成文件的名称  
            string fn = "ParkIONumber" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
            //组合成文件的路径  
            string path = @"" + pathToFiles + "\\" + fn;

            //判断是否已经存在文件  
            if (!System.IO.File.Exists(path))
            {
                //新建文件，并写入数据  
                System.IO.File.WriteAllText(path, data, Encoding.UTF8);
            }
            else
            {
                //文件已存在，添加写入数据  
                System.IO.File.AppendAllText(path, data, Encoding.UTF8);
            }
            return Content("/" + fn);//返回文件名提供下载  

        }

        #endregion

        #region 车牌分析
        void EntyPlateNumberPrefix(List<PlateNumberPrefixModel> platenumberprefix)
        {
            if (platenumberprefix == null || platenumberprefix.Count == 0)
                return;

            foreach (var plate in platenumberprefix)
            {
                if (plate.ParkingName == null || plate.ParkingName.Trim().Length == 0)
                    plate.ParkingName = "-";
                if (plate.PlateNumberPrefix == null || plate.PlateNumberPrefix.Trim().Length == 0)
                    plate.PlateNumberPrefix = "-";
                if (plate.Rate == null || plate.Rate.Trim().Length == 0)
                    plate.Rate = "0%";
            }
        }

        public string Export_PlateNumberPrifix()
        {
            ReportDocument rdocument = new ReportDocument();
            List<PlateNumberPrefixModel> orderlist = new List<PlateNumberPrefixModel>();
            InParams paras = new InParams
            {
                ParkingID = Request.Params["parkingid"],
                StartTime = DateTime.Parse(Request.Params["starttime"]),
                EndTime = DateTime.Parse(Request.Params["endtime"]),
                ReportType = ReportType.PlateNumberPrefix
            };
            if (string.IsNullOrWhiteSpace(paras.ParkingID))
                return string.Empty;
            orderlist = StatisticsServices.Search_PlateNumberPrefix(paras);
            rdocument.Load(Server.MapPath("../../Report/ReportViewer/ReportView_PlateNumberPrefix.rpt"));
            EntyPlateNumberPrefix(orderlist);
            rdocument.SetDataSource(orderlist);
            CrystalDecisions.Shared.DiskFileDestinationOptions DiskOpts = new CrystalDecisions.Shared.DiskFileDestinationOptions();
            rdocument.ExportOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;
            rdocument.ExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.Excel;
            string filename = "PlateNumberPrefix" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            DiskOpts.DiskFileName = Server.MapPath("~/") + @"Report\\ReportFile\\" + filename;
            rdocument.ExportOptions.DestinationOptions = DiskOpts;
            rdocument.Export();
            return filename;
        }
        /// <summary>
        /// 车牌分析
        /// </summary>
        /// <returns></returns>
        [CheckPurview(Roles = "PK010413")]
        public ActionResult PlateNumberAnalyse()
        {
            return View();
        }
        /// <summary>
        /// 获取车牌分析结果
        /// </summary>
        /// <returns></returns>
        public string GetPlateNumberAnalyse()
        {
            try
            {
                InParams paras = new InParams
                {
                    ParkingID = Request.Params["parkingid"],
                    StartTime = DateTime.Parse(Request.Params["starttime"]),
                    EndTime = DateTime.Parse(Request.Params["endtime"])
                };
                if (string.IsNullOrWhiteSpace(paras.ParkingID))
                    return string.Empty;
                List<PlateNumberPrefixModel> prefixlist = StatisticsServices.Search_PlateNumberPrefix(paras);
                StringBuilder sb = new StringBuilder();
                int rows = 0;
                int total = 0;
                string str = JsonHelper.GetJsonString(prefixlist);
                sb.Append("{");
                sb.Append("\"total\":" + total + ",");
                sb.Append("\"rows\":" + str + ",");
                sb.Append("\"index\":" + rows);
                sb.Append("}");
                return sb.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion

        #region 月卡信息

        public string Export_MonthCardInfo()
        {
            ReportDocument rdocument = new ReportDocument();
            List<MonthCardInfoModel> orderlist = new List<MonthCardInfoModel>();
            InParams paras = new InParams
            {
                ParkingID = Request.Params["parkingid"],
                CardType = Request.Params["cardtype"],
                PlateNumber = Request.Params["platenumber"],
                Mobile = Request.Params["mobile"],
                Addr = Request.Params["addr"],
                Due = bool.Parse(Request.Params["Due"])
            };
            
            if (string.IsNullOrWhiteSpace(paras.ParkingID))
                return string.Empty;
            orderlist = StatisticsServices.Search_MonthCardInfo(paras);
            rdocument.Load(Server.MapPath("../../Report/ReportViewer/ReportView_MonthCardInfo.rpt"));
            EmptyTransfer(orderlist);
            rdocument.SetDataSource(orderlist);
            CrystalDecisions.Shared.DiskFileDestinationOptions DiskOpts = new CrystalDecisions.Shared.DiskFileDestinationOptions();
            rdocument.ExportOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;
            rdocument.ExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.Excel;
            string filename = "TempPay" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            DiskOpts.DiskFileName = Server.MapPath("~/") + @"Report\\ReportFile\\" + filename;
            rdocument.ExportOptions.DestinationOptions = DiskOpts;
            rdocument.Export();
            return filename;
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
        [CheckPurview(Roles = "PK010414")]
        public ActionResult MonthCardInfo()
        {
            return View();
        }
        /// <summary>
        /// 获得月卡信息
        /// </summary>
        /// <returns></returns>
        public string Search_MonthCardInfo()
        {
            try
            {
                int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int rows = string.IsNullOrEmpty(Request.Params["rows"]) ? 0 : int.Parse(Request.Params["rows"]);
                InParams paras = new InParams
                {
                    ParkingID = Request.Params["parkingid"],
                    CardType = Request.Params["cardtype"],
                    PlateNumber = Request.Params["platenumber"],
                    Mobile = Request.Params["mobile"],
                    Addr = Request.Params["addr"],
                    Due = bool.Parse(Request.Params["Due"])
                };
                if (string.IsNullOrWhiteSpace(paras.ParkingID))
                    return string.Empty;
                Pagination pagination = StatisticsServices.Search_MonthCardInfo(paras, rows, page);
                StringBuilder sb = new StringBuilder();
                string str = JsonHelper.GetJsonString(pagination.MonthCardList);
                sb.Append("{");
                sb.Append("\"total\":" + pagination.Total + ",");
                sb.Append("\"rows\":" + str + ",");
                sb.Append("\"index\":" + rows);
                sb.Append("}");
                return sb.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
         
        #endregion

        #region 访客信息
        [CheckPurview(Roles = "PK010415")]
        public ActionResult ParkVisitorInfo()
        {
            return View();
        }
        /// <summary>
        /// 访客信息
        /// </summary>
        /// <returns></returns>
        public string Search_ParkVisitorInfo()
        {
            try
            {
                int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int rows = string.IsNullOrEmpty(Request.Params["rows"]) ? 0 : int.Parse(Request.Params["rows"]);
                VisitorReportCondition paras = new VisitorReportCondition();
                if (string.IsNullOrWhiteSpace(Request.Params["ParkingId"]) || Request.Params["ParkingId"].ToString() == "-1")
                {
                    List<BaseParkinfo> parkings = ParkingServices.QueryParkingByVillageIds(base.GetLoginUserVillages.Select(u => u.VID).ToList());
                    if (parkings.Count > 0)
                    {
                        paras.ParkingIds = parkings.Select(p => p.PKID).ToList();
                    }
                }
                else {
                    paras.ParkingIds = new List<string>() { Request.Params["ParkingId"].ToString() };
                }
                paras.PlateNumber = Request.Params["PlateNumber"].ToString();
                paras.MoblieOrName = Request.Params["MoblieOrName"].ToString();
                paras.BeginTime = DateTime.Parse(Request.Params["BeginTime"].ToString());
                paras.EndTime = DateTime.Parse(Request.Params["EndTime"].ToString());
                if (!string.IsNullOrWhiteSpace(Request.Params["VisitorSource"]) && Request.Params["VisitorSource"].ToString() != "-1") {
                    paras.VisitorSource = int.Parse(Request.Params["VisitorSource"].ToString());
                }

                if (!string.IsNullOrWhiteSpace(Request.Params["VisitorState"]) && Request.Params["VisitorState"].ToString() != "-1")
                {
                    paras.VisitorState = int.Parse(Request.Params["VisitorState"].ToString());
                }
              
                Pagination pagination = StatisticsServices.QueryParkVisitorReport(paras, rows, page);
                StringBuilder sb = new StringBuilder();
                string str = JsonHelper.GetJsonString(pagination.VisitorList);
                sb.Append("{");
                sb.Append("\"total\":" + pagination.Total + ",");
                sb.Append("\"rows\":" + str + ",");
                sb.Append("\"index\":" + rows);
                sb.Append("}");
                return sb.ToString();
            }
            catch(Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "获取访客信息异常");
                return string.Empty;
            }
        }
        #endregion

        #region 基础数据
        /// <summary>
        /// 获得所有车场
        /// </summary>
        /// <returns></returns>
        public JsonResult GetSellers()
        {
            JsonResult json = new JsonResult();
            try
            {
                json.Data = ParkSellerServices.QueryByVillageIds(GetLoginUserVillages.Select(p => p.VID).ToList());
            }
            catch { }
            return json;
        }
        /// <summary>
        /// 获得所有车场
        /// </summary>
        /// <returns></returns>
        public JsonResult GetParks()
        {
            JsonResult json = new JsonResult();
            try
            {
                json.Data = ParkingServices.QueryParkingByVillageIds(base.GetLoginUserVillages.Select(u => u.VID).ToList());
            }
            catch (Exception ex)
            { }
            return json;
        }
        /// <summary>
        /// 车辆进场类型
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCardEntranceType()
        {
            string parkingid = Request.Params["parkingid"];
            JsonResult json = new JsonResult();
            try
            {
                json.Data = ParkCarTypeServices.QueryParkCarTypeByParkingId(parkingid);
            }
            catch { }
            return json;
        }
        /// <summary>
        /// 进通道
        /// </summary>
        /// <returns></returns>
        public JsonResult GetEntranceGates()
        {
            string parkingid = Request.Params["parkingid"];
            JsonResult json = new JsonResult();
            try
            {
                json.Data = ParkGateServices.QueryByParkingIdAndIoState(parkingid, IoState.GoIn);
            }
            catch { }
            return json;
        }
        /// <summary>
        /// 获得车场所有通道
        /// </summary>
        /// <returns></returns>
        public JsonResult GetGates()
        {
            string parkingid = Request.Params["parkingid"];
            JsonResult json = new JsonResult();
            try
            {
                json.Data = ParkGateServices.QueryByParkingId(parkingid);
            }
            catch { }
            return json;
        }
        /// <summary>
        /// 出通道
        /// </summary>
        /// <returns></returns>
        public JsonResult GetExitGates()
        {
            string parkingid = Request.Params["parkingid"];
            JsonResult json = new JsonResult();
            try
            {
                json.Data = ParkGateServices.QueryByParkingIdAndIoState(parkingid, IoState.GoOut);
            }
            catch { }
            return json;
        }
        /// <summary>
        /// 获得车形
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCarTypes()
        {
            string parkingid = Request.Params["ParkingID"];
            JsonResult json = new JsonResult();
            try
            {
                json.Data = ParkCarModelServices.QueryByParkingId(parkingid);
            }
            catch { }
            return json;
        }
        /// <summary>
        /// 获取区域
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAreas()
        {
            string parkingid = Request.Params["parkingid"];
            JsonResult json = new JsonResult();
            try
            {
                json.Data = ParkAreaServices.GetParkAreaByParkingId(parkingid);
            }
            catch { }
            return json;
        }
        /// <summary>
        /// 获得岗亭
        /// </summary>
        /// <returns></returns>
        public JsonResult GetBoxes()
        {
            string parkingid = Request.Params["parkingid"];
            JsonResult json = new JsonResult();
            try
            {
                json.Data = ParkBoxServices.QueryByParkingID(parkingid);
            }
            catch
            { }
            return json;
        }
        /// <summary>
        /// 当班人
        /// </summary>
        /// <returns></returns>
        public JsonResult GetOnDutys()
        {
            string parkingid = Request.Params["parkingid"];
            JsonResult json = new JsonResult();
            try
            {
                json.Data = SysUserServices.QuerySysUserByParkingId(parkingid);
            }
            catch
            { }
            return json;
        }
        /// <summary>
        /// 获取事件类型
        /// </summary>
        /// <returns></returns>
        public JsonResult GetEventType()
        {
            JsonResult json = new JsonResult();
            try
            {
                json.Data = ParkEventServices.GetEventType();
            }
            catch
            { }
            return json;
        }
        #endregion

        #region 日报表按时
        /// <summary>
        /// 日报表
        /// </summary>
        /// <returns></returns>
        [CheckPurview(Roles = "PK010501")]
        public ActionResult StatisticsEdit()
        {
            return View();
        }
        /// <summary>
        /// 获取日报表数据
        /// </summary>
        /// <returns></returns>
        public string Search_DailyStatisticsEdit()
        {
            try
            {
                InParams paras = new InParams
                {
                    ParkingID = Request.Params["parkingid"],
                    StartTime = DateTime.Parse(Request.Params["starttime"]),
                    EndTime = DateTime.Parse(Request.Params["endtime"])
                };
                int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int rows = string.IsNullOrEmpty(Request.Params["rows"]) ? 0 : int.Parse(Request.Params["rows"]);
                Pagination pagination = StatisticsServices.Search_DailyStatistics(paras, rows, page);
                StringBuilder sb = new StringBuilder();
                sb.Append("{");
                sb.Append("\"total\":" + pagination.Total + ",");
                sb.Append("\"rows\":" + JsonHelper.GetJsonString(pagination.StatisticsGatherList) + ",");
                sb.Append("\"index\":" + rows);
                sb.Append("}");
                return sb.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
        public string Export_DailyEdit()
        {
            ReportDocument rdocument = new ReportDocument();
            List<Statistics_Gather> orderlist = new List<Statistics_Gather>();
            InParams paras = new InParams
            {
                ParkingID = Request.Params["ParkingID"],
                StartTime = DateTime.Parse(Request.Params["starttime"]),
                EndTime = DateTime.Parse(Request.Params["endtime"])
            };
            if (string.IsNullOrWhiteSpace(paras.ParkingID))
                return string.Empty;
            orderlist = StatisticsServices.Search_MonthStatistics(paras);
            rdocument.Load(Server.MapPath("../../Report/ReportViewer/ReportView_Daily.rpt"));
            rdocument.SetDataSource(orderlist);
            CrystalDecisions.Shared.DiskFileDestinationOptions DiskOpts = new CrystalDecisions.Shared.DiskFileDestinationOptions();
            rdocument.ExportOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;
            rdocument.ExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.Excel;
            string filename = "Daily" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            DiskOpts.DiskFileName = Server.MapPath("~/") + @"Report\\ReportFile\\" + filename;
            rdocument.ExportOptions.DestinationOptions = DiskOpts;
            rdocument.Export();
            return filename;

        }
        #endregion

        #region 二维码推广人员
        /// <summary>
        /// 二维码推广统计
        /// </summary>
        /// <returns></returns>
        [CheckPurview(Roles = "PK010510")]
        public ActionResult StatisticsPersontg()
        {
            return View();
        }
        /// <summary>
        /// 统计推广人员订单记录，按人员id进行分组
        /// </summary>
        /// <returns></returns> 
        public string CountTgPersonOrder()
        {
            try
            {
                int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int rows = string.IsNullOrEmpty(Request.Params["rows"]) ? 0 : int.Parse(Request.Params["rows"]);
                InParams paras = new InParams
                {
                    ParkingID = Request.Params["parkingid"], 
                    StartTime = DateTime.Parse(Request.Params["starttime"]),
                    EndTime = DateTime.Parse(Request.Params["endtime"])
                }; 
                Pagination pagination = TgOrderService.CountTgPersonOrder(paras, rows, page);
                StringBuilder sb = new StringBuilder();
                string str = JsonHelper.GetJsonString(pagination.StatisticsTgOrderList);
                sb.Append("{");
                sb.Append("\"total\":" + pagination.Total + ",");
                sb.Append("\"rows\":" + str + ",");
                sb.Append("\"index\":" + rows);
                sb.Append("}");
                return sb.ToString();
            }
            catch(Exception ex)
            {
                return string.Empty;
            }
        }

        public ActionResult Export_TgOrderInfo()
        {
            try
            {
                InParams paras = new InParams
                {
                    ParkingID = Request.Params["parkingid"],
                    StartTime = DateTime.Parse(Request.Params["starttime"]),
                    EndTime = DateTime.Parse(Request.Params["endtime"])
                };
                // 1.获取数据集合  
                List<TgOrder> result = TgOrderService.QueryAllCountTgPersonOrder(paras);
                var obj = (from p in result
                           select new
                           {
                               pKName = p.PKName,
                               personId = p.PersonId ,
                               personName = p.PersonName,
                               count = p.Count,
                               amount = p.Amount
                               
                           }).ToList();
                string str = JsonHelper.GetJsonString(obj);
                var dt = JsonToDataTable(str.ToString());
                // 2.设置单元格抬头
                Dictionary<string, string> cellheader = new Dictionary<string, string> {
                    { "pKName", "车场名称" },
                    { "personId", "人员编号" },
                    { "personName", "人员名称" },
                    { "count", "推广次数" },
                    { "amount", "推广获利总额" },                    
                };
                return EntityListToExcel(cellheader, dt, "推广二维码统计记录");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [ValidateInput(false)]
        /// <summary>
        /// 实体类集合导出到Excle2003
        /// </summary>
        /// <param name="cellHeard">单元头的Key和Value：{ { "UserName", "姓名" }, { "Age", "年龄" } };</param>
        /// <param name="enList">数据源</param>
        /// <param name="sheetName">工作表名称</param>
        /// <returns>文件的下载地址</returns>
        public ActionResult EntityListToExcel(Dictionary<string, string> cellHeard, DataTable dt, string sheetName)
        {
            try
            {
                //创建Excel文件的对象  
                NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
                //添加一个sheet  
                NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("OrderInfo");
                //给sheet1添加第一行的头部标题  
                NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
                List<string> keys = cellHeard.Keys.ToList();
                for (int i = 0; i < keys.Count; i++)
                {
                    row1.CreateCell(i).SetCellValue(cellHeard[keys[i]]); // 列名为Key的值
                }
                //将数据逐步写入sheet1各个行  
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    NPOI.SS.UserModel.IRow row2 = sheet1.CreateRow(i + 1);
                    for (int j = 0; j < dt.Columns.Count; j++)
                        row2.CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
                }

                // 返回下载路径
                string pathToFiles = Server.MapPath("/");
                string filename = sheetName + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
                string path = @"" + pathToFiles + filename;

                //写入到客户端
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    book.Write(ms);

                    using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                    {
                        byte[] data = ms.ToArray();
                        fs.Write(data, 0, data.Length);
                        fs.Flush();
                    }
                    book = null;

                }
                return Content("/" + filename);//返回文件名提供下载  
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable JsonToDataTable(string strJson)
        {
            DataTable tb = null;
            //获取数据  
            Regex rg = new Regex(@"(?<={)[^}]+(?=})");
            MatchCollection mc = rg.Matches(strJson);
            for (int i = 0; i < mc.Count; i++)
            {
                string strRow = mc[i].Value;
                string[] strRows = strRow.Split(',');
                //创建表  
                if (tb == null)
                {
                    tb = new DataTable();
                    tb.TableName = "";
                    foreach (string str in strRows)
                    {
                        DataColumn dc = new DataColumn();
                        string[] strCell = str.Split(':');

                        dc.ColumnName = strCell[0].ToString().Replace("\"", "").Trim();
                        tb.Columns.Add(dc);
                    }
                    tb.AcceptChanges();
                }

                //增加内容  
                DataRow dr = tb.NewRow();
                for (int r = 0; r < strRows.Length; r++)
                {
                    dr[r] = strRows[r].Split(':')[1].Trim().Replace("，", ",").Replace("：", ":").Replace("/", "").Replace("\"", "").Trim();
                }
                tb.Rows.Add(dr);
                tb.AcceptChanges();
            }

            return tb;
        }

        public string Searchrwm()
        {
            try
            {
                InParams paras = new InParams
                {
                    ParkingID = Request.Params["parkingid"],
                    StartTime = DateTime.Parse(Request.Params["starttime"] + " 00:00:00"),
                    EndTime = DateTime.Parse(Request.Params["endtime"] + " 23:59:59")
                };
                int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int rows = string.IsNullOrEmpty(Request.Params["rows"]) ? 0 : int.Parse(Request.Params["rows"]);
                int counts = Search_rwmCount(paras);
                List<userinfo> parkorder = new List<userinfo>();
                string strSql = "select top(" + rows + ") temp.* from (select top(35) temp.* from (select top(35)s.times as times,g.name as name,g.phone as phone,s.username,s.time as time from TgCount s,PersonTg g where s.keyid='qrscene_'+  CAST(g.id AS nvarchar(20)) and 1=1";
                if (!string.IsNullOrEmpty(paras.ParkingID))
                {
                    strSql += " and g.id=@ParkingID";
                }
                if (paras.StartTime != null)
                {
                    strSql += " and s.times>=@StartTime";
                }
                if (paras.EndTime != null)
                {
                    strSql += " and s.times<=@EndTime";
                }
                strSql += " )as temp)as temp";
                //strSql += string.Format(" order by c.CreateTime desc) AS temp WHERE temp.rownum BETWEEN {0} AND {1} order by CreateTime desc", PageSize * (PageIndex - 1) + 1, PageSize * PageIndex);
                using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
                {
                    dboperator.ClearParameters();
                    dboperator.AddParameter("ParkingID", paras.ParkingID);
                    dboperator.AddParameter("StartTime", paras.StartTime);
                    dboperator.AddParameter("EndTime", paras.EndTime);
                    using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                    {
                        while (dr.Read())
                        {
                            parkorder.Add(DataReaderToModel<userinfo>.ToModel(dr));
                        }
                    }
                }

                StringBuilder sb = new StringBuilder();
                sb.Append("{");
                sb.Append("\"total\":" + counts + ",");
                sb.Append("\"rows\":" + JsonHelper.GetJsonString(parkorder) + ",");
                sb.Append("\"index\":" + rows);
                sb.Append("}");
                return sb.ToString();

            }
            catch
            {
                return string.Empty;
            }
        }

        public int Search_rwmCount(InParams paras)
        {
            int _total = 0;
            string strSql2 = "select count(1) Count from (select s.times as times,g.name as name,g.phone as phone,s.username,s.time as time from TgCount s,PersonTg g where s.keyid='qrscene_'+  CAST(g.id AS nvarchar(20)) and 1=1";
            if (!string.IsNullOrEmpty(paras.ParkingID))
            {
                strSql2 += " and g.id=@ParkingID";
            }
            if (paras.StartTime != null)
            {
                strSql2 += " and s.times>=@StartTime";
            }
            if (paras.EndTime != null)
            {
                strSql2 += " and s.times<=@EndTime";
            }
            strSql2 += ") a";
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("ParkingID", paras.ParkingID);
                dboperator.AddParameter("StartTime", paras.StartTime);
                dboperator.AddParameter("EndTime", paras.EndTime);
                using (DbDataReader dr = dboperator.ExecuteReader(strSql2))
                {
                    if (dr.Read())
                    {
                        _total = int.Parse(dr["Count"].ToString());
                    }
                }
            }
            return _total;
        }

        #endregion

        #region 运营数据

        public ActionResult overment()
        {
            return View();
        }

        public string Search_Goverment()
        {
            try
            {
                InParams paras = new InParams
                {
                    ParkingID = Request.Params["parkingid"],
                    StartTime = DateTime.Parse(Request.Params["starttime"] + " 00:00:00"),
                    EndTime = DateTime.Parse(Request.Params["endtime"] + " 23:59:59")
                };
                List<goverment> goverments = new List<goverment>();
                string sqlstr = "select * from (select b.PKName,a.* from (select MAX(ParkingID) as pkid, SUM(Receivable_Amount) as 'ys',"
                    + "SUM(Real_Amount) as 'ss',SUM(Cash_Amount) as 'xj',SUM(OnLine_Amount) as 'xs',"
                    + "SUM(OnLine_Amount)/(SUM(OnLine_Amount)+SUM(Cash_Amount)+1) as 'bl',"
                    + "sum(Exit_Count) as 'fxcs',SUM(ReleaseType_Free) as 'mfcs',SUM(ReleaseType_Catch) as 'sffxcs',"
                    + "sum(OnLine_Count) as 'ssjfcs' from Statistics_Gather where OnLine_Amount>=0 and GatherTime >='" + paras.StartTime+ "' and"
                    + " GatherTime <'"+paras.EndTime+"' group by ParkingID ) as a,BaseParkinfo b where a.pkid = b.PKID and a.pkid='"+paras.ParkingID+"') as s "
                    + "left join "
                    + "(select COUNT(*) as zs,MAX(PKID) as pkid from OnlineOrder where   OrderTime >='"+paras.StartTime+"' and OrderTime <'"+paras.EndTime+"' and "
                    + "Payer not in (select Payer from OnlineOrder where OrderTime <'"+paras.EndTime+"') group by PKID) as b "
                    + "on s.pkid = b.pkid";
                int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int rows = string.IsNullOrEmpty(Request.Params["rows"]) ? 0 : int.Parse(Request.Params["rows"]);
                int counts = 30;
                using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
                {
                    dboperator.ClearParameters();
                    using (DbDataReader dr = dboperator.ExecuteReader(sqlstr))
                    {
                        while (dr.Read())
                        {
                            goverments.Add(DataReaderToModel<goverment>.ToModel(dr));
                        }
                    }
                }
                StringBuilder sb = new StringBuilder();
                sb.Append("{");
                sb.Append("\"total\":" + counts + ",");
                sb.Append("\"rows\":" + JsonHelper.GetJsonString(goverments) + ",");
                sb.Append("\"index\":" + rows);
                sb.Append("}");
                return sb.ToString();
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        #endregion
    }
}
