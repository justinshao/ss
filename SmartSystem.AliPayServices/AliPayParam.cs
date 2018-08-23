using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartSystem.AliPayServices
{
    public class AliPayParam
    {
        public static string serverUrl = "https://openapi.alipay.com/gateway.do";
        public static string authUrl = "https://openauth.alipay.com/oauth2/publicAppAuthorize.htm";
        public static string charset = "GBK";
        public static string format = "json";
        public static string version = "1.0";
    }
}
