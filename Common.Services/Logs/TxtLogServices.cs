using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web.Hosting;
using System.Xml.Linq;
using Common.DataAccess;
using System.Data;
using System.Web;
using System.Web.SessionState;
using ClassLibrary1;

namespace Common.Services
{
    public class TxtLogServices
    {
        static readonly object Lock = new object();
        /// <summary>
        /// 记录日志,文件名为yyyyMMdd.log
        /// </summary>
        /// <param name="format">日志内容</param>
        /// <param name="args">参数</param>
        public static void WriteTxtLog(string format, params object[] args)
        {
            WriteTxtLogEx(DateTime.Now.ToString("yyyyMMdd"), format, args);
        }

        /// <summary>
        /// 写日志,以天为单位建立文件夹,后跟文件名
        /// </summary>
        /// <param name="filename">日志文件名</param>
        /// <param name="format">日志内容</param>
        /// <param name="args">参数</param>
        public static void WriteTxtLogEx(string filename, string format, params object[] args)
        {


            //解析XML--renjiang 
            //if (filename.Equals("Api_Post_Request"))
            //{
            //    XElement xdoc = XElement.Parse(format);


            //    var toUserName = xdoc.Element("ToUserName").Value;
            //    var fromUserName = xdoc.Element("FromUserName").Value;
            //    var createTime = xdoc.Element("CreateTime").Value;
            //    var key = string.Empty;
            //    if (xdoc.Element("EventKey") != null)
            //    {
            //        key = xdoc.Element("EventKey").Value;
            //    }

            //    //var content = xdoc.Element("Ticket").Value;
            //    if (!string.IsNullOrEmpty(key))
            //    {
            //        string strSql = "select AppId,AppSecret from WX_ApiConfig"; string appid = ""; string appsecret = "";
            //        using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            //        {
            //            DataTable dt = dboperator.ExecuteTable(strSql, 30000);
            //            if (dt.Columns.Count > 0)
            //            {
            //                appid = dt.Rows[0][0].ToString().Trim();
            //                appsecret = dt.Rows[0][1].ToString().Trim();
            //            }
            //        }

            //        DateTime datatime = DateTime.Now;
            //        //string token = string.Empty;
            //        //if (!string.IsNullOrEmpty(staticValue.token))
            //        //{
            //        //    Errcode er = wxApi.Token(staticValue.token);
            //        //    if (er.Errcodes == 42001)
            //        //    {
            //        //        token = wxApi.GetToken(appid, appsecret).Accesstoken;
            //        //    }
            //        //    else
            //        //    {
            //        //        token = staticValue.token.Trim();
            //        //    }
            //        //}
            //        //else
            //        //{
            //        //    token = wxApi.GetToken(appid, appsecret).Accesstoken;
            //        //}
            //        //userin User = wxApi.GetNickname(wxApi.GetToken(appid, appsecret).Accesstoken, fromUserName);
            //        //string createSql = "insert into TgCount values('" + key + "','" + createTime + "','" + User.Nickname + "','" + toUserName + "','" + datatime + "')";
            //        //using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            //        //{
            //        //    dboperator.ExecuteNonQuery(createSql, 30000);
            //        //    //staticValue.token = token;
            //        //}

            //    }

            //}


            if (string.IsNullOrWhiteSpace(filename))
            {
                return;
            }
            lock (Lock)
            {
                try
                {
                    var body = args.Length > 0 ? string.Format(format, args) : format;
                    body = string.Format("{0:HH:mm:ss fff}\t{1}", DateTime.Now, body);
                    var logPath = GetLogPath();
                    if (string.IsNullOrEmpty(logPath))
                        return;
                    logPath = Path.Combine(logPath, DateTime.Now.ToString("yyyyMMdd"));
                    if (!Directory.Exists(logPath))
                    {
                        Directory.CreateDirectory(logPath);
                    }
                    logPath = Path.Combine(logPath, filename + ".log");
                    using (var sw = File.AppendText(logPath))
                    {
                        sw.WriteLine(body);
                    }
                }
                catch (Exception ex)
                {
                    //ExceptionsServices.(ex, "记录文本日志出错");
                }
            }
        }
        public static void WriteTxtLogEx(string filename, Exception ex)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                return;
            }
            WriteTxtLogEx(filename, string.Format("错误描述:{0},堆栈信息：{1}", ex.Message, ex.StackTrace));
        }
        public static void WriteTxtLogEx(string filename, string description, Exception ex)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                return;
            }
            WriteTxtLogEx(filename, string.Format("错误描述:{0}；{1},堆栈信息：{2}", description, ex.Message, ex.StackTrace));
        }
        private static string _logPath;
        private static string GetLogPath()
        {
            if (_logPath == null)
            {
                _logPath = HostingEnvironment.MapPath("~/Logs/");
                if (string.IsNullOrEmpty(_logPath))
                    _logPath = string.Empty;
            }
            return _logPath;
        }

        private static void SetLogPath(string logPath)
        {
            _logPath = logPath;
        }
    }
}
