using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Services.Park;
using Common.Entities;
using Common.Services;
using System.Text;
using Common.Entities.Parking;
using Common.Entities.Enum;
using Common.Utilities.Helpers;
using SmartSystem.WeiXinServices;
using SmartSystem.Interface.Services;
using SmartSystem.OmnipotentCardWebUI.Models;
using SmartSystem.OmnipotentCardWebUI.Controllers;
using System.Drawing.Imaging;
using System.IO;

namespace SmartSystem.OmnipotentCardWebUI.Areas.Parking.Controllers
{
    /// <summary>
    /// 优免二维码
    /// </summary>
    [CheckPurview(Roles = "PK010303")]
    public class ParkDerateQRCodeController : BaseController
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
                List<ParkDerateQRcode> models = ParkDerateQRcodeServices.QueryPage(sellerId, derateId, 0,queryStatus,derateQRCodeSource, rows, page, out total);
                if (models.Count > 0)
                {
                    ParkSeller seller = ParkSellerServices.QueryBySellerId(sellerId);
                    if (seller == null) throw new MyException("获取商家信息失败");

                    List<BaseParkinfo> parkings = ParkingServices.QueryParkingByVillageId(seller.VID);

                    Dictionary<string, int> result = ParkCarDerateServices.QuerySettlementdCarDerate(models.Select(p => p.RecordID).ToList());
                    foreach (var item in models)
                    {
                        int useTimes = result.ContainsKey(item.RecordID) ? result[item.RecordID] : 0;
                        string canUseTimes = item.CanUseTimes == 0 ? "不限" : item.CanUseTimes.ToString();
                        item.UseTimesDes = string.Format("{0}/{1}", canUseTimes, useTimes);
                        BaseParkinfo park = parkings.FirstOrDefault(p => p.PKID == item.PKID);
                        if (park != null)
                        {
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
                ExceptionsServices.AddExceptions(ex, "获取优免二维码失败");
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
                ExceptionsServices.AddExceptions(ex, "优免二维码根据商家编号获取优免信息失败");
                return result;
            }

        }
        [HttpPost]
        [CheckPurview(Roles = "PK01030302,PK01030303")]
        public JsonResult AddOrUpdate(ParkDerateQRcode model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.RecordID))
                {
                    model.OperatorId = GetLoginUser.RecordID;
                    model.DerateQRcodeType = 0;
                    model.DataSource = DerateQRCodeSource.Platefrom;
                    bool result = ParkDerateQRcodeServices.Add(model);
                    if (!result) throw new MyException("添加二维码成功");
                }
                else
                {
                    bool result = ParkDerateQRcodeServices.Update(model);
                    if (!result) throw new MyException("修改二维码成功");
                }
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "保存优免二维码失败");
                return Json(MyResult.Error("保存优免二维码失败"));
            }
        }
        /// <summary>
        /// 复制二维码
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="vid"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckPurview(Roles = "PK01030305")]
        public JsonResult AddIdenticalQRCode(string recordId,string vid,bool isAdd)
        {
            try
            {
                ParkDerateQRcode derate = ParkDerateQRcodeServices.QueryByRecordId(recordId);
                if (derate == null) throw new MyException("获取优免二维码失败");

                if (isAdd)
                {
                    derate.AlreadyUseTimes = 0;
                    derate.CreateTime = DateTime.Now;
                    derate.OperatorId = GetLoginUser.RecordID;
                    bool result = ParkDerateQRcodeServices.Add(derate);
                    if (!result) throw new MyException("添加优免二维码失败");
                }
                if (string.IsNullOrWhiteSpace(SystemDefaultConfig.SystemDomain))
                {
                    throw new MyException("获取系统域名失败");
                }
                BaseVillage village = VillageServices.QueryVillageByRecordId(vid);
                if (village == null) throw new MyException("获取小区信息失败");

                string content = string.Format("{0}/QRCodeDerate/Index?vid={1}&qid={2}&sign={3}", SystemDefaultConfig.SystemDomain, vid, recordId, GetSignature(vid, recordId));
                using (System.Drawing.Image image = QRCodeServices.GenerateQRCode(content, 430))
                {
                    ImageFormat format = image.RawFormat;
                    byte[] buffer;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        image.Save(ms, ImageFormat.Jpeg);
                        buffer = new byte[ms.Length];
                        ms.Seek(0, SeekOrigin.Begin);
                        ms.Read(buffer, 0, buffer.Length);
                    }
                    return Json(MyResult.Success("添加二维码成功", System.Convert.ToBase64String(buffer)));
                }
               
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "复制优免二维码失败");
                return Json(MyResult.Error("复制优免二维码失败"));
            }
        }
        [HttpPost]
        [CheckPurview(Roles = "PK01030304")]
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
                ExceptionsServices.AddExceptions(ex, "删除优免二维码失败");
                return Json(MyResult.Error("删除优免二维码失败"));
            }
        }
        [CheckPurview(Roles = "PK01030305")]
        public JsonResult DownloadQRCode(string vid, string qid, string sellerName, string derateName)
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
                if (string.IsNullOrWhiteSpace(SystemDefaultConfig.SystemDomain))
                {
                    throw new MyException("获取系统域名失败");
                }
                BaseVillage village = VillageServices.QueryVillageByRecordId(vid);
                if (village == null) throw new MyException("获取小区信息失败");

                string content = string.Format("{0}/QRCodeDerate/Index?vid={1}&qid={2}&sign={3}", SystemDefaultConfig.SystemDomain, vid, qid, GetSignature(vid, qid));
                foreach (var item in dics)
                {
                    try
                    {
                        string fileName = string.Format("{0}_{1}_{2}_{3}", sellerName, derateName, item, qid);
                        string result = QRCodeServices.GenerateQRCode(village.CPID, content, item, fileName);
                        imgs.Add(item.ToString() + "|" + result);
                        TxtLogServices.WriteTxtLogEx("DownloadQRCode", item.ToString() + "|" + result);
                    }
                    catch (Exception ex)
                    {
                        ExceptionsServices.AddExceptions(ex, "生存优免二维码失败");
                        imgs.Add(item.ToString() + "|");
                    }

                }

                return Json(MyResult.Success("", imgs));
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "下载优免二维码失败");
                return Json(MyResult.Error("下载优免二维码失败"));
            }
        }
        private string GetSignature(string vid, string qid)
        {
            Dictionary<string, string> paraMap = new Dictionary<string, string>();
            paraMap.Add("vid", vid);
            paraMap.Add("qid", qid);
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
                            option8.text = "下载二维码";
                            option8.id = "btndownloadqrcode";
                            option8.handler = "DownloadQRCode";
                            option8.iconCls = "icon-import";
                            option8.sort = 4;
                            options.Add(option8);

                            SystemOperatePurview option9 = new SystemOperatePurview();
                            option9.text = "查看二维码";
                            option9.id = "btndownloadqrcode";
                            option9.handler = "ShowDerateQRCode";
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