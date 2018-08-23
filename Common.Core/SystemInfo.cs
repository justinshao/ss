using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Management;

namespace Common.Core
{
    public class SystemInfo
    {
        /// <summary>
        /// 查询CPU编号
        /// </summary>
        /// 
        [DllImport("kernel32.dll", EntryPoint = "GetSystemDefaultLCID")]
        public static extern int GetSystemDefaultLCID();

        [DllImport("kernel32.dll", EntryPoint = "SetLocaleInfoA")]
        public static extern int SetLocaleInfo(int Locale, int LCType, string lpLCData);
        public const int LOCALE_SLONGDATE = 0x20;
        public const int LOCALE_SSHORTDATE = 0x1F;
        public const int LOCALE_STIME = 0x1003;

        public static void SetDateTimeFormat()
        {
            try
            {
                int x = GetSystemDefaultLCID();
                SetLocaleInfo(x, LOCALE_STIME, "HH:mm:ss");        //时间格式
                SetLocaleInfo(x, LOCALE_SSHORTDATE, "yyyy-MM-dd");   //短日期格式  
                SetLocaleInfo(x, LOCALE_SLONGDATE, "yyyy-MM-dd");   //长日期格式 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public string GetCpuId()
        {
            ManagementClass mClass = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = mClass.GetInstances();
            string cpuId = null;
            foreach (ManagementObject mo in moc)
            {
                cpuId = mo.Properties["ProcessorId"].Value.ToString();
                break;
            }
            return cpuId;
        }

        /// <summary>
        /// 查询硬盘编号
        /// </summary>
        public string GetMainHardDiskId()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
                String strHardDiskID = null;
                foreach (ManagementObject mo in searcher.Get())
                {
                    strHardDiskID = mo["SerialNumber"].ToString().Trim();
                    break;
                }
                return strHardDiskID;
            }
            catch
            {
                return "";
            }
        }

        public static string ImgPath { set; get; }
        public static string ImgServerIP { set; get; }
        public static string OnlineIP { set; get; }
        public static string ProxyNo { set; get; }
        public static bool IsLoacl = false;
        public static bool IsUpDate = false;
        public static string OnlineIPxj { set; get; }
        public static bool IsUpDatexj { set; get; }
        public static string JZUser { set; get; }
        public static string XTJC { set; get; }

        public static bool OnlineIPsz { set; get; }
        public static bool IsUpDatesz { set; get; }
        public static string RZM { set; get; }
        public static string TCCID { set; get; }
        public static string GKJIP { set; get; }
       
        public static string JGJIP { set; get; }

        #region 辉通
        public static bool IsUpDateht { set; get; }
        public static string HTJKDZ { set; get; }
        public static string HTHZSID { set; get; }
        public static string HTSecretID{set;get;}
        #endregion

        #region 中兴
        public static bool IsUpDateZx{set;get;}
        public static string ZXJKDZ{set;get;}
        public static string ZXAPPID{set;get;}
        public static string ZXSecretID{set;get; }
      
        #endregion

        #region 上海交管局
        public static int Seqno = 0;
        public static bool IsUpDateSHJGJ;
        public static string SHJGJIP ;
        public static string SHJGJPort;
        public static string ClientId ;
        public static string SHJCPTIP;
        public static string SHJCPTDK;
        #endregion

        #region 成都定制
        public static bool IsUpDatecd;
        public static string CDIP;
        #endregion

        #region 北青
        public static bool IsUpDateBQ;
        public static string BQJK;
        #endregion

        #region 大汉
        public static string PKNo;
        public static string Topic { set; get; }
        public static string ConsumerId { set; get; }
        public static string DHWYGY { set; get; }
        public static string DHWYSY { set; get; }
        #endregion

        #region 凯达尔
        public static string AppId { set; get; }
        public static string SecretKey { set; get; }
        public static string UserName { set; get; }
        public static string Password { set; get; }
        public static string AccessToken { set; get; }
        public static string RefreshToken { set; get; }
        #endregion

        #region 新疆小区
        public static bool IsUpDateXJXQ;
        public static string XJXQJK;
        public static string AreaCode;
        public static string StationId;
        public static string StationName;
        public static int Placetype;
        public static string XJXQTP;
        public static string Dareaname;
        public static string ZDKX;
        public static string ZDKY;
        #endregion

        #region 新疆三张网
        public static bool IsUpDateXJSZW;
        public static string XJSZWJK;
        public static string CustomID;
        public static string CustomPwd;
        #endregion

        #region 新疆内保
        public static bool IsUpDateXJNB;
        public static string XJNBIP;
        public static string XJNBapp_Key;
        public static string XJNBSecretID;
        #endregion

        #region 新疆华奥

        public static bool IsUpDateXJHA;
        public static string XJHAIP;
        public static string XJHAapp_Key;
        public static string XJHASecretID;

        #endregion

        #region 拉风
        public static string _accessToken;
        public static int _ReqTS;
        #endregion
    }
}
