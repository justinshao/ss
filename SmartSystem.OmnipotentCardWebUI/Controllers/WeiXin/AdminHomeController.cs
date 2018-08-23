using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.OmnipotentCardWebUI.Models;
using Common.Services;
using Common.Entities;
using Common.Entities.Statistics;
using Common.Entities.Enum;
using Common.Services.Statistics;
using Common.Entities.Other;
using Common.Utilities.Helpers;
using Common.Entities.Parking;
using Common.Services.Park;
using SmartSystem.WeiXinInerface;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin
{
    /// <summary>
    /// 管理员微信首页
    /// </summary>
    [CheckWeiXinAdminPurview]
    public class AdminHomeController : WeiXinController
    {
        public ActionResult Index()
        {
            ViewBag.WeiXinHead = WeiXinUser == null || string.IsNullOrWhiteSpace(WeiXinUser.Headimgurl) ? "/Content/images/weixin/defaultweixinhead.png" : WeiXinUser.Headimgurl;
            ViewBag.AdminUserAccount = AdminLoginUser.UserAccount;
            return View();
        }
        /// <summary>
        /// 在场车辆
        /// </summary>
        /// <returns></returns>
        public ActionResult PresentCar()
        {
            List<BaseParkinfo> parings = ParkingServices.QueryParkingByVillageIds(GetLoginUserVillages.Select(p => p.VID).ToList());
            ViewBag.StartTime = DateTime.Now.Date.AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss").Replace(" ", "T");
            ViewBag.EndTime = DateTime.Now.Date.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss").Replace(" ", "T");
            return View(parings);
        }
        public ActionResult GetPresentCarData()
        {
            try
            {
                int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int rows = 15;

                InParams paras = new InParams
                {
                    ParkingID = Request.Params["parkingid"],
                    PlateNumber = Request.Params["platenumber"],
                    StartTime = DateTime.Parse(Request.Params["starttime"]),
                    EndTime = DateTime.Parse(Request.Params["endtime"]),
                    ReportType = ReportType.Presence,
                    CardType = "-1",
                    InGateID = "-1"
                };

                Pagination pagination = StatisticsServices.Search_Presence(paras, rows, page);
                var result = from p in pagination.IORecordsList
                             select new
                             {
                                 PKName = p.PKName,
                                 PlateNumber = p.PlateNumber,
                                 CarTypeName = p.CarTypeName,
                                 InGateName = p.InGateName,
                                 AreaName = p.AreaName,
                                 EntranceTime = p.EntranceTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                 LongTime = p.LongTime,
                                 EmployeeName = string.IsNullOrWhiteSpace(p.EmployeeName) ? "" : p.EmployeeName,
                                 MobilePhone = string.IsNullOrWhiteSpace(p.MobilePhone) ? "" : p.MobilePhone,
                                 InOperatorName = p.InOperatorName
                             };
                return Json(MyResult.Success("", result));

            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "微信管理端获取在场车辆失败");
                return Json(MyResult.Error("获取失败"));
            }
        }
        /// <summary>
        /// 进出记录
        /// </summary>
        /// <returns></returns>
        public ActionResult InOutRecord()
        {
            ViewBag.StartTime = DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm:ss").Replace(" ", "T");
            ViewBag.EndTime = DateTime.Now.Date.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss").Replace(" ", "T");
            return View();
        }
        public ActionResult GetInOutRecordData()
        {
            try
            {
                int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int rows = 15;
                InParams paras = new InParams
                {
                    ParkingID = Request.Params["parkingid"],
                    CardType = Request.Params["cardtypeid"],
                    CarType = Request.Params["cartypeid"],
                    OutGateID = "-1",
                    InGateID = "-1",
                    OutOperator = Request.Params["exitoperatorid"],
                    ReleaseType = int.Parse(Request.Params["releasetype"]),
                    AreaID = "-1",
                    StartTime = DateTime.Parse(Request.Params["starttime"]),
                    EndTime = DateTime.Parse(Request.Params["endtime"]),
                    IsExit = int.Parse(Request.Params["isexit"]),
                    PlateNumber = Request.Params["platenumber"],
                    Owner = Request.Params["owner"],
                    ReportType = ReportType.InOut
                };

                Pagination pagination = StatisticsServices.Search_InOutRecords(paras, rows, page);
                var result = from p in pagination.IORecordsList
                             select new
                             {
                                 PKName = p.PKName,
                                 PlateNumber = p.PlateNumber,
                                 CarTypeName = p.CarTypeName,
                                 InGateName = p.InGateName,
                                 AreaName = p.AreaName,
                                 EntranceTime = p.EntranceTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                 LongTime = p.LongTime,
                                 EmployeeName = string.IsNullOrWhiteSpace(p.EmployeeName) ? "" : p.EmployeeName,
                                 MobilePhone = string.IsNullOrWhiteSpace(p.MobilePhone) ? "" : p.MobilePhone,
                                 InOperatorName = p.InOperatorName,
                                 IsExit = p.IsExit ? "已出场" : "在场",
                                 CarModelName = p.CarModelName,
                                 ReleaseTypeName = p.ReleaseTypeName,
                                 ExitTime = p.IsExit ? p.ExitTime.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty,
                                 OutGateName = string.IsNullOrWhiteSpace(p.OutGateName) ? "" : p.OutGateName,
                             };
                return Json(MyResult.Success("", result));

            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "微信管理端获取进出记录失败");
                return Json(MyResult.Error("获取失败"));
            }
        }
        /// <summary>
        /// 停车缴费记录
        /// </summary>
        /// <returns></returns>
        public ActionResult ParkPaymentRecord()
        {
            ViewBag.StartTime = DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm:ss").Replace(" ", "T");
            ViewBag.EndTime = DateTime.Now.Date.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss").Replace(" ", "T");
            return View();
        }
        public ActionResult GetParkPaymentRecordData()
        {
            try
            {
                int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int rows = 15;
                InParams paras = new InParams
                {
                    ParkingID = Request.Params["parkingid"],
                    OnLineOffLine = int.Parse(Request.Params["onlineoffline"]),
                    OrderSource = int.Parse(Request.Params["ordersource"]),
                    StartTime = DateTime.Parse(Request.Params["starttime"]),
                    EndTime = DateTime.Parse(Request.Params["endtime"]),
                    BoxID = Request.Params["boxid"],
                    PlateNumber = Request.Params["platenumber"],
                    OutOperator = Request.Params["exitoperatorid"],
                    ReleaseType = -1,
                    ReportType = ReportType.TempPay
                };

                Pagination pagination = StatisticsServices.Search_TempPays(paras, rows, page);
                var result = from p in pagination.OrderList
                             select new
                             {
                                 PKName = p.PKName,
                                 PlateNumber = p.PlateNumber,
                                 OrderNo = p.OrderNo,
                                 Amount = p.Amount,
                                 PayAmount = p.PayAmount,
                                 UnPayAmount = p.UnPayAmount,
                                 DiscountAmount = p.DiscountAmount.ToString(),
                                 OrderTime = p.OrderTime!=DateTime.MinValue ? p.OrderTime.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty,
                                 EntranceTime = p.EntranceTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                 LongTime = p.LongTime,
                                 PayWayName = p.PayWayName,
                                 OrderSourceName = p.OrderSourceName,
                                 Operator = string.IsNullOrWhiteSpace(p.Operator) ? string.Empty : p.Operator,
                                 ExitTime = p.ExitTime.ToString("yyyy-MM-dd HH:mm:ss")
                             };
                return Json(MyResult.Success("", result));

            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "微信管理端获取停车缴费记录失败");
                return Json(MyResult.Error("获取失败"));
            }
        }
        /// <summary>
        /// 日汇总
        /// </summary>
        /// <returns></returns>
        public ActionResult DayGather()
        {
            ViewBag.StartTime = DateTime.Now.Date.AddDays(-7).ToString("yyyy-MM-dd");
            ViewBag.EndTime = DateTime.Now.Date.ToString("yyyy-MM-dd");
            return View();
        }
        public JsonResult GetDayGatherData()
        {
            try
            {
                int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int rows = 15;
                InParams paras = new InParams
                {
                    ParkingID = Request.Params["parkingid"],
                    StartTime = DateTime.Parse(Request.Params["starttime"] + " 00:00:00"),
                    EndTime = DateTime.Parse(Request.Params["endtime"] + " 23:59:59")
                };

                Pagination pagination = StatisticsServices.Search_DailyStatistics(paras, rows, page);
                var result = from p in pagination.StatisticsGatherList
                             select new
                             {
                                 ParkingName = p.ParkingName,
                                 KeyName = p.KeyName,
                                 Receivable_Amount = p.Receivable_Amount,
                                 Real_Amount = p.Real_Amount,
                                 Diff_Amount = p.Diff_Amount,
                                 Entrance_Count = p.Entrance_Count,
                                 Exit_Count = p.Exit_Count,
                                 ReleaseType_Normal = p.ReleaseType_Normal,
                                 ReleaseType_Charge = p.ReleaseType_Charge,
                                 ReleaseType_Free = p.ReleaseType_Free,
                                 ReleaseType_Catch = p.ReleaseType_Catch,
                                 VIPExtend_Count = p.VIPExtend_Count,
                                 OnLineMonthCardExtend_Count = p.OnLineMonthCardExtend_Count,
                                 MonthCardExtend_Count = p.MonthCardExtend_Count,
                                 OnLineStordCard_Count = p.OnLineStordCard_Count,
                                 StordCardRecharge_Count = p.StordCardRecharge_Count
                             };
                return Json(MyResult.Success("", result));

            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "微信管理端获取日汇总失败");
                return Json(MyResult.Error("获取失败"));
            }
        }
        /// <summary>
        /// 月汇总
        /// </summary>
        /// <returns></returns>
        public ActionResult MonthGather()
        {
            ViewBag.StartTime = DateTime.Now.Date.AddMonths(-5).ToString("yyyy-MM-dd");
            ViewBag.EndTime = DateTime.Now.Date.ToString("yyyy-MM-dd");
            return View();
        }
        public ActionResult GetMonthGatherData()
        {
            try
            {
                int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int rows = 15;
                InParams paras = new InParams
                {
                    ParkingID = Request.Params["parkingid"],
                    StartTime = DateTime.Parse(Request.Params["starttime"] + "-01 00:00:00"),
                    EndTime = DateTime.Parse(Request.Params["endtime"] + "-01 00:00:00").AddSeconds(-1).AddMonths(1)
                };

                Pagination pagination = StatisticsServices.Search_MonthStatistics(paras, rows, page);
                return Json(MyResult.Success("", pagination.StatisticsGatherList));

            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "微信管理端获取月汇总失败");
                return Json(MyResult.Error("获取失败"));
            }
        }
        /// <summary>
        /// 异常放行
        /// </summary>
        /// <returns></returns>
        public ActionResult ExceptionRelease()
        {
            ViewBag.StartTime = DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm:ss").Replace(" ", "T");
            ViewBag.EndTime = DateTime.Now.Date.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss").Replace(" ", "T");
            return View();
        }
        /// <summary>
        /// 获取异常放行数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetExceptionReleaseData()
        {
            try
            {
                int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int rows = 15;
                InParams paras = new InParams
                {
                    ParkingID = Request.Params["parkingid"],
                    PlateNumber = Request.Params["platenumber"],
                    OutGateID = Request.Params["exitgateid"],
                    StartTime = DateTime.Parse(Request.Params["starttime"]),
                    EndTime = DateTime.Parse(Request.Params["endtime"]),
                    ReportType = ReportType.ExceptionRelease
                };

                Pagination pagination = StatisticsServices.Search_ExceptionRelease(paras, rows, page);
                var result = from p in pagination.IORecordsList
                             select new
                             {
                                 PKName = p.PKName,
                                 PlateNumber = p.PlateNumber.Contains("无车牌") ? "无车牌" : p.PlateNumber,
                                 CarTypeName = p.CarTypeName,
                                 InGateName = p.InGateName,
                                 AreaName = p.AreaName,
                                 EntranceTime = p.EntranceTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                 EmployeeName = string.IsNullOrWhiteSpace(p.EmployeeName) ? "" : p.EmployeeName,
                                 MobilePhone = string.IsNullOrWhiteSpace(p.MobilePhone) ? "" : p.MobilePhone,
                                 InOperatorName = p.InOperatorName,
                                 CarModelName = p.CarModelName,
                                 ReleaseTypeName = p.ReleaseTypeName,
                                 ExitTime = p.IsExit ? p.ExitTime.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty,
                                 OutGateName = string.IsNullOrWhiteSpace(p.OutGateName) ? "" : p.OutGateName,
                                 Remark = p.Remark
                             };
                return Json(MyResult.Success("", pagination.IORecordsList));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "微信管理端获取异常放行数据失败");
                return Json(MyResult.Error("获取失败"));
            }
        }
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
            string parkingid = Request.Params["parkingid"];
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

        /// <summary>
        /// 远程开闸
        /// </summary>
        /// <returns></returns>
        public ActionResult RemotelyOpenGate()
        {
            return View();
        }
        public JsonResult QueryRemotelyOpenGateData()
        {
            try
            {
                List<string> parkingIds = new List<string>();
                if (!string.IsNullOrWhiteSpace(Request.Params["parkingId"]))
                {
                    parkingIds.Add(Request.Params["parkingId"]);
                }
                else {
                   List<BaseParkinfo> parkings = ParkingServices.QueryParkingByVillageIds(GetLoginUserVillages.Select(p => p.VID).ToList());
                   if (parkings.Count > 0) {
                       parkingIds.AddRange(parkings.Select(p=>p.PKID));
                   }
                }
                string areaId = Request.Params["areaId"];
                string boxId = Request.Params["boxId"];

                int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int rows = 15;
                int recordTotalCount = 0;
                List<RemotelyOpenGateView> models = ParkGateServices.QueryRemotelyOpenGate(parkingIds, areaId, boxId, page, rows, out recordTotalCount);
                return Json(MyResult.Success("", models));

            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "微信获取远程开闸数据失败");
                return Json(MyResult.Error("获取失败"));
            }
        }
        [HttpPost]
        public JsonResult OpenGate(string parkingId, string gateId, string remark) {
            try
            {
               int result = AdminOperateServices.RemoteGate(AdminLoginUser.RecordID, parkingId, gateId, remark);
               if (result == 0) {
                   return Json(MyResult.Success());
               }
               if (result == 1) throw new MyException("车场网络异常");
               if (result == 2) throw new MyException("通道不支持远程开门");
               throw new MyException("开闸失败");

            }
            catch (MyException ex) {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "远程开闸失败");
                return Json(MyResult.Error("远程开闸失败"));
            }
        }
    }
}
