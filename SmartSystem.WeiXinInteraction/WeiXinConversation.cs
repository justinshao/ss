using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Common.Services.WeiXin;
using SmartSystem.WeiXinInerface;
using Common.Entities.Enum;
using Common.Services;
using Common.Entities;
using SmartSystem.WeiXinBase;
using Common.Entities.WX;

namespace SmartSystem.WeiXinInteraction
{
    public class WeiXinConversation : Conversation
    {
        public WX_Info user;
        WX_ApiConfig config;
        public string RMsgType;
        public long? ReqId = null;
        public WeiXinConversation(string token,Stream inputStream): base(inputStream)
        {
            config = WXApiConfigServices.QueryByToKen(token);
            if (config == null || string.IsNullOrWhiteSpace(config.AppId) || string.IsNullOrWhiteSpace(config.AppSecret)
                || string.IsNullOrWhiteSpace(config.Domain) || string.IsNullOrWhiteSpace(config.Token)) {
                    CancelExcute = true;
            }
        }

        public override void OnExecuting()
        {
            try
            {
                user = WeiXinAccountService.QueryWXByOpenId(OpenId);
                if (user == null || (WxUserState)user.FollowState == WxUserState.UnAttention)
                {
                    bool IsNewAdd = user == null;
                    user = WxUserInfo.GetWxUserBaseInfo(config, OpenId);
                    if (user == null) {
                        TxtLogServices.WriteTxtLogEx("WeiXinConversation","拉取微信用户信息失败,OPENID：{0}",OpenId);
                        return;
                    }
                    user.OpenID = OpenId;
                    user.UserType = 0;
                    user.FollowState = (int)WxUserState.Attention;
                    user.CompanyID = config.CompanyID;
                    bool result = false;
                    if (IsNewAdd)
                    {
                        TxtLogServices.WriteTxtLogEx("WeiXinConversation", "RegisterAccount");
                       result = WeiXinAccountService.RegisterAccount(user);
                       TxtLogServices.WriteTxtLogEx("WeiXinConversation", string.Format("RegisterAccount Result:{0}",result ? "1" : "0"));
                    }
                    else {
                        TxtLogServices.WriteTxtLogEx("WeiXinConversation", "EditWXInfo");
                        result = WeiXinAccountService.EditWXInfo(user);
                        TxtLogServices.WriteTxtLogEx("WeiXinConversation", string.Format("EditWXInfo Result:{0}", result ? "1" : "0"));
                    }
                    if (result) {
                        user = WeiXinAccountService.QueryWXByOpenId(OpenId);
                    }
                    string resultDes = result ? "成功" : "失败";
                    TxtLogServices.WriteTxtLogEx("WeiXinConversation", "保存微信用户信息{0},OPENID：{1}", resultDes,OpenId);
                }
                else
                {
                    //每7天或头像为空时更新一次微信用户信息
                    if (user.LastSubscribeDate < DateTime.Now.AddDays(-7) || string.IsNullOrWhiteSpace(user.Headimgurl))
                    {
                        WX_Info newUser = WxUserInfo.GetWxUserBaseInfo(config, OpenId);
                        if (newUser == null)
                        {
                            TxtLogServices.WriteTxtLogEx("WeiXinConversation", "更新时拉取微信用户信息失败,OPENID：{0}", OpenId);
                            return;
                        }
                        TxtLogServices.WriteTxtLogEx("WeiXinConversation", "Update");
                        newUser.AccountID = user.AccountID;
                        newUser.OpenID = OpenId;
                        newUser.UserType =0;
                        newUser.CompanyID = config.CompanyID;
                        bool result = WeiXinAccountService.EditWXInfo(newUser);
                        TxtLogServices.WriteTxtLogEx("WeiXinConversation", string.Format("Update Result:{0}", result ? "1" : "0"));
                    }
                }
            }
            catch (Exception ex) {
                ExceptionsServices.AddExceptions(ex, string.Format("创建微信账号或者更新微信账号信息失败,OPENID:{0}",OpenId),LogFrom.WeiXin);
                TxtLogServices.WriteTxtLogEx("WeiXinConversation", ex);
            }
        }

