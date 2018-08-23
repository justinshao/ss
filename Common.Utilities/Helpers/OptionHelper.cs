using Common.Core.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace Common.Utilities.Helpers
{
    /// <summary>
    /// 配置文件操作类
    /// </summary>
    public class OptionHelper
    {
      
        private static OptionHandler _Handler = new OptionHandler("Option.config");
         
        /// <summary>
        /// 数据库连接
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                return AES.Decrypt(ReadString("ConnectionStrings", "CurrentConnection", ""));
            }
            set
            {
                WriteString("ConnectionStrings", "CurrentConnection",  AES.Encrypt(value));
            }
        }
       
        public static string ReadString(string section, string name, string defaultValue)
        {
            return _Handler.ReadString(section, name, defaultValue);
        }

        public static int ReadInt(string section, string name, int defaultValue)
        {
            return _Handler.ReadInt(section, name, defaultValue);
        }
        public static bool ReadBool(string section, string name, bool defaultValue)
        {
            return _Handler.ReadBool(section, name, defaultValue);
        }


        public static bool WriteBool(string section, string name, bool defaultValue)
        {
            return _Handler.WriteBool(section, name, defaultValue);
        }
        public static bool WriteInt(string section, string name, int defaultValue)
        {
            return _Handler.WriteInt(section, name, defaultValue);
        }

        public static bool WriteString(string section, string name, string defaultValue)
        {
            return _Handler.WriteString(section, name, defaultValue);
        }
        /// <summary>
        /// 获取数据库连接中的项
        /// </summary>
        /// <param name="tag">项标识</param>
        /// <returns>项值</returns>
        private static string GetConnectionStringItem(string tag)
        {
            string[] array = OptionHelper.ConnectionString.Split(';');
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].IndexOf(tag) > -1)
                {
                    return array[i].Substring(array[i].IndexOf("=") + 1);
                }
            }
            return "";
        }

        /// <summary>
        /// 数据源
        /// </summary>
        public static string DataSource
        {
            get
            {
                return GetConnectionStringItem("Data Source");
            }
        }

        /// <summary>
        /// 数据库
        /// </summary>
        public static string Database
        {
            get
            {
                return GetConnectionStringItem("Initial Catalog");
            }
        }

        /// <summary>
        /// 登陆名
        /// </summary>
        public static string LoginName
        {
            get
            {
                return GetConnectionStringItem("User ID");
            }
        }

        /// <summary>
        /// 密码
        /// </summary>
        public static string Password
        {
            get
            {
                return GetConnectionStringItem("Password");
            }
        }


        #region MySql

        /// <summary>
        /// 获取数据库连接中的项
        /// </summary>
        /// <param name="tag">项标识</param>
        /// <returns>项值</returns>
        private static string GetMySqlConnectionStringItem(string tag)
        {
            string[] array = OptionHelper.ConnectionString.Split(';');
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].IndexOf(tag) > -1)
                {
                    return array[i].Substring(array[i].IndexOf("=") + 1);
                }
            }
            return "";
        }

        /// <summary>
        /// 数据源
        /// </summary>
        public static string MySqlDataSource
        {
            get
            {
                return GetConnectionStringItem("server");
            }
        }

        /// <summary>
        /// 数据库
        /// </summary>
        public static string MySqlDatabase
        {
            get
            {
                return GetConnectionStringItem("database");
            }
        }

        /// <summary>
        /// 登陆名
        /// </summary>
        public static string MySqlLoginName
        {
            get
            {
                return GetConnectionStringItem("User Id");
            }
        }

        /// <summary>
        /// 密码
        /// </summary>
        public static string MySqlPassword
        {
            get
            {
                return GetConnectionStringItem("password");
            }
        }
        /// <summary>
        /// 端口
        /// </summary>
        public static uint MySqlPort
        {
            get
            {
                var port = (uint)GetConnectionStringItem("port").ToInt();
                return port == 0 ? 3306 : port;
            }
        }
        #endregion
    }
}
