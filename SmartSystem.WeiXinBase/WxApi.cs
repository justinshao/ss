using System;
using System.IO;
using Common.Services;
using Common.Services.WeiXin;
using Common.Entities.WX;

namespace SmartSystem.WeiXinBase
{
    public class WxApi
    {
        /// <summary>
        /// 获取凭证接口
        /// </summary>
        /// <param name="grantType">获取access_token填写client_credential</param>
        /// <param name="appid">第三方用户唯一凭证</param>
        /// <param name="secret">第三方用户唯一凭证密钥，既appsecret</param>
        /// <returns></returns>
        public static AccessToken GetToken(string appid, string secret, string grantType = "client_credential")
        {
            var url = WxUrl.GetAccessToken.ToFormat(grantType,appid, secret); 
            return WxHttp.Get<AccessToken>(url);
        }

        /// <summary>
        /// 媒体文件上传接口
        ///注意事项
        ///1.上传的媒体文件限制：
        ///图片（image) : 1MB，支持JPG格式
        ///语音（voice）：1MB，播放长度不超过60s，支持MP4格式
        ///视频（video）：10MB，支持MP4格式
        ///缩略图（thumb)：64KB，支持JPG格式
        ///2.媒体文件在后台保存时间为3天，即3天后media_id失效
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="type">上传文件类型</param>
        /// <param name="fileName">上传文件完整路径+文件名</param>
        /// <returns></returns>
        //public static MediaUpload UploadMediaFile(string accessToken, MediaType type, string fileName)
        //{
        //    var fileStream = FileService.GetFileStream(fileName);
        //    var url = WxUrl.UploadMedia.ToFormat(accessToken, type.ToString().ToLower());
        //    var data = new
        //    {
        //        filename = Path.GetFileName(fileName),
        //        filelength = fileStream != null ? fileStream.Length : 0
        //    };
        //    return WxHttp.Post<MediaUpload>(url, data, fileStream);
        //}
        public static MediaUpload UploadMediaFile(string accessToken, MediaType type, string fileName)
        {
            //var fileStream = FileService.GetFileStream(fileName);
            var url = WxUrl.UploadMedia.ToFormat(accessToken, type.ToString().ToLower());
            //url += string.Format("&filename={0}&filelength={1}", Path.GetFileName(fileName), fileStream != null ? fileStream.Length : 0);
            return WxHttp.Post<MediaUpload>(url, fileName);
        }
        /// <summary>
        /// 公众号可调用本接口来获取多媒体文件
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        public static string GetMediaUrl(string accessToken, string mediaId)
        {
            return WxUrl.GetMedia.ToFormat(accessToken, mediaId);
        }

        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="mainButton"></param>
        /// <returns></returns>
        public static bool CreateMenu(string companyId,string accessToken, MainButton mainButton)
        {
            ResError result = WxHttp.Post(WxUrl.CreateMenu.ToFormat(accessToken), mainButton);
            if (result.errcode == ResCode.请求成功)
            {
                return true;
            }
            if (result.errcode == ResCode.获取accessToken时AppSecret错误或者accessToken无效)
            {
                WX_ApiConfig config = WXApiConfigServices.QueryWXApiConfig(companyId);
                var newAccessToken = AccessTokenContainer.TryGetToken(config.AppId, config.AppSecret, true);
                ResError newResult = WxHttp.Post(WxUrl.CreateMenu.ToFormat(newAccessToken), mainButton);
                if (newResult.errcode == ResCode.请求成功)
                {
                    return true;
                }
            }
            TxtLogServices.WriteTxtLogEx("WeiXinBase", "微信创建菜单错误！错误代码：{0}，说明：{1}", (int)result.errcode, result.errmsg);
            return false;
        }

        public static MainButton GetMenu(string accessToken)
        {
            var url = WxUrl.GetMenu.ToFormat(accessToken);
            try
            {
                return WxHttp.Get<FullButtonGroupMenu>(url).ToMainButton();
            }
            catch (Exception ex)
            {
                TxtLogServices.WriteTxtLogEx("WeiXinBase",ex);
                return null;
            }
        }

        public static bool DeleteMenu(string companyId,string accessToken)
        {
            ResError result = WxHttp.Get(WxUrl.DeleteMenu.ToFormat(accessToken));
            if (result.errcode == ResCode.请求成功)
            {
                return true;
            }
            if (result.errcode == ResCode.获取accessToken时AppSecret错误或者accessToken无效)
            {
                WX_ApiConfig config = WXApiConfigServices.QueryWXApiConfig(companyId);
                var newAccessToken = AccessTokenContainer.TryGetToken(config.AppId, config.AppSecret, true);
                ResError newResult = WxHttp.Get(WxUrl.DeleteMenu.ToFormat(newAccessToken));
                return newResult.errcode == ResCode.请求成功;
            }
            return false;
        }
    }
}
