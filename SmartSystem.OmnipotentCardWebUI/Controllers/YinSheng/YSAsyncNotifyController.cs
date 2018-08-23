using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YinSheng.Pay;
using Common.Services;
using Common.Entities;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.YinSheng
{
    /// <summary>
    /// 银盛异步通知
    /// </summary>
    public class YSAsyncNotifyController : Controller
    {
        public void Index()
        {
            try
            {
                PayDictionary queryArray = YinShengCommon.TransQueryString(Request.Form.ToString());
                queryArray.Sort(PaySortEnum.Asc);//异步验证签名需要配需
                if (queryArray["sign"] == null)
                {
                    Response.Write("缺少签名参数");
                    Response.End();
                }
                string sign = HttpUtility.UrlDecode(queryArray["sign"]);
                queryArray.Remove("sign");
                string parStr = queryArray.GetParmarStr();
                if (YinShengCommon.SignVerify(parStr, sign))
                {
                    Response.Write("success");
                    Response.End();
                }
                else
                {
                    Response.Write("签证签名不一致");
                    Response.End();
                }
            }
            catch (Exception ex) {
                ExceptionsServices.AddExceptionToDbAndTxt("YSAsyncNotify", "支付通知出错", ex, LogFrom.WeiXin);
                Response.Write("error");
                Response.End();
            }
           
        }

    }
}
