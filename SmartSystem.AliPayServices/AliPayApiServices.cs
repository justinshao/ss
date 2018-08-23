using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Services.AliPay;
using Common.Entities.AliPay;
using System.Web;
using Aop.Api.Request;
using Aop.Api.Response;
using Aop.Api;
using Common.Services;
using Aop.Api.Util;
using SmartSystem.AliPayServices.Entities;
using Common.Utilities.Helpers;
using Com.Alipay;
using Com.Alipay.Domain;
using Com.Alipay.Business;
using Com.Alipay.Model;

namespace SmartSystem.AliPayServices
{
    public class AliPayApiServices
    {
        public static string GetPublicAppAuthorizeUrl(string state, AliPayApiConfig config)
        {
            if (config == null) throw new MyException("获取支付宝配置失败");

            string redirect_uri = HttpUtility.UrlEncode(string.Format("{0}/AliPayAuthorize/Index", config.SystemDomain));
            return string.Format("{0}?app_id={1}&scope=auth_base&redirect_uri={2}&state={3}", AliPayParam.authUrl, config.AppId, redirect_uri, state);
        
        }
        private static void CheckApiConfigInfo(string companyId) {
            AliPayApiConfig config = AliPayApiConfigServices.QueryAliPayConfig(companyId);
            if (config == null) throw new MyException("获取支付宝配置失败");
            if (string.IsNullOrWhiteSpace(config.SystemDomain)) throw new MyException("获取支付宝SystemDomain配置失败");
            if (string.IsNullOrWhiteSpace(config.AppId)) throw new MyException("获取支付宝AppId配置失败");
            if (string.IsNullOrWhiteSpace(config.PrivateKey)) throw new MyException("获取支付宝PrivateKey配置失败");
            if (string.IsNullOrWhiteSpace(config.PublicKey)) throw new MyException("获取支付宝PublicKey配置失败");
            if (string.IsNullOrWhiteSpace(config.SystemName)) throw new MyException("获取支付宝SystemName配置失败");
            if (string.IsNullOrWhiteSpace(config.PayeeAccount)) throw new MyException("获取支付宝PayeeAccount配置失败");
        }
        public static IAopClient GetDefaultAopClient(string companyId)
        {
            AliPayApiConfig config = AliPayApiConfigServices.QueryAliPayConfig(companyId);
            if (config == null) throw new MyException("获取支付宝配置失败");

            string signType = config.AliPaySignType == 0 ? "RSA" : "RSA2";
            return new DefaultAopClient(AliPayParam.serverUrl, config.AppId, config.PrivateKey, AliPayParam.format, AliPayParam.version, signType, config.PublicKey, AliPayParam.charset, false);
        }

        public static IAlipayTradeService GetDefaultF2FClient(string companyId)
        {
            AliPayApiConfig config = AliPayApiConfigServices.QueryAliPayConfig(companyId);
            if (config == null) throw new MyException("获取支付宝配置失败");

            string signType = config.AliPaySignType == 0 ? "RSA" : "RSA2";
            return F2FBiz.CreateClientInstance(AliPayParam.serverUrl, config.AppId, config.PrivateKey, AliPayParam.version,
                             signType, config.PublicKey, "utf-8");
        }


        #region 获取用户授权
        public static string GetAccessToken(string companyId,string auth_code, ref string userId)
        {
            try
            {
                AlipaySystemOauthTokenRequest request = new AlipaySystemOauthTokenRequest();
                request.GrantType = "authorization_code";
                request.Code = auth_code;
                AlipaySystemOauthTokenResponse response = GetDefaultAopClient(companyId).Execute(request);
                if (response.IsError)
                {
                    TxtLogServices.WriteTxtLogEx("AliPayApiServices",string.Format("GetAccessToken(),获取用户授权失败:" + auth_code + ":{0}", response.Body));
                }
                else
                {
                    userId = response.UserId;
                    return response.AccessToken;
                }
            }
            catch (Exception ex)
            {
                TxtLogServices.WriteTxtLogEx("AliPayApiServices", string.Format("GetAccessToken()获取用户授权失败:" + auth_code + ":{0}", ex.Message));
            }
            return "";
        }
        #endregion

