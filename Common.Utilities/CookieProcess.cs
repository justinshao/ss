using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Common.Utilities
{
    public class CookieProcess
    {
        #region  创建COOKIE
        /// <summary>
        /// 创建COOKIE
        /// Key COOKIE名称，yk55 值
        /// </summary>
        public static void Set_Cookie(string Key, string yk55)
        {
            if (HttpContext.Current.Response.Cookies[Key] == null)//cookie不存在，添加
            {
                HttpCookie cookie = new HttpCookie(Key);
                cookie.Value = yk55;
                cookie.Expires = DateTime.Now.AddDays(7);
                HttpContext.Current.Response.Cookies.Add(cookie);

            }
            else
            {
                HttpContext.Current.Response.Cookies[Key].Value = yk55;
                HttpContext.Current.Response.Cookies[Key].Expires = DateTime.Now.AddDays(60);
            }
        }
        #endregion

        #region  取出COOKIE
        /// <summary>
        /// 取出COOKIE
        /// Key COOKIE名称
        /// </summary>
        public static string Get_Cookie(string Key)
        {
            try
            {
                return System.Web.HttpContext.Current.Request.Cookies[Key].Value.ToString();
            }
            catch
            {
                return "";
            }
        }
        #endregion
    }
}
