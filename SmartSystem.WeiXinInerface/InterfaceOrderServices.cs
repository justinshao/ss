using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartSystem.WeiXinInerface.WXService;
using SmartSystem.WeiXinInerface;

namespace SmartSystem.WXInerface
{
    public class InterfaceOrderServices
    {
        /// <summary>
        /// 判断订单是否有效
        /// </summary>
        /// <param name="interfaceOrderId">接口订单号</param>
        /// <returns>0-订单已失效 1-可正常支付 2-重复支付</returns>
        public static int OrderWhetherEffective(string interfaceOrderId)
        {
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            int result = client.OrderWhetherEffective(interfaceOrderId);
            client.Close();
            client.Abort();
            return result;
         
        }


        /// <summary>
        /// 判断订单是否有效
        /// </summary>
        /// <param name="interfaceOrderId">接口订单号</param>
        /// <returns>0-订单已失效 1-可正常支付 2-重复支付</returns>
        public static int OrderWhetherEffective(string interfaceOrderId, string parkingID, string ioRecordID)
        {
            WXServiceClient client = ServiceUtil<WXServiceClient>.GetServiceClient("WXService");
            int result = client.OrderWhetherEffectiveNew(interfaceOrderId, parkingID, ioRecordID);
            client.Close();
            client.Abort();
            return result;

        }
    }
}
