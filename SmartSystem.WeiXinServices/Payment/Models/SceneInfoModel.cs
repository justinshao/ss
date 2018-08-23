using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartSystem.WeiXinServices.Payment.Models
{
    public class SceneInfoModel
    {
        //WAP网站应用
//{"h5_info": {"type":"Wap","wap_url": "https://pay.qq.com","wap_name": "腾讯充值"}} 
        public SceneInfo h5_info { get; set; }
    }
    public class SceneInfo {
        public string type { get; set; }
        /// <summary>
        /// 域名
        /// </summary>
        public string wap_url { get; set; }
        /// <summary>
        /// 应用名称
        /// </summary>
        public string wap_name { get; set; }
    }
}
