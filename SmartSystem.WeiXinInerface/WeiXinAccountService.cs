using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;
using Common.Utilities.Helpers;
using SmartSystem.WeiXinInerface.WXService;

namespace SmartSystem.WeiXinInerface
{
    public class WeiXinAccountService
    {
        /// <summary>
        /// 注册微信账号
        /// </summary>
        /// <param name="wxAccount"></param>
        /// <returns></returns>
        public static bool RegisterAccount(WX_Info info)
        {
            if (info == null) return false;

            string account = JsonHelper.GetJsonString(info);
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            bool result = client.RegisterAccount(account);
            client.Close();
            client.Abort();
            return result; 
        }
        /// <summary>
        /// 绑定交易密码
        /// </summary>
        /// <param name="OpenID">微信编号</param>
        /// <param name="TradePassword">交易密码</param>
        /// <param name="Mobile">手机号</param>
        /// <returns>accountId</returns>
        public static bool WXBindingMobilePhone(string accountId, string mobile)
        {
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            bool result = client.WXBindingMobilePhone(accountId, mobile);
            client.Close();
            client.Abort();
            return result; 
        }
        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="openId">微信编号</param>
        /// <returns></returns>
        public static bool WXUnsubscribe(string openId)
        {
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            bool result = client.WXUnsubscribe(openId);
            client.Close();
            client.Abort();
            return result; 
        }
        /// <summary>
        /// 查询微信账户信息
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static WX_Info QueryWXByOpenId(string openId)
        {
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            string result = client.QueryWXByOpenId(openId);
            client.Close();
            client.Abort();
            return JsonHelper.GetJson<WX_Info>(result);
        }
        /// <summary>
        /// 修改微信基本信息
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static bool EditWXInfo(WX_Info info)
        {
            if (info == null) return false;

            string account = JsonHelper.GetJsonString(info);
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            bool result = client.EditWXInfo(account);
            client.Close();
            client.Abort();
            return result; 
        }
        /// <summary>
        /// 获取账号信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static WX_Account GetAccountByID(string accountId)
        {
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            string result = client.QueryAccountByAccountID(accountId);
            client.Close();
            client.Abort();
            return JsonHelper.GetJson<WX_Account>(result); 
        }
        /// <summary>
        /// 修改支付车牌号（暂时无用）
        /// </summary>
        /// <param name="OpenID"></param>
        /// <param name="LastPlateNumber"></param>
        /// <returns></returns>
        public static bool EidtWX_infoLastPlateNumber(string OpenID, string LastPlateNumber)
        {
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            bool result = client.EidtWX_infoLastPlateNumber(OpenID,LastPlateNumber);
            client.Close();
            client.Abort();
            return result; 
        }
    }
}
