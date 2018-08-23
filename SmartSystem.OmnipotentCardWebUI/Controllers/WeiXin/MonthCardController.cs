using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.OmnipotentCardWebUI.Models;
using ClassLibrary1;
using Newtonsoft.Json;
using Common.Utilities.Helpers;
using System.Text;
using Common.Services;
using Common.Services.BaseData;
using Common.Entities.Enum;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin
{
    public class MonthCardController : WeiXinController
    {
        [PageBrowseRecord]
        [CheckWeiXinPurview(Roles = "Login")]
        public ActionResult Index()
        {
            try
            {
                string auth = AppUserToken;

                if (auth.IsEmpty())
                {
                    //没有登录
                    //
                    return RedirectToAction("Index", "ErrorPrompt", new { message = "用户登录失败" });
                }
                else if (auth == "-1")
                {
                    return RedirectToAction("Register", "ParkingPayment");
                }
                Car car = Carapi.bb(auth);
                Car mycar = JsonConvert.DeserializeObject<Car>(JsonHelper.GetJsonString(car));

                return View(mycar);
            }
            catch(Exception e) 
            {
                return View();
            }
        }
        /// <summary>
        /// 月卡明细
        /// </summary>
        /// <returns></returns>
        public ActionResult MonthCardMX()
        {
            string auth = AppUserToken;

            if (auth.IsEmpty())
            {
                //没有登录
                //
                return RedirectToAction("Index", "ErrorPrompt", new { message = "用户登录失败" });
            }
            else if (auth == "-1")
            {
                return RedirectToAction("Register", "ParkingPayment");
            }
            CardDetail cd = Carapi.cd(1, 1, 50, auth);
            CardDetail detail = JsonConvert.DeserializeObject<CardDetail>(JsonHelper.GetJsonString(cd));
            return View(detail);
        }
        /// <summary>
        /// 月卡信息
        /// </summary>
        /// <param name="CarID"></param>
        /// <returns></returns>
        public ActionResult MonthCardInfo(string CarID)
        {
            string auth = AppUserToken;

            if (auth.IsEmpty())
            {
                //没有登录
                //
                return RedirectToAction("Index", "ErrorPrompt", new { message = "用户登录失败" });
            }
            else if (auth == "-1")
            {
                return RedirectToAction("Register", "ParkingPayment");
            }
            MonthCard mc1 = Carapi.mc(auth, CarID);
            MonthCard mymc = JsonConvert.DeserializeObject<MonthCard>(JsonHelper.GetJsonString(mc1));
            if (mymc == null) { return null; }
            return View(mymc);
        }
        /// <summary>
        /// 月卡详情
        /// </summary>
        /// <param name="CarID"></param>
        /// <returns></returns>

        public ActionResult MonthCardDetails(decimal amount,string cardid,int day,DateTime endtime,string licenseplate,int maxmonth,int maxvalue,string parkname,DateTime starttime,int state)
        {
            MonthCardResult mc = new MonthCardResult
            {
                Amount = amount,
                CardID = cardid,
                Day = day,
                EndTime = endtime,
                LicensePlate = licenseplate,
                MaxMonth = maxmonth,
                MaxValue = maxvalue,
                ParkName = parkname,
                StartTime = starttime,
                State = state
            };
            return View(mc);
        }
        /// <summary>
        /// 包月缴费
        /// </summary>
        /// <param name="CardID"></param>
        /// <returns></returns>
        public ActionResult MonthCardPayment(decimal amount, string cardid, int day, DateTime endtime, string licenseplate, int maxmonth, int maxvalue, string parkname, DateTime starttime, int state)
        {
            MonthCardResult mcp = new MonthCardResult
           {
                Amount = amount,
                CardID = cardid,
                Day = day,
                EndTime = endtime,
                LicensePlate = licenseplate,
                MaxMonth = maxmonth,
                MaxValue = maxvalue,
                ParkName = parkname,
                StartTime = starttime,
                State = state,
           };
            return View(mcp);
        }
        /// <summary>
        /// 根据月卡包月数获取可用优惠券
        /// </summary>
        /// <param name="CardID"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public string GetCouponByMonth(string CardID, int month)
        {
            StringBuilder strData = new StringBuilder();
            string auth = AppUserToken;

            CouponByMonth cb = Carapi.cbm(CardID, month, auth);
            CouponByMonth coup = JsonConvert.DeserializeObject<CouponByMonth>(JsonHelper.GetJsonString(cb));
            List<CouponByMon> list = new List<CouponByMon>(coup.Result.CouponList);
            var obj = from p in list
                      select new
                      {
                          Full = p.Full.ToString(),
                          Cut = p.Cut.ToString(),
                          ReceiveTime = p.ReceiveTime.ToString("yyyy-MM-dd HH:mm:ss"),
                          DeadlineTime = p.DeadlineTime.ToString("yyyy-MM-dd HH:mm:ss"),
                          Status = (int)p.Status,
                          CouponID = p.CouponID,
                          CapPrice = p.CapPrice.ToString(),
                          CouponType = p.CouponType,
                          Brife = p.Brife,
                          CutPrice = p.CutPrice.ToString(),
                      };
            //strData.Append("{");

            strData.Append(JsonHelper.GetJsonString(obj));

            //strData.Append("}");
            return strData.ToString();
        }
        /// <summary>
        /// 立刻购买
        /// </summary>
        /// <param name="CardID"></param>
        /// <param name="month"></param>
        /// <param name="CouponID"></param>
        /// <returns></returns>
        public ActionResult Pay(string CardID, int month, DateTime starttime, DateTime endtime, string licenseplate, string Amount, string CouponID)
        {
            string auth = AppUserToken;

            if (auth.IsEmpty())
            {
                //没有登录
                //
                return RedirectToAction("Index", "ErrorPrompt", new { message = "用户登录失败" });
            }
            else if (auth == "-1")
            {
                return RedirectToAction("Register", "ParkingPayment");
            }
            //CreateCardOrder order = Carapi.order(CardID, month, CouponID,auth);
            //CreateCardOrder ordernum = JsonConvert.DeserializeObject<CreateCardOrder>(JsonHelper.GetJsonString(order));
            string afterdate = CalculateNewEndDate(starttime, endtime, month);
            //return RedirectToAction("CreateOrder", "MonthCard", new { orderno = ordernum.Result });
            return RedirectToAction("SubmitMonthRenewals", "CardRenewal", 
                new { cardId = CardID, month=month, paymoney= Amount, paytype= PaymentChannel.WeiXinPay, afterdate=afterdate, plateno= licenseplate, source=1 }
                );
        }



        private string CalculateNewEndDate(DateTime? beginDate, DateTime? endDate, int month)
        {

            TxtLogServices.WriteTxtLog(Convert.ToString(beginDate));
            TxtLogServices.WriteTxtLog(Convert.ToString(endDate));
            //TxtLogServices.WriteTxtLog(beginDate);
            DateTime start = beginDate.HasValue ? (beginDate.Value == DateTime.MaxValue ? DateTime.Now : beginDate.Value) : DateTime.Now;
            TxtLogServices.WriteTxtLog(Convert.ToString(beginDate));
            TxtLogServices.WriteTxtLog(Convert.ToString(endDate));
            return BaseCardServices.CalculateNewEndDate(start, endDate, month).ToString("yyyy-MM-dd");
        }
        //public ActionResult CreateOrder(string orderno, int type = 1, int paywpwd = -1)
        //{
        //    string auth = AppUserToken;

        //    if (auth.IsEmpty())
        //    {
        //        //没有登录
        //        //
        //        return RedirectToAction("Index", "ErrorPrompt", new { message = "用户登录失败" });
        //    }
        //    else if (auth == "-1")
        //    {
        //        return RedirectToAction("Register", "ParkingPayment");
        //    }
        //    YKOnlineOrder ono = Carapi.onor(orderno, type, paywpwd, auth);
        //    YKOnlineOrder onlineorder = JsonConvert.DeserializeObject<YKOnlineOrder>(JsonHelper.GetJsonString(ono));
        //    return View();//支付代码
        //}
    }
}
