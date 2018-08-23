using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartSystem.WeiXinInerface.WXService;
using Common.Utilities.Helpers;
using Common.Entities;
using Common.Entities.WX;
using Common.Services;

namespace SmartSystem.WeiXinInerface
{
    public class CarService
    {
        /// <summary>
        /// 获取用户车牌信息
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public static List<WX_CarInfo> GetCarInfoByAccountID(string accountID)
        {
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            string plates = client.GetCarInfoByAccountID(accountID);
            client.Close();
            client.Abort();
            if (string.IsNullOrWhiteSpace(plates)) {
                return new List<WX_CarInfo>();
            }
            return JsonHelper.GetJson<List<WX_CarInfo>>(plates);
        }
        /// <summary>
        /// 添加车牌号
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int AddWX_CarInfo(WX_CarInfo model)
        {
            string plate = JsonHelper.GetJsonString(model);
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            int result = client.AddWX_CarInfo(plate);
            client.Close();
            client.Abort();
            return result;
        }
        /// <summary>
        /// 删除车牌信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DelWX_CarInfo(string recordId)
        {
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            bool result = client.DelWX_CarInfo(recordId);
            client.Close();
            client.Abort();
            return result;
        }
        /// <summary>
        /// 查询信息
        /// </summary>
        /// <param name="accountplate"></param>
        /// <returns></returns>
        public static WX_CarInfo GetCarInfoByPlateNo(string accountId, string plateNo)
        {
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            string result = client.GetCarInfoByPlateNo(accountId, plateNo);
            client.Close();
            client.Abort();
            return JsonHelper.GetJson<WX_CarInfo>(result);
        }
        /// <summary>
        /// 查询待锁车或已锁车车辆
        /// </summary>
        /// <param name="accountID"></param>
        public static List<WX_LockCar> GetLockCarByAccountID(string accountID)
        {
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
           string result = client.GetLockCarByAccountID(accountID);
            client.Close();
            client.Abort();
            if (string.IsNullOrWhiteSpace(result))
            {
                return new List<WX_LockCar>();
            }
            return JsonHelper.GetJson<List<WX_LockCar>>(result);
        }
        /// <summary>
        /// 锁车
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="parkingID"></param>
        /// <param name="plateNumber"></param>
        /// <param name="systemID"></param>
        /// <returns>0成功,1代理断开,2失败</returns>
        public static int WXLockCarInfo(string accountID, string parkingID, string plateNumber)
        {
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            int result = client.WXLockCarInfo(accountID, parkingID, plateNumber);
            client.Close();
            client.Abort();
            return result; 
        }
        /// <summary>
        /// 解锁
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="parkingID"></param>
        /// <param name="plateNumber"></param>
        /// <param name="systemID"></param>
        /// <returns>0成功,1代理断开,2失败</returns>
        public static int WXUlockCarInfo(string accountID, string parkingID, string plateNumber)
        {

            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            int result = client.WXUlockCarInfo(accountID, parkingID, plateNumber);
            client.Close();
            client.Abort();
            return result; 
        }
        /// <summary>
        /// 获取待缴费的车辆信息
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public static List<string> GetTempCarInfoIn(string accountID)
        {
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            List<string> result = client.GetTempCarInfoIn(accountID).ToList();
            client.Close();
            client.Abort();
            if (result == null) {
                return new List<string>();
            }
            return result; 
        }

        /// <summary>
        /// 测试代理服务是否开启
        /// </summary>
        /// <param name="VillageID">小区编号</param>
        /// <returns></returns>
        public static bool WXTestClientProxyConnectionByVID(string VillageID)
        {
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            bool result = client.WXTestClientProxyConnectionByVID(VillageID);
            client.Close();
            client.Abort();
            return result; 
        }
        /// <summary>
        /// 测试代理服务是否开启
        /// </summary>
        /// <param name="ParkingID">车场编号</param>
        /// <param name="SystemID"></param>
        /// <returns></returns>
        public static bool WXTestClientProxyConnectionByPKID(string ParkingID)
        {
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            bool result = client.WXTestClientProxyConnectionByPKID(ParkingID);
            client.Close();
            client.Abort();
            return result; 
        }
    }
}
