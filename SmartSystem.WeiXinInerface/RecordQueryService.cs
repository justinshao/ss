using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartSystem.WeiXinInerface.WXService;
using Common.Entities.WX;
using Common.Utilities.Helpers;

namespace SmartSystem.WeiXinInerface
{
    public class RecordQueryService
    {
        /// <summary>
        ///获取月卡缴费记录
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public static List<PkOrderMonth> GetPkOrderMonth(string accountId)
        {
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            string result = client.GetPkOrderMonthByAccountID(accountId);
            client.Close();
            client.Abort();
            if (string.IsNullOrWhiteSpace(result))
            {
                return new List<PkOrderMonth>();
            }
            return JsonHelper.GetJson<List<PkOrderMonth>>(result);
        }
        public static List<PkOrderTemp> GetPkOrderTemp(string accountId)
        {
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            string result = client.GetPkOrderTempByAccountID(accountId);
            client.Close();
            client.Abort();
            if (string.IsNullOrWhiteSpace(result))
            {
                return new List<PkOrderTemp>();
            }
            return JsonHelper.GetJson<List<PkOrderTemp>>(result);
        }
    }
}
