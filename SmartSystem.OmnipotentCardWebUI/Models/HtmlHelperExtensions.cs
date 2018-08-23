using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.Mvc;

namespace System.Runtime.CompilerServices
{
    public class ExtensionAttribute : Attribute { }
}
namespace SmartSystem.OmnipotentCardWebUI.Models
{
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// 从配置文件中读取版本号（为了统一管理脚本和样式表的版本）
        /// </summary>
        private static readonly string fileVersion = ConfigurationManager.AppSettings["FileVersion"] ?? DateTime.Now.ToString("yyMMddHHmmss");

        /// <summary>
        ///     引用脚本方法
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static MvcHtmlString ReferenceScriptHtml(this HtmlHelper htmlHelper, string path)
        {
            return MvcHtmlString.Create(string.Format("<script type=\"text/javascript\" src=\"{0}?v={1}\"></script>\r\n", path, fileVersion));
        }

        /// <summary>
        ///  引用样式表方法
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static MvcHtmlString ReferenceCssHtml(this HtmlHelper htmlHelper, string path)
        {
            return MvcHtmlString.Create(string.Format("<link href=\"{0}?v={1}\" rel=\"stylesheet\">", path, fileVersion));
        }
    }
}