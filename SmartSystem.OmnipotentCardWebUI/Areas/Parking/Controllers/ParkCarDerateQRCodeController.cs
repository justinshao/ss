using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using SmartSystem.OmnipotentCardWebUI.Controllers;
using Common.Entities.Enum;
using Common.Services.Park;
using Common.Entities.Parking;
using Common.Utilities.Helpers;
using Common.Services;
using Common.Entities;
using SmartSystem.Interface.Services;
using SmartSystem.OmnipotentCardWebUI.Models;
using SmartSystem.WeiXinServices;
using Common.Utilities;

namespace SmartSystem.OmnipotentCardWebUI.Areas.Parking.Controllers
{
    /// <summary>
    /// 一次性优免二维码
    /// </summary>
    [CheckPurview(Roles = "PK01030401")]
    public class ParkCarDerateQRCodeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.DefaultStartTime = DateTime.Now.Date;
            ViewBag.DefaultEndTime = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
            return View();
        }
        public string GetParkDerateQRcodeData()
        {

            StringBuilder strData = new StringBuilder();
            try
            {
                if (string.IsNullOrEmpty(Request.Params["sellerId"])) return string.Empty;
                string sellerId = Request.Params["sellerId"].ToString();
                string derateId = Request.Params["derateId"].ToString();

                int? queryStatus = null;
                int status = 0;
                if (!string.IsNullOrWhiteSpace(Request.Params["status"]) && int.TryParse(Request.Params["status"].ToString(), out status))
                {
                    queryStatus = status;
                }
                DerateQRCodeSource? derateQRCodeSource = null;
                int source = 0;
                if (!string.IsNullOrWhiteSpace(Request.Params["DerateQRCodeSource"]) && int.TryParse(Request.Params["DerateQRCodeSource"].ToString(), out source))
                {
                    derateQRCodeSource = (DerateQRCodeSource)source;
                }


                int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int rows = string.IsNullOrEmpty(Request.Params["rows"]) ? 0 : int.Parse(Request.Params["rows"]);
                int total = 0;
                List<ParkDerateQRcode> models = ParkDerateQRcodeServices.QueryPage(sellerId, derateId, 1, queryStatus, derateQRCodeSource, rows, page, out total);
                if (models.Count > 0) {
                    ParkSeller seller = ParkSellerServices.QueryBySellerId(sellerId);
                    if (seller == null) throw new MyException("获取商家信息失败");

                    List<BaseParkinfo> parkings = ParkingServices.QueryParkingByVillageId(seller.VID);

                    Dictionary<string, int> result = ParkCarDerateServices.QuerySettlementdCarDerate(models.Select(p => p.RecordID).ToList());
                   foreach (var item in models) { 
                       int useTimes = result.ContainsKey(item.RecordID)?result[item.RecordID]:0;
                       string canUseTimes = item.CanUseTimes == 0 ? "不限" : item.CanUseTimes.ToString();
                       item.UseTimesDes = string.Format("{0}/{1}", canUseTimes, useTimes);
                       BaseParkinfo park = parkings.FirstOrDefault(p => p.PKID == item.PKID);
                       if (park != null) {
                           item.ParkName = park.PKName;
                       }
                   }
                }
                strData.Append("{");
                strData.Append("\"total\":" + total + ",");
                strData.Append("\"rows\":" + JsonHelper.GetJsonString(models) + ",");
                strData.Append("\"index\":" + page);
                strData.Append("}");
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "获取一次性优免二维码失败");
            }
            return strData.ToString();
        }
        public string GetSellerDerateTree(bool needDefaultValue = false)
        {
            try
            {
                if (string.IsNullOrEmpty(Request.Params["sellerId"])) return "[]";

                string sellerId = Request.Params["sellerId"].ToString();
                StringBuilder strTree = new StringBuilder();
                strTree.Append("[");
                if (needDefaultValue)
                {
                    strTree.Append("{\"id\":\"\",");
                    strTree.Append("\"text\":\"不限\"");
                    strTree.Append("}");
                }
                List<ParkDerate> derates = ParkDerateServices.QueryBySellerID(sellerId);
                foreach (var item in derates)
                {
                    if (strTree.ToString() != "[")
                    {
                        strTree.Append(",");
                    }
                    strTree.Append("{\"id\":\"" + item.DerateID + "\",");
                    strTree.Append("\"text\":\"" + item.Name + "\"");
                    strTree.Append("}");
                }
                strTree.Append("]");
                return strTree.ToString();
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "优免二维码根据商家编号获取优免信息失败");
                return "[]";
            }

        }
        public ActionResult GetSellerDerateData()
        {
            JsonResult result = new JsonResult();
            try
            {
                if (string.IsNullOrEmpty(Request.Params["sellerId"])) return result;
                string sellerId = Request.Params["sellerId"].ToString();

                List<ParkDerate> derates = ParkDerateServices.QueryBySellerID(sellerId);
                result.Data = derates;
                return result;
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "优免券发放获取优免信息失败");
                return result;
            }

        }
        public ActionResult GetParkingData()
        {
            JsonResult result = new JsonResult();
            try
            {
                if (string.IsNullOrEmpty(Request.Params["sellerId"])) return result;
                string sellerId = Request.Params["sellerId"].ToString();

                ParkSeller seller = ParkSellerServices.QueryBySellerId(sellerId);
                if (seller == null) throw new MyException("获取商家信息失败");

                List<BaseParkinfo> parkings = ParkingServices.QueryParkingByVillageId(seller.VID);
                result.Data = parkings;
                return result;
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "优免券发放车场信息失败");
                return result;
            }

        }
        [HttpPost]
        [CheckPurview(Roles = "PK01030402,PK01030403")]
        public JsonResult AddOrUpdate(ParkDerateQRcode model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.RecordID))
                {
                    model.StartTime = DateTime.Now;
                    model.OperatorId = GetLoginUser.RecordID;
                    model.DerateQRcodeType = 1;
                    model.DataSource = DerateQRCodeSource.Platefrom;
                    bool result = ParkDerateQRcodeServices.Add(model);
                    if (!result) throw new MyException("添加一次性二维码成功");
                }
                else
                {
                    model.StartTime = DateTime.Now;
                    bool result = ParkDerateQRcodeServices.Update(model);
                    if (!result) throw new MyException("修改一次性二维码成功");
                }
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "保存一次性优免二维码失败");
                return Json(MyResult.Error("保存一次性优免二维码失败"));
            }
        }
        [HttpPost]
        [CheckPurview(Roles = "PK01030404")]
        public JsonResult Delete(string recordId)
        {
            try
            {
                bool result = ParkDerateQRcodeServices.Delete(recordId);
                if (!result) throw new MyException("删除失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "删除一次性优免二维码失败");
                return Json(MyResult.Error("删除一次性优免二维码失败"));
            }
        }
        /// <summary>
        /// 发放优免券
        /// </summary>
        /// <param name="vid"></param>
        /// <param name="qid"></param>
        /// <param name="sellerName"></param>
        /// <param name="derateName"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        [CheckPurview(Roles = "PK01030405")]
        public JsonResult GrantCarDerate(string vid, string qid,string sellerName, string derateName, int number)
        {
            try
            {
                if (number <= 0) throw new MyException("发放优免券数量不正确");

                if (string.IsNullOrWhiteSpace(SystemDefaultConfig.SystemDomain)){
                    throw new MyException("获取系统域名失败");
                }
                ParkDerateQRcode qrCode = ParkDerateQRcodeServices.QueryByRecordId(qid);
                if (qrCode == null) throw new MyException("优免券规则不存在");

                ParkDerate derate = ParkDerateServices.Query(qrCode.DerateID);
                if (derate == null) throw new MyException("获取优免规则失败");

                if (derate.DerateType == DerateType.SpecialTimePeriodPayment) {
                    string errorMsg = string.Empty;
                    ParkSeller seller = ParkSellerServices.GetSeller(derate.SellerID, out errorMsg);
                    if (derate == null) throw new MyException("获取优免规则失败");

                    decimal totalAmount = qrCode.DerateValue * number;
                    if ((seller.Creditline + seller.Balance) < totalAmount) throw new MyException("商家余额不足");
                }

                BaseVillage village = VillageServices.QueryVillageByRecordId(vid);
                if (village == null) throw new MyException("获取小区信息失败");

                string folderName = string.Format("{0}_{1}_{2}", sellerName, derateName,IdGenerator.Instance.GetId().ToString());
                List<string> carDerateIds = new List<string>();
                for (int i = 0; i < number; i++) {
                    string carDerateId = GuidGenerator.GetGuidString();
                    string content = string.Format("{0}/QRCodeDerate/Index?vid={1}&qid={2}&did={3}&sign={4}", SystemDefaultConfig.SystemDomain, vid, qid, carDerateId, GetSignature(vid, qid, carDerateId));
                    string result = QRCodeServices.GenerateQRCode(village.CPID, content, 430,carDerateId,folderName);
                    if (string.IsNullOrWhiteSpace(result)) throw new MyException("创建二维码失败");
                    carDerateIds.Add(carDerateId);
                }
                string filePath =string.Format("/Uploads/{0}", folderName);
                string zipFilePath = string.Format("{0}/{1}_{2}.zip", filePath, sellerName,derateName);
                string mapPath = Server.MapPath("~/");

                ZipHelper.ZipFiles(string.Format("{0}/{1}", mapPath, filePath), string.Format("{0}/{1}", mapPath, zipFilePath));
                if (carDerateIds.Count != number) throw new MyException("二维码数量与待创建的数量不匹配");

                bool grantResult = ParkDerateQRcodeServices.GrantCarDerate(carDerateIds, zipFilePath, qid);
               if (!grantResult) throw new MyException("发放券失败");

               return Json(MyResult.Success("", zipFilePath));
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "发放优免券失败");
                return Json(MyResult.Error("发放优免券失败"));
            }
        }
        private string GetSignature(string vid, string qid, string did)
        {
            Dictionary<string, string> paraMap = new Dictionary<string, string>();
            paraMap.Add("vid", vid);
            paraMap.Add("qid", qid);
            paraMap.Add("did", did);
            paraMap.Add("key", SystemDefaultConfig.Secretkey);
            return SignatureServices.Signature(paraMap);
        }
        /// <summary>
        /// 获取优免二维码操作权限
        /// </summary>
        /// <returns></returns>
        public JsonResult GetParkDerateQRCodeOperatePurview()
        {
            JsonResult result = new JsonResult();
            List<SystemOperatePurview> options = new List<SystemOperatePurview>();
            List<SysRoleAuthorize> roleAuthorizes = GetLoginUserRoleAuthorize.Where(p => p.ParentID == "PK010303").ToList();

            foreach (var item in roleAuthorizes)
            {
                switch (item.ModuleID)
                {
                    case "PK01030302":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "添加";
                            option.handler = "Add";
                            option.sort = 1;
                            options.Add(option);
                            break;
                        }
                    case "PK01030303":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "修改";
                            option.handler = "Update";
                            option.sort = 2;
                            options.Add(option);
                            break;
                        }
                    case "PK01030304":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "删除";
                            option.handler = "Delete";
                            option.sort = 3;
                            options.Add(option);
                            break;
                        }
                    case "PK01030305":
                        {
                            SystemOperatePurview option8 = new SystemOperatePurview();
                            option8.text = "发放优免券";
                            option8.id = "btngrantqrcode";
                            option8.handler = "GrantCarDerate";
                            option8.iconCls = "icon-add";
                            option8.sort = 4;
                            options.Add(option8);

                            SystemOperatePurview option9 = new SystemOperatePurview();
                            option9.text = "下载最后下发二维码";
                            option9.id = "btndownloadqrcode";
                            option9.handler = "DownloadQRCode";
                            option9.iconCls = "icon-import";
                            option9.sort = 5;
                            options.Add(option9);
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
