using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.OmnipotentCardWebUI.Models;
using Common.Entities.Parking;
using Common.Services.Park;
using Common.Entities;
using Common.Services;
using Common.Utilities;
using SmartSystem.Interface.Services;
using SmartSystem.WeiXinServices;
using System.Drawing.Imaging;
using System.IO;
using Common.Entities.Enum;
using Common.Utilities.Helpers;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.XFJM
{
    public class XFJMDerateQRCodeController : XFJMController
    {
        [CheckSellerPurview]
        [CheckWeiXinPurview(Roles = "Login")]
        public ActionResult Index()
        {
            List<ParkDerate> derates = ParkDerateServices.QueryBySellerID(SellerLoginUser.SellerID);
            List<EnumContext> derateContexts = new List<EnumContext>();
            foreach (var item in derates)
            {
                EnumContext model = new EnumContext();
                model.EnumString = item.DerateID;
                model.Description = item.Name;
                model.EnumValue = (int)item.DerateType;
                derateContexts.Add(model);
            }
            ViewBag.DerateContexts = derateContexts;
            ViewBag.CarDerateStatus = EnumHelper.GetEnumContextList(typeof(CarDerateStatus), true);
            ViewBag.StartTime = DateTime.Now.AddDays(-7).Date.AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss").Replace(" ", "T");
            ViewBag.EndTime = DateTime.Now.Date.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss").Replace(" ", "T");
            return View();
        }
        public ActionResult GetParkDerateQRcodeData(string derateId, int? status,int page)
        {
            try
            {
                int rows = 10;
                int total = 0;
                List<ParkDerateQRcode> models = ParkDerateQRcodeServices.QueryPage(SellerLoginUser.SellerID, derateId,0,status, DerateQRCodeSource.Seller, rows, page, out total);
                if (models.Count > 0)
                {
                    ParkSeller seller = ParkSellerServices.QueryBySellerId(SellerLoginUser.SellerID);
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
                return Json(MyResult.Success("", models));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "获取优免二维码失败",LogFrom.WeiXin);
                return Json(MyResult.Error("获取优免二维码失败"));
            }
        }
        [CheckSellerPurview]
        [CheckWeiXinPurview(Roles = "Login")]
        public ActionResult Edit(string recordId)
        {
            try
            {
                ParkDerateQRcode derate = new ParkDerateQRcode();
                if (!string.IsNullOrWhiteSpace(recordId))
                {
                    derate = ParkDerateQRcodeServices.QueryByRecordId(recordId);
                }
                else
                {
                    derate.StartTime = DateTime.Now;
                    derate.EndTime = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                }

                List<ParkDerate> derates = ParkDerateServices.QueryBySellerID(SellerLoginUser.SellerID);
                List<EnumContext> derateContexts = new List<EnumContext>();
                foreach (var item in derates)
                {
                    EnumContext model = new EnumContext();
                    model.EnumString = item.DerateID;
                    model.Description = item.Name;
                    model.EnumValue = (int)item.DerateType;
                    derateContexts.Add(model);
                }
                ViewBag.DerateContexts = derateContexts;
                return View(derate);
            }
            catch (MyException ex)
            {
                return RedirectToAction("Index", "XFJMDerateQRCode", new { RemindUserContent = ex.Message });
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "微信修优免二维码失败", LogFrom.WeiXin);
                return RedirectToAction("Index", "XFJMDerateQRCode", new { RemindUserContent = "操作失败" });
            }
        }
        [HttpPost]
        [CheckSellerPurview]
        [CheckWeiXinPurview(Roles = "Login")]
        public ActionResult AddOrUpdate(ParkDerateQRcode model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.RecordID))
                {
                    model.OperatorId = SellerLoginUser.SellerID;
                    model.DataSource = DerateQRCodeSource.Seller;
                    bool result = ParkDerateQRcodeServices.Add(model);
                    if (!result) throw new MyException("添加二维码失败");
                }
                else
                {
                    bool result = ParkDerateQRcodeServices.Update(model);
                    if (!result) throw new MyException("修改二维码失败");
                }
                return RedirectToAction("Index", "XFJMDerateQRCode", new { RemindUserContent = "保存成功" });
            }
            catch (MyException ex)
            {
                return RedirectToAction("Index", "XFJMDerateQRCode", new { RemindUserContent = ex.Message });
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "保存优免二维码失败", LogFrom.WeiXin);
                return RedirectToAction("Index", "XFJMDerateQRCode", new { RemindUserContent = "保存优免二维码失败" });
            }
        }
        public ActionResult DerateQRCode(string recordId)
        {
            try
            {
                ParkDerateQRcode derateQRcode = ParkDerateQRcodeServices.QueryByRecordId(recordId);
                if (derateQRcode == null) throw new MyException("该二维码不存在");

                ParkDerate derate = ParkDerateServices.Query(derateQRcode.DerateID);
                if (derate == null) throw new MyException("优免规则不存在");
                derateQRcode.DerateName = derate.Name;

                ParkSeller seller =  ParkSellerServices.QueryBySellerId(derate.SellerID);
                if (seller == null) throw new MyException("商家不存在");
                derateQRcode.SellerName = seller.SellerName;

                if (string.IsNullOrWhiteSpace(SystemDefaultConfig.SystemDomain))
                {
                    throw new MyException("获取系统域名失败");
                }
                BaseVillage village = VillageServices.QueryVillageByRecordId(seller.VID);
                if (village == null) throw new MyException("获取小区信息失败");

                string content = string.Format("{0}/QRCodeDerate/Index?vid={1}&qid={2}&sign={3}", SystemDefaultConfig.SystemDomain, seller.VID, recordId, GetSignature(seller.VID, recordId));
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
                    derateQRcode.ImageData = System.Convert.ToBase64String(buffer);
                }
                ViewBag.CompanyID = village.CPID;
                ViewBag.ShareAction = "XFJMDerateQRCode/DerateQRCode?recordId=" + recordId;
                return View(derateQRcode);
            }
            catch (MyException ex)
            {
                return RedirectToAction("Index", "XFJMDerateQRCode", new { RemindUserContent = ex.Message });
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "微信获取二维码失败", LogFrom.WeiXin);
                return RedirectToAction("Index", "XFJMDerateQRCode", new { RemindUserContent = "获取二维码失败" });
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
        [HttpPost]
        [CheckSellerPurview]
        [CheckWeiXinPurview(Roles = "Login")]
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

        [CheckSellerPurview]
        [CheckWeiXinPurview(Roles = "Login")]
        public ActionResult AddIdenticalQRCode(string recordId)
        {
            try
            {
                ParkDerateQRcode derate = ParkDerateQRcodeServices.QueryByRecordId(recordId);
                if (derate == null) throw new MyException("获取优免二维码失败");

                derate.AlreadyUseTimes = 0;
                derate.CreateTime = DateTime.Now;
                derate.OperatorId = SellerLoginUser.SellerID;
                derate.DataSource = DerateQRCodeSource.Seller;
                bool result = ParkDerateQRcodeServices.Add(derate);
                if (!result) throw new MyException("复制二维码失败");
                return RedirectToAction("Index", "XFJMDerateQRCode", new { RemindUserContent = "复制二维码保存成功" });
            }
            catch (MyException ex)
            {
                return RedirectToAction("Index", "XFJMDerateQRCode", new { RemindUserContent = ex.Message });
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "复制优免二维码失败");
                return RedirectToAction("Index", "XFJMDerateQRCode", new { RemindUserContent = "复制优免二维码失败" });
            }
        }
    }
}
