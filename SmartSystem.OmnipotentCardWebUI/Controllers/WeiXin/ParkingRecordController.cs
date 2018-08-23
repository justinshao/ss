using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.OmnipotentCardWebUI.Models;
using System.Net;
using SmartSystem.WeiXinBase;
using Newtonsoft.Json;
using System.Collections.Specialized;
using ClassLibrary1;
using Common.Entities.Order;
using Common.Utilities.Helpers;
using System.Text;


namespace SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin
{
    [PageBrowseRecord]
    [CheckWeiXinPurview(Roles = "Login")]
    public class ParkingRecordController : WeiXinController
    {
        
        public ActionResult Index()
        {
            
            bool number = false;
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
            return RedirectToAction("GetParkingList", "ParkingRecord", new { status = -1, number = number });
        }
        /// <summary>
        /// 查询停车记录
        /// </summary>
        /// <returns></returns>
        public ActionResult GetParkingList(int status,bool number = false)
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
                int pageIndex = 1;
                int pageSize = 35;
                
                ParkingList pklist = ParkingApi.bb(status, pageIndex, pageSize, auth);
                ParkingList pk = JsonConvert.DeserializeObject<ParkingList>(JsonHelper.GetJsonString(pklist));
                return View(pk);
            }
            catch (Exception e)
            {
                return View();
            }
            
        }
        public ActionResult CXOrder(string orderid, int type)
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
                ParkingDetail pkd = ParkingApi.cc(orderid, type, auth);
                //Session[token]
                ParkingDetail pk = JsonConvert.DeserializeObject<ParkingDetail>(JsonHelper.GetJsonString(pkd));
                return View(pk);
            }
            catch 
            {
                return View();
            }
        }
    }
}
      
