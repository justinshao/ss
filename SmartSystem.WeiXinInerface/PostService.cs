using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using SmartSystem.WeiXinInerface.WXService;
using Common.Utilities.Helpers;
using Common.Entities.WX;
using Common.Entities.Other;

namespace SmartSystem.WeiXinInerface
{
    public class PostService
    {
        /// <summary>
        /// 发送客户端通知
        /// </summary>
        /// <param name="sellerID"></param>
        /// <param name="VID"></param>
        /// <returns></returns>
        public static void SendNotify(string title,string text)
        {
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            client.SendNotify(title, text);
            client.Close();
            client.Abort(); 
        }
    }
}