        #region 用户登录授权 auth_base auth_user
        public static string LoginAccess(string companyId,string scopes, string state, string returnUrl)
        {
            try
            {
                AlipayUserInfoAuthRequest request = new AlipayUserInfoAuthRequest();
                request.SetReturnUrl(returnUrl);
                request.BizContent = "{" +
                "\"scopes\":[" +
                "\"auth_base\"" +
                "]," +
                "\"state\":\"init\"," +
                "\"is_mobile\":\"true\"" +
                "}";
                AlipayUserInfoAuthResponse response = GetDefaultAopClient(companyId).Execute(request);
                if (response.IsError)
                {
                    TxtLogServices.WriteTxtLogEx("AliPayApiServices",string.Format("LoginAccess()用户登录授权失败" + ":{0}", response.Body));
                }
                else
                {
                    TxtLogServices.WriteTxtLogEx("AliPayApiServices",string.Format("LoginAccess()用户登录授权成功" + ":{0}", response.Body));
                    return "";
                }
            }
            catch (Exception ex)
            {
                TxtLogServices.WriteTxtLogEx("AliPayApiServices", string.Format("LoginAccess()用户登录授权失败,Message:{0},StackTrace:{1}", ex.Message, ex.StackTrace));
            }
            return "";
        }
        #endregion

        #region 静默获取用户ID
        public static string GetUserId(string companyId,string auth_code)
        {
            try
            {
                AlipayUserUserinfoShareRequest request = new AlipayUserUserinfoShareRequest();
                AlipayUserUserinfoShareResponse response = GetDefaultAopClient(companyId).Execute(request);
                if (response.IsError)
                {
                    TxtLogServices.WriteTxtLogEx("AliPayApiServices", string.Format("GetUserId()获取用户授权失败,auth_code:{0},body:{1}", auth_code, response.Body));
                }
                else
                {
                    return response.UserId;
                }
            }
            catch (Exception ex)
            {
                TxtLogServices.WriteTxtLogEx("AliPayApiServices", string.Format("GetUserId()获取用户授权失败,auth_code:{0},Message:{1}", auth_code, ex.Message));
            }
            return "";
        }
        #endregion

        #region 获取车牌信息
        public static string GetPlateNumber(string companyId,string car_id, string accessToken)
        {
            try
            {
                AlipayEcoMycarParkingVehicleQueryRequest request = new AlipayEcoMycarParkingVehicleQueryRequest();
                request.BizContent = "{\"car_id\":\"" + car_id + "\"}";
                AlipayEcoMycarParkingVehicleQueryResponse response = GetDefaultAopClient(companyId).Execute(request, accessToken);
                if (!response.IsError)
                {
                    return response.CarNumber;
                }
                else
                {
                    TxtLogServices.WriteTxtLogEx("AliPayApiServices", string.Format("获取用户车牌信息失败:{0}", response.Body));
                }
            }
            catch (Exception ex)
            {
                TxtLogServices.WriteTxtLogEx("AliPayApiServices", string.Format("获取车牌信息结果:{0}", ex.Message));
            }
            return "";
        }
        #endregion

        #region 统一下单
        public static string MakeAliPayOrder(string companyId,AlipayTradeOrderModel model)
        {
            try
            {
                AliPayApiConfig config = AliPayApiConfigServices.QueryAliPayConfig(companyId);
                if (config == null) throw new MyException("获取支付宝配置失败");

                string aliPayNotifyUrl = string.Format("{0}/AliPayNotify",config.SystemDomain);
                AlipayTradeCreateRequest request = new AlipayTradeCreateRequest();
                request.BizContent = JsonHelper.GetJsonString(model);
                request.SetNotifyUrl(aliPayNotifyUrl);
                AlipayTradeCreateResponse response = GetDefaultAopClient(companyId).Execute(request);
                if (response.IsError)
                {
                    TxtLogServices.WriteTxtLogEx("AliPayApiServices", string.Format("统一下单失败1:{0}", response.Body));
                    return "";
                }
                return response.TradeNo;
            }
            catch (Exception ex)
            {
                TxtLogServices.WriteTxtLogEx("AliPayApiServices", string.Format("统一下单失败2:{0}", ex.Message));
            }
            return "";
        }
        #endregion

        #region 当面付支付交易

