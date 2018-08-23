using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Common.Utilities.Helpers;
using Common.Services;

namespace Common.ExternalInteractions.BWY
{
    public class BWYInterfaceProcess
    {
        private static string BWYInterfaceUrl = ConfigurationManager.AppSettings["BWYInterfaceUrl"] ?? "";
        private static string BWYSessionID = ConfigurationManager.AppSettings["BWYSessionID"] ?? "";

        /// <summary>
        /// 获取临停缴费信息
        /// </summary>
        /// <param name="plateNumber">车牌号</param>
        /// <returns></returns>
        public static BWYOrderQueryResult TempParkingFee(string plateNumber)
        {
            try
            {
                TxtLogServices.WriteTxtLogEx("BWYInterfaceProcess", string.Format("TempParkingFee方法，BWYInterfaceUrl:{0},BWYSessionID:{1}", BWYInterfaceUrl, BWYSessionID));

                if (string.IsNullOrWhiteSpace(BWYInterfaceUrl) || string.IsNullOrWhiteSpace(BWYSessionID))
                    return null;

                string webPageUrl = string.Format("{0}/ParkingCloud/Rest/OpenApi/ParkingRecord/Current/LPR/{1}", BWYInterfaceUrl.TrimEnd('/'), plateNumber);
                string strResult = BWYHttpHelper.RequestUrl(webPageUrl, BWYSessionID);
                TxtLogServices.WriteTxtLogEx("BWYInterfaceProcess", string.Format("TempParkingFee方法，strResult:{0}", strResult));
                BWYOrderQueryResult result = JsonHelper.GetJson<BWYOrderQueryResult>(strResult);
                TxtLogServices.WriteTxtLogEx("BWYInterfaceProcess", string.Format("TempParkingFee方法，转换模型:{0}", result == null ? "is null" : "not null"));
                return result;
            }
            catch (Exception ex) {
                TxtLogServices.WriteTxtLogEx("BWYInterfaceProcess", string.Format("TempParkingFee方法，异常:{0}"), ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 支付通知
        /// </summary>
        /// <param name="amount">金额（分）</param>
        /// <param name="parkingId">车场编号</param>
        /// <param name="orderId">订单编号</param>
        /// <returns></returns>
        public static BWYOrderPaymentResult PayNotice(int amount, string parkingId, string orderId)
        {
            try
            {
                TxtLogServices.WriteTxtLogEx("BWYInterfaceProcess", string.Format("PayNotice方法，BWYInterfaceUrl:{0},BWYSessionID:{1}", BWYInterfaceUrl, BWYSessionID));

                if (string.IsNullOrWhiteSpace(BWYInterfaceUrl) || string.IsNullOrWhiteSpace(BWYSessionID))
                    return null;

                TxtLogServices.WriteTxtLogEx("BWYInterfaceProcess", string.Format("PayNotice方法，amount:{0},parkingId:{1},orderId:{2}", amount, parkingId,orderId));
                BWYOrderPaymentModel model = new BWYOrderPaymentModel();
                model.FeeOfPayable = amount;
                model.FeeOfPaid = amount;
                model.CodeType = 1;
                model.PayState = 3;

                string content= JsonHelper.GetJsonString(model);
                string webPageUrl = string.Format("{0}/ParkingCloud/Rest/OpenApi/Bill/{1}/{2}", BWYInterfaceUrl.TrimEnd('/'), parkingId,orderId);
                string strResult = BWYHttpHelper.RequestUrl(webPageUrl, BWYSessionID, content,"PUT");
                TxtLogServices.WriteTxtLogEx("BWYInterfaceProcess", string.Format("PayNotice方法，strResult:{0}", strResult));
                BWYOrderPaymentResult result = JsonHelper.GetJson<BWYOrderPaymentResult>(strResult);
                TxtLogServices.WriteTxtLogEx("BWYInterfaceProcess", string.Format("PayNotice方法，转换模型:{0}", result == null ? "is null" : "not null"));
                return result;
            }
            catch (Exception ex)
            {
                TxtLogServices.WriteTxtLogEx("BWYInterfaceProcess", string.Format("PayNotice方法，异常:{0}"), ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 查询出口车辆信息
        /// </summary>
        public static OutCarResult QueryOutCar(int gateId)
        {
            try
            {
                TxtLogServices.WriteTxtLogEx("BWYInterfaceProcess", string.Format("QueryOutCar方法，BWYInterfaceUrl:{0},BWYSessionID:{1},gateId:{2}", BWYInterfaceUrl, BWYSessionID, gateId));

                if (string.IsNullOrWhiteSpace(BWYInterfaceUrl) || string.IsNullOrWhiteSpace(BWYSessionID))
                    return null;

                string webPageUrl = string.Format("{0}/ParkingCloud/Rest/OpenApi/VehicleOfPass/Entrance/{1}/Last", BWYInterfaceUrl.TrimEnd('/'), gateId);
                string strResult = BWYHttpHelper.RequestUrl(webPageUrl, BWYSessionID);
                TxtLogServices.WriteTxtLogEx("BWYInterfaceProcess", string.Format("QueryOutCar方法，strResult:{0}", strResult));
                OutCarResult result = JsonHelper.GetJson<OutCarResult>(strResult);
                TxtLogServices.WriteTxtLogEx("BWYInterfaceProcess", string.Format("QueryOutCar方法，转换模型:{0}", result == null ? "is null" : "not null"));
                return result;
            }
            catch (Exception ex)
            {
                TxtLogServices.WriteTxtLogEx("BWYInterfaceProcess", string.Format("QueryOutCar方法，异常:{0}"), ex.Message);
                return null;
            }
        }
    }
}
