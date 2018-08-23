using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.OmnipotentCardWebUI.Controllers;
using System.Text;
using Common.Services.Park;
using Common.Entities;
using Common.Entities.Parking;
using Common.Utilities.Helpers;
using Common.Services;
using SmartSystem.OmnipotentCardWebUI.Models;
using Common.Utilities;
using Common.Services.BaseData;

namespace SmartSystem.OmnipotentCardWebUI.Areas.Parking.Controllers
{
    /// <summary>
    /// 月租车申请审核
    /// </summary>
    [CheckPurview(Roles = "PK010207")]
    public class AduitMonthlyCarApplyController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.StartTime = DateTime.Now.Date;
            ViewBag.EndTime = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
            return View();
        }
        public string GetAduitMonthlyCarApplyData()
        {
            StringBuilder strData = new StringBuilder();
            try
            {
                if (string.IsNullOrEmpty(Request.Params["parkingId"])) return string.Empty;
                List<string> parkingIds = new List<string>();
                parkingIds.Add(Request.Params["parkingId"].ToString());

                string applyInfo = Request.Params["applyInfo"];

                 DateTime startTime = DateTime.Parse(Request.Params["starttime"]);
                  DateTime  endTime = DateTime.Parse(Request.Params["endtime"]);

                MonthlyCarApplyStatus? status=null;
                int intStatus=0;
                if (!string.IsNullOrWhiteSpace(Request.Params["status"]) && int.TryParse(Request.Params["status"].ToString(), out intStatus)) {
                    status = (MonthlyCarApplyStatus)intStatus;
                }
                int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int rows = string.IsNullOrEmpty(Request.Params["rows"]) ? 0 : int.Parse(Request.Params["rows"]);

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
                                 PlateNo = p.PlateNo,
                                 PKLot = p.PKLot,
                                 FamilyAddress = p.FamilyAddress,
                                 ApplyRemark = p.ApplyRemark,
                                 ApplyStatus = (int)p.ApplyStatus,
                                 ApplyStatusDes = p.ApplyStatus.GetDescription(),
                                 AuditRemark = p.AuditRemark,
                                 ApplyDateTime = p.ApplyDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                 CarTypeName = p.CarTypeName,
                                 CarModelName = p.CarModelName
                             };

                strData.Append("{");
                strData.Append("\"total\":" + total + ",");
                strData.Append("\"rows\":" + JsonHelper.GetJsonString(result) + ",");
                strData.Append("\"index\":" + page);
                strData.Append("}");
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "查询月租车申请信息失败");
            }

            return strData.ToString();
        }
        public string GetAreaDataByParkingId(string parkingId)
        {
            StringBuilder strTree = new StringBuilder();
            strTree.Append("[");
            strTree.Append("{\"id\":\"-1\",");
            strTree.Append("\"text\":\"所有\",\"selected\":true");
            strTree.Append("}");

            if (!string.IsNullOrWhiteSpace(parkingId))
            {
                List<ParkArea> areas = ParkAreaServices.GetParkAreaByParkingId(parkingId);
                foreach (var item in areas)
                {
                    strTree.Append(",{\"id\":\"" + item.AreaID + "\",");
                    strTree.Append("\"text\":\"" + item.AreaName + "\"");
                    strTree.Append("}");
                }
            }
            strTree.Append("]");
            return strTree.ToString();
        }
        public string GetGateDataByAreaIds(string areaIds)
        {
            StringBuilder strTree = new StringBuilder();
            strTree.Append("[");
            strTree.Append("{\"id\":\"-1\",");
            strTree.Append("\"text\":\"所有\",\"selected\":true");
            strTree.Append("}");

            if (!string.IsNullOrWhiteSpace(areaIds))
            {
                List<string> areaIdlist = areaIds.Split(',').ToList();
                List<ParkGate> gates = ParkGateServices.QueryByParkAreaRecordIds(areaIdlist).ToList();
                foreach (var item in gates)
                {
                    strTree.Append(",{\"id\":\"" + item.GateID + "\",");
                    strTree.Append("\"text\":\"" + item.GateName + "\"");
                    strTree.Append("}");
                }
            }
            strTree.Append("]");
            return strTree.ToString();
        }
        [HttpPost]
        public JsonResult GetCarTypeData(string parkingId)
        {
            JsonResult json = new JsonResult();
            try
            {
                List<ParkCarType> carTypes = ParkCarTypeServices.QueryParkCarTypeByParkingId(parkingId);
                json.Data = carTypes;
                return json;
            }
            catch (MyException ex)
            {
                ExceptionsServices.AddExceptions(ex, ex.Message);
                return json;
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "获取车类失败");
                return json;
            }
        }
        [HttpPost]
        public JsonResult GetCarModelData(string parkingId)
        {
            JsonResult json = new JsonResult();
            try
            {
              
                List<ParkCarModel> carModels = ParkCarModelServices.QueryByParkingId(parkingId);
                json.Data = carModels;
                return json;
            }
            catch (MyException ex)
            {
                ExceptionsServices.AddExceptions(ex, ex.Message);
                return json;
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "获取车型失败");
                return json;
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
        public JsonResult Passed(string RecordID, string AuditRemark, string CarTypeID, string CarModelID, string AreaIDS,string GateID)
        {
            try
            {
                 bool result = ParkMonthlyCarApplyServices.Passed(RecordID, AuditRemark, CarTypeID, CarModelID, AreaIDS, GateID, GetLoginUser.RecordID);
                if (!result) throw new MyException("审核失败");

                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "审核月租车申请操作失败");
                return Json(MyResult.Error("审核操作失败"));
            }
        }
        [HttpPost]
        [CheckPurview(Roles = "PK01020702")]
        public JsonResult Refused(string RecordID, string AuditRemark)
        {
            try
            {
                bool result = ParkMonthlyCarApplyServices.Refused(RecordID, AuditRemark);
                if (!result) throw new MyException("拒绝失败");

                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "拒绝月租车申请操作失败");
                return Json(MyResult.Error("拒绝操作失败"));
            }
        }
        /// <summary>
        /// 获取审核月租车申请操作权限
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAduitMonthlyCarApplyOperatePurview()
        {
            JsonResult result = new JsonResult();
            List<SystemOperatePurview> options = new List<SystemOperatePurview>();
            List<SysRoleAuthorize> roleAuthorizes = GetLoginUserRoleAuthorize.Where(p => p.ParentID == "PK010207").ToList();

            foreach (var item in roleAuthorizes)
            {
                switch (item.ModuleID)
                {
                    case "PK01020701":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "审核";
                            option.handler = "Update";
                            option.iconCls = "icon-edit";
                            option.id = "btnUpdate";
                            option.sort = 2;
                            options.Add(option);
                            break;
                        }
                    case "PK01020702":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "拒绝";
                            option.iconCls = "icon-remove";
                            option.handler = "Delete";
                            option.id = "btnDelete";
                            option.sort = 3;
                            options.Add(option);
                            break;
                        }
                }
            }
            SystemOperatePurview roption = new SystemOperatePurview();
            roption.text = "刷新";
            roption.handler = "Refresh";
            roption.sort = 6;
            options.Add(roption);

            result.Data = options.OrderBy(p => p.sort);
            return result;
        }
    }
}
