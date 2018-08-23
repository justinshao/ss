using Common.Entities.Order;
using Common.Services;
using Common.Utilities.Helpers;
using SmartSystem.AliPayServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartSystem.AliPayServices
{
    public class AliPayPay
    {
        public static bool Run(OnlineOrder order,out string sDataInfo)
        {
            sDataInfo = "";

            string parkid = order.PKID;
            AlipayTradeOrderModel model = new AlipayTradeOrderModel();
            model.out_trade_no = order.OrderID.ToString();
            model.total_amount = order.Amount.ToString("F2");
            model.discountable_amount = "";
            model.undiscountable_amount = "";
            model.seller_id = order.PayeeAccount;
            model.buyer_logon_id = "";
            model.subject = order.PKName + "," + order.PlateNo + "," + order.EntranceTime.ToString("yyyy-MM-dd HH:mm:ss");
            model.buyer_id = order.PayAccount;
            model.store_id = parkid;
            model.body = string.Format("临停缴费-{0}-{1}", order.PKName, order.PlateNo);
            TxtLogServices.WriteTxtLogEx("AliPayApiServices", JsonHelper.GetJsonString(model));
            string tradeNo = AliPayApiServices.BarCodePayOrder(order.CompanyID, model);
            if (string.IsNullOrWhiteSpace(tradeNo))
            {
                return false;
            }

            if (tradeNo.Length > 2)
            {
                sDataInfo = tradeNo;
                return true;
            }

            int queryTimes = 10;//查询次数计数器
            while (queryTimes-- > 0)
            {
                bool isSuc = Query(order.CompanyID, order.OrderID.ToString());
                if (isSuc)
                {
                    sDataInfo = tradeNo;
                    return true;
                }
            }

            return false;
        }

        public static bool Query(string companyID, string out_trade_no)
        {
            DateTime payTime = DateTime.Now;
            return  AliPayApiServices.QueryPayResult(companyID, out_trade_no, "", out payTime);
            
        }


        public static bool Cancel(string companyID, string out_trade_no,string trade_no)
        {
            return AliPayApiServices.CancelOrder(companyID, out_trade_no, trade_no);
        }
    }
}
