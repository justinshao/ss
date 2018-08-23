using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common.Services;
using Common.Services.WeiXin;
using Common.Entities.WX;


namespace SmartSystem.WeiXinBase
{
    public class WxAdvApi
    {
        #region SendMsg
        /// <summary>
        /// 发送文本信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openId"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool SendText(string companyId,string accessToken, string openId, string content)
        {
            var data = new
            {
                touser = openId,
                msgtype = "text",
                text = new
                {
                    content = content
                }
            };
            ResError result = WxHttp.Post(WxUrl.SendMsg.ToFormat(accessToken), data);
            if (result.errcode == ResCode.请求成功)
            {
                return true;
            }

            if (result.errcode == ResCode.获取accessToken时AppSecret错误或者accessToken无效)
            {
                WX_ApiConfig config = WXApiConfigServices.QueryWXApiConfig(companyId);
                var newAccessToken = AccessTokenContainer.TryGetToken(config.AppId, config.AppSecret, true);
                ResError newResult = WxHttp.Post(WxUrl.SendMsg.ToFormat(newAccessToken), data);
                if (newResult.errcode == ResCode.请求成功) {
                    return true;
                }
            }
            TxtLogServices.WriteTxtLogEx("WeiXinBase", "发送文本信息错误！错误代码：{0}，说明：{1}", (int)result.errcode, result.errmsg);
            return false;
        }

        /// <summary>
        /// 发送图片消息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openId"></param>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        public static bool SendImage(string companyId, string accessToken, string openId, string mediaId)
        {
            var data = new
            {
                touser = openId,
                msgtype = "image",
                image = new
                {
                    media_id = mediaId
                }
            };
          
            ResError result = WxHttp.Post(WxUrl.SendMsg.ToFormat(accessToken), data);
            if (result.errcode == ResCode.请求成功)
            {
                return true;
            }
            if (result.errcode == ResCode.获取accessToken时AppSecret错误或者accessToken无效)
            {
                WX_ApiConfig config = WXApiConfigServices.QueryWXApiConfig(companyId);
                var newAccessToken = AccessTokenContainer.TryGetToken(config.AppId, config.AppSecret, true);
                ResError newResult = WxHttp.Post(WxUrl.SendMsg.ToFormat(newAccessToken), data);
                if (newResult.errcode == ResCode.请求成功)
                {
                    return true;
                }
            }
            TxtLogServices.WriteTxtLogEx("WeiXinBase", "发送图片消息错误！错误代码：{0}，说明：{1}", (int)result.errcode, result.errmsg);
            return false;
        }

        /// <summary>
        /// 发送语音消息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openId"></param>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        public static bool SendVoice(string companyId, string accessToken, string openId, string mediaId)
        {
            var data = new
            {
                touser = openId,
                msgtype = "voice",
                voice = new
                {
                    media_id = mediaId
                }
            };
         
            ResError result = WxHttp.Post(WxUrl.SendMsg.ToFormat(accessToken), data);
            if (result.errcode == ResCode.请求成功)
            {
                return true;
            }
            if (result.errcode == ResCode.获取accessToken时AppSecret错误或者accessToken无效)
            {
                WX_ApiConfig config = WXApiConfigServices.QueryWXApiConfig(companyId);
                var newAccessToken = AccessTokenContainer.TryGetToken(config.AppId, config.AppSecret, true);
                ResError newResult = WxHttp.Post(WxUrl.SendMsg.ToFormat(newAccessToken), data);
                if (newResult.errcode == ResCode.请求成功) {
                    return true;
                }
            }
            TxtLogServices.WriteTxtLogEx("WeiXinBase", "发送语音消息错误！错误代码：{0}，说明：{1}", (int)result.errcode, result.errmsg);
            return false;
        }

