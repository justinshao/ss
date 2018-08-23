using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.Interface.Entities;
using Common.Utilities.Helpers;
using Common.Services;
using Common.Entities;
using SmartSystem.Interface.Services;

namespace SmartSystem.OmnipotentCardWebUI.Controllers
{
    public class WebApiController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                string data = Request.Params["data"];
                TxtLogServices.WriteTxtLogEx("WebApi", "data:{0}", data);
                RequestData model = JsonHelper.GetJson<RequestData>(data);
                bool signatureResult =SignatureServices.CheckSignature(model);
                if (!signatureResult) throw new InterfaceException("签名验证失败");

                ReturnResult result = new ReturnResult();
                result.return_code = "SUCCESS";
                switch (model.business_code) {
                        //扫码枪支付
                    case "SMQPAY": {
                       
                        break;
                    }
                    default: throw new InterfaceException("未知接口类型");
                }
                return Content("");
            }
            catch (InterfaceException ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("WebApi", "接口调用失败", ex, LogFrom.WeiXin);
                string result = JsonHelper.GetJsonString(new ReturnResult()
                {
                    return_code = "FAIL",
                    return_msg = ex.Message
                });
                return Content(result);
            }
            catch (Exception ex) {
                ExceptionsServices.AddExceptionToDbAndTxt("WebApi", "接口调用失败", ex, LogFrom.WeiXin);
                string result = JsonHelper.GetJsonString(new ReturnResult()
                {
                    return_code = "FAIL",
                    return_msg="未知异常"
                });
                return Content(result);
            }
        }

    }
}
