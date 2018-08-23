using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.Interface.Services;
using Common.Services;
using Common.Entities;
using Common.Entities.WX;
using Common.Entities.Enum;
using SmartSystem.WeiXinInerface;
using Common.Services.WeiXin;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.Html5
{
    /// <summary>
    /// 请求入口
    /// </summary>
    public class H5RequestController : Controller
    {
        /// <summary>
        /// APP请求入口 cid:单位编号 mp:手机号 sign:签名MD5
        /// </summary>
        /// <param name="id">cid=11^mp=187111111111^sign=2222222</param>
        /// <returns></returns>
        public ActionResult Index(string id)
        {
            try
            {
                Dictionary<string, string> requestParams = CheckRequest(id);
                Login(requestParams);
                return RedirectToAction("Index", "H5Home");
            }
            catch (MyException ex)
            {
                return RedirectToAction("Error", "ErrorPrompt", new { message = ex.Message });
            }
            catch (Exception ex) {
                TxtLogServices.WriteTxtLogEx("H5RequestError", ex);
                return RedirectToAction("Error", "ErrorPrompt", new { message = "未知异常" });
            }
        }
        public Dictionary<string, string> CheckRequest(string id)
        {
            Dictionary<string, string> requestParams = new Dictionary<string, string>();
            var separator = new[] { '^' };//^参数分隔符
            var ids = id.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in ids)
            {
                var parame = item.Split(new[] { '=' });
                if (parame.Length < 2) throw new MyException("参数出现错误，请联系管理员");

                requestParams.Add(parame[0], parame[1]);
            }
            if (!requestParams.ContainsKey("cid")) throw new MyException("获取单位信息失败");
            if (!requestParams.ContainsKey("sign")) throw new MyException("获取签名失败");

            BaseCompany company = CompanyServices.QueryCompanyByRecordId(requestParams["cid"]);
            if (company == null) throw new MyException("单位信息不存在");
            requestParams.Add("key", company.Secretkey);
            Response.Cookies.Add(new HttpCookie("SmartSystem_H5_CompanyID", company.CPID));
            if (requestParams.ContainsKey("mp") && !string.IsNullOrWhiteSpace(requestParams["mp"]))
            {
                Response.Cookies.Add(new HttpCookie("SmartSystem_H5_MobilePhone", requestParams["mp"]));
            }
            if (SignatureServices.Signature(requestParams) != requestParams["sign"]) throw new MyException("签名验证失败");
            return requestParams;
           
        }
        public void Login(Dictionary<string, string> requestParams)
        {
            if (requestParams.ContainsKey("mp") && !string.IsNullOrWhiteSpace(requestParams["mp"]))
            {
                WX_Info user = new WX_Info();
                user.OpenID = string.Empty;
                user.UserType = 0;
                user.FollowState = (int)WxUserState.UnAttention;
                user.CompanyID = requestParams["cid"];
                user.City = string.Empty;
                user.Country = string.Empty;
                user.Headimgurl = string.Empty;
                user.Language = string.Empty;
                user.NickName = string.Empty;
                user.Province = string.Empty;
                user.Sex = "M";
                user.MobilePhone = requestParams["mp"];
                user.LastSubscribeDate =DateTime.Now;
                WX_Info result = WXAccountServices.AddOrGetWXInfo(user);
                if (result == null) throw new MyException("保存用户信息失败");
                Session["SmartSystem_H5_WX_Info"] = result;
                Response.Cookies.Add(new HttpCookie("SmartSystem_H5_MobilePhone", requestParams["mp"]));
            }
        }
    }
}