        /// <summary>
        /// 发送视频消息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openId"></param>
        /// <param name="mediaId"></param>
        /// <param name="thumbMediaId"></param>
        /// <returns></returns>
        public static bool SendVideo(string companyId, string accessToken, string openId, string mediaId, string thumbMediaId)
        {
            var data = new
            {
                touser = openId,
                msgtype = "video",
                video = new
                {
                    media_id = mediaId,
                    thumb_media_id = thumbMediaId
                }
            };
            ResError result = WxHttp.Post(WxUrl.SendMsg.ToFormat(accessToken), data);
            if (result.errcode == ResCode.请求成功){
                return true;
            }

            if (result.errcode == ResCode.获取accessToken时AppSecret错误或者accessToken无效)
            {
                WX_ApiConfig config = WXApiConfigServices.QueryWXApiConfig(companyId);
                var newAccessToken = AccessTokenContainer.TryGetToken(config.AppId, config.AppSecret, true);
                ResError newResult = WxHttp.Post(WxUrl.SendMsg.ToFormat(newAccessToken), data);
                if (newResult.errcode == ResCode.请求成功) {
                    return true;
                }
            }

            TxtLogServices.WriteTxtLogEx("WeiXinBase", "发送视频消息错误！错误代码：{0}，说明：{1}", (int)result.errcode, result.errmsg);
            return false;
        }

        /// <summary>
        /// 发送音乐消息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openId"></param>
        /// <param name="title">音乐标题（非必须）</param>
        /// <param name="description">音乐描述（非必须）</param>
        /// <param name="musicUrl">音乐链接</param>
        /// <param name="hqMusicUrl">高品质音乐链接，wifi环境优先使用该链接播放音乐</param>
        /// <param name="thumbMediaId">视频缩略图的媒体ID</param>
        /// <returns></returns>
        public static bool SendMusic(string companyId, string accessToken, string openId, string title, string description, string musicUrl, string hqMusicUrl, string thumbMediaId)
        {
            var data = new
            {
                touser = openId,
                msgtype = "music",
                music = new
                {
                    title = title,
                    description = description,
                    musicurl = musicUrl,
                    hqmusicurl = hqMusicUrl,
                    thumb_media_id = thumbMediaId
                }
            };
            ResError result = WxHttp.Post(WxUrl.SendMsg.ToFormat(accessToken), data);
            if (result.errcode == ResCode.请求成功)
            {
                return true;
            }

            if (result.errcode == ResCode.获取accessToken时AppSecret错误或者accessToken无效)
            {
                WX_ApiConfig config = WXApiConfigServices.QueryWXApiConfig(companyId);
                var newAccessToken = AccessTokenContainer.TryGetToken(config.AppId, config.AppSecret, true);
                ResError newResult = WxHttp.Post(WxUrl.SendMsg.ToFormat(newAccessToken), data);
                if (newResult.errcode == ResCode.请求成功) {
                    return true;
                }
               
            }
            TxtLogServices.WriteTxtLogEx("WeiXinBase", "发送音乐消息错误！错误代码：{0}，说明：{1}", (int)result.errcode, result.errmsg);
            return false;
        }

        /// <summary>
        /// 发送图文消息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openId"></param>
        /// <param name="articles"></param>
        /// <returns></returns>
        public static bool SendNews(string companyId, string accessToken, string openId, List<Article> articles)
        {
            var data = new
            {
                touser = openId,
                msgtype = "news",
                news = new
                {
                    articles = articles.Select(z => new
                    {
                        title = z.Title,
                        description = z.Description,
                        url = z.Url,
                        picurl = z.PicUrl//图文消息的图片链接，支持JPG、PNG格式，较好的效果为大图640*320，小图80*80
                    }).ToList()
                }
            };
            ResError result = WxHttp.Post(WxUrl.SendMsg.ToFormat(accessToken), data);
            if (result.errcode == ResCode.请求成功)
            {
                return true;
            }

            if (result.errcode == ResCode.获取accessToken时AppSecret错误或者accessToken无效)
            {
                WX_ApiConfig config = WXApiConfigServices.QueryWXApiConfig(companyId);
                var newAccessToken = AccessTokenContainer.TryGetToken(config.AppId, config.AppSecret, true);
                ResError newResult = WxHttp.Post(WxUrl.SendMsg.ToFormat(newAccessToken), data);
                if (newResult.errcode == ResCode.请求成功) {
                    return true;
                }

            }
            TxtLogServices.WriteTxtLogEx("WeiXinBase", "发送图文消息错误！错误代码：{0}，说明：{1}", (int)result.errcode, result.errmsg);
            return false;
        }
        #endregion

        #region QrCode