        public static string BarCodePayOrder(string companyId, AlipayTradeOrderModel model)
        {

            try
            {
                AliPayApiConfig config = AliPayApiConfigServices.QueryAliPayConfig(companyId);
                if (config == null) throw new MyException("获取支付宝配置失败");

                IAlipayTradeService serviceClient = GetDefaultF2FClient(companyId);
                
                AlipayTradePayContentBuilder builder = BuildPayContent(model);
                string out_trade_no = builder.out_trade_no;

                AlipayF2FPayResult payResult = serviceClient.tradePay(builder);

                TxtLogServices.WriteTxtLogEx("AliPayApiServices", string.Format("F2F支付:{0}\r\nResult:{1}",builder.ToXml(Encoding.GetEncoding("GB2312")), payResult.response.Body));
                switch (payResult.Status)
                {
                    case ResultEnum.SUCCESS:
                        return payResult.response.TradeNo;
                    case ResultEnum.FAILED:
                        return "";
                    case ResultEnum.UNKNOWN:
                        return "-1";
                }
            }
            catch (Exception ex)
            {
                TxtLogServices.WriteTxtLogEx("AliPayApiServices", string.Format("统一下单失败2:{0}", ex.Message));
            }

            return "-2";
        }


        /// <summary>
        /// 构造支付请求数据
        /// </summary>
        /// <returns>请求数据集</returns>
        private static AlipayTradePayContentBuilder BuildPayContent(AlipayTradeOrderModel model)
        {
            //线上联调时，请输入真实的外部订单号。
            string out_trade_no = model.out_trade_no;

            //扫码枪扫描到的用户手机钱包中的付款条码
            AlipayTradePayContentBuilder builder = new AlipayTradePayContentBuilder();

            //收款账号
            builder.seller_id = model.seller_id;
            //订单编号
            builder.out_trade_no = out_trade_no;
            //支付场景，无需修改
            builder.scene = "bar_code";
            //支付授权码,付款码
            builder.auth_code = model.buyer_id;
            //订单总金额
            builder.total_amount = model.total_amount;
            //参与优惠计算的金额
            builder.discountable_amount = model.discountable_amount;
            //不参与优惠计算的金额
            builder.undiscountable_amount = model.undiscountable_amount;
            //订单名称
            builder.subject = model.subject;
            //自定义超时时间
            builder.timeout_express = "10m";
            //订单描述
            builder.body = model.body;
            //门店编号，很重要的参数，可以用作之后的营销
            builder.store_id = model.store_id;
            //操作员编号，很重要的参数，可以用作之后的营销
            builder.operator_id = "HAND";


            //传入商品信息详情
            List<GoodsInfo> gList = new List<GoodsInfo>();

            GoodsInfo goods = new GoodsInfo();
            goods.goods_id = "";
            goods.goods_name = model.body;
            goods.price = model.total_amount;
            goods.quantity = "1";
            gList.Add(goods);
            builder.goods_detail = gList;

            //系统商接入可以填此参数用作返佣
            //ExtendParams exParam = new ExtendParams();
            //exParam.sysServiceProviderId = "20880000000000";
            //builder.extendParams = exParam;

            return builder;

        }

        #endregion

        #region 查询订单交易状态
        public static bool QueryPayResult(string companyId,string out_trade_no, string trade_no,out DateTime payTime)
        {
            payTime = DateTime.Now;
            try
            {
               
                AlipayTradeQueryRequest request = new AlipayTradeQueryRequest();
                request.BizContent = "{" +
                "\"out_trade_no\":\"" + out_trade_no + "\"," +
                "\"trade_no\":\"" + trade_no + "\"" +
                "}";
                AlipayTradeQueryResponse response = GetDefaultAopClient(companyId).Execute(request);
                if (response.IsError)
                {
                    TxtLogServices.WriteTxtLogEx("AliPayApiServices", string.Format("查询支付宝交易状态失败:{0}", response.Body));
                    return false;
                }
                TxtLogServices.WriteTxtLogEx("AliPayApiServices", string.Format("查询支付状态结果:{0}", response.Body));
                if (response.TradeStatus == "TRADE_SUCCESS" || response.TradeStatus == "TRADE_FINISHED")
                {
                    TxtLogServices.WriteTxtLogEx("AliPayApiServices", string.Format("查询支付状态结果 支付时间:{0}", response.SendPayDate));
                    DateTime.TryParse(response.SendPayDate, out payTime);
                    return true;
                }
            }
            catch (Exception ex)
            {
                TxtLogServices.WriteTxtLogEx("AliPayApiServices", string.Format("查询支付宝交易状态失败:{0}", ex.Message));
            }
            return false;
        }
        #endregion

