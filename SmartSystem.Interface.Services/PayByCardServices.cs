using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartSystem.Interface.Entities;
using System.Configuration;

namespace SmartSystem.Interface.Services
{
    /// <summary>
    /// 刷卡支付
    /// </summary>
    public class PayByCardServices
    {
        private static string Secretkey = ConfigurationManager.ConnectionStrings["Secretkey"].ToString();
        public static ReturnResult Pay(RequestData model,ReturnResult result)
        {
            result.sign = GetReturnResultSign(model, result);
            return result;
        }
        private static string GetReturnResultSign(RequestData model,ReturnResult result)
        {
            Dictionary<string, string> paraMap = new Dictionary<string, string>();
            if (model.access_code == "1bb8c53acc1f72948a47d6e0edf0f355")
            {
                paraMap.Add("key", Secretkey);
            }
            paraMap.Add("return_code", result.return_code);
            paraMap.Add("return_msg", result.return_msg);
            paraMap.Add("err_code_des", result.err_code_des);
            paraMap.Add("access_code", result.access_code);
            paraMap.Add("business_code", result.business_code);
            paraMap.Add("nonce_str", result.nonce_str);
            paraMap.Add("sign", result.sign);
            paraMap.Add("sign_type", model.sign);
            paraMap.Add("result_code", result.result_code);
            paraMap.Add("err_code", result.err_code);
            paraMap.Add("data", result.data);
            return SignatureServices.Signature(paraMap);
        }
    }
}
