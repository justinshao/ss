using System;
using System.Net;


using System.Collections.Generic;
using System.Configuration;

namespace SmartSystem.WeiXinInerface
{
    public static class Options
    {
        static Options()
        { 
        }
          

        /// <summary>
        /// 服务地址
        /// </summary>
        public static string ServiceHost = ConfigurationManager.AppSettings["WeiXinServiceIP"].ToString();
        /// <summary>
        /// 以Net.Tcp公布的服务端口
        /// </summary>
        public static int NetTcpPort = 4508;

        /// <summary>
        /// 以Http 端口
        /// </summary>
        public static int BasicHttpPort = 80;

        /// <summary>
        /// 获取服务的方式:Net.Tcp或BasicHttp
        /// </summary>
        //public static string Binding = "Net.Tcp";

        /// <summary>
        /// 获取服务的方式:Net.Tcp或BasicHttp
        /// </summary>
        public static string Binding = "BasicHttp";  
    } 
}
