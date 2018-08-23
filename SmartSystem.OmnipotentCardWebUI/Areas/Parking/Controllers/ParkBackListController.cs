using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.OmnipotentCardWebUI.Controllers;
using System.Text;
using Common.Entities.Parking;
using Common.Services.Park;
using Common.Utilities;
using Common.Services;
using Common.Entities;
using SmartSystem.OmnipotentCardWebUI.Models;
using Common.Utilities.Helpers;
using Common.Entities.Enum;

namespace SmartSystem.OmnipotentCardWebUI.Areas.Parking.Controllers
{
    [CheckPurview(Roles = "PK010205")]
    public class ParkBackListController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public string GetBlackListData()
        {
            StringBuilder strData = new StringBuilder();
            try
            {
                if (string.IsNullOrEmpty(Request.Params["parkingId"])) return string.Empty;
                string parkingId = Request.Params["parkingId"].ToString();

                string plateNo = Request.Params["queryPlateNo"];

                int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int rows = string.IsNullOrEmpty(Request.Params["rows"]) ? 0 : int.Parse(Request.Params["rows"]);

                int total = 0;
                List<ParkBlacklist> blacklist = ParkBlacklistServices.QueryPage(parkingId, plateNo, rows, page, out total);
                var result = from p in blacklist
                             select new
                             {
                                 ID = p.ID,
                                 RecordID = p.RecordID,
                                 PKID = p.PKID,
                                 PlateNumber = p.PlateNumber,
                                 Remark = p.Remark,
                                 LastUpdateTime = p.LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                 Status = (int)p.Status,
                                 StatusDes = p.Status.GetDescription()
                             };

                strData.Append("{");
                strData.Append("\"total\":" + total + ",");
                strData.Append("\"rows\":" + JsonHelper.GetJsonString(result) + ",");
                strData.Append("\"index\":" + page);
                strData.Append("}");
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "查询黑名单信息失败");
            }

            return strData.ToString();
        }
        public JsonResult GetBlackListStatusData()
        {
            JsonResult json = new JsonResult();
            json.Data = EnumHelper.GetEnumContextList(typeof(BlackListStatus));
            return json;
        }
        [HttpPost]
        [CheckPurview(Roles = "PK01020501,PK01020502")]
        public JsonResult SaveBlackList(ParkBlacklist model)
        {

            try
            {
                model.PlateNumber = model.PlateNumber.ToPlateNo();
                if (string.IsNullOrWhiteSpace(model.RecordID))
                {
                    bool result = ParkBlacklistServices.Add(model);
                    if (!result) throw new MyException("保存失败");
                }
                else
                {
                    bool result = ParkBlacklistServices.Update(model);
                    if (!result) throw new MyException("保存失败");
                }
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "保存黑名单信息失败");
                return Json(MyResult.Error("保存失败"));
            }
        }
        [HttpPost]
        [CheckPurview(Roles = "PK01020503")]
        public JsonResult Delete(string recordId)
        {
            try
            {
                bool result = ParkBlacklistServices.Delete(recordId);
                if (!result) throw new MyException("删除失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "删除黑名单信息失败");
                return Json(MyResult.Error("删除失败"));
            }
        }
        /// <summary>
        /// 获取黑名单操作权限
        /// </summary>
        /// <returns></returns>
        public JsonResult GetBlackListOperatePurview()
        {
            JsonResult result = new JsonResult();
            List<SystemOperatePurview> options = new List<SystemOperatePurview>();
            List<SysRoleAuthorize> roleAuthorizes = GetLoginUserRoleAuthorize.Where(p => p.ParentID == "PK010205").ToList();

            foreach (var item in roleAuthorizes)
            {
                switch (item.ModuleID)
                {
                    case "PK01020501":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "添加";
                            option.handler = "Add";
                            option.sort = 1;
                            options.Add(option);
                            break;
                        }
                    case "PK01020502":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "修改";
                            option.handler = "Update";
                            option.sort = 2;
                            options.Add(option);
                            break;
                        }
                    case "PK01020503":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "删除";
                            option.handler = "Delete";
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