        #region 退款请求
        /// <summary>
        /// 退款请求
        /// </summary>
        /// <param name="out_trade_no">订单编号</param>
        /// <param name="trade_no">交易单好</param>
        /// <param name="refund_amount">退款金额（元）</param>
        /// <param name="refund_reason">原因</param>
        /// <param name="out_request_no">退款订单号</param>
        /// <returns></returns>
        public static bool RefundRequest(string companyId,string out_trade_no, string trade_no, string refund_amount, string refund_reason, string out_request_no)
        {
            try
            {
                AlipayTradeRefundRequest request = new AlipayTradeRefundRequest();
                request.BizContent = "{" +
                "\"out_trade_no\":\"" + out_trade_no + "\"," +
                "\"trade_no\":\"" + trade_no + "\"," +
                "\"refund_amount\":" + refund_amount + "," +
                "\"refund_reason\":\"" + refund_reason + "\"," +
                "\"out_request_no\":\"" + out_request_no + "\"" +
                "}";
                AlipayTradeRefundResponse response = GetDefaultAopClient(companyId).Execute(request);
                if (response.IsError)
                {
                    TxtLogServices.WriteTxtLogEx("AliPayApiServices", string.Format("退款请求失败:{0}", response.Body));
                    return false;
                }
                TxtLogServices.WriteTxtLogEx("AliPayApiServices",string.Format("退款请求结果:{0}", response.Body));
                return response.Code == "10000";
            }
            catch (Exception ex)
            {
                TxtLogServices.WriteTxtLogEx("AliPayApiServices",string.Format("退款请求失败:{0}", ex.Message));
                return false;
            }
        }
        #endregion

        #region 统一收单交易撤销接口
        /// <summary>
        /// 统一收单交易撤销接口
        /// 支付交易返回失败或支付系统超时，调用该接口撤销交易。如果此订单用户支付失败，支付宝系统会将此订单关闭；如果用户支付成功，支付宝系统会将此订单资金退还给用户
        /// </summary>
        /// <param name="out_trade_no">支付宝订单号</param>
        /// <param name="trade_no">平台编号</param>
        /// <returns></returns>
        public static bool CancelOrder(string companyId,string out_trade_no, string trade_no)
        {
            try
            {
                AlipayTradeCancelRequest request = new AlipayTradeCancelRequest();
                request.BizContent = "{" +
                "\"out_trade_no\":\"" + out_trade_no + "\"," +
                "\"trade_no\":\"" + trade_no + "\"" +
                "}";
                AlipayTradeCancelResponse response = GetDefaultAopClient(companyId).Execute(request);
                if (response.IsError)
                {
                    TxtLogServices.WriteTxtLogEx("AliPayApiServices",string.Format("取消订单失败:{0}", response.Body));
                    return false;
                }
                TxtLogServices.WriteTxtLogEx("AliPayApiServices",string.Format("取消订单结果:{0}", response.Body));
                return response.Code == "10000";
            }
            catch (Exception ex)
            {
                TxtLogServices.WriteTxtLogEx("AliPayApiServices",string.Format("取消订单失败:{0}", ex.Message));
                return false;
            }
        }
        #endregion

        #region 统一收单交易退款查询
        /// <summary>
        /// 统一收单交易退款查询
        /// </summary>
        /// <param name="trade_no">支付宝订单号</param>
        /// <param name="out_trade_no">平台订单号</param>
        /// <param name="out_request_no">退款申请单号</param>
        /// <returns></returns>
        public static AlipayTradeFastpayRefundQueryResponse QueryRefundOrder(string companyId,string trade_no, string out_trade_no, string out_request_no)
        {
            try
            {
                AlipayTradeFastpayRefundQueryRequest request = new AlipayTradeFastpayRefundQueryRequest();
                request.BizContent = "{" +
                "\"trade_no\":\"" + trade_no + "\"," +
                "\"out_trade_no\":\"" + out_trade_no + "\"," +
                "\"out_request_no\":\"" + out_request_no + "\"" +
                "}";
                AlipayTradeFastpayRefundQueryResponse response = GetDefaultAopClient(companyId).Execute(request);
                if (response.IsError)
                {
                    TxtLogServices.WriteTxtLogEx("AliPayApiServices",string.Format("统一收单交易退款查询失败:{0}", response.Body));
                    throw new MyException("交易退款查询失败");
                }
                TxtLogServices.WriteTxtLogEx("AliPayApiServices",string.Format("交易退款查询结果:{0}", response.Body));
                return response;
            }
            catch (Exception ex)
            {
                TxtLogServices.WriteTxtLogEx("AliPayApiServices",string.Format("统一收单交易退款查询失败:{0}", ex.Message));
                throw new MyException("交易退款查询失败");
            }
        }
        #endregion

