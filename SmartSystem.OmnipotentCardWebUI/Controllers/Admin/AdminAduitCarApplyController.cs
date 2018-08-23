using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin;
using System.Text;
using Common.Entities;
using Common.Services.Park;
using Common.Entities.Parking;
using Common.Services;
using Common.Utilities;
using SmartSystem.OmnipotentCardWebUI.Models;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.Admin
{
    /// <summary>
    /// 月租车审核
    /// </summary>
    [CheckWeiXinPurview(Roles = "Login")]
    [CheckPurview(Roles = "PK010104")]
    public class AdminAduitCarApplyController : WeiXinController
    {
        public ActionResult Index()
        {
            List<BaseParkinfo> parings = ParkingServices.QueryParkingByVillageIds(GetLoginUserVillages.Select(p => p.VID).ToList());
            ViewBag.StartTime = DateTime.Now.AddDays(-7).Date.AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss").Replace(" ", "T");
            ViewBag.EndTime = DateTime.Now.Date.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss").Replace(" ", "T");
            return View(parings);
        }
        public ActionResult GetAduitMonthlyCarApplyData()
        {
            try
            {

                List<string> parkingIds = new List<string>();
                if (!string.IsNullOrEmpty(Request.Params["parkingId"]))
                {
                    parkingIds.Add(Request.Params["parkingId"].ToString());
                }
                else {
                    List<BaseParkinfo> parings = ParkingServices.QueryParkingByVillageIds(GetLoginUserVillages.Select(p => p.VID).ToList());
                    parkingIds = parings.Select(p => p.PKID).ToList();
                }
                if (parkingIds.Count == 0) throw new MyException("获取车场编号失败");

                string applyInfo = Request.Params["applyInfo"];

                DateTime startTime = DateTime.Parse(Request.Params["starttime"]);
                DateTime endTime = DateTime.Parse(Request.Params["endtime"]);

                MonthlyCarApplyStatus? status = null;
                int intStatus = 0;
                if (!string.IsNullOrWhiteSpace(Request.Params["status"]) && int.TryParse(Request.Params["status"].ToString(), out intStatus))
                {
                    status = (MonthlyCarApplyStatus)intStatus;
                }
                int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int rows = 15;

                int total = 0;
                List<ParkMonthlyCarApply> models = ParkMonthlyCarApplyServices.QueryPage(parkingIds, applyInfo, status, startTime, endTime, rows, page, out total);

                var result = from p in models
                             select new
                             {
                                 ID = p.ID,
                                 RecordID = p.RecordID,
                                 PKID = p.PKID,
                                 CarTypeID = p.CarTypeID,
                                 CarModelID = p.CarModelID,
                                 ApplyName = p.ApplyName,
                                 ApplyMoblie = p.ApplyMoblie,
                                 PlateNo = GetNullToEmpty(p.PlateNo),
                                 PKLot = GetNullToEmpty(p.PKLot),
                                 FamilyAddress = GetNullToEmpty(p.FamilyAddress),
                                 ApplyRemark = GetNullToEmpty(p.ApplyRemark),
                                 ApplyStatus = (int)p.ApplyStatus,
                                 ApplyStatusDes = p.ApplyStatus.GetDescription(),
                                 AuditRemark =  GetNullToEmpty(p.AuditRemark),
                                 ApplyDateTime = p.ApplyDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                 CarTypeName = p.CarTypeName,
                                 CarModelName = p.CarModelName,
                                 PKName=p.PKName,
                             };

                return Json(MyResult.Success("", result));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("AdminAduitCarApply", "获取月租车申请信息失败", ex, LogFrom.WeiXin);
                return Json(MyResult.Error("获取月租车申请信息失败"));
            }
        }
        public string GetNullToEmpty(string value) {
            if (string.IsNullOrWhiteSpace(value)) {
                return string.Empty;
            }
            return value;
        }
        /// <summary>
        /// 审核
        /// </summary>
        /// <returns></returns>
         [CheckPurview(Roles = "PK01020701")]
        public ActionResult Passed(string recordId)
        {
            try
            {
                List<BaseVillage> villages = VillageServices.QueryVillageByEmployeeMobilePhone(WeiXinUser.MobilePhone);
                List<EnumContext> parkContexts = new List<EnumContext>();
                if (villages.Count > 0)
                {
                    List<BaseParkinfo> parkings = ParkingServices.QueryParkingByVillageIds(villages.Select(p => p.VID).ToList());
                    foreach (var item in parkings)
                    {
                        EnumContext model = new EnumContext();
                        model.Description = item.PKName;
                        model.EnumString = item.PKID;
                        parkContexts.Add(model);
                    }
                }
                ViewBag.ParkContexts = parkContexts;
 
                ParkMonthlyCarApply monthlyCarApply = ParkMonthlyCarApplyServices.QueryByRecordID(recordId);
                if (monthlyCarApply == null) throw new MyException("申请信息不存在");

                List<ParkArea> areas = ParkAreaServices.GetParkAreaByParkingId(monthlyCarApply.PKID);
                List<EnumContext> areaContexts = new List<EnumContext>();
                foreach (var item in areas)
                {
                    EnumContext model = new EnumContext();
                    model.Description = item.AreaName;
                    model.EnumString = item.AreaID;
                    areaContexts.Add(model);
                }
                ViewBag.AreaContexts = areaContexts;

                List<EnumContext> gateContexts = new List<EnumContext>();
                foreach (var item in areaContexts) {
                    List<ParkGate> gates = ParkGateServices.QueryByParkAreaRecordIds(new List<string>() { item.EnumString});

                    foreach (var gate in gates)
                    {
                        EnumContext model = new EnumContext();
                        model.Description = gate.GateName;
                        model.EnumString = string.Format("{0}|{1}", gate.GateID, item.EnumString);
                        gateContexts.Add(model);
                    }
                }
               
                ViewBag.GateContexts = gateContexts;

                return View(monthlyCarApply);
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("AdminAduitCarApply", "查看月租车申请详情失败", ex, LogFrom.WeiXin);
                return RedirectToAction("Index", "AdminAduitCarApply", new { RemindUserContent = "审核失败" });
            }
        }
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="RecordID"></param>
        /// <param name="AuditRemark"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckPurview(Roles = "PK01020701")]
        public ActionResult Passed(string RecordID, string AuditRemark, string CarTypeID, string CarModelID, string AreaIDS, string GateID)
        {
            try
            {
                bool result = ParkMonthlyCarApplyServices.Passed(RecordID, AuditRemark, CarTypeID, CarModelID, AreaIDS, GateID, AdminLoginUser.RecordID);
                if (!result) throw new MyException("审核失败");

                return RedirectToAction("Index", "AdminAduitCarApply", new { RemindUserContent = "审核成功" });
            }
            catch (MyException ex)
            {
                return RedirectToAction("Passed", "AdminAduitCarApply", new { recordId = RecordID, RemindUserContent = ex.Message });
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "审核月租车申请操作失败");
                return RedirectToAction("Passed", "AdminAduitCarApply", new { recordId = RecordID, RemindUserContent = "审核操作失败" });
            }
        }
        /// <summary>
        /// 拒绝
        /// </summary>
        /// <returns></returns>
        [CheckPurview(Roles = "PK01020702")]
        public ActionResult Refused(string recordId)
        {
            try
            {
             
                ParkMonthlyCarApply monthlyCarApply = ParkMonthlyCarApplyServices.QueryByRecordID(recordId);
                if (monthlyCarApply == null) throw new MyException("申请信息不存在");
                return View(monthlyCarApply);
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("AdminAduitCarApply", "查看月租车申请详情失败", ex, LogFrom.WeiXin);
                return RedirectToAction("Index", "AdminAduitCarApply", new { RemindUserContent = "拒绝失败" });
            }
        }
        [HttpPost]
        [CheckPurview(Roles = "PK01020702")]
        public ActionResult Refused(string RecordID, string AuditRemark)
        {
            try
            {
                bool result = ParkMonthlyCarApplyServices.Refused(RecordID, AuditRemark);
                if (!result) throw new MyException("拒绝失败");

                return RedirectToAction("Index", "AdminAduitCarApply", new { RemindUserContent = "拒绝成功" });
            }
            catch (MyException ex)
            {
                return RedirectToAction("Refused", "AdminAduitCarApply", new { recordId = RecordID, RemindUserContent = ex.Message });
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "拒绝月租车申请操作失败");
                return RedirectToAction("Refused", "AdminAduitCarApply", new { recordId = RecordID, RemindUserContent = "拒绝失败" });
            }
        }
        [HttpPost]
        public JsonResult GetCarTypeData(string parkingId)
        {
            try
            {
                List<ParkCarType> carTypes = ParkCarTypeServices.QueryParkCarTypeByParkingId(parkingId);
                carTypes = carTypes.Where(p => p.BaseTypeID != BaseCarType.TempCar).ToList();
                return Json(MyResult.Success("", carTypes));
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "获取车类失败");
                return Json(MyResult.Error("获取车类失败"));
            }
        }
        [HttpPost]
        public JsonResult GetCarModelData(string parkingId)
        {
            try
            {
                List<ParkCarModel> carModels = ParkCarModelServices.QueryByParkingId(parkingId);
                return Json(MyResult.Success("", carModels));
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "获取车型失败");
                return Json(MyResult.Error("获取车型失败"));
            }
        }
    }
}
