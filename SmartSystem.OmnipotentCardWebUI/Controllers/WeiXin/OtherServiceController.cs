using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.OmnipotentCardWebUI.Models;
using Common.Entities;
using Common.Services;
using Common.Entities.WX;
using Common.Services.WeiXin;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin
{
    /// <summary>
    /// 其他服务
    /// </summary>
    [PageBrowseRecord]
    [CheckWeiXinPurview(Roles = "Login")]
    public class OtherServiceController : WeiXinController
    {
        /// <summary>
        /// 常见问题
        /// </summary>
        /// <returns></returns>
        public ActionResult OperatExplanation()
        {
            return View();
        }
        /// <summary>
        /// 关于我们
        /// </summary>
        /// <returns></returns>
        public ActionResult Contact() {
            return View();
        }
        /// <summary>
        /// 意见反馈
        /// </summary>
        /// <returns></returns>
        public ActionResult ProblemFeedback() {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveProblem(string content) {
            try
            {
                if (string.IsNullOrWhiteSpace(content))
                {
                    return PageAlert("ProblemFeedback", "OtherService", new { RemindUserContent = "内容不能为空" });
                }
                WX_OpinionFeedback model = new WX_OpinionFeedback();
                model.OpenId = WeiXinUser.OpenID;
                model.FeedbackContent = content;
                model.CompanyID = WeiXinUser.CompanyID;
                WXOpinionFeedbackServices.Create(model);
                return PageAlert("ProblemFeedback", "OtherService", new { RemindUserContent = "我们已收到您的反馈信息，我们会尽快对您的问题进行处理" });
            }
            catch (Exception ex) {
                ExceptionsServices.AddExceptionToDbAndTxt("WeiXinPageError", "保存问题反馈异常", ex, LogFrom.WeiXin);
                return PageAlert("ProblemFeedback", "OtherService", new { RemindUserContent = "保存反馈信息异常" });
            }
        }
    }
}
