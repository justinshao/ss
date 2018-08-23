using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;

namespace Common.Entities
{
    public static class SystemDefaultConfig
    {
        /// <summary>
        /// 数据修改默认值
        /// </summary>
        private static int _dataUpdateFlag = 0;

        public static int DataUpdateFlag
        {
            get { return _dataUpdateFlag; }
            set { _dataUpdateFlag = value; }
        }
        private static string _systemDomain;
        /// <summary>
        /// 系统域名
        /// </summary>
        public static string SystemDomain {
            get {
                if (!string.IsNullOrWhiteSpace(_systemDomain)) {
                    return _systemDomain.TrimEnd('/');
                }
                return string.Empty;
            }
            set { _systemDomain = value; }
        }
        /// <summary>
        /// 当前使用的数据库
        /// </summary>
        private static string _databaseProvider = "Sql";
        public static string DatabaseProvider
        {
            get { return _databaseProvider; }
            set { _databaseProvider = value; }
        }
    
        private static string _writeConnectionString = "";
        private static string _readConnectionString = "";
        public static string WriteConnectionString
        {
            get { return _writeConnectionString; }
            set { _writeConnectionString = value; }
        }
        public static string ReadConnectionString
        {
            get { return _readConnectionString; }
            set { _readConnectionString = value; }
        }
        public static void CreateImageUploadFile() {
            try
            {
                string absolutePath = System.Web.HttpContext.Current.Server.MapPath("~/Pic");

                if (!Directory.Exists(absolutePath))
                    Directory.CreateDirectory(absolutePath);
            }
            catch { 
            
            }
        
        }
        public static LogFrom LogFrom { set; get; }
        public static string OperatorUserAccount { set; get; }

        public static bool IsCS = false;
        /// <summary>
        /// 泊物云车场编号（非标）
        /// </summary>
        public static string BWPKID { set; get; }
        public static string Secretkey { get; set; }
        /// <summary>
        /// 赛菲姆车场编号（非标）
        /// </summary>
        public static string SFMPKID { set; get; }
    }
}
