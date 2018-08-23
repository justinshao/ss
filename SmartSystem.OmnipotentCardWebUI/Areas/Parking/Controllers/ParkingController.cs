using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.OmnipotentCardWebUI.Controllers;
using System.Text;
using Common.Services;
using Common.Entities;
using Common.Utilities;
using Common.Entities.BaseData;
using SmartSystem.OmnipotentCardWebUI.Models;
using Common.Utilities.Helpers;
using SmartSystem.WeiXinServices;
using Common.Services.WeiXin;
using Common.Entities.WX;
using Common.Services.Park;
using Common.Entities.Parking;

namespace SmartSystem.OmnipotentCardWebUI.Areas.Parking.Controllers
{
    [CheckPurview(Roles = "PK010103")]
    public class ParkingController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public string GetParkingData() {
            try
            {
                StringBuilder str = new StringBuilder();
                if (string.IsNullOrWhiteSpace(Request.Params["villageId"]))
                {
                    return str.ToString();
                }

                int pageIndex = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int pageSize = string.IsNullOrEmpty(Request.Params["rows"]) ? 0 : int.Parse(Request.Params["rows"]);
               
                int totalCount = 0;
                List<BaseParkinfo> parkData = ParkingServices.QueryPage(Request.Params["villageId"].ToString(), pageIndex, pageSize, out totalCount);

                str.Append("{");
                str.Append("\"total\":" + totalCount + ",");
                str.Append("\"rows\":" + JsonHelper.GetJsonString(parkData) + ",");
                str.Append("\"index\":" + pageIndex);
                str.Append("}");
                return str.ToString();
            }
            catch (Exception ex) {
                ExceptionsServices.AddExceptions(ex, "获取车场信息失败");
                return string.Empty;
            }
        }
        [HttpPost]
        [CheckPurview(Roles = "PK01010301,PK01010302")]
        public JsonResult SaveParking(BaseParkinfo model)
        {
            try
            {
                bool result = false;
                if (string.IsNullOrWhiteSpace(model.PKID))
                {
                    result = ParkingServices.Add(model);
                }
                else
                {
                    result = ParkingServices.Update(model);
                }
                if (!result) throw new MyException("保存失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex) {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "保存车场信息失败");
                return Json(MyResult.Error("保存失败"));
            }

        }
        [HttpPost]
        [CheckPurview(Roles = "PK01010303")]
        public JsonResult Delete(string parkingId)
        {
            try
            {
                bool result = ParkingServices.Delete(parkingId);
                if (!result) throw new MyException("删除失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "删除车场信息失败");
                return Json(MyResult.Error("删除失败"));
            }
        }
        public JsonResult GetPassRemarkData(string parkingId)
        {
            JsonResult json = new JsonResult();
            try
            {
                if (string.IsNullOrWhiteSpace(parkingId)) return json;
                List<BasePassRemark> remarks  = PassRemarkServices.QueryByParkingId(parkingId, null);
                var result = from p in remarks select new {
                    ID = p.ID,
                    PKID = p.PKID,
                    RecordID = p.RecordID,
                    PassType = (int)p.PassType,
                    PassTypeDes = p.PassType.GetDescription(),
                    Remark = p.Remark,
                };
                json.Data = result;
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "获取车场放行备注失败");
            }
            return json;
        }
        [HttpPost]
        [CheckPurview(Roles = "PK01010304")]
        public JsonResult SavePassRemark()
        {
            try
            {
                BasePassRemark model = new BasePassRemark();
                model.PKID = Request.Params["ParkingID"];
                model.Remark = Request.Params["Remark"];
                model.PassType = (PassRemarkType)int.Parse(Request.Params["PassType"].ToString());

                bool result = PassRemarkServices.Add(model);
                if (!result) throw new MyException("保存放行备注失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex) {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "保存放行备注失败");
                return Json(MyResult.Error("保存放行备注失败"));
            }

        }
        public JsonResult DownloadQRCode(string parkingId,int size) {
            try
            {
                List<int> dics = new List<int>();
                dics.Add(258);
                dics.Add(344);
                dics.Add(430);
                dics.Add(860);
                dics.Add(1280);

                List<string> imgs = new List<string>();
                if (string.IsNullOrWhiteSpace(SystemDefaultConfig.SystemDomain))
                {
                    throw new MyException("获取系统域名失败");
                }
                BaseParkinfo parking = ParkingServices.QueryParkingByParkingID(parkingId);
                if (parking == null) throw new MyException("获取车场信息失败");

                BaseVillage village = VillageServices.QueryVillageByRecordId(parking.VID);
                if (village == null) throw new MyException("获取小区信息失败");

                string content = string.Format("{0}/qrl/qrp_ix_pid={1}", SystemDefaultConfig.SystemDomain, parkingId);
                foreach (var item in dics) {
                    try
                    {
                        string parkingName = string.Format("{0}_{1}", parking.PKName, item);
                        string result = QRCodeServices.GenerateQRCode(village.CPID,content, item, parkingName);
                        imgs.Add(item.ToString() + "|" + result);
                        TxtLogServices.WriteTxtLogEx("DownloadQRCode", item.ToString() + "|" + result);
                    }
                    catch (Exception ex) {
                        ExceptionsServices.AddExceptions(ex, "生存车场二维码失败");
                        imgs.Add(item.ToString() + "|");
                    }
                   
                }

                return Json(MyResult.Success("", imgs));
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex) {
                ExceptionsServices.AddExceptions(ex, "下载二维码失败");
                return Json(MyResult.Error("下载二维码失败"));
            }
        }
        [HttpPost]
        [CheckPurview(Roles = "PK01010303")]
        public JsonResult DeletePassRemark(string recordId)
        {
            try
            {
                bool result = PassRemarkServices.Delete(recordId);
                if (!result) throw new MyException("删除放行备注失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "保删除放行备注失败");
                return Json(MyResult.Error("删除放行备注失败"));
            }
        }
        public string GetPassTypeTreeData() {
            List<EnumContext> options = EnumHelper.GetEnumContextList(typeof(PassRemarkType));
            StringBuilder str = new StringBuilder();
            try
            {
                str.Append("[");
               
                int index = 1;
                foreach (var item in options)
                {
                  
                    str.Append("{\"id\":\"" + item.EnumValue + "\",");
                    str.Append("\"attributes\":{\"type\":1},");
                    str.AppendFormat("\"text\":\"{0}\"", item.Description);
                    str.Append("}");
                    if (index != options.Count())
                    {
                        str.Append(",");
                    }
                    index++;
                }
                str.Append("]");
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "构建放行类型树结构失败");
            }
            return str.ToString();
        }

        [HttpPost]
        [CheckPurview(Roles = "PK01010306")]
        public JsonResult GetDerateConfigData(string parkingId)
        {
            try
            {

                List<ParkDerateConfig> result = ParkDerateConfigServices.QueryByParkingId(parkingId);
                return Json(MyResult.Success(string.Empty,result.OrderBy(p=>p.ConsumeStartAmount)));
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "获取车场优免配置失败");
                return Json(MyResult.Error("获取车场优免配置失败"));
            }
        }
        [HttpPost]
        [CheckPurview(Roles = "PK01010306")]
        public JsonResult SaveDerateConfig(ParkDerateConfig model)
        {
            try
            {
                if (model.ConsumeStartAmount < 0 || model.ConsumeEndAmount < 0) {
                    throw new MyException("消费开始金额或结束金额格式不正确");
                }
                if (model.ConsumeStartAmount >= model.ConsumeEndAmount)
                {
                    throw new MyException("消费开始金额不能大于消费结束金额");
                }
                if (model.DerateValue<0)
                {
                    throw new MyException("消费减免值格式不正确");
                }
                if (!string.IsNullOrWhiteSpace(model.RecordID))
                {
                    bool result = ParkDerateConfigServices.Update(model);
                    if (!result) throw new MyException("修改车场优免配置失败");
                }
                else {
                    bool result = ParkDerateConfigServices.Add(model);
                    if (!result) throw new MyException("添加车场优免配置失败");
                }
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "保存车场优免配置失败");
                return Json(MyResult.Error("保存车场优免配置失败"));
            }
        }
        [HttpPost]
        [CheckPurview(Roles = "PK01010306")]
        public JsonResult DeleteDerateConfig(string recordId)
        {
            try
            {
                bool result = ParkDerateConfigServices.Delete(recordId);
                if (!result) throw new MyException("删除车场优免配置失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "删除车场优免配置失败");
                return Json(MyResult.Error("删除车场优免配置失败"));
            }
        }
        /// <summary>
        /// 获取车场操作权限
        /// </summary>
        /// <returns></returns>
        public JsonResult GetParkingOperatePurview()
        {
            JsonResult result = new JsonResult();
            List<SystemOperatePurview> options = new List<SystemOperatePurview>();
            List<SysRoleAuthorize> roleAuthorizes = GetLoginUserRoleAuthorize.Where(p => p.ParentID == "PK010103").ToList();

            foreach (var item in roleAuthorizes)
            {
                switch (item.ModuleID)
                {
                    case "PK01010301":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "添加";
                            option.handler = "Add";
                            option.sort = 1;
                            options.Add(option);
                            break;
                        }
                    case "PK01010302":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "修改";
                            option.handler = "Update";
                            option.sort = 2;
                            options.Add(option);
                            break;
                        }
                    case "PK01010303":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "删除";
                            option.handler = "Delete";
                            option.sort = 3;
                            options.Add(option);
                            break;
                        }
                    case "PK01010304":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "放行备注";
                            option.id = "btnremark";
                            option.handler = "PassRmark";
                            option.iconCls = "icon-add";
                            option.sort = 4;
                            options.Add(option);
                            break;
                        }
                    case "PK01010306":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "消费减免";
                            option.id = "btnderate";
                            option.handler = "ParkDerate";
                            option.iconCls = "icon-add";
                            option.sort = 5;
                            options.Add(option);
                            break;
                        }
                    case "PK01010305":
                        {
                            SystemOperatePurview option8 = new SystemOperatePurview();
                            option8.text = "下载二维码";
                            option8.id = "btndownloadqrcode";
                            option8.handler = "DownloadQRCode";
                            option8.iconCls = "icon-import";
                            option8.sort = 6;
                            options.Add(option8);
                            break;
                        }
                }
            }
     

            SystemOperatePurview roption = new SystemOperatePurview();
            roption.text = "刷新";
            roption.handler = "Refresh";
            roption.sort = 7;
            options.Add(roption);

            result.Data = options.OrderBy(p => p.sort);
            return result;
        }
        public JsonResult GetParkingPassRemarkOperatePurview()
        {
            JsonResult result = new JsonResult();
            List<SystemOperatePurview> options = new List<SystemOperatePurview>();
            List<SysRoleAuthorize> roleAuthorizes = GetLoginUserRoleAuthorize.Where(p => p.ParentID == "PK010103").ToList();

            foreach (var item in roleAuthorizes)
            {
                switch (item.ModuleID)
                {
                    case "PK01010304":
                        {

                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "删除";
                            option.handler = "DeletePassRemark";
                            option.sort = 2;
                            options.Add(option);

                            break;
                        }
                }
            }
            SystemOperatePurview roption = new SystemOperatePurview();
            roption.text = "刷新";
            roption.handler = "RefreshPassRemark";
            roption.sort = 6;
            options.Add(roption);

            result.Data = options.OrderBy(p => p.sort);
            return result;
        }
    }
}
