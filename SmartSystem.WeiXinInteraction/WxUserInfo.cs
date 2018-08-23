using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;
using SmartSystem.WeiXinBase;
using Common.Utilities.Helpers;
using System.Net;
using System.Web;
using System.IO;
using Common.Services;

namespace SmartSystem.WeiXinInteraction
{
    public class WxUserInfo
    {
        public static WX_Info GetWxUserBaseInfo(WX_ApiConfig config, string openId, bool errorRetry = true)
        {
            var accessToken = AccessTokenContainer.TryGetToken(config.AppId, config.AppSecret);
            if (!string.IsNullOrWhiteSpace(accessToken) && !string.IsNullOrWhiteSpace(openId))
            {
                try
                {
                    WX_Info user = new WX_Info();
                    var userInfo = WxAdvApi.GetUserInfo(accessToken, openId);
                    if (userInfo.subscribe == 0)
                    {
                        return null;
                    }
                    user.City = userInfo.city;
                    user.Country = userInfo.country;
                    user.Headimgurl = DownloadHeadImg(openId, userInfo.headimgurl,config.CompanyID);
                    user.Language = userInfo.language;
                    user.NickName = userInfo.nickname;
                    user.Province = userInfo.province;
                    user.Sex = userInfo.sex == "1" ? "M" : "F";
                    user.LastSubscribeDate = DateTimeHelper.TransferUnixDateTime(userInfo.subscribe_time);
                    return user;
                }
                catch (Exception)
                {
                    if (errorRetry)
                    {
                        GetWxUserBaseInfo(config, openId, false);
                    }
                }
            }
            return null;
        }
        public static string DownloadHeadImg(string openId, string url,string companyId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(url))
                {
                    return url;
                }
                using (var wc = new WebClient())
                {
                    var directorypath = "/Uploads/WeiXinHeadImg/" + companyId + "/";
                    var realpath = HttpContext.Current.Server.MapPath(directorypath);
                    if (!Directory.Exists(realpath))
                    {
                        Directory.CreateDirectory(realpath);
                    }
                    var filepath = string.Format("{0}{1}.png", directorypath, openId);
                    if (File.Exists(filepath))
                    {
                        File.Delete(filepath);
                    }
                    string path = HttpContext.Current.Server.MapPath(filepath);
                    wc.DownloadFile(url, path);
                    return filepath;
                }
            }
            catch (Exception ex)
            {
                TxtLogServices.WriteTxtLogEx("DownloadHeadImg", ex);
                return string.Empty;
            }
        }
    }
}
