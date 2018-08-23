using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ClassLibrary1
{
    public class Weixi
    {

        public const string getAccessToken = "https://api.weixin.qq.com/cgi-bin/token?grant_type={0}&appid={1}&secret={2}";

        #region 二维码
        public const string GetTicketStr = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}";
        public const string GetEWMstr = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket={0}";
        #endregion
        
        #region 手机AppDate
        public const string getPhone = "http://spsapp.spsing.com/api/VerifyCode/Send?phone={0}";
        public const string getThirdLogin="http://spsapp.spsing.com/api/User/TPLogin";
        public const string getBingding = "http://spsapp.spsing.com/api/User/BindingPhone";
        public const string getCarManage = "http://spsapp.spsing.com/api/Car/GetMyCar";
        public const string getCarLine = "http://spsapp.spsing.com/api/Home/GetPresenceOrder";


        //钱包 
        public const string getUserInfo = "http://spsapp.spsing.com/api/User/Info";  //个人信息查询
        public const string getBalaceGetCoupon = "http://spsapp.spsing.com/api/Order/BalaceGetCoupon?price={0}&api_key=UnifiedOrder"; //充值优惠券
        public const string getBalanceOrder = "http://spsapp.spsing.com/api/Order/CreateBalanceOrder";//余额充值-创建订单(需授权)
        public const string getUnifiedOrder = "http://spsapp.spsing.com/api/Order/UnifiedOrder?orderNo={0}&type={1}&paypwd=-1";
        public const string getMyCoupon = "http://spsapp.spsing.com/api/User/MyCoupon?status={0}&pageIndex=1&pageSize=100"; //
        
        #endregion



    }
}