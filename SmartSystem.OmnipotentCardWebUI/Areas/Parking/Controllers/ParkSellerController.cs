using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.OmnipotentCardWebUI.Controllers;
using System.Text;
using Common.Entities;
using Common.Services.Park;
using Common.Utilities;
using Common.Services;
using Common.Core;
using SmartSystem.OmnipotentCardWebUI.Models;
using Common.Utilities.Helpers;

namespace SmartSystem.OmnipotentCardWebUI.Areas.Parking.Controllers
{
     [CheckPurview(Roles = "PK010301")]
    public class ParkSellerController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public string GetParkSellerData()
        {
            StringBuilder strData = new StringBuilder();
            try
            {
                if (string.IsNullOrWhiteSpace(Request["villageId"])) return string.Empty;

                string villageId = Request["villageId"].ToString();
                string sellerName = string.Empty;
                if (!string.IsNullOrWhiteSpace(Request["sellerName"]))
                {
                    sellerName = Request["sellerName"].Trim();
                }
                int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int rows = string.IsNullOrEmpty(Request.Params["rows"]) ? 0 : int.Parse(Request.Params["rows"]);
                int total = 0;
                List<ParkSeller> models = ParkSellerServices.QueryPage(villageId, sellerName, rows, page, out total);

                strData.Append("{");
                strData.Append("\"total\":" + total + ",");
                strData.Append("\"rows\":" + JsonHelper.GetJsonString(models) + ",");
                strData.Append("\"index\":" + page);
                strData.Append("}");
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "获取商家信息失败");
            }

            return strData.ToString();
        }
        [HttpPost]
        [CheckPurview(Roles = "PK01030101,PK01030102")]
         public JsonResult AddOrUpdate(ParkSeller model)
        {
            try
            {
                string errorMsg = string.Empty;
                if (string.IsNullOrWhiteSpace(model.SellerID))
                {
                    model.PWD = MD5.Encrypt(model.PWD);
                    bool result = ParkSellerServices.Add(model);
                    if (!result) throw new MyException("添加商家失败");
                }
                else
                {
                    ParkSeller dbSeller = ParkSellerServices.QueryBySellerId(model.SellerID);
                    if (dbSeller == null) throw new MyException("修改的商家不存在");
                    model.PWD = dbSeller.PWD;
                    model.Balance = dbSeller.Balance;
                    bool result = ParkSellerServices.ModifySeller(model, out errorMsg);
                    if (!result) throw new MyException("修改商家失败");
                }
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "保存商家信息失败");
                return Json(MyResult.Error("保存失败"));
            }
        }
        [HttpPost]
        [CheckPurview(Roles = "PK01030105")]
        public JsonResult ResetPassword(string sellerId)
        {
            try
            {
                string password = "123456";
                bool result = ParkSellerServices.UpdatePassword(sellerId, MD5.Encrypt(password));
                if (!result) throw new MyException("重置密码失败");
                return Json(MyResult.Success(string.Format("重置密码成功，密码为：{0}", password)));
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "重置密码失败");
                return Json(MyResult.Error("重置密码失败"));
            }
        }
        [HttpPost]
        [CheckPurview(Roles = "PK01030103")]
        public JsonResult Delete(string sellerId)
        {
            try
            {
                bool result = ParkSellerServices.Delete(sellerId);
                if (!result) throw new MyException("删除失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "删除商家失败");
                return Json(MyResult.Error("删除商家失败"));
            }
        }
        [HttpPost]
        [CheckPurview(Roles = "PK01030104")]
        public JsonResult SellerCharge(decimal chargeBalance, string SellerID)
        {
            try
            {
                if (chargeBalance <= 0) throw new MyException("充值金额不正确");
                bool result = ParkSellerServices.SellerCharge(SellerID, chargeBalance, GetLoginUser.RecordID);
                if (!result) throw new MyException("充值失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "商家充值失败");
                return Json(MyResult.Error("充值失败"));
            }

        }
        public JsonResult GetSellerOperatePurview()
        {
            JsonResult result = new JsonResult();
            List<SystemOperatePurview> options = new List<SystemOperatePurview>();
            List<SysRoleAuthorize> roleAuthorizes = GetLoginUserRoleAuthorize.Where(p => p.ParentID == "PK010301").ToList();

            foreach (var item in roleAuthorizes)
            {
                switch (item.ModuleID)
                {
                    case "PK01030101":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "添加";
                            option.handler = "Add";
                            option.sort = 1;
                            options.Add(option);
                            break;
                        }
                    case "PK01030102":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "修改";
                            option.handler = "Update";
                            option.sort = 2;
                            options.Add(option);
                            break;
                        }
                    case "PK01030103":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "删除";
                            option.handler = "Delete";
                            option.sort = 5;
                            options.Add(option);
                            break;
                        }
                    case "PK01030104":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.id = "btncharge";
                            option.iconCls = "icon-charge";
                            option.text = "充值";
                            option.handler = "Charge";
                            option.sort = 3;
                            options.Add(option);
                            break;
                        }
                    case "PK01030105":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.id = "btnresetpassword";
                            option.iconCls = "icon-resetpassword";
                            option.text = "重置密码";
                            option.handler = "ResetPassword";
                            option.sort = 4;
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
