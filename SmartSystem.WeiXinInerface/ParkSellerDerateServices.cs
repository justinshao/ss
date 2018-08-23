using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using SmartSystem.WeiXinInerface.WXService;
using Common.Utilities.Helpers;
using Common.Entities.Parking;
using Common.Entities.WX;
using Common.Entities.Other;

namespace SmartSystem.WeiXinInerface
{
    public class ParkSellerDerateServices
    {
        /// <summary>
        /// 获取商户优免规则
        /// </summary>
        /// <param name="sellerID"></param>
        /// <param name="VID"></param>
        /// <returns></returns>
        public static List<ParkDerate> WXGetParkDerate(string sellerID, string VID)
        {
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            string result = client.WXGetParkDerate(sellerID, VID, string.Empty);
            client.Close();
            client.Abort();
            return JsonHelper.GetJson<List<ParkDerate>>(result);
        }
        /// <summary>
        /// 获取车牌进场信息
        /// </summary>
        /// <param name="PlateNumber"></param>
        /// <param name="VID"></param>
        /// <param name="SellerID"></param>
        /// <returns></returns>
        public static List<ParkIORecord> WXGetIORecordByPlateNumber(string PlateNumber, string VID, string SellerID)
        {
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            string result = client.WXGetIORecordByPlateNumber(PlateNumber, VID, SellerID,string.Empty);
            client.Close();
            client.Abort();
            return JsonHelper.GetJson<List<ParkIORecord>>(result);
        }
        /// <summary>
        /// 消费打折
        /// </summary>
        /// <param name="IORecordID"></param>
        /// <param name="DerateID"></param>
        /// <param name="VID"></param>
        /// <param name="SellerID"></param>
        /// <param name="DerateMoney"></param>
        /// <param name="ProxyNo"></param>
        /// <returns></returns>
        public static ConsumerDiscountResult WXDiscountPlateNumber(string IORecordID, string DerateID, string VID, string SellerID, decimal DerateMoney)
        {
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            string result = client.WXDiscountPlateNumber(IORecordID, DerateID, VID, SellerID, DerateMoney,string.Empty);
            client.Close();
            client.Abort();
            return JsonHelper.GetJson<ConsumerDiscountResult>(result);
        }
        /// <summary>
        /// 获取商户信息
        /// </summary>
        /// <param name="SellerNo"></param>
        /// <param name="PWD"></param>
        /// <param name="SellerID"></param>
        /// <param name="ProxyNo"></param>
        /// <returns></returns>
        public static ParkSeller WXGetSellerInfo(string SellerNo, string PWD)
        {
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            string result = client.WXGetSellerInfo(SellerNo, PWD,string.Empty, string.Empty);
            client.Close();
            client.Abort();
            return JsonHelper.GetJson<ParkSeller>(result);
        }
        /// <summary>
        /// 修改商户密码
        /// </summary>
        /// <param name="SellerID"></param>
        /// <param name="Pwd"></param>
        /// <param name="ProxyNo"></param>
        /// <returns></returns>
        public static bool WXEditSellerPwd(string SellerID, string Pwd, string ProxyNo)
        {
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            bool result = client.WXEditSellerPwd(SellerID, Pwd, string.Empty);
            client.Close();
            client.Abort();
            return result;
        }
        /// <summary>
        /// 获取商家打折详情
        /// </summary>
        /// <param name="parms"></param>
        /// <param name="rows"></param>
        /// <param name="pageindex"></param>
        /// <param name="ProxyNo"></param>
        /// <returns></returns>
        public static Pagination WXGetParkCarDerate(string parms, int rows, int pageindex)
        {
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            string result = client.WXGetParkCarDerate(parms, rows, pageindex, string.Empty);
            client.Close();
            client.Abort();
            return JsonHelper.GetJson<Pagination>(result);
        }
    }
}
