using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.OmnipotentCardWebUI.Controllers;
using Common.Services;
using Common.Entities;
using SmartSystem.OmnipotentCardWebUI.Models;

namespace SmartSystem.OmnipotentCardWebUI.Areas.Parking.Controllers
{
    [CheckPurview(Roles = "PK010202")]
    public class ParkCarModelController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetCarModelData()
        {
            JsonResult json = new JsonResult();
            try
            {
                if (string.IsNullOrEmpty(Request.Params["parkingId"])) return json;

                string parkingid = Request.Params["parkingid"].ToString();
                List<ParkCarModel> models = ParkCarModelServices.QueryByParkingId(parkingid);
                var result = from p in models select new {
                    CarModelID = p.CarModelID,
                    CarModelName = p.CarModelName,
                    PlateColor = string.IsNullOrWhiteSpace(p.PlateColor) ? "无" : p.PlateColor,
                    IsDefault = p.IsDefault,
                    IsNaturalDay = p.IsNaturalDay,
                    MaxUseMoney = p.MaxUseMoney,
                    DayMaxMoney = p.DayMaxMoney,
                    NightMaxMoney = p.NightMaxMoney,
                    DayStartTime = p.DayStartTime==DateTime.MinValue? "00:00" : p.DayStartTime.ToString("HH:mm"),
                    DayEndTime = p.DayEndTime == DateTime.MinValue ? "00:00" : p.DayEndTime.ToString("HH:mm"),
                    NightStartTime = p.NightStartTime == DateTime.MinValue ? "00:00" : p.NightStartTime.ToString("HH:mm"),
                    NightEndTime = p.NightEndTime == DateTime.MinValue ? "00:00" : p.NightEndTime.ToString("HH:mm"),
                    DayDes = GetDes(p.DayMaxMoney,p.DayStartTime,p.DayEndTime),
                    NightDes = GetDes(p.NightMaxMoney, p.NightStartTime, p.NightEndTime)
                };
                json.Data = result;
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "车型管理查询车类型集合失败");
            }
            return json;
        }
        private string GetDes(decimal amount,DateTime start,DateTime end) {
            string strStart = "00:00";
            if (start != DateTime.MinValue) {
                strStart = start.ToString("HH:mm");
            }
            string strEnd = "00:00";
            if (end != DateTime.MinValue)
            {
                strEnd = end.ToString("HH:mm");
            }
            string amountDes = "不限金额";
            if (amount > 0) {
                amountDes = string.Format("最大金额{0}",amount);
            }
            return string.Format("{0}至{1}/{2}", strStart, strEnd, amountDes);
        }
        [HttpPost]
        [CheckPurview(Roles = "PK01020201,PK01020202")]
        public JsonResult SaveEdit(ParkCarModel model)
        {
            try
            {
                bool result = false;
                if (string.IsNullOrWhiteSpace(model.CarModelID))//增加
                {
                    result = ParkCarModelServices.Add(model);
                    if (!result) throw new MyException("添加失败");
                    return Json(MyResult.Success());
                }
                else //修改
                {
                    result = ParkCarModelServices.Update(model);
                    if (!result) throw new MyException("修改失败");
                    return Json(MyResult.Success());
                }
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "保存车型信息失败");
                return Json(MyResult.Error("保存车型信息失败"));
            }
        }

        /// <summary>
        /// 获取车辆类型操作权限
        /// </summary>
        /// <returns></returns>
        public JsonResult GetParkCarModelOperatePurview()
        {
            JsonResult result = new JsonResult();
            List<SystemOperatePurview> options = new List<SystemOperatePurview>();
            List<SysRoleAuthorize> roleAuthorizes = GetLoginUserRoleAuthorize.Where(p => p.ParentID == "PK010202").ToList();

            foreach (var item in roleAuthorizes)
            {
                switch (item.ModuleID)
                {
                    case "PK01020201":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "添加";
                            option.handler = "Add";
                            option.sort = 1;
                            options.Add(option);
                            break;
                        }
                    case "PK01020202":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "修改";
                            option.handler = "Update";
                            option.sort = 2;
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
