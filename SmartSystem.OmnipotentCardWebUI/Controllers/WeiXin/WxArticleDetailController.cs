using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Services.WeiXin;
using Common.Entities.WX;
using Common.Entities;
using Common.Services;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin
{
    public class WxArticleDetailController : Controller
    {
        public ActionResult Index(string openId, int articleId)
        {
            try
            {
                WX_Article article = WXArticleServices.QueryById(articleId);
                if (article == null)
                {
                    return RedirectToAction("NotPage", "ErrorPrompt", new { message = "获取微信基础信息失败" });
                }
                return View(article);
            }
            catch (Exception ex) {
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPageError", "Index方法处理异常", ex, LogFrom.WeiXin);
                return RedirectToAction("NotPage", "ErrorPrompt", new { message = "获取数据异常" });
            }
        }

    }
}
