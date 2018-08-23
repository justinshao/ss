using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartSystem.Interface.Entities;
using System.Configuration;
using Common.Core.Security;
using Common.Entities;

namespace SmartSystem.Interface.Services
{
    public class SignatureServices
    {
        /// <summary>
        /// 获取签名（值为空不参与签名）
        /// </summary>
        /// <param name="paraMap"></param>
        /// <param name="urlencode"></param>
        /// <returns></returns>
        public static string Signature(Dictionary<string, string> paraMap)
        {
            string buff = "";
            var result = from pair in paraMap orderby pair.Key select pair;
            foreach (KeyValuePair<string, string> pair in result)
            {
                if (pair.Key != "sign" && pair.Value != null && pair.Value.ToString() != "")
                {
                    buff += pair.Key + "=" + pair.Value + "&";
                }
            }
            buff = buff.Trim('&');
            return MD5.Encrypt(buff);
        }
        
        public static bool CheckSignature(RequestData model) {
            Dictionary<string, string> paraMap = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(model.access_code))
                throw new InterfaceException("access_code参数错误");

            if (model.access_code == "1bb8c53acc1f72948a47d6e0edf0f355")
            {
                paraMap.Add("key", SystemDefaultConfig.Secretkey);
            }

            paraMap.Add("access_code", model.access_code);

            if (string.IsNullOrWhiteSpace(model.business_code))
                throw new InterfaceException("business_code参数错误");
            paraMap.Add("business_code", model.business_code);

            if (string.IsNullOrWhiteSpace(model.nonce_str))
                throw new InterfaceException("nonce_str参数错误");
            paraMap.Add("nonce_str", model.nonce_str);

            if (string.IsNullOrWhiteSpace(model.sign_type))
                throw new InterfaceException("sign_type参数错误");
            paraMap.Add("sign_type", model.sign_type);

            if (string.IsNullOrWhiteSpace(model.sign))
                throw new InterfaceException("sign参数错误");
            paraMap.Add("sign", model.sign);

            paraMap.Add("data",model.data);
            if (Signature(paraMap) == paraMap["sign"]) {
                return true;
            }
            return false;
        }
    }
}
