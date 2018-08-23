using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base.Common;
using Common.Services;
using Common.Entities;
using Common.Entities.Order;

namespace SmartSystem.WeiXinServices.Background
{
    public class SyncPayResultFailProcess
    {
        public static SyncPayResultFailProcess Instance { get; private set; }
        private static int[] IntervalTime = new int[] { 5,15,25,45,60,120};
        static SyncPayResultFailProcess()
        {
            Instance = new SyncPayResultFailProcess();
        }
        public void Process() {
            try
            {
                List<OnlineOrder> models = OnlineOrderServices.QueryBySyncPayResultFail();
                foreach (var item in models)
                {
                    int index = item.SyncResultTimes==0?0:item.SyncResultTimes - 1;
                    if (index > 5) {
                        continue;
                    }
                    if (item.LastSyncResultTime.AddSeconds(IntervalTime[index]) < DateTime.Now)
                    {
                        OnlineOrderServices.AgainSyncPayResult(item.OrderID, item.RealPayTime);
                    }
                }
            }
            catch (Exception ex) {
                ExceptionsServices.AddExceptionToDbAndTxt("Background", "后台工作线程同步处理支付结果失败", ex, LogFrom.WeiXin);
            }
        }
    }
}
