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
    [CheckPurview(Roles = "PK010104")]
    public class ParkAreaController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 根据车场信息获取区域信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetParkAreaData(string parkingId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(parkingId)) return "{\"rows\":[]}";

                List<ParkArea> parkAreas = ParkAreaServices.GetParkAreaByParkingId(parkingId);

                string json = "{\"rows\":[";
                var topAreas = parkAreas.Where(p => string.IsNullOrWhiteSpace(p.MasterID));
                foreach (var dr in topAreas)
                {
                    json += "{\"id\":" + dr.ID + ",";
                    json += "\"AreaID\":\"" + dr.AreaID + "\"";
                    json += ",\"AreaName\":\"" + dr.AreaName + "\"";
                    json += ",\"PKID\":\"" + dr.PKID + "\"";
                    json += ",\"CarbitNum\":" + dr.CarbitNum + "";
                    json += ",\"MasterID\":\"" + dr.MasterID + "\"";
                    json += ",\"Remark\":\"" + dr.Remark + "\"";
                    json += ",\"NeedToll\":\"" + (int)dr.NeedToll + "\"";
                    json += ",\"TwoCameraWait\":\"" + (int)dr.TwoCameraWait + "\"";
                    json += ",\"CameraWaitTime\":\"" + dr.CameraWaitTime + "\"";
                    json += ",\"iconCls\":\"my-pkarea-icon\"},";
                    var childs = parkAreas.Where(p=>p.MasterID==dr.AreaID);
                    foreach (var obj in childs)
                    {
                        json += "{\"id\":" + obj.ID + ",";
                        json += "\"AreaID\":\"" + obj.AreaID + "\"";
                        json += ",\"AreaName\":\"" + obj.AreaName + "\"";
                        json += ",\"PKID\":\"" + obj.PKID + "\"";
                        json += ",\"ParkingID\":\"" + obj.PKID + "\"";
                        json += ",\"CarbitNum\":" + obj.CarbitNum + "";
                        json += ",\"MasterID\":\"" + obj.MasterID + "\"";
                        json += ",\"Remark\":\"" + obj.Remark + "\"";
                        json += ",\"NeedToll\":\"" + (int)obj.NeedToll + "\"";
                        json += ",\"TwoCameraWait\":\"" + (int)obj.TwoCameraWait + "\"";
                        json += ",\"CameraWaitTime\":\"" + obj.CameraWaitTime + "\"";
                        json += ",\"_parentId\":\"" + dr.AreaID + "\"";
                        json += ",\"iconCls\":\"my-pkarea-icon\"},";
                    }
                }
                if (topAreas.Count() > 0)
                {
                    json = json.Substring(0, json.Length - 1);
                }
                json += "]}";
                return json;
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "根据车场编号获取车场区域信息失败");
                return "{\"rows\":[]}";
            }
        }

        /// <summary>
        /// 根据车场ID获取区域树
        /// </summary>
        /// <returns></returns>
        public string GetAreaTreeData()
        {
            string parkingId = Request.Params["parkingId"].ToString();
            string excludeAreaID = string.Empty;
            if (!string.IsNullOrWhiteSpace(Request.Params["excludeAreaID"])) {
                excludeAreaID = Request.Params["excludeAreaID"].ToString();
            }
            List<ParkArea> models = ParkAreaServices.GetParkAreaByParkingId(parkingId);

            List<ParkArea> parkAreas = new List<ParkArea>();
            if (!string.IsNullOrWhiteSpace(excludeAreaID))
            {
                //是修改，并且存在下属区域
                List<ParkArea> childParkAreas = models.Where(p => p.MasterID == excludeAreaID).ToList();
                if (childParkAreas.Count == 0)
                {
                    parkAreas = models.Where(p => string.IsNullOrWhiteSpace(p.MasterID) && p.AreaID != excludeAreaID).ToList();
                }
            }
            else {
                parkAreas = models.Where(p => string.IsNullOrWhiteSpace(p.MasterID)).ToList();
            }
          
            parkAreas.Insert(0, new ParkArea{AreaID=string.Empty,AreaName="顶级"});

            string json = "[";
            foreach (var dr in parkAreas)
            {
                json += "{\"id\":\"" + dr.AreaID + "\",";
                json += "\"attributes\":{\"type\":0},";
                json += "\"text\":\"" + dr.AreaName + "\"";
                json += "},";
            }
            if (parkAreas.Count() > 0)
            {
                json = json.Substring(0, json.Length - 1);
            }
            json += "]";
            return json;
        }

        /// <summary>
        /// 删除区域信息
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckPurview(Roles = "PK01010403")]
        public JsonResult Delete(string areaId)
        {
            try
            {
                bool result = ParkAreaServices.Delete(areaId);
                if (!result) throw new MyException("删除区域失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "删除区域信息失败");
                return Json(MyResult.Error("删除失败"));
            }
        }

        /// <summary>
        /// 保存区域信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckPurview(Roles = "PK01010401,PK01010402")]
        public JsonResult Edit(ParkArea model)
        {
            try
            {
                bool result = false;
                if (string.IsNullOrWhiteSpace(model.AreaID))
                {
                    result  = ParkAreaServices.Add(model);
                }
                else
                {
                    result = ParkAreaServices.Update(model);
                }
                if (!result) throw new MyException("保存区域失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "保存区域信息失败");
                return Json(MyResult.Error("保存失败"));
            }
        }

        /// <summary>
        /// 获取车场区域操作权限
        /// </summary>
        /// <returns></returns>
        public JsonResult GetParkKAreaOperatePurview()
        {
            JsonResult result = new JsonResult();
            List<SystemOperatePurview> options = new List<SystemOperatePurview>();
            List<SysRoleAuthorize> roleAuthorizes = GetLoginUserRoleAuthorize.Where(p => p.ParentID == "PK010104").ToList();

            foreach (var item in roleAuthorizes)
            {
                switch (item.ModuleID)
                {
                    case "PK01010401":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "添加";
                            option.handler = "Add";
                            option.sort = 1;
                            options.Add(option);
                            break;
                        }
                    case "PK01010402":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "修改";
                            option.handler = "Update";
                            option.sort = 2;
                            options.Add(option);
                            break;
                        }
                    case "PK01010403":
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
