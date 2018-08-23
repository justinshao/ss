using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.OmnipotentCardWebUI.Controllers;
using Common.Services;
using Common.Entities;
using Common.Services.Park;
using Common.Entities.Parking;
using SmartSystem.OmnipotentCardWebUI.Areas.Parking.Models;
using SmartSystem.OmnipotentCardWebUI.Models;
using Common.Utilities;
using Common.Utilities.Helpers;

namespace SmartSystem.OmnipotentCardWebUI.Areas.Parking.Controllers
{
    /// <summary>
    /// 收费规则
    /// </summary>
     [CheckPurview(Roles = "PK010203")]
    public class ParkFeeRuleController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetParkFeeRuleData()
        {
            JsonResult json = new JsonResult();
            try
            {
                if (string.IsNullOrEmpty(Request.Params["parkingId"])) return json;

                string parkingId = Request.Params["parkingId"].ToString();

                List<ParkCarType> carTypes = ParkCarTypeServices.QueryParkCarTypeByParkingId(parkingId);
                List<ParkCarModel> carModels = ParkCarModelServices.QueryByParkingId(parkingId);
                List<ParkFeeRule> rules = ParkFeeRuleServices.QueryParkFeeRuleByParkingId(parkingId);
                List<ParkArea> areas = ParkAreaServices.GetParkAreaByParkingId(parkingId);
                List<ParkFeeRuleView> models = new List<ParkFeeRuleView>();
                foreach (var item in rules)
                {
                    ParkFeeRuleView view = new ParkFeeRuleView().ToParkFeeRuleView(item, areas, carTypes, carModels);
                    view.ParkingID = parkingId;
                    models.Add(view);
                }
                json.Data = models;
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "获取收费规则集合失败");
            }
            return json;
        }
        public JsonResult GetParkCarType()
        {
            JsonResult json = new JsonResult();
            if (string.IsNullOrEmpty(Request.Params["parkingId"])) return json;

            string parkingId = Request.Params["parkingid"].ToString();
            json.Data = ParkCarTypeServices.QueryParkCarTypeByParkingId(parkingId)
                        .Where(p => p.BaseTypeID == BaseCarType.StoredValueCar || p.BaseTypeID == BaseCarType.TempCar);

            return json;
        }
        public JsonResult GetParkArea()
        {
            JsonResult json = new JsonResult();
            if (string.IsNullOrEmpty(Request.Params["parkingId"])) return json;
            string parkingId = Request.Params["parkingId"].ToString();
            json.Data = ParkAreaServices.GetParkAreaByParkingId(parkingId);
            return json;
        }
        public JsonResult GetParkCarModel()
        {
            JsonResult json = new JsonResult();
            if (string.IsNullOrEmpty(Request.Params["parkingId"])) return json;
            string parkingId = Request.Params["parkingId"].ToString();

            json.Data = ParkCarModelServices.QueryByParkingId(parkingId);
            return json;
        }
        public JsonResult GetFeeType()
        {
            JsonResult json = new JsonResult();
            try
            {
                json.Data = EnumHelper.GetEnumContextList(typeof(FeeType));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "收费模式转换对象集合失败");
            }
            return json;
        }
        public JsonResult GetLoopType()
        {
            JsonResult json = new JsonResult();
            try
            {
                json.Data = EnumHelper.GetEnumContextList(typeof(LoopType));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "循环包含字段转换对象集合失败");
            }
            return json;
        }
        public JsonResult GetFeeRuleType()
        {
            JsonResult json = new JsonResult();
            try
            {
                json.Data = EnumHelper.GetEnumContextList(typeof(FeeRuleType));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "计费类型转换对象集合失败");
            }
            return json;
        }
         [HttpPost]
         [ValidateInput(false)]
         [CheckPurview(Roles = "PK01020301,PK01020302")]
        public JsonResult SaveEdit(ParkFeeRuleView model)
        {
            try
            {
                ParkFeeRule ruleModel = model.ToParkFeeRule();
                bool result=false;
                if (string.IsNullOrWhiteSpace(model.FeeRuleID))
                {
                    var parkingFeeRules = ParkFeeRuleServices.QueryFeeRules(model.ParkingID, model.CarTypeID, model.CarModelID);
                    if (parkingFeeRules != null && parkingFeeRules.Count > 0 && parkingFeeRules.Exists(p => p.AreaID == model.AreaID))
                    {
                        throw new MyException("该区域已经存在相同的收费规则了");
                    }
                     result = ParkFeeRuleServices.Add(ruleModel);
                     if (!result) throw new MyException("添加收费规则失败");
                }
                else
                {
                    var parkingFeeRules = ParkFeeRuleServices.QueryFeeRules(model.ParkingID, model.CarTypeID, model.CarModelID);
                    if (parkingFeeRules != null && parkingFeeRules.Count > 0
                        && parkingFeeRules.Exists(p => p.AreaID == model.AreaID && p.FeeRuleID != model.FeeRuleID))
                    {
                        throw new MyException("该区域已经存在相同的收费规则了!");
                    }

                     result = ParkFeeRuleServices.Update(ruleModel);
                    if (!result) throw new MyException("修改收费规则失败");
                }
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "收费规则管理设置收费规则失败");
                return Json(MyResult.Error("设置收费规则失败"));
            }

        }
         [HttpPost]
         [CheckPurview(Roles = "PK01020303")]
        public JsonResult Delete(string feeRuleId)
        {
            try
            {
                bool result = ParkFeeRuleServices.Delete(feeRuleId);
                if (!result) throw new MyException("删除收费规则失败");
                return Json(MyResult.Success());
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "删除收费规则失败");
                return Json(MyResult.Error("删除收费规则失败"));
            }
        }
        [HttpPost]
        public JsonResult TestCalculateFee(DateTime StartTime, DateTime EndTime, string FeeRuleId)
        {
            try
            {
                decimal fee = ParkFeeRuleServices.TestCalculateFee(StartTime, EndTime, FeeRuleId);
                return Json(MyResult.Success("成功", fee));
            }
            catch (MyException ex) {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "测试算费失败");
                return Json(MyResult.Error("算费失败"));
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult PostFeeRuleFile()
        {
            string info = string.Empty;
            try
            {
                HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
                if (files.Count > 0)
                {
                    int fileLen = files[0].ContentLength;//获取上传文件的大小
                    byte[] input = new byte[fileLen];
                    System.IO.Stream UpLoadStream = files[0].InputStream;
                    UpLoadStream.Read(input, 0, fileLen);
                    UpLoadStream.Position = 0;
                    System.IO.StreamReader sr = new System.IO.StreamReader(UpLoadStream, System.Text.Encoding.UTF8);
                    string content = sr.ReadToEnd();
                    sr.Close();
                    sr.Dispose();
                    UpLoadStream.Close();
                    UpLoadStream.Dispose();
                    return Json(MyResult.Success("",content));
                }
                else
                {
                    return Json(MyResult.Error("获取上传文件失败"));
                }
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "上传收费规则文件失败");
                return Json(MyResult.Error("上传收费规则文件失败"));
            }
        }
        /// <summary>
        /// 获取车辆类型操作权限
        /// </summary>
        /// <returns></returns>
        public JsonResult GetParkFeeRuleOperatePurview()
        {
            JsonResult result = new JsonResult();
            List<SystemOperatePurview> options = new List<SystemOperatePurview>();
            List<SysRoleAuthorize> roleAuthorizes = GetLoginUserRoleAuthorize.Where(p => p.ParentID == "PK010203").ToList();

            foreach (var item in roleAuthorizes)
            {
                switch (item.ModuleID)
                {
                    case "PK01020301":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "添加";
                            option.handler = "Add";
                            option.sort = 1;
                            options.Add(option);
                            break;
                        }
                    case "PK01020302":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "修改";
                            option.handler = "Update";
                            option.sort = 2;
                            options.Add(option);
                            break;
                        }
                    case "PK01020303":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "删除";
                            option.handler = "Delete";
                            option.sort = 3;
                            options.Add(option);
                            break;
                        }
                    case "PK01020304":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.id = "btntest";
                            option.iconCls = "icon-calculator";
                            option.text = "收费测试";
                            option.handler = "TestFeeRule";
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