        public override void OnExecuted()
        {
            try
            {

                WX_InteractionInfo model = new WX_InteractionInfo
                {
                    OpenID = OpenId,
                    CreateTime = DateTime.Now,
                    MsgType = (WxMsgType)((int)Request.MsgType),
                    InteractionContent = RequestDocument.ToString(),
                    CompanyID=config.CompanyID
                };
                bool result = false;
                if ((int)Request.MsgType != 6)
                {
                    result = WXInteractionInfoServices.Add(model);
                    if (!result)
                    {

                        string msg = string.Format("保存微信和平台交换信息失败:OpenID:{0};CreateTime:{1}:MsgType:{2};Content:{3}", model.OpenID, model.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"), model.MsgType.GetDescription(), model.InteractionContent);
                        TxtLogServices.WriteTxtLogEx("WeiXinConversation", msg);
                    }
                
                }
              
                if (ResponseDocument != null)
                {
                    int replyId = 0;
                    if (result) {
                        replyId =WXInteractionInfoServices.QueryMaxIdByOpenId(OpenId);
                    }
                    //回复给用户的消息
                    WX_InteractionInfo respModel = new WX_InteractionInfo
                    {
                        OpenID = OpenId,
                        CreateTime = DateTime.Now,
                        MsgType = (WxMsgType)((int)Request.MsgType),
                        InteractionContent = ResponseDocument.ToString(),
                        ReplyID = replyId.ToString(),
                        CompanyID=config.CompanyID
                    };
                    WXInteractionInfoServices.Add(respModel);
                }
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, string.Format("保存微信交互消息失败,OPENID:{0}", OpenId), LogFrom.WeiXin);
                TxtLogServices.WriteTxtLogEx("WeiXinConversation", ex);
            }
        }

        protected override IWRespBase DoForRequest_Text(WReqText request)
        {
            var gResponse = RequestToKey.GoGKey(config,ReplyType.AutoReplay, request.Content, request);
            return gResponse;
        }

        protected override IWRespBase DoForRequest_Event_Subscribe(WReqEventSubscribe request)
        {
            if (user != null) {
                TxtLogServices.WriteTxtLogEx("WeiXinConversation", "DoForRequest_Event_Subscribe");
                user.FollowState = (int)WxUserState.Attention;
                user.LastSubscribeDate = DateTime.Now;
                user.SubscribeTimes = user.SubscribeTimes + 1;
                bool result = WeiXinAccountService.EditWXInfo(user);
                TxtLogServices.WriteTxtLogEx("WeiXinConversation", "DoForRequest_Event_Subscribe REsult:{0}", result?"1":"0");
            }
            var gResponse = RequestToKey.GoGKey(config,ReplyType.Subscribe, string.Empty, request);
            return gResponse;
        }
        protected override IWRespBase DoForRequest_Event_Card(WReqEventCard request) {
            switch (request.EventKey) {
                //卡券通过审核
                case "card_pass_check":
                    break;
                //卡券未通过审核
                case "card_not_pass_check":
                    break;
                //领取事件，用户领取卡券
                case "user_get_card":
                    break;
                //用户删除拥有卡券事件。
                case "user_del_card":
                    break;
                //核销事件推送
                case "user_consume_card":
                    break;
            }
            return null;
        }
        protected override IWRespBase DoForRequest_Event_Scan(WReqEventScan request)
        {
            return DoForScan(request, Convert.ToInt64(request.EventKey));
        }

        private IWRespBase DoForScan(WReqBase request, long sceneId)
        {
            var gResponse = RequestToKey.GoGKey(config,ReplyType.Scan, sceneId.ToString(), request);
            return gResponse;
        }

        protected override IWRespBase DoForRequest_Event_Unsubscribe(WReqEventUnSubscribe request)
        {
            if (user != null) {
                TxtLogServices.WriteTxtLogEx("WeiXinConversation", "DoForRequest_Event_Unsubscribe");
                bool result = WeiXinAccountService.WXUnsubscribe(user.OpenID);
                TxtLogServices.WriteTxtLogEx("WeiXinConversation", "DoForRequest_Event_Unsubscribe Result:{0}",result?"1":"0");
            }
            var response = request.CreateResponse<WRespText>();
            response.Content = "取消关注成功!";
            return response;
        }

        protected override IWRespBase DoForRequest_Event_Click(WReqEventClick request)
        {
            var gResponse = RequestToKey.GoGKey(config,ReplyType.AutoReplay, request.EventKey, request);
            return gResponse;
        }
        protected override IWRespBase DoForRequest_Default(WReqBase request)
        {
            var response = RequestToKey.GoGKey(config,ReplyType.Default, string.Empty, request);
            return response;
        }
        protected override IWRespBase DoForRequest_Event_Location(WReqEventLocation request)
        {
            try
            {
                WX_UserLocation location = new WX_UserLocation();
                location.OpenId = request.FromUserName;
                location.Latitude = request.Latitude;
                location.Longitude = request.Longitude;
                location.Precision = request.Precision;
                location.LastReportedTime = request.CreateTime;
                location.CompanyID = config.CompanyID;
                WXUserLocationServices.AddOrUpdate(location);
                return null;
            }
            catch (Exception ex) {
                ExceptionsServices.AddExceptions(ex, string.Format("保存微信用户位置信息失败,OPENID:{0}", OpenId), LogFrom.WeiXin);
                TxtLogServices.WriteTxtLogEx("WeiXinConversation", ex);
                return null;
            }
        }
    }
}
