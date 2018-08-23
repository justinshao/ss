using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.WeiXinServices;
using Common.Entities;
using SmartSystem.WeiXinInerface;
using Common.Services.WeiXin;
using System.Drawing.Imaging;
using Common.ExternalInteractions.Sms.JuHe;
using Common.Entities.WX;
using Common.Services;
using System.Threading;
using System.Diagnostics;
using Common.Services.AliPay;
using Common.Entities.AliPay;
using Common.Utilities.Helpers;
using Common.Services.Park;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin
{
    public class TestController : Controller
    {
        public ActionResult Test() {
            RechargeService.SFMTempParkingFeeResult("2");
            //OnlineOrderServices.PaySuccess(11806020022000001, "4200000062201804255544381248", DateTime.Now);
            return View();
        }
        public ActionResult Index() {
            
            //ParkCarDerateServices.QueryBySellerIdAndIORecordId("46f54c5e-ada9-4efe-9552-a8560130a960", "6fe28c13-716a-48d1-b1ba-a8ee00061572");
            //string geng = Server.MapPath("~/TempFile/20180526");
            //string savepath = Server.MapPath("~/TempFile/201805263.zip");
            //ZipHelper.ZipFiles(geng, savepath);

            Stopwatch watch = Stopwatch.StartNew();//创建一个监听器

            for (var i = 0; i <= 100; i++)
            {
                TxtLogServices.WriteTxtLogEx("Test", "开始");
                var user = WeiXinAccountService.QueryWXByOpenId("oussCwE6unIvKaG-1wec-B7n3J0Q");
                //DateTime ts2 = DateTime.Now;
                //TimeSpan ts3 = ts1.Subtract(ts2).Duration();
                TxtLogServices.WriteTxtLogEx("Test", "结束");
                Thread.Sleep(10000);

                //DateTime ts1 = DateTime.Now;

                //var user = WeiXinAccountService.QueryWXByOpenId("oussCwE6unIvKaG-1wec-B7n3J0Q");
                ////DateTime ts2 = DateTime.Now;
                ////TimeSpan ts3 = ts1.Subtract(ts2).Duration();
                //TxtLogServices.WriteTxtLogEx("Test", watch.ElapsedMilliseconds.ToString());
                //Thread.Sleep(10000);
            }
            watch.Stop();

            var redirectUrl = "http://park.xinfu.info/r/ParkingPayment_Index_moduleid=2^cid=f50a5dee-d098-4100-a370-a7ca017ab4cf";
            string strRedirectUrl = HttpUtility.UrlEncode(redirectUrl);
            //WXApiConfigServices.QueryWXApiConfig
            //WX_ApiConfig confing = WXApiConfigServices.QueryWXApiConfig("83495fa0-611f-497a-9e10-a78c009f66cd");
            string s = string.Empty;
            //cd1e590c-e91e-45f7-b7d6-a75300e652a9^pn=ÔÁB88765
            //byte[] result = WXQRCodeServices.GenerateByteQRCode("http://ykt.bsgoal.net.cn", "cd1e590c-e91e-45f7-b7d6-a75300e652a9", "粤B88765", 430, "");
            //System.IO.MemoryStream ms = new System.IO.MemoryStream(result);
            //System.Drawing.Image img = System.Drawing.Image.FromStream(ms);

            //string filePath = string.Format("/Uploads/QRCode/{0}.{1}", "test111111111", "jpg");
            //string absolutePath = System.Web.HttpContext.Current.Server.MapPath(filePath);
            //img.Save(absolutePath, ImageFormat.Jpeg);

            //DateTime t1 = DateTime.Now.AddMinutes(-10).AddSeconds(-2);
            //DateTime t2 = DateTime.Now;
            //string s = t1.GetParkingDuration(t2);
            //string s = "1   2 34  5566".PlateNumberToUpper();
            //JsonObject obj = new JsonObject();

            //PlatformOrderBLL.ManualRefund(123123, "");
            //string v = XmlConfig.GetValue("PromptAttentionPage");
            //string openid = "odvkywSnlKr8anm3ddoIcredwvN0";
            //string url = "http://wx.qlogo.cn/mmopen/gKlic31XKbJ7BOJyEvicpgpW0ym5rfqGS0ibBSWLVOlDaSm4QZ1vCqEAxKohVtuj3fEn1vHfia6Y4fXEN9zXhxrhuRmMgoELyBll/0";
            //WeiXinBaseInfo.DownloadHeadImg(openid, url);
            //RoutematrixService.GetRoutematrix("113.977295", "22.731472", "113.687295", "22.761472");
            //List<BaseParkinfo> parks = QueryParkingService.QueryParkinfo("22.735069".ToDouble(), "113.988769".ToDouble(), 4000);
            //WX_ApiConfig config = GetApiConfig("moduleid=2^cid=0642cbb1-d55d-4629-855d-a73c0100397b");
            return View();
        }
        private WX_ApiConfig GetApiConfig(string ids)
        {
           
            //var separator = new[] { '|', '_' };
            var param = new[] { '^' };//^参数分隔符
            //var ids = id.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            var parameters = ids.Split(param, StringSplitOptions.RemoveEmptyEntries);
            if (parameters.Length == 0) throw new MyException("获取单位失败");
            foreach (var item in parameters)
            {
                var parame = item.Split(new[] { '=' });
                if (parame.Length <2)
                {
                    TxtLogServices.WriteTxtLogEx("AliPayAuthorize", "Redir方法参数设置错误：param:{0}", "1");
                }
            }
            //throw new MyException("获取微信配置失败，id:" + id);
            return null;
        }
        public ActionResult SendSms() {
            //string result = "{\"reason\":\"操作成功\",\"result\":{\"sid\":\"1000630160738462600\",\"fee\":1,\"count\":1},\"error_code\":0}";

            //SmsResultModel smsResult = JsonHelper.GetJson<SmsResultModel>(result);

            //JuHeSmsResult result = JuHeCodeSmsService.SendGateCongestionSms("15217725639", "通道名称",DateTime.Now,2);
           //string s = string.Empty;
           return View();
        }
        public ActionResult QRCode()
        {
            // string content = string.Format("{0}/qrl/qrp_ix_pid={1}", config.Domain, parkingId);//根据车场扫码
            // string content = string.Format("{0}/qrl/qrp_ix_pid={1}^pn={2}", config.Domain, parkingId);//根据车场和车牌号扫码
            // string content = string.Format("{0}/qrl/scio_ix_pid={1}^io={2}", config.Domain, parkingId,gateNo);//根据车场和通道扫码
            int size = 1280;
            string content = string.Format("{0}/qrl/qrp_ix_pid={1}", "http://localhost:60157", "parkingId");
            string parkingName = string.Format("{0}_{1}", "test", size);
            //string result = QRCodeServices.GenerateQRCode(content, size, parkingName);
            
            return View();
        }

        public ActionResult TestAction(string param1,string param2) {
            ViewBag.P1 = param1;
            ViewBag.P2 = param2;
            return View();
        }
        public ActionResult Dialog() {
            return View();
        }
    }
}
