using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Base.Common;
using SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin;
using SmartSystem.OmnipotentCardWebUI.Models;
using Common.Entities.Enum;


namespace SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin
{
    public class PaymentTypeController : WeiXinController
    {
        //
        // GET: /PaymentType/
        /// <summary>
        /// 支付方式
        /// </summary>
        /// <param name="preferentialMoney">优惠金额</param>
        /// <param name="payMoney">支付金额</param>
        /// <param name="showPreferentialMoney">是否显示优惠金额</param>
        /// <returns></returns>
        public ActionResult Index(string preferentialMoney = "0", string payMoney = "0", bool showPreferentialMoney = true, string alreadyPayment="0",bool showWaitPayMoney = true)
        {
            ViewBag.PreferentialMoney = preferentialMoney;
            ViewBag.PayMoney = payMoney;
            ViewBag.ShowWaitPayMoney = showWaitPayMoney;
            ViewBag.ShowPreferentialMoney = showPreferentialMoney;
            ViewBag.AlreadyPayment = alreadyPayment;
            ViewBag.SourceClient = SourceClient;
            //#if DEBUG
            //    ViewBag.SourceClient = RequestSourceClient.WeiXin;
            //#endif
            return PartialView();
        }
    }
}
