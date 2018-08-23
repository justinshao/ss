using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.OmnipotentCardWebUI.Controllers;
using System.Text;
using Common.Services;
using Common.Entities;
using SmartSystem.OmnipotentCardWebUI.Models;
using Common.Services.WeiXin;
using Common.Entities.WX;
using SmartSystem.WeiXinServices;

namespace SmartSystem.OmnipotentCardWebUI.Areas.Parking.Controllers
{
    [CheckPurview(Roles = "PK010105")]
    public class ParkBoxController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
      
        public JsonResult GetParkBoxData()
        {
            JsonResult json = new JsonResult();
            if (string.IsNullOrEmpty(Request.Params["areaId"])) return json;
            List<ParkBox> parkBoxs = ParkBoxServices.QueryByParkAreaId(Request.Params["areaId"].ToString());

            var parkBoxDatas = from p in parkBoxs
                           select new
                           {
                                BoxID=p.BoxID,
                                BoxNo = p.BoxNo,
                                BoxName = p.BoxName,
                                ComputerIP = p.ComputerIP,
                                AreaID = p.AreaID,
                                Remark = p.Remark,
                                IsCenterPayment = p.IsCenterPayment,
                                AreaName = !string.IsNullOrEmpty(Request.Params["areaName"]) ? Request.Params["areaName"].ToString() : string.Empty,
                                ParkingName = !string.IsNullOrEmpty(Request.Params["parkName"]) ? Request.Params["parkName"].ToString() : string.Empty,
                            };
            json.Data = parkBoxDatas;
            return json;
        }
        [HttpPost]
        [CheckPurview(Roles = "PK01010501,PK01010502")]
        public JsonResult SaveEdit(ParkBox model)
        {
            try
            {
                bool result = false;
                if (string.IsNullOrWhiteSpace(model.BoxID))
                {
                    result = ParkBoxServices.Add(model);
                    if (!result) throw new MyException("添加失败");
                    return Json(MyResult.Success());
                }
                else
                {
                    result = ParkBoxServices.Update(model);
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
        [CheckPurview(Roles = "PK01010503")]
        public JsonResult Delete(string recordId)
        {
            try
            {
                bool result = ParkBoxServices.Delete(recordId);
                if (!result) throw new MyException("删除失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "删除岗亭信息失败");
                return Json(MyResult.Error("删除岗亭信息失败"));
            }
        }
        public JsonResult DownloadQRCode(string boxId, int size)
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

                if (string.IsNullOrWhiteSpace(SystemDefaultConfig.SystemDomain)){
                    throw new MyException("获取域名失败");
                }
                ParkBox box = ParkBoxServices.QueryByRecordId(boxId);
                if (box == null) throw new MyException("获取岗亭信息失败");

                ParkArea area =ParkAreaServices.GetParkAreaByParkBoxRecordId(boxId);
                if (area == null) throw new MyException("获取区域信息失败");

                BaseParkinfo parking = ParkingServices.QueryParkingByRecordId(area.PKID);
                if (parking == null) throw new MyException("获取车场信息失败");
              
               BaseCompany company = CompanyServices.QueryByBoxID(boxId);
               if (company == null) throw new MyException("获取单位编号失败");

                string content = string.Format("{0}/qrl/qrp_ix_bid={1}", SystemDefaultConfig.SystemDomain, boxId);
                foreach (var item in dics)
                {
                    string parkingName = string.Format("{0}_{1}_{2}_{3}", parking.PKName, area.AreaName, box.BoxName, item);
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
                ExceptionsServices.AddExceptions(ex, "下载岗亭二维码失败");
                return Json(MyResult.Error("下载二维码失败"));
            }
        }

        /// <summary>
        /// 获取卡片操作权限
        /// </summary>
        /// <returns></returns>
        public JsonResult GetParkBoxOperatePurview()
        {
            JsonResult result = new JsonResult();
            List<SystemOperatePurview> options = new List<SystemOperatePurview>();
            List<SysRoleAuthorize> roleAuthorizes = GetLoginUserRoleAuthorize.Where(p => p.ParentID == "PK010105").ToList();

            foreach (var item in roleAuthorizes)
            {
                switch (item.ModuleID)
                {
                    case "PK01010501":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "添加";
                            option.handler = "Add";
                            option.sort = 1;
                            options.Add(option);
                            break;
                        }
                    case "PK01010502":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "修改";
                            option.handler = "Update";
                            option.sort = 2;
                            options.Add(option);
                            break;
                        }
                    case "PK01010503":
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
            result.Data = options.OrderBy(p => p.sort);
            return result;
        }
    }
}
