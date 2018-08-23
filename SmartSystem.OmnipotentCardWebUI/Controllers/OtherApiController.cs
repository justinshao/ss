using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.OmnipotentCardWebUI.Models;
using Common.Entities;
using Common.Services;
using Common.Entities.WX;
using Common.Services.WeiXin;
using SmartSystem.WeiXinServices;
using Common.Entities.Validation;
using Common.Entities.Other;
using Common.Utilities.Helpers;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin
{
    /// <summary>
    /// 其他API
    /// </summary>
    public class OtherApiController : Controller
    {
        public ActionResult Index()
        {
            //bool isSuc = TemplateMessageServices.SendParkIn("cd08c4e1-535d-4655-b54f-a8650124a713", "浙B22222", "微软大厦", "2018-04-24 13:00:00", "oZKcm0wNr9mCPqwJHIKPsgWUbf0Y");

            return View();
        }

        //[HttpPost]
        public string SendParkingInNotify(string cmd, string plateNo, string cpid, string pkname, string indate)
        {
            if (cmd.IsEmpty())
            {
                return "-4";
            }

            WX_ApiConfig config = WXApiConfigServices.QueryWXApiConfig(cpid);
            if (config == null)
            {
                return "-1";
            }
            if (!config.Status)
            {
                return "-2";
            }

            WX_Info user = WXUserServices.GetWXInfoByPlateNo(plateNo);
            if (user == null)
            {
                return "-3";
            }

            if (cmd == "In")
            {
                bool isSuc = TemplateMessageServices.SendParkIn(config.CompanyID, plateNo, pkname, indate, user.OpenID);
                if (isSuc)
                {
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            else if (cmd == "Out")
            {

            }

            return "";
        }

        //public string SendParkingOutNotify(string cmd, string plateNo, string cpid,
        //                                string pkname, string indate, string outdate, string durtime, string amount, string app)
        //{
        //    return SendParkingOutNotify(cmd,plateNo,cpid,pkname,indate,outdate,durtime,"",amount,app);
        //}

        //[HttpPost]
        public string SendParkingOutNotify(string cmd, string plateNo, string cpid, 
                                        string pkname, string indate,string outdate,string durtime,string payType, string amount,string app)
        {
            if (cmd.IsEmpty())
            {
                return "-4";
            }

            WX_ApiConfig config = WXApiConfigServices.QueryWXApiConfig(cpid);
            if (config == null)
            {
                return "-1";
            }
            if (!config.Status)
            {
                return "-2";
            }

            WX_Info user = WXUserServices.GetWXInfoByPlateNo(plateNo);
            if (user == null)
            {
                return "-3";
            }

            if (cmd == "In")
            {
                
            }
            else if (cmd == "Out")
            {
                bool isApp = app == "1" ? true : false;

                if (amount.EndsWith("元"))
                {
                    amount = amount + "元";
                }

                if (payType.IsEmpty())
                {
                    //默认
                    payType = "APP支付";
                }

                bool isSuc = TemplateMessageServices.SendParkOut(config.CompanyID, plateNo, pkname, indate, outdate,durtime, payType, amount, user.OpenID, isApp);
                if (isSuc)
                {
                    return "1";
                }
                else
                {
                    return "0";
                }
            }

            return "";
        }


        public string SendParkingFees(string sData)
        {
            TempParkingFeeResult model = JsonHelper.GetJson<TempParkingFeeResult>(sData);

            string plateNo = model.PlateNumber;
            //string rid = model.Pkorder.TagID;
            string pid = model.ParkingID;
            //string bid = model.BoxID;
            //string gid = model.GateID;
            model.PayDate = DateTime.Now; //统一服务器时间

            string sKey = plateNo + pid;
            bool bSuc = WeiXinInerface.ParkingFeeService.SetParkingFee(sKey, model);

            if (bSuc)
            {
                return "1";
            }

            return "0";
        }

    }
}
