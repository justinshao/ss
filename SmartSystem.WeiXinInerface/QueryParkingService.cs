using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utilities.Helpers;
using SmartSystem.WeiXinInerface.WXService;
using Common.Entities;

namespace SmartSystem.WeiXinInerface
{
    public class QueryParkingService
    {
        /// <summary>
        ///获取车场信息
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public static BaseParkinfo GetBaseParkinfoByPKID(string ParkingID)
        {
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            string result = client.GetBaseParkinfoByPKID(ParkingID);
            client.Close();
            client.Abort();
            return JsonHelper.GetJson<BaseParkinfo>(result);
        }
        /// <summary>
        /// 查询车场信息
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public static List<BaseParkinfo> QueryParkinfo(double lat, double lng, int searchRound)
        {
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            string parking = client.GetPkparkinfoList(lat,lng,searchRound);
            client.Close();
            client.Abort();
            if (string.IsNullOrWhiteSpace(parking))
            {
                return new List<BaseParkinfo>();
            }
            return JsonHelper.GetJson<List<BaseParkinfo>>(parking);
        }
    }
}