        #region 统一收单交易关闭接口
        /// <summary>
        /// 统一收单交易关闭接口
        /// 用于交易创建后，用户在一定时间内未进行支付，可调用该接口直接将未付款的交易进行关闭。
        /// </summary>
        /// <param name="out_trade_no"></param>
        /// <param name="trade_no"></param>
        /// <param name="operator_id"></param>
        /// <returns></returns>
        public static bool CloseOrder(string companyId,string out_trade_no, string trade_no, string operator_id = "")
        {
            try
            {
                AlipayTradeCloseRequest request = new AlipayTradeCloseRequest();
                request.BizContent = "{" +
                "\"trade_no\":\"" + trade_no + "\"," +
                "\"out_trade_no\":\"" + out_trade_no + "\"," +
                "\"operator_id\":\"" + operator_id + "\"" +
                "}";
                AlipayTradeCloseResponse response = GetDefaultAopClient(companyId).Execute(request);
                if (response.IsError)
                {
                    TxtLogServices.WriteTxtLogEx("AliPayApiServices",string.Format("统一收单交易关闭接口失败:{0}", response.Body));
                    throw new MyException("关闭订单失败");
                }
                TxtLogServices.WriteTxtLogEx("AliPayApiServices",string.Format("统一收单交易关闭接口结果:{0}", response.Body));
                return response.Code == "10000";
            }
            catch (Exception ex)
            {
                TxtLogServices.WriteTxtLogEx("AliPayApiServices",string.Format("统一收单交易关闭接口失败:{0}", ex.Message));
                return false;
            }
        }
        #endregion

        #region 预下单
        public static string MakePreAliPayOrder(string companyId,PreAliPayOrderModel model)
        {
            try
            {
                AliPayApiConfig config = AliPayApiConfigServices.QueryAliPayConfig(companyId);
                if (config == null) throw new MyException("获取支付宝配置失败");

                string aliPayNotifyUrl = string.Format("{0}/AliPayNotify", config.SystemDomain);
                AlipayTradePrecreateRequest request = new AlipayTradePrecreateRequest();
                request.BizContent = JsonHelper.GetJsonString(model);
                request.SetNotifyUrl(aliPayNotifyUrl);
                AlipayTradePrecreateResponse response = GetDefaultAopClient(companyId).Execute(request);
                if (response.IsError)
                {
                    TxtLogServices.WriteTxtLogEx("AliPayApiServices",string.Format("预下单失败:{0}", response.Body));
                    return "";
                }
                return response.QrCode;
            }
            catch (Exception ex)
            {
                TxtLogServices.WriteTxtLogEx("AliPayApiServices",string.Format("预下单失败:{0}", ex.Message));
            }
            return "";
        }
        #endregion

        #region 手机网站支付
        public static string MakeWapPayOrder(string companyId,PreAliPayOrderModel model, string returnUrl, string notifyUrl)
        {
            try
            {
                AlipayTradeWapPayRequest request = new AlipayTradeWapPayRequest();
                request.BizContent = JsonHelper.GetJsonString(model);
                request.SetNotifyUrl(notifyUrl);
                request.SetReturnUrl(returnUrl);
                AlipayTradeWapPayResponse response = GetDefaultAopClient(companyId).pageExecute(request);
                if (response.IsError)
                {
                    TxtLogServices.WriteTxtLogEx("AliPayApiServices",string.Format("wap网站支付失败:{0}", response.Body));
                    return "";
                }
                return response.Body;
            }
            catch (Exception ex)
            {
                TxtLogServices.WriteTxtLogEx("AliPayApiServices",string.Format("网站支付失败:{0}", ex.Message));
            }
            return "";
        }
        #endregion

        #region 验签RSACheckV1
        public static bool RSACheckV1(string companyId,IDictionary<string, string> paramsMap)
        {
            try
            {
                AliPayApiConfig config = AliPayApiConfigServices.QueryAliPayConfig(companyId);
                if (config == null) throw new MyException("获取支付宝配置失败");

                string signType = config.AliPaySignType == 0 ? "RSA" : "RSA2";
                return AlipaySignature.RSACheckV1(paramsMap, config.PublicKey, AliPayParam.charset, signType, false);
            }
            catch (Exception ex)
            {
                TxtLogServices.WriteTxtLogEx("AliPayApiServices","验签报错：" + ex.StackTrace);
                return false;
            }
        }
        #endregion
    }
}
