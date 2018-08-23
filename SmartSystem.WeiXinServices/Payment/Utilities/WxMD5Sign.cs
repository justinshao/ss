using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartSystem.WeiXinServices.Payment
{
    public class WxMD5Sign
    {
        public static string Sign(string content, string key)
        {
            string signStr = "";

            if ("" == key){
                throw new MyException("财付通签名key不能为空！");
            }
            if ("" == content){
                throw new MyException("财付通签名内容不能为空");
            }
            signStr = content + "&key=" + key;
            return MD5Util.MD5(signStr).ToUpper();
        }

        public static bool VerifySignature(string content, string sign, string md5Key)
        {
            String signStr = content + "&key=" + md5Key;
            String calculateSign = MD5Util.MD5(signStr).ToUpper();
            String tenpaySign = sign.ToUpper();
            return (calculateSign == tenpaySign);
        }
    }
}
