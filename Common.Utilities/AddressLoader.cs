namespace Common.Utilities
{
    using System.Collections.Generic;
    using System.Management;
    using System.Net;
    using System.Net.Sockets;
    using System.Text.RegularExpressions;

    public class AddressLoader
    {
        /// <summary>
        /// 获取Mac地址(仅启用)
        /// </summary>
        public static IList<string> GetMacAddress() {
            IList<string> result = new List<string>();
            ManagementObjectCollection queryCollection = GetManagementCollection();
            foreach (ManagementObject mo in queryCollection) {
                if (mo["IPEnabled"].ToString().ToLower() == "true") {
                    result.Add(mo["MacAddress"].ToString());
                }
            }
            return result;
        }
        /// <summary>
        /// 获取所有Mac地址
        /// </summary>
        public static IList<string> GetAllMacAddress() {
            IList<string> result = new List<string>();
            ManagementObjectCollection queryCollection = GetManagementCollection();
            foreach (ManagementObject mo in queryCollection) {
                object macAddress = mo["MacAddress"];
                if (macAddress != null) {
                    result.Add(macAddress.ToString());
                }
            }
            return result;
        }
        private static ManagementObjectCollection GetManagementCollection() {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");
            return searcher.Get();
        }

        /// <summary>
        /// 获取局域网IP地址
        /// </summary>
        /// <returns></returns>
        public static IList<string> GetLanAddress() {
            IList<string> result = new List<string>();
            IPAddress[] addresses = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress item in addresses) {
                if (item.AddressFamily == AddressFamily.InterNetwork) {
                    result.Add(item.ToString());
                }
            }
            return result;
        }

        /// <summary>
        /// 获取广域网IP地址
        /// </summary>
        public static string GetWanAddress() {
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.Default;
            return GetWanAddress(client);
        }
        /// <summary>
        /// 获取当前服务器的IP地址
        /// 仅适用于网站
        /// </summary>
        public static string GetServerIp() {
            string result = "0.0.0.0";
            try {
                IPAddress[] addresses = Dns.GetHostAddresses(System.Web.HttpContext.Current.Request.Url.DnsSafeHost);
                foreach (IPAddress item in addresses) {
                    if (item.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) {
                        result = item.ToString();
                        break;
                    }
                }
            } catch { }
            return result;
        }
        /// <summary>
        /// 获取广域网IP地址
        /// </summary>
        public static string GetWanAddress(string host, int port, string userName, string password, string domain) {
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.Default;
            WebProxy proxy = new WebProxy(host, port);
            proxy.Credentials = new NetworkCredential(userName, password, domain);
            client.Proxy = proxy;
            return GetWanAddress(client);
        }
        private static string GetWanAddress(WebClient client) {
            string reply = client.DownloadString("http://www.ip138.com/ip2city.asp");
            Match match = Regex.Match(reply, @"\[(?<ip>\d{1,3}.\d{1,3}.\d{1,3}.\d{1,3})\]");
            if (match.Success) {
                return match.Groups["ip"].Value;
            }
            return string.Empty;
        }
        public static string GetClientIP() {
            return (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null && System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != string.Empty) ? System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] : System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        }
    }
}
