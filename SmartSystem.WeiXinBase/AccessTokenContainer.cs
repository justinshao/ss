using System;
using System.Collections.Generic;

namespace SmartSystem.WeiXinBase
{
    public class AccessTokenContainer
    {
        private static readonly Dictionary<string, AccessTokenBag> AccessTokenCollection = new Dictionary<string, AccessTokenBag>();

        /// <summary>
        /// 注册应用凭证信息，此操作只是注册，不会马上获取Token，并将清空之前的Token，
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        private static void Register(string appId, string appSecret)
        {
            AccessTokenCollection[appId] = new AccessTokenBag
            {
                AppId = appId,
                AppSecret = appSecret,
                ExpireTime = DateTime.MinValue,
                AccessToken = new AccessToken()
            };
        }

        /// <summary>
        /// 检查是否已经注册
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        private static bool CheckRegistered(string appId)
        {
            return AccessTokenCollection.ContainsKey(appId);
        }

        /// <summary>
        /// 获取可用Token
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="getNew">是否强制重新获取新的Token</param>
        /// <returns></returns>
        private static AccessToken GetTokenModel(string appId, bool getNew = false)
        {
            var accessTokenBag = AccessTokenCollection[appId];
            //获取缓存
            if (!getNew && accessTokenBag.ExpireTime > DateTime.Now)
                return accessTokenBag.AccessToken;
            //已过期，重新获取
            accessTokenBag.AccessToken = WxApi.GetToken(accessTokenBag.AppId, accessTokenBag.AppSecret);
            if (accessTokenBag.AccessToken.ExpiresIn - 1000 > 1000) {
                accessTokenBag.AccessToken.ExpiresIn = accessTokenBag.AccessToken.ExpiresIn - 1000;
            }
            accessTokenBag.ExpireTime = DateTime.Now.AddSeconds(accessTokenBag.AccessToken.ExpiresIn);
            return accessTokenBag.AccessToken;
        }

        /// <summary>
        /// 获取可用Token
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="getNew">是否强制重新获取新的Token</param>
        /// <returns></returns>
        private static string GetToken(string appId, bool getNew = false)
        {
            return GetTokenModel(appId, getNew).Accesstoken;
        }

        /// <summary>
        /// 使用完整的应用凭证获取Token，如果不存在将自动注册
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="getNew"></param>
        /// <returns></returns>
        public static string TryGetToken(string appId, string appSecret, bool getNew = false)
        {
            if (getNew || !CheckRegistered(appId))
            {
                Register(appId, appSecret);
            }
            return GetToken(appId);
        }

        public static AccessToken TryGetTokenModel(string appId, string appSecret, bool getNew = false)
        {
            if (getNew || !CheckRegistered(appId))
            {
                Register(appId, appSecret);
            }
            return GetTokenModel(appId);
        }
    }

    class AccessTokenBag
    {
        public string AppId { get; set; }
        public string AppSecret { get; set; }
        public DateTime ExpireTime { get; set; }
        public AccessToken AccessToken { get; set; }
    }

}
