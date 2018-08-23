using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Common.Utilities
{
    public class ClientIpHelper
    {
        public static string GetClientIp() {
            var context = HttpContext.Current;
            if (context == null) return string.Empty;
            var request = context.Request;
            if (request == null) return string.Empty;
            var variables = request.ServerVariables;
            if (variables == null) return request.UserHostAddress;
            var forwarded = variables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(forwarded))
                return forwarded.Split(',')[0];
            var remoteAddr = variables["REMOTE_ADDR"];
            if (!string.IsNullOrEmpty(remoteAddr))
                return remoteAddr;
            return request.UserHostAddress;
        }
    }
}
