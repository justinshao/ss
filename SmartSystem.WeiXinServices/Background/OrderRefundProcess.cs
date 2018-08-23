using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base.Common;
using System.Configuration;
using System.Web;
using Common.Entities.Order;
using Common.Services;
using Common.Entities;
using System.Web.Caching;

namespace SmartSystem.WeiXinServices.Background
{
    public class OrderRefundProcess
    {
        public static OrderRefundProcess Instance { get; private set; }
        static OrderRefundProcess()
        {
            Instance = new OrderRefundProcess();
        }
        public void Process()
        {
            try
            {
                List<BaseParkinfo> parkings = GetParkingBySupportAutoRefund();
                if (parkings.Count > 0)
                {
                    List<OnlineOrder> models = OnlineOrderServices.QueryWaitRefund(parkings.Select(p => p.PKID).ToList(), DateTime.Now.Date);
                    foreach (var item in models)
                    {
                        if (item.SyncResultTimes < 3) continue;
                        try
                        {
                            OnlineOrderServices.AutoRefund(item.OrderID, BackgroundWorkerManager.EnvironmentPath);
                        }
                        catch (Exception ex) {
                            TxtLogServices.WriteTxtLogEx("Background", "自动退款失败：orderId:{0},Message:{1},StackTrace:{2}", item.OrderID, ex.Message, ex.StackTrace);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("Background", "后台工作线程处理退款异常", ex, LogFrom.WeiXin);
            }
        }
        private List<BaseParkinfo> GetParkingBySupportAutoRefund()
        {
            Cache cache = HttpRuntime.Cache;
            if (cache["SupportAutoRefund_Parking_Cache"] != null)
            {
                return cache["SupportAutoRefund_Parking_Cache"] as List<BaseParkinfo>;

            }
            List<BaseParkinfo> parkings = ParkingServices.GetParkingBySupportAutoRefund();
            cache.Insert("SupportAutoRefund_Parking_Cache", parkings, null, DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration);
            return parkings;
        }
    }
}
