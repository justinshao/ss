using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartSystem.WeiXinInerface.WXService;
using Common.Utilities.Helpers;
using Common.Entities.WX;
using Common.Utilities;
using Common.Entities.Parking;

namespace SmartSystem.WeiXinInerface
{
    public class PkBitBookingServices
    {
        /// <summary>
        /// 车位预定
        /// </summary>
        /// <param name="AccountID">账户ID</param>
        /// <param name="BitNo">车位号(保留)</param>
        /// <param name="parking_id">车场ID</param>
        /// <param name="AreaID">停车区域ID</param>
        /// <param name="license_plate">车牌号码</param>
        /// <param name="start_time">开始时间</param>
        /// <param name="end_time">结束时间</param>
        /// <returns></returns>
        public static WXReserveBitResult WXReservePKBit(string AccountID, string BitNo, string parking_id, string AreaID, string license_plate, DateTime start_time, DateTime end_time)
        {
            //WXReserveBitResult model = new WXReserveBitResult();
            //model.code = 0;
            //Common.Entities.ZHDZ.ZHParkOrder Order = new Common.Entities.ZHDZ.ZHParkOrder();
            //Order.Amount=1;
            //Order.OrderID = IdGenerator.Instance.GetId().ToString();
            //Order.PKID = "063fa716-1325-4cc6-b65d-a76700a75bc2";
            //Order.ReserveID = IdGenerator.Instance.GetId().ToString();
            //model.Order = Order;
            //return model;
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            string result = client.WXReservePKBit(AccountID,BitNo,parking_id,AreaID,license_plate,start_time,end_time);
            client.Close();
            client.Abort();
            return JsonHelper.GetJson<WXReserveBitResult>(result);
        }
        /// <summary>
        /// 预约支付
        /// </summary>
        /// <param name="ReserveID">预约ID</param>
        /// <param name="OrderID">订单编号</param>
        /// <param name="Amount">金额</param>
        /// <param name="PKID">车场ID</param>
        /// <param name="OnlineOrderID">线上订单编码</param>
        /// <returns></returns>
        public static bool WXReserveBitPay(string ReserveID, string OrderID, decimal Amount, string PKID, string OnlineOrderID)
        {
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            bool result = client.WXReserveBitPay(ReserveID, OrderID, Amount, PKID, OnlineOrderID);
            client.Close();
            client.Abort();
            return result;
        }
        /// <summary>
        /// 预约记录
        /// </summary>
        /// <param name="AccountID">账号编号</param>
        /// <returns></returns>
        public static List<ParkReserveBit> WXReserveBitPay(string AccountID)
        {
            //List<ParkReserveBit> models = new List<ParkReserveBit>();
            //ParkReserveBit model = new ParkReserveBit();
            //model.PKName = "车场名称";
            //model.BitNo = "BitNo";
            //model.StartTime = DateTime.Now;
            //model.EndTime = DateTime.Now;
            //model.Remark = "Remark";
            //model.CreateTime = DateTime.Now;
            //model.PlateNumber = "PlateNumber";
            //model.Status = 0;
            //models.Add(model);

            //ParkReserveBit model1 = new ParkReserveBit();
            //model1.PKName = "车场名称";
            //model1.BitNo = "BitNo";
            //model1.StartTime = DateTime.Now;
            //model1.EndTime = DateTime.Now;
            //model1.Remark = "Remark";
            //model1.CreateTime = DateTime.Now;
            //model1.PlateNumber = "PlateNumber";
            //model1.Status = 0;
            //models.Add(model1);
            //return models;
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            string result = client.WXGetReservePKBit(AccountID);
            client.Close();
            client.Abort();
            return JsonHelper.GetJson<List<ParkReserveBit>>(result);
        }
    }
}
