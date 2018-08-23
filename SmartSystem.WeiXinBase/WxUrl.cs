using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSystem.WeiXinBase
{
    public class WxUrl
    {
        #region Token
        /// <summary>
        /// grant_type:{0}
        /// appid:{1}
        /// secret:{2}
        /// </summary>
        public const string GetAccessToken = "https://api.weixin.qq.com/cgi-bin/token?grant_type={0}&appid={1}&secret={2}";
        #endregion
        #region Media
        public const string UploadMedia = "http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}";
        public const string GetMedia = "http://file.api.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}";
        #endregion
        #region Msg
        public const string SendMsg = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}";
        #endregion
        #region Group & User
        public const string CreateGroup = "https://api.weixin.qq.com/cgi-bin/groups/create?access_token={0}";
        public const string GetGroups = "https://api.weixin.qq.com/cgi-bin/groups/get?access_token={0}";
        public const string GetUserGroupId = "https://api.weixin.qq.com/cgi-bin/groups/getid?access_token={0}";
        public const string UpdateGroup = "https://api.weixin.qq.com/cgi-bin/groups/update?access_token={0}";
        public const string UpdateUserGroup = "https://api.weixin.qq.com/cgi-bin/groups/members/update?access_token={0}";
        public const string GetUserInfo = "https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang={2}";
        public const string GetUsers = "https://api.weixin.qq.com/cgi-bin/user/get?access_token={0}&next_openid={1}";
        #endregion
        #region Oauth
        public const string OAuth = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type={2}&scope={3}&state={4}#wechat_redirect";
        public const string GetCodeAccessToken = "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type={3}";
        public const string RefreshCodeAccessToken = "https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={0}&grant_type={2}&refresh_token={3}";
        public const string SnsApiUserInfo = "https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang={2}";
        #endregion
        #region Menu
        public const string CreateMenu = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}";
        public const string GetMenu = "https://api.weixin.qq.com/cgi-bin/menu/get?access_token={0}";
        public const string DeleteMenu = "https://api.weixin.qq.com/cgi-bin/menu/delete?access_token={0}";
        #endregion
        #region QrCode
        public const string CreateQrCode = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}";
        public const string GetQrCode = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket={0}";
        #endregion
        #region SendAll
        public const string SendAllUploadNews ="https://api.weixin.qq.com/cgi-bin/media/uploadnews?access_token={0}";
        public const string SendAll = "https://api.weixin.qq.com/cgi-bin/message/mass/sendall?access_token={0}";
        public const string SendAllToOpenId = "https://api.weixin.qq.com/cgi-bin/message/mass/send?access_token={0}";
        public const string SendAllDelete="https://api.weixin.qq.com//cgi-bin/message/mass/delete?access_token={0}";
        #endregion
        #region Template
        public const string TemplateSend = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";
        #endregion
        public const string GetTicket = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi&t={1}";
    }
}
