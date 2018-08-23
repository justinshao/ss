using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utilities.Helpers;
using System.Configuration;
using Common.Services;

namespace Common.ExternalInteractions.SFM
{
    public class SFMInterfaceProcess
    {
        private static string SFMSecretKey = ConfigurationManager.AppSettings["SFMSecretKey"] ?? "";
        private static string SFMInterfaceUrl = ConfigurationManager.AppSettings["SFMInterfaceUrl"] ?? "";
        /// <summary>
        /// 车牌获取停车费用
        /// </summary>
        /// <param name="plateNumber">车牌号</param>
        /// <returns></returns>
        public static PlateQueryResult GetCarPrice(string plateNumber)
        {
            try
            {
                TxtLogServices.WriteTxtLogEx("SFMError", string.Format("GetCarPrice方法，plateNumber：{0}", plateNumber));

                if (string.IsNullOrWhiteSpace(SFMSecretKey)) throw new MyException("获取密钥失败");
                if (string.IsNullOrWhiteSpace(SFMInterfaceUrl)) throw new MyException("获取请求地址失败");

                string result = HttpHelper.RequestUrl(string.Format("{0}ApiPlatform/GetCarPrice", SFMInterfaceUrl),
                                                  new List<KeyValuePair<string, string>>
                                                      {
                                                                        new KeyValuePair<string,string>("SecretKey",SFMSecretKey),
                                                                        new KeyValuePair<string, string>("carNo",plateNumber)
                                             
                                                      }, HttpMethod.GET);
                TxtLogServices.WriteTxtLogEx("SFMError", string.Format("GetCarPrice result:{0}", result));
                return JsonHelper.GetJson<PlateQueryResult>(result);
            }
            catch (Exception ex)
            {
                TxtLogServices.WriteTxtLogEx("SFMError", string.Format("GetCarPrice方法，异常：{0},StackTrace:{1}", ex.Message, ex.StackTrace));
                return null;
            }
        }
        /// <summary>
        /// 创建支付订单
        /// </summary>
        /// <param name="orderNo">停车订单号码</param>
        /// <returns></returns>
        public static SFMResult CarOrderPay(string orderNo)
        {
            try
            {
                TxtLogServices.WriteTxtLogEx("SFMError", string.Format("CarOrderPay方法，orderNo：{0}", orderNo));

                if (string.IsNullOrWhiteSpace(SFMSecretKey)) throw new MyException("获取密钥失败");
                string result = HttpHelper.RequestUrl(string.Format("{0}/ApiPlatform/CarOrderPay", SFMInterfaceUrl),
                                                   new List<KeyValuePair<string, string>>
                                                          {
                                                                            new KeyValuePair<string,string>("SecretKey",SFMSecretKey),
                                                                            new KeyValuePair<string, string>("orderNo",orderNo)
                                             
                                                          }, HttpMethod.GET);
                TxtLogServices.WriteTxtLogEx("SFMError", string.Format("CarOrderPay result:{0}", result));

                return JsonHelper.GetJson<SFMResult>(result);
            }
            catch (Exception ex)
            {
                TxtLogServices.WriteTxtLogEx("SFMError", string.Format("CarOrderPay方法，异常：{0},StackTrace:{1}", ex.Message, ex.StackTrace));
                return null;
            }

        }
        /// <summary>
        /// 订单支付回调
        /// </summary>
        /// <param name="payOrder">支付订单号</param>
        /// <param name="payedSN">交易订单号</param>
        /// <param name="payedMoney">实际支付金额 单位元</param>
        /// <param name="isPayScene">是出口扫码支付</param>
        /// <returns></returns>
        public static SFMResult PayNotify(string payOrder, string payedSN, string payedMoney, bool isPayScene)
        {
            try
            {
                TxtLogServices.WriteTxtLogEx("SFMError", string.Format("PayNotify方法，payOrder：{0},payedSN：{1},payedMoney:{2},isPayScene:{3}", payOrder, payedSN, payedMoney, isPayScene));
                string result = string.Empty;
                if (isPayScene)
                {
                    result = HttpHelper.RequestUrl(string.Format("{0}ApiPlatform/PayNotify", SFMInterfaceUrl),
                                  new List<KeyValuePair<string, string>>
                                                          {
                                                                            new KeyValuePair<string,string>("SecretKey",SFMSecretKey),
                                                                            new KeyValuePair<string, string>("payOrder",payOrder),
                                                                            new KeyValuePair<string, string>("payedSN",payedSN),
                                                                            new KeyValuePair<string, string>("payedMoney",payedMoney),
                                                                            new KeyValuePair<string, string>("isPayScene","false")
                                             
                                                          }, HttpMethod.GET);
                }
                else
                {
                    result = HttpHelper.RequestUrl(string.Format("{0}ApiPlatform/PayNotify", SFMInterfaceUrl),
                                                      new List<KeyValuePair<string, string>>
                                                          {
                                                                            new KeyValuePair<string,string>("SecretKey",SFMSecretKey),
                                                                            new KeyValuePair<string, string>("payOrder",payOrder),
                                                                            new KeyValuePair<string, string>("payedSN",payedSN),
                                                                            new KeyValuePair<string, string>("payedMoney",payedMoney)
                                             
                                                          }, HttpMethod.GET);
                }
                TxtLogServices.WriteTxtLogEx("SFMError", string.Format("PayNotify result:{0}", result));
                return JsonHelper.GetJson<SFMResult>(result);
            }
            catch (Exception ex)
            {
                TxtLogServices.WriteTxtLogEx("SFMError", string.Format("PayNotify方法，异常：{0},StackTrace:{1}", ex.Message, ex.StackTrace));
                return null;
            }

        }
        /// <summary>
        /// 获取所有车场信息
        /// </summary>
        /// <returns></returns>
        public static SFMParkInfoResult GetParkInfo()
        {
            try
            {
                TxtLogServices.WriteTxtLogEx("SFMError", "GetParkInfo 开始 KEY:" + SFMSecretKey);

                string result = HttpHelper.RequestUrl(string.Format("{0}ApiPlatform/getParkInfo", SFMInterfaceUrl),
                                                    new List<KeyValuePair<string, string>>
                                                                      {
                                                                                new KeyValuePair<string,string>("SecretKey",SFMSecretKey),
                                                                                new KeyValuePair<string,string>("pageIndex","1"),
                                                                                new KeyValuePair<string,string>("pageSize","100000")
                                             
                                                                      }, HttpMethod.GET);
                TxtLogServices.WriteTxtLogEx("SFMError", string.Format("GetParkInfo result:{0}", result));

                return JsonHelper.GetJson<SFMParkInfoResult>(result);
            }
            catch (Exception ex)
            {
                TxtLogServices.WriteTxtLogEx("SFMError", string.Format("GetParkInfo方法，异常：{0},StackTrace:{1}", ex.Message, ex.StackTrace));
                return null;
            }
        }
        /// <summary>
        /// 出口扫码支付
        /// </summary>
        /// <param name="parkNo"></param>
        /// <param name="gateId"></param>
        /// <returns></returns>
        public static OutCarInfoResult QueryOutCarOrder(string parkNo, string gateId)
        {
            string result = HttpHelper.RequestUrl(string.Format("{0}/ApiPlatform/QueryOutCarOrder", SFMInterfaceUrl),
                      new List<KeyValuePair<string, string>>
                              {
                                        new KeyValuePair<string,string>("secretKey",SFMSecretKey),
                                        new KeyValuePair<string,string>("parkId",parkNo),
                                        new KeyValuePair<string,string>("ctlno",gateId)
                                             
                              }, HttpMethod.GET);

            TxtLogServices.WriteTxtLogEx("SFMError", string.Format("SecretKey:{0}", SFMSecretKey));
            TxtLogServices.WriteTxtLogEx("SFMError", string.Format("parkId:{0}", parkNo));
            TxtLogServices.WriteTxtLogEx("SFMError", string.Format("ctlno:{0}", gateId));
            TxtLogServices.WriteTxtLogEx("SFMError", string.Format("QueryOutCarOrder result:{0}", result));

            return JsonHelper.GetJson<OutCarInfoResult>(result);
        }
    }
}
