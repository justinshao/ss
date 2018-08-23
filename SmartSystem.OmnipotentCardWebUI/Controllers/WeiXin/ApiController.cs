using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.WeiXinBase;
using Common.Services;
using Common.Entities;
using SmartSystem.WeiXinInteraction;
using Common.Services.WeiXin;
using Common.Entities.WX;

using System.Xml.Linq;
using Common.Utilities.Helpers;
using CrystalDecisions.CrystalReports.Engine;
using Common.Entities.Enum;
using Common.Utilities.Helpers;
using System.Data.Common;
using SmartSystem.OmnipotentCardWebUI.Models;
using Common.DataAccess;
using SmartSystem.OmnipotentCardWebUI.Areas.Parking.Controllers;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin
{
    public class ApiController : Controller
    {
        [HttpGet]
        [ActionName("Index")]
        public ActionResult Get(string token,string signature, string timestamp, string nonce, string echostr)
        {
            try
            {

                WX_ApiConfig config = WXApiConfigServices.QueryByToKen(token);
                if (config == null || string.IsNullOrWhiteSpace(config.AppId) || string.IsNullOrWhiteSpace(config.AppSecret)
                 || string.IsNullOrWhiteSpace(config.Domain) || string.IsNullOrWhiteSpace(config.Token))
                {
                    return Content(string.Empty);
                }
                if (WxService.CheckSignature(config.Token, timestamp, nonce, signature))
                {
                    return Content(echostr);
                }
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "微信公众账号提交验证失败", LogFrom.WeiXin);
                TxtLogServices.WriteTxtLogEx("Api_Get", "signature:{0};timestamp:{1};nonce:{2};echostr:{3}", signature, timestamp, nonce, echostr);
            }
            return Content(string.Empty);
        }
        [HttpPost]
        [ActionName("Index")]
        public ActionResult Post(string token,string signature, string timestamp, string nonce, string echostr)
        {

                WX_ApiConfig config = WXApiConfigServices.QueryByToKen(token);
                if (config == null || string.IsNullOrWhiteSpace(config.AppId) || string.IsNullOrWhiteSpace(config.AppSecret)
                 || string.IsNullOrWhiteSpace(config.Domain) || string.IsNullOrWhiteSpace(config.Token))
                {
                    return Content(string.Empty);
                }

                //接收数据
                //System.IO.StreamReader reader = new System.IO.StreamReader(Request.InputStream);
                //String xmlData = reader.ReadToEnd();
                //XElement xdoc = XElement.Parse(xmlData);

                //解析XML
                //var toUserName = xdoc.Element("ToUserName").Value;
                //var fromUserName = xdoc.Element("FromUserName").Value;
                //var createTime = xdoc.Element("CreateTime").Value;
                //string key = xdoc.Element("EventKey").Value;
                //var content = xdoc.Element("Ticket").Value;
                //DateTime datatime = DateTime.Now;
                //if (!string.IsNullOrEmpty(key))
                //{
                //userin User = wxApi.GetNickname(wxApi.GetToken(config.AppId, config.AppSecret).Accesstoken, fromUserName);
                //    string createSql = "insert into TgCount values('" + key + "','" + createTime + "','" + User.Nickname + "','" + toUserName + "','" + datatime + "')";
                //    using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
                //    {
                //        dboperator.ExecuteNonQuery(createSql, 30000);
                //    }

                //}


                try
                {


                TxtLogServices.WriteTxtLogEx("Api_Post", "AppId:{0};AppSecret:{1};Domain:{2};Token:{3}", config.AppId, config.AppSecret, config.Domain, config.Token);
                if (!config.Status)
                {
                    return Content("该公众账号暂停使用，请稍后再试！");
                }
                if (!WxService.CheckSignature(config.Token, timestamp, nonce, signature))
                {
                    return Content(string.Empty);
                }

                var conversation = new WeiXinConversation(token,Request.InputStream);
                try
                {
                    TxtLogServices.WriteTxtLogEx("Api_Post_Request", conversation.RequestDocument.ToString());
                  
                    //执行微信处理过程
                    conversation.Execute();

                    if (conversation.ResponseDocument != null)
                    {
                        TxtLogServices.WriteTxtLogEx("Api_Post_Response", conversation.ResponseDocument.ToString());
                    }

                }
                catch (Exception ex)
                {

                    ExceptionsServices.AddExceptions(ex, "执行微信处理过程失败", LogFrom.WeiXin);
                    TxtLogServices.WriteTxtLogEx("Api_Post_Error",ex);

                    if (conversation.ResponseDocument != null)
                    {
                        TxtLogServices.WriteTxtLogEx("Api_Post_Error", conversation.ResponseDocument.ToString());
                    }
                }
                if (conversation.ResponseDocument != null)
                {
                    //如果是微信位置推送则不回复
                    if (conversation.Request.MsgType == WReqType.Event)
                    {
                        WReqEventBase wreqevent = conversation.Request as WReqEventBase;
                        if (wreqevent != null && wreqevent.Event == EventType.Location)
                        {
                            return Content(string.Empty);
                        }
                    }
                    return Content(conversation.ResponseDocument.ToString());
                }
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "微信提交事件处理失败", LogFrom.WeiXin);
                TxtLogServices.WriteTxtLogEx("Api_Post_Error", ex);
            }


            return Content(string.Empty);
        }

    }
}