        /// <summary>
        /// 创建二维码
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="sceneId">场景值ID，临时二维码时为32位整型，永久二维码时最大值为1000</param>
        /// <param name="expireSeconds">该二维码有效时间，以秒为单位。 最大不超过1800。不填和0时为永久二维码</param>
        /// <returns></returns>
        public static QrCode CreateQrCode(string companyId, string accessToken, int sceneId, int expireSeconds = 0)
        {
            var url = WxUrl.CreateQrCode.ToFormat(accessToken);
            var guid = Guid.NewGuid().ToString("N");
            object data;
            if (expireSeconds > 0)
            {
                data = new
                {
                    expire_seconds = expireSeconds,
                    action_name = "QR_SCENE",
                    action_info = new
                    {
                        scene = new
                        {
                            scene_id = sceneId
                        }
                    }
                };
                //LogServices.WriteTxtLogEx("CreateQrCode", "Create sceneId:{0},expire_seconds:{1},guid:{2}", sceneId, expireSeconds, guid);
            }
            else
            {
                data = new
                {
                    action_name = "QR_LIMIT_SCENE",
                    action_info = new
                    {
                        scene = new
                        {
                            scene_id = sceneId
                        }
                    }
                };
                //LogServices.WriteTxtLogEx("CreateQrCode", "Create static sceneId:{0},guid:{1}", sceneId, guid);
            }
            var qrCode = WxHttp.Post<QrCode>(url, data);
            //LogServices.WriteTxtLogEx("CreateQrCode", "Create sceneId:{0},guid:{1},ticket:{2}", sceneId, guid, qrCode.Ticket);
            return qrCode;
        }
        /// <summary>
        /// 获取QrCode的地址
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public static string GetQrCodeUrl(string ticket)
        {
            return WxUrl.GetQrCode.ToFormat(ticket);
        }

        #endregion

        #region OAuth
        /// <summary>
        /// 获取验证地址
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="redirectUrl"></param>
        /// <param name="state"></param>
        /// <param name="scope"></param>
        /// <param name="responseType"></param>
        /// <returns></returns>
        public static string GetAuthorizeUrl(string appId, string redirectUrl, string state, OAuthScope scope, string responseType = "code")
        {
            var url = WxUrl.OAuth.ToFormat(appId, HttpUtility.UrlEncode(redirectUrl), responseType, scope, state);

            /* 这一步发送之后，客户会得到授权页面，无论同意或拒绝，都会返回redirectUrl页面。
             * 如果用户同意授权，页面将跳转至 redirect_uri/?code=CODE&state=STATE。这里的code用于换取access_token（和通用接口的access_token不通用）
             * 若用户禁止授权，则重定向后不会带上code参数，仅会带上state参数redirect_uri?state=STATE
             */
            return url;
        }


        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="secret"></param>
        /// <param name="code">code作为换取access_token的票据，每次用户授权带上的code将不一样，code只能使用一次，5分钟未被使用自动过期。</param>
        /// <param name="grantType"></param>
        /// <returns></returns>
        public static OAuthAccessToken GetAccessToken(string appId, string secret, string code, string grantType = "authorization_code")
        {
            var url = WxUrl.GetCodeAccessToken.ToFormat(appId, secret, code, grantType);
            return WxHttp.Get<OAuthAccessToken>(url);
        }

        /// <summary>
        /// 刷新access_token（如果需要）
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="refreshToken">填写通过access_token获取到的refresh_token参数</param>
        /// <param name="grantType"></param>
        /// <returns></returns>
        public static OAuthAccessToken RefreshToken(string appId, string refreshToken, string grantType = "refresh_token")
        {
            var url = WxUrl.RefreshCodeAccessToken.ToFormat(appId, grantType, refreshToken);
            return WxHttp.Get<OAuthAccessToken>(url);
        }

        /// <summary>
        /// 通过OAuthAccessToken获取用户信息
        /// </summary>
        /// <returns></returns>
        public static OAuthUserInfo GetOAuthUserInfo(string oAuthAccessToken, string openId, string lang = "zh_CN")
        {
            var url = WxUrl.SnsApiUserInfo.ToFormat(oAuthAccessToken, openId, lang);
            return WxHttp.Get<OAuthUserInfo>(url);
        }

        #endregion

        #region Group
        /// <summary>
        /// 创建分组
        /// </summary>
        /// <returns></returns>
        public static GroupCreate CreateGroup(string accessToken, string name)
        {
            var url = WxUrl.CreateGroup.ToFormat(accessToken);
            var data = new
            {
                group = new
                {
                    name = name
                }
            };
            return WxHttp.Post<GroupCreate>(url, data);
        }

