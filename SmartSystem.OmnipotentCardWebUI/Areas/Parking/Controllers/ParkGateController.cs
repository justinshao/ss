using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.OmnipotentCardWebUI.Controllers;
using Common.Services;
using Common.Entities;
using Common.Entities.Parking;
using Common.Utilities;
using SmartSystem.OmnipotentCardWebUI.Models;
using Common.Utilities.Helpers;
using Common.Entities.WX;
using SmartSystem.WeiXinServices;
using Common.Services.WeiXin;

namespace SmartSystem.OmnipotentCardWebUI.Areas.Parking.Controllers
{
     [CheckPurview(Roles = "PK010106")]
    public class ParkGateController : BaseController
    {
        public ActionResult Index()
        {
            
            return View();
        }
        public JsonResult GetParkGateData()
        {
            JsonResult json = new JsonResult();
            try
            {
                if (string.IsNullOrEmpty(Request.Params["boxId"])) return json;
                json.Data = ParkGateServices.QueryByParkBoxRecordId(Request.Params["boxId"].ToString());
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "获取通道数据失败");
            }
            return json;
        }
         [HttpPost]
         [CheckPurview(Roles = "PK01010601,PK01010602")]
        public JsonResult SaveEdit(ParkGate model)
        {
            try
            {
                bool result = false;
                if (string.IsNullOrWhiteSpace(model.GateID))
                {
                    result = ParkGateServices.Add(model);
                    if (!result) throw new MyException("添加失败");
                    return Json(MyResult.Success());
                }
                else
                {
                    result = ParkGateServices.Update(model);
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
                ExceptionsServices.AddExceptions(ex, "保存通道信息失败");
                return Json(MyResult.Error("保存通道信息失败"));
            }
        }
         [HttpPost]
         [CheckPurview(Roles = "PK01010603")]
        public JsonResult Delete(string recordId)
        {
            try
            {
                bool result = ParkGateServices.Delete(recordId);
                if (!result) throw new MyException("删除失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "通道管理删除通道信息失败");
               return Json(MyResult.Error("删除通道信息失败"));
            }

        }
        public JsonResult GetParkGateDevicesData()
        {
            JsonResult json = new JsonResult();
            try
            {
                if (string.IsNullOrEmpty(Request.Params["gateId"])) {
                    json.Data = new List<ParkDevice>();
                    return json;
                }
                List<ParkDevice> devices = ParkDeviceServices.QueryParkDeviceByGateRecordId(Request.Params["gateId"].ToString());
                json.Data = from p in devices
                             select new
                             {
                                 DeviceID = p.DeviceID,
                                 GateID = p.GateID,
                                 DeviceType = p.DeviceType,
                                 DeviceTypeDes = p.DeviceType.GetDescription(),
                                 PortType = p.PortType,
                                 PortTypeDes = p.PortType.GetDescription(),
                                 Baudrate = p.Baudrate,
                                 SerialPort = p.SerialPort,
                                 IpAddr = p.IpAddr,
                                 IpPort = p.IpPort,
                                 UserName = p.UserName,
                                 UserPwd = p.UserPwd,
                                 NetID = p.NetID,
                                 LedNum = p.LedNum,
                                 DeviceNo = p.DeviceNo,
                                 OfflinePort = p.OfflinePort,
                                 IsCapture = p.IsCapture,
                                 IsSVoice = p.IsSVoice,
                                 IsCarBit=p.IsCarBit,
                                 IsContestDev = p.IsContestDev,
                                 DeviceTypeBK = p.ControllerType,
                                 DisplayMode = p.DisplayMode,
                                 IsMonitor=p.IsMonitor
                             };
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "获取设备数据失败");
            }
            return json;
        }
        public JsonResult GetParkGateDeviceType()
        {
            JsonResult json = new JsonResult();
            try
            {
                json.Data = EnumHelper.GetEnumContextList(typeof(DeviceType));
            }
            catch (Exception ex) {
                ExceptionsServices.AddExceptions(ex, "设备类型枚举转换对象集合失败");
            }
            return json;
        }
        public JsonResult GetParkDeviceTypeBK() {
            JsonResult json = new JsonResult();
            try
            {
                json.Data = EnumHelper.GetEnumContextList(typeof(DeviceTypeBK));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "卡板类型枚举转换对象集合失败");
            }
            return json;
        }
        public JsonResult GetParkGateDevicePortType()
        {
            JsonResult json = new JsonResult();
            try
            {
                json.Data = EnumHelper.GetEnumContextList(typeof(PortType));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "通讯类型枚举转换对象集合失败");
            }
            return json;
        }
        [HttpPost]
        [CheckPurview(Roles = "PK01010701,PK01010702")]
        public JsonResult SaveEditDevice(ParkDevice model)
        {
            try
            {
                bool result = false;
                if (string.IsNullOrWhiteSpace(model.DeviceID))
                {
                    result = ParkDeviceServices.Add(model);
                    if (!result) throw new MyException("添加失败");
                    return Json(MyResult.Success());
                }
                else
                {
                    result = ParkDeviceServices.Update(model);
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
                ExceptionsServices.AddExceptions(ex, "保存设备信息失败");
                return Json(MyResult.Error("保存设备信息失败"));
            }

        }
         [HttpPost]
        public JsonResult GetParkDeviceParam(string deviceId)
        {
            try
            {
                ParkDevice device = ParkDeviceServices.QueryParkDeviceByRecordId(deviceId);
                if (device == null) throw new MyException("获取设备失败");

                if (device.DeviceType != DeviceType.NZ_CONTROL) throw new MyException("设备类型不正确");

                ParkDeviceParam deviceParam = ParkDeviceServices.QueryParkDeviceParamByDID(deviceId);
                return Json(MyResult.Success("获取成功",deviceParam));
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "删除设备信息失败");
                return Json(MyResult.Error("删除设备信息失败"));
            }
        }
         [HttpPost]
         public JsonResult SaveParkDeviceParam(ParkDeviceParam model)
         {
             try
             {
                 bool result = ParkDeviceServices.UpdateParam(model);
                 if (!result) throw new MyException("保存设备参数失败!");
                 return Json(MyResult.Success());
             }
             catch (MyException ex)
             {
                 return Json(MyResult.Error(ex.Message));
             }
             catch (Exception ex)
             {
                 ExceptionsServices.AddExceptions(ex, "保存设备参数失败");
                 return Json(MyResult.Error("保存设备参数失败"));
             }
         }
        [HttpPost]
        [CheckPurview(Roles = "PK01010703")]
        public JsonResult DeleteDevice(string recordId)
        {
            try
            {
                bool result = ParkDeviceServices.Delete(recordId);
                if (!result) throw new MyException("删除失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "删除设备信息失败");
                return Json(MyResult.Error("删除设备信息失败"));
            }
        }
        /// <summary>
        /// 获取通道操作权限
        /// </summary>
        /// <returns></returns>
        public JsonResult GetParkGateOperatePurview()
        {
            JsonResult result = new JsonResult();
            List<SystemOperatePurview> options = new List<SystemOperatePurview>();
            List<SysRoleAuthorize> roleAuthorizes = GetLoginUserRoleAuthorize.Where(p => p.ParentID == "PK010106").ToList();

            foreach (var item in roleAuthorizes)
            {
                switch (item.ModuleID)
                {
                    case "PK01010601":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "添加";
                            option.handler = "Add";
                            option.sort = 1;
                            options.Add(option);
                            break;
                        }
                    case "PK01010602":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "修改";
                            option.handler = "Update";
                            option.sort = 2;
                            options.Add(option);
                            break;
                        }
                    case "PK01010603":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "删除";
                            option.handler = "Delete";
                            option.sort = 4;
                            options.Add(option);
                            break;
                        }
                    case "PK01010604":
                        {
                            SystemOperatePurview option8 = new SystemOperatePurview();
                            option8.text = "下载二维码";
                            option8.id = "btndownloadqrcode";
                            option8.handler = "DownloadQRCode";
                            option8.iconCls = "icon-import";
                            option8.sort = 8;
                            options.Add(option8);
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
        public JsonResult DownloadQRCode(string gateId, int size)
        {
            try
            {
                List<int> dics = new List<int>();
                dics.Add(258);
                dics.Add(344);
                dics.Add(430);
                dics.Add(860);
                dics.Add(1280);

                List<string> imgs = new List<string>();
                ParkGate gate = ParkGateServices.QueryByRecordId(gateId);
                if (gate == null) throw new MyException("获取通道信息失败");
                BaseCompany company = CompanyServices.QueryByBoxID(gate.BoxID);
                if (company == null) throw new MyException("获取单位信息失败");

               ParkBox box = ParkBoxServices.QueryByRecordId(gate.BoxID);
               if (box == null) throw new MyException("获取岗亭信息失败");

               ParkArea area = ParkAreaServices.QueryByRecordId(box.AreaID);
               if (area == null) throw new MyException("获取区域信息失败");

               BaseParkinfo parking = ParkingServices.QueryParkingByParkingID(area.PKID);
                if (parking == null) throw new MyException("获取车场信息失败");
                string content = string.Format("{0}/qrl/scio_ix_pid={1}^io={2}", SystemDefaultConfig.SystemDomain, parking.PKID, gate.GateID);
                foreach (var item in dics)
                {
                    string parkingName = string.Format("{0}_{1}_{2}_{3}_{4}", parking.PKName, area.AreaName,box.BoxName,gate.GateName, item);
                    string result = QRCodeServices.GenerateQRCode(company.CPID,content, item, parkingName);
                    imgs.Add(item.ToString() + "|" + result);
                }

                return Json(MyResult.Success("", imgs));
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "下载二维码失败");
                return Json(MyResult.Error("下载二维码失败"));
            }
        }
        /// <summary>
        /// 获取设备操作权限
        /// </summary>
        /// <returns></returns>
        public JsonResult GetParkDevicesOperatePurview()
        {
            JsonResult result = new JsonResult();
            List<SystemOperatePurview> options = new List<SystemOperatePurview>();
            List<SysRoleAuthorize> roleAuthorizes = GetLoginUserRoleAuthorize.Where(p => p.ParentID == "PK010107").ToList();

            foreach (var item in roleAuthorizes)
            {
                switch (item.ModuleID)
                {
                    case "PK01010701":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "添加";
                            option.handler = "AddDevice";
                            option.sort = 1;
                            options.Add(option);
                            break;
                        }
                    case "PK01010702":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "修改";
                            option.handler = "UpdateDevice";
                            option.sort = 2;
                            options.Add(option);
                            break;
                        }
                    case "PK01010704":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "修改设备参数";
                            option.iconCls = "icon-edit";
                            option.handler = "UpdateDeviceParam";
                            option.id = "btnupdatedeviceparam";
                            option.sort = 3;
                            options.Add(option);
                            break;
                        }
                    case "PK01010703":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "删除";
                            option.handler = "DeleteDevice";
                            option.sort = 3;
                            options.Add(option);
                            break;
                        }
                }
            }
            SystemOperatePurview roption = new SystemOperatePurview();
            roption.text = "刷新";
            roption.handler = "RefreshDevice";
            roption.sort = 6;
            options.Add(roption);

            result.Data = options.OrderBy(p => p.sort);
            return result;
        }
    }
}
