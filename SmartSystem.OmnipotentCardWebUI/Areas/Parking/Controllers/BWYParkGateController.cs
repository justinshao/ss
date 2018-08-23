using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.OmnipotentCardWebUI.Controllers;
using Common.Services;
using Common.Entities.BWY;
using System.Text;
using Common.Utilities.Helpers;
using Common.Entities;
using System.Configuration;
using SmartSystem.WeiXinServices;
using SmartSystem.OmnipotentCardWebUI.Models;
using Common.ExternalInteractions.SFM;

namespace SmartSystem.OmnipotentCardWebUI.Areas.Parking.Controllers
{
    [CheckPurview(Roles = "PKBWY01")]
    public class BWYParkGateController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public string GetBWYParkGateData()
        {
            JsonResult json = new JsonResult();
            string parkName = Request.Params["ParkName"];
            string gateName = Request.Params["GateName"];
            int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
            int rows = string.IsNullOrEmpty(Request.Params["rows"]) ? 0 : int.Parse(Request.Params["rows"]);

            int? dataSource = null;
            int source = 0;
            if (!string.IsNullOrWhiteSpace(Request.Params["DataSource"]) && int.TryParse(Request.Params["DataSource"], out source))
            {
                dataSource = source;
            }
            StringBuilder strData = new StringBuilder();
            try
            {
                int total = 0;
                List<BWYGateMapping> models = BWYGateMappingServices.QueryPage(parkName, gateName, dataSource, page, rows, out total);

                strData.Append("{");
                strData.Append("\"total\":" + total + ",");
                strData.Append("\"rows\":" + JsonHelper.GetJsonString(models) + ",");
                strData.Append("\"index\":" + page);
                strData.Append("}");
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "查询错误日志失败");
            }
            return strData.ToString();
        }
        [HttpPost]
        [CheckPurview(Roles = "PKBWY0101,PKBWY0102")]
        public JsonResult SaveEdit(BWYGateMapping model)
        {
            try
            {
                bool result = false;
                if (string.IsNullOrWhiteSpace(model.RecordID))
                {
                    model.DataSource = 0;
                    result = BWYGateMappingServices.Add(model);
                    if (!result) throw new MyException("添加失败");
                    return Json(MyResult.Success());
                }
                else
                {
                    result = BWYGateMappingServices.Update(model);
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
                ExceptionsServices.AddExceptions(ex, "保存岗亭信息失败");
                return Json(MyResult.Error("保存岗亭信息失败"));
            }
        }
        [HttpPost]
        [CheckPurview(Roles = "PKBWY0101,PKBWY0102")]
        public JsonResult SaveQRCodeParkNo(BWYGateMapping model)
        {
            try
            {
                bool result = BWYGateMappingServices.UpdateParkNo(model.RecordID, model.ParkNo);
                if (!result) throw new MyException("保存失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "保存二维码车场编号失败");
                return Json(MyResult.Error("保存二维码车场编号失败"));
            }
        }
        [HttpPost]
        [CheckPurview(Roles = "PKBWY0101,PKBWY0102")]
        public JsonResult ImportSFMParking()
        {
            try
            {
                SFMParkInfoResult result = SFMInterfaceProcess.GetParkInfo();
                if (result == null || !result.Success) throw new MyException("获取车场信息失败");
                if (result.Data == null || result.Data.TotalRecord == 0
                    || result.Data.ResultList == null || result.Data.ResultList.Count == 0) throw new MyException("无车场信息");

                List<BWYGateMapping> models = new List<BWYGateMapping>();
                foreach (var item in result.Data.ResultList)
                {
                    BWYGateMapping model = new BWYGateMapping();
                    model.ParkingID = item.parkKey;
                    model.ParkingName = item.parkName;
                    model.DataSource = 1;
                    List<ParkSentry> parkSentry = item.parkSentry;
                    if (parkSentry != null && parkSentry.Count > 0)
                    {
                        foreach (var sentry in parkSentry)
                        {
                            model.ParkBoxID = sentry.sentryNo;
                            model.ParkBoxName = sentry.sentryName;
                            List<ParkLane> parkLanes = sentry.parkLane;
                            if (parkLanes != null)
                                foreach (var lane in parkLanes)
                                {
                                    if (lane.vehicleType != "1") continue;

                                    BWYGateMapping gateMap = new BWYGateMapping();

                                    gateMap.ParkingID = model.ParkingID;
                                    gateMap.ParkingName = model.ParkingName;
                                    gateMap.DataSource = model.DataSource;
                                    gateMap.ParkBoxID = model.ParkBoxID;
                                    gateMap.ParkBoxName = model.ParkBoxName;

                                    gateMap.GateID = lane.vehicleNo;
                                    gateMap.GateName = lane.vehicleName;
                                    models.Add(gateMap);
                                }
                        }
                    }
                }
                if (models.Count == 0) throw new MyException("请核实塞菲姆车场数据的完整性");

                bool addResult = BWYGateMappingServices.UpdateSFMParking(models);
                if (!addResult) throw new MyException("导入塞菲姆车场信息失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "导入塞菲姆车场信息失败");
                return Json(MyResult.Error("导入塞菲姆车场信息失败"));
            }
        }
        [HttpPost]
        [CheckPurview(Roles = "PKBWY0103")]
        public JsonResult Delete(string recordId)
        {
            try
            {
                bool result = BWYGateMappingServices.Delete(recordId);
                if (!result) throw new MyException("删除失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "删除信息失败");
                return Json(MyResult.Error("删除信息失败"));
            }
        }

        [CheckPurview(Roles = "PKBWY0104")]
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

                BWYGateMapping gate = BWYGateMappingServices.QueryByRecordId(gateId);
                if (gate == null) throw new MyException("获取车场信息失败");


                if (string.IsNullOrWhiteSpace(SystemDefaultConfig.SystemDomain))
                {
                    throw new MyException("获取域名失败");
                }
                BaseCompany company = null;
                if (gate.DataSource == 0)
                {
                    if (string.IsNullOrWhiteSpace(SystemDefaultConfig.BWPKID))
                    {
                        throw new MyException("获取车场编号失败");
                    }
                    company = CompanyServices.QueryByParkingId(SystemDefaultConfig.BWPKID);
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(SystemDefaultConfig.SFMPKID))
                    {
                        throw new MyException("获取车场编号失败");
                    }
                    if (string.IsNullOrWhiteSpace(gate.ParkNo))
                    {
                        throw new MyException("二维码车场编号不能为空");
                    }
                    company = CompanyServices.QueryByParkingId(SystemDefaultConfig.SFMPKID);
                }
                if (company == null) throw new MyException("获取单位编号失败");

                string content = string.Format("{0}/qrl/scio_ix_pid={1}^io={2}^source=0", SystemDefaultConfig.SystemDomain, SystemDefaultConfig.BWPKID, gate.GateID);
                if (gate.DataSource == 1)
                {
                    content = string.Format("{0}/qrl/scio_ix_pid={1}^io={2}^source=0", SystemDefaultConfig.SystemDomain, SystemDefaultConfig.SFMPKID, gate.ParkNo + "$" + gate.GateID);
                }
                foreach (var item in dics)
                {
                    string parkingName = string.Format("{0}_{1}_{2}", gate.ParkingName, gate.GateName, item);
                    string result = QRCodeServices.GenerateQRCode(company.CPID, content, item, parkingName);
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
                ExceptionsServices.AddExceptions(ex, "下载外部车场二维码失败");
                return Json(MyResult.Error("下载外部车场二维码失败"));
            }
        }

        /// <summary>
        /// 获取卡片操作权限
        /// </summary>
        /// <returns></returns>
        public JsonResult GetParkGateOperatePurview()
        {
            JsonResult result = new JsonResult();
            List<SystemOperatePurview> options = new List<SystemOperatePurview>();
            List<SysRoleAuthorize> roleAuthorizes = GetLoginUserRoleAuthorize.Where(p => p.ParentID == "PKBWY01").ToList();

            foreach (var item in roleAuthorizes)
            {
                switch (item.ModuleID)
                {
                    case "PKBWY0101":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "添加";
                            option.handler = "Add";
                            option.sort = 1;
                            options.Add(option);

                            SystemOperatePurview option9 = new SystemOperatePurview();
                            option9.text = "导入塞菲姆车场";
                            option9.id = "btndownloadqrcode";
                            option9.handler = "DownloadSFMParking";
                            option9.iconCls = "icon-import";
                            option9.sort = 8;
                            options.Add(option9);

                            SystemOperatePurview option10 = new SystemOperatePurview();
                            option10.text = "塞菲姆车场编号";
                            option10.id = "btnupdateparkno";
                            option10.handler = "UpdateParkNo";
                            option10.iconCls = "icon-edit";
                            option10.sort = 10;
                            options.Add(option10);
                            break;
                        }
                    case "PKBWY0102":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "修改";
                            option.handler = "Update";
                            option.sort = 2;
                            options.Add(option);
                            break;
                        }
                    case "PKBWY0103":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "删除";
                            option.handler = "Delete";
                            option.sort = 3;
                            options.Add(option);
                            break;
                        }
                    case "PKBWY0104":
                        {
                            if (!string.IsNullOrWhiteSpace(SystemDefaultConfig.SystemDomain))
                            {
                                SystemOperatePurview option8 = new SystemOperatePurview();
                                option8.text = "下载二维码";
                                option8.id = "btndownloadqrcode";
                                option8.handler = "DownloadQRCode";
                                option8.iconCls = "icon-import";
                                option8.sort = 7;
                                options.Add(option8);

                            }
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