        /// <summary>
        /// 查询所有分组
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static GroupList GetGroup(string accessToken)
        {
            var url = WxUrl.GetGroups.ToFormat(accessToken);
            return WxHttp.Get<GroupList>(url);
        }

        /// <summary>
        /// 查询用户所在分组
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static GroupId GetGroupId(string accessToken, string openId)
        {
            var url = WxUrl.GetUserGroupId.ToFormat(accessToken);
            var data = new
            {
                openid = openId
            };
            return WxHttp.Post<GroupId>(url, data);
        }

        /// <summary>
        /// 修改分组名
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="groupId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool UpdateGroup(string companyId, string accessToken, int groupId, string name)
        {
          
            var data = new
            {
                group = new
                {
                    id = groupId,
                    name = name
                }
            };
            ResError result = WxHttp.Post(WxUrl.UpdateGroup.ToFormat(accessToken), data);
            if (result.errcode == ResCode.请求成功)
            {
                return true;
            }
            if (result.errcode == ResCode.获取accessToken时AppSecret错误或者accessToken无效)
            {
                WX_ApiConfig config = WXApiConfigServices.QueryWXApiConfig(companyId);
                var newAccessToken = AccessTokenContainer.TryGetToken(config.AppId, config.AppSecret, true);
                ResError newResult = WxHttp.Post(WxUrl.UpdateGroup.ToFormat(newAccessToken), data);
                if (newResult.errcode == ResCode.请求成功) {
                    return true;
                }
            }
            TxtLogServices.WriteTxtLogEx("WeiXinBase", "微信修改分组名错误！错误代码：{0}，说明：{1}", (int)result.errcode, result.errmsg);
            return false;
        }

        /// <summary>
        /// 移动用户分组
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openId"></param>
        /// <param name="toGroupId"></param>
        /// <returns></returns>
        public static bool UpdateUserGroup(string companyId, string accessToken, string openId, int toGroupId)
        {
            var data = new
            {
                openid = openId,
                to_groupid = toGroupId
            };
            ResError result = WxHttp.Post(WxUrl.UpdateUserGroup.ToFormat(accessToken), data);
            if (result.errcode == ResCode.请求成功)
            {
                return true;
            }
            if (result.errcode == ResCode.获取accessToken时AppSecret错误或者accessToken无效)
            {
                WX_ApiConfig config = WXApiConfigServices.QueryWXApiConfig(companyId);
                var newAccessToken = AccessTokenContainer.TryGetToken(config.AppId, config.AppSecret, true);
                ResError newResult = WxHttp.Post(WxUrl.UpdateUserGroup.ToFormat(newAccessToken), data);
                if (newResult.errcode == ResCode.请求成功) {
                    return true;
                }
            }
            TxtLogServices.WriteTxtLogEx("WeiXinBase", "移动用户分组发生错误！错误代码：{0}，说明：{1}", (int)result.errcode, result.errmsg);
            return false;
        }
        #endregion

        #region User

        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openId"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public static UserInfo GetUserInfo(string accessToken, string openId, string lang = "zh_CN")
        {
            var url = WxUrl.GetUserInfo.ToFormat(accessToken, openId, lang);
            return WxHttp.Get<UserInfo>(url);
        }

        /// <summary>
        /// 取帐号的关注者列表
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="nextOpenId"></param>
        /// <returns></returns>
        public static UserGet GetUserList(string accessToken, string nextOpenId)
        {
            var url = WxUrl.GetUsers.ToFormat(accessToken, nextOpenId);
            return WxHttp.Get<UserGet>(url);
        }

        #endregion

        #region SendAll
        /// <summary>
        /// 上传图文消息素材
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="news"></param>
        /// <returns></returns>
        public static SendAllNewsCreate UploadNews(string accessToken, SendAllNews news)
        {
            var url = WxUrl.SendAllUploadNews.ToFormat(accessToken);
            return WxHttp.Post<SendAllNewsCreate>(url, news);
        }
        /// <summary>
        /// 根据分组进行群发
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="groupId">群发到的分组的group_id</param>
        /// <param name="mediaId">用于群发的消息的media_id</param>
        /// <param name="msgtype">群发的消息类型，图文消息为mpnews</param>
        /// <returns></returns>
        public static SendAllResult SendAll(string accessToken, string groupId, string mediaId, string msgtype = "mpnews")
        {
            var url = WxUrl.SendAll.ToFormat(accessToken);
            var data = new
            {
                filter = new
                {
                    group_id = groupId
                },
                mpnews = new
                {
                    media_id = mediaId
                },
                msgtype = msgtype
            };
            return WxHttp.Post<SendAllResult>(url, data);
        }
        /// <summary>
        /// 根据OpenID列表群发
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="mediaId">用于群发的图文消息的media_id</param>
        /// <param name="toOpenIds">填写图文消息的接收者，一串OpenID列表，OpenID最少1个，最多10000个</param>
        /// <param name="msgtype">群发的消息类型，图文消息为mpnews</param>
        /// <returns></returns>
        public static SendAllResult SendAll(string accessToken, string mediaId, IEnumerable<string> toOpenIds, string msgtype = "mpnews")
        {
            var url = WxUrl.SendAllToOpenId.ToFormat(accessToken);
            var data = new
            {
                touser = toOpenIds,
                mpnews = new
                {
                    media_id = mediaId
                },
                msgtype = msgtype
            };
            return WxHttp.Post<SendAllResult>(url, data);
        }
        /// <summary>
        /// 删除群发
        /// 请注意，只有已经发送成功的消息才能删除删除消息只是将消息的图文详情页失效，已经收到的用户，还是能在其本地看到消息卡片。
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="msgId">发送出去的消息ID</param>
        /// <returns></returns>
        public static bool SendAllDelete(string companyId, string accessToken, long msgId)
        {
            var data = new
            {
                msgid = msgId
            };

            ResError result = WxHttp.Post(WxUrl.SendAllDelete.ToFormat(accessToken), data);
            if (result.errcode == ResCode.请求成功)
            {
                return true;
            }
            if (result.errcode == ResCode.获取accessToken时AppSecret错误或者accessToken无效)
            {
                WX_ApiConfig config = WXApiConfigServices.QueryWXApiConfig(companyId);
                var newAccessToken = AccessTokenContainer.TryGetToken(config.AppId, config.AppSecret, true);
                ResError newResult = WxHttp.Post(WxUrl.SendAllDelete.ToFormat(newAccessToken), data);
                if (newResult.errcode == ResCode.请求成功)
                {
                    return true;
                }
            }
            TxtLogServices.WriteTxtLogEx("WeiXinBase", "删除群发发生错误！错误代码：{0}，说明：{1}", (int)result.errcode, result.errmsg);
            return false;
        }
        #endregion

        #region Template
        public static bool SendTemplateMessage<T>(string companyId,string accessToken, string openId, string templateId, string topcolor, T data)
        {
           
            var pdata = new Templete
            {
                template_id = templateId,
                topcolor = topcolor,
                touser = openId,
                data = data
            };
            ResError result = WxHttp.Post(WxUrl.TemplateSend.ToFormat(accessToken), pdata);
            if (result.errcode == ResCode.请求成功)
            {
                return true;
            }
            if (result.errcode == ResCode.获取accessToken时AppSecret错误或者accessToken无效)
            {
                WX_ApiConfig config = WXApiConfigServices.QueryWXApiConfig(companyId);
                var newAccessToken = AccessTokenContainer.TryGetToken(config.AppId, config.AppSecret, true);
                ResError newResult = WxHttp.Post(WxUrl.TemplateSend.ToFormat(newAccessToken), pdata);
                if (newResult.errcode == ResCode.请求成功) {
                    return true;
                }
            }
            TxtLogServices.WriteTxtLogEx("WeiXinBase", "发送模板消息错误！错误代码：{0}，说明：{1}", (int)result.errcode, result.errmsg);
            return false;

        }
        #endregion
        /// <summary>
        /// 获取JsapiTicket
        /// </summary> 
        public static JsapiTicket GetTicket(AccessToken token)
        {
            var url = WxUrl.UpdateUserGroup.ToFormat(token.Accesstoken,DateTime.Now.Ticks);
            return WxHttp.Get<JsapiTicket>(url);
        }
        /// <summary>
        /// 获取JsapiTicket
        /// </summary> 
        public static JsapiTicket GetTicket(string accesstoken)
        {
            var url = WxUrl.GetTicket.ToFormat(accesstoken, DateTime.Now.Ticks);
            return WxHttp.Get<JsapiTicket>(url);
        }
    }
}
