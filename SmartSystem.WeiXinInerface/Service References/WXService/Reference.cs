﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmartSystem.WeiXinInerface.WXService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="WXService.WXService")]
    public interface WXService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/RegisterAccount", ReplyAction="http://tempuri.org/WXService/RegisterAccountResponse")]
        bool RegisterAccount(string strwxinfo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/WXBindingMobilePhone", ReplyAction="http://tempuri.org/WXService/WXBindingMobilePhoneResponse")]
        bool WXBindingMobilePhone(string AccountID, string MobilePhone);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/WXUnsubscribe", ReplyAction="http://tempuri.org/WXService/WXUnsubscribeResponse")]
        bool WXUnsubscribe(string OpenID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/QueryWXByOpenId", ReplyAction="http://tempuri.org/WXService/QueryWXByOpenIdResponse")]
        string QueryWXByOpenId(string OpenID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/EditWXInfo", ReplyAction="http://tempuri.org/WXService/EditWXInfoResponse")]
        bool EditWXInfo(string wx_info);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/QueryAccountByAccountID", ReplyAction="http://tempuri.org/WXService/QueryAccountByAccountIDResponse")]
        string QueryAccountByAccountID(string AccountID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/QueryAccountByMobilePhone", ReplyAction="http://tempuri.org/WXService/QueryAccountByMobilePhoneResponse")]
        string QueryAccountByMobilePhone(string MobilePhone);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/EditAccountOption", ReplyAction="http://tempuri.org/WXService/EditAccountOptionResponse")]
        bool EditAccountOption(string wx_account);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/EidtWX_infoVisitDate", ReplyAction="http://tempuri.org/WXService/EidtWX_infoVisitDateResponse")]
        bool EidtWX_infoVisitDate(string OpenID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/EidtWX_infoLastPlateNumber", ReplyAction="http://tempuri.org/WXService/EidtWX_infoLastPlateNumberResponse")]
        bool EidtWX_infoLastPlateNumber(string OpenID, string LastPlateNumber);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/GetPkparkinfoList", ReplyAction="http://tempuri.org/WXService/GetPkparkinfoListResponse")]
        string GetPkparkinfoList(double currX, double currY, int Leng);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/GetPkparkinByAccountIDList", ReplyAction="http://tempuri.org/WXService/GetPkparkinByAccountIDListResponse")]
        string GetPkparkinByAccountIDList(string AccountID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/AddWX_CarInfo", ReplyAction="http://tempuri.org/WXService/AddWX_CarInfoResponse")]
        int AddWX_CarInfo(string CarInfo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/DelWX_CarInfo", ReplyAction="http://tempuri.org/WXService/DelWX_CarInfoResponse")]
        bool DelWX_CarInfo(string RecordID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/GetCarInfoByAccountID", ReplyAction="http://tempuri.org/WXService/GetCarInfoByAccountIDResponse")]
        string GetCarInfoByAccountID(string AccountID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/GetCarInfoByPlateNo", ReplyAction="http://tempuri.org/WXService/GetCarInfoByPlateNoResponse")]
        string GetCarInfoByPlateNo(string AccountID, string PlateNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/GetTempCarInfoIn", ReplyAction="http://tempuri.org/WXService/GetTempCarInfoInResponse")]
        string[] GetTempCarInfoIn(string AccountID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/GetMonthCarInfoByAccountID", ReplyAction="http://tempuri.org/WXService/GetMonthCarInfoByAccountIDResponse")]
        string GetMonthCarInfoByAccountID(string AccountID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/GetMonthCarInfoByPlateNumber", ReplyAction="http://tempuri.org/WXService/GetMonthCarInfoByPlateNumberResponse")]
        string GetMonthCarInfoByPlateNumber(string PlateNumber);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/GetBaseParkinfoByPKID", ReplyAction="http://tempuri.org/WXService/GetBaseParkinfoByPKIDResponse")]
        string GetBaseParkinfoByPKID(string PKID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/GetLockCarByAccountID", ReplyAction="http://tempuri.org/WXService/GetLockCarByAccountIDResponse")]
        string GetLockCarByAccountID(string AccountID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/WXLockCarInfo", ReplyAction="http://tempuri.org/WXService/WXLockCarInfoResponse")]
        int WXLockCarInfo(string AccountID, string PKID, string PlateNumber);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/WXUlockCarInfo", ReplyAction="http://tempuri.org/WXService/WXUlockCarInfoResponse")]
        int WXUlockCarInfo(string AccountID, string PKID, string PlateNumber);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/OrderWhetherEffective", ReplyAction="http://tempuri.org/WXService/OrderWhetherEffectiveResponse")]
        int OrderWhetherEffective(string OrderNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/OrderWhetherEffectiveNew", ReplyAction="http://tempuri.org/WXService/OrderWhetherEffectiveNewResponse")]
        int OrderWhetherEffectiveNew(string OrderNo, string ParkID, string IORecordID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/WXMonthlyRenewals", ReplyAction="http://tempuri.org/WXService/WXMonthlyRenewalsResponse")]
        string WXMonthlyRenewals(string CardID, string PKID, int MonthNum, decimal Amount, string AccountID, int PayWay, int OrderSource, string OnlineOrderID, System.DateTime PayDate);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/WXTempParkingFee", ReplyAction="http://tempuri.org/WXService/WXTempParkingFeeResponse")]
        string WXTempParkingFee(string PlateNumber, string PKID, System.DateTime CalculatDate, string AccountID, int OrderSource);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/WXTempStopPayment", ReplyAction="http://tempuri.org/WXService/WXTempStopPaymentResponse")]
        string WXTempStopPayment(string OrderID, int PayWay, decimal Amount, string PKID, string OnlineOrderID, string AccountID, System.DateTime PayDate);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/WXScanCodeTempParkingFee", ReplyAction="http://tempuri.org/WXService/WXScanCodeTempParkingFeeResponse")]
        string WXScanCodeTempParkingFee(string BoxID, string AccountID, int OrderSource);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/WXScanCodeTempParkingFeeByGateID", ReplyAction="http://tempuri.org/WXService/WXScanCodeTempParkingFeeByGateIDResponse")]
        string WXScanCodeTempParkingFeeByGateID(string GateID, string AccountID, int OrderSource);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/GetPkOrderTempByAccountID", ReplyAction="http://tempuri.org/WXService/GetPkOrderTempByAccountIDResponse")]
        string GetPkOrderTempByAccountID(string AccountID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/GetPkOrderMonthByAccountID", ReplyAction="http://tempuri.org/WXService/GetPkOrderMonthByAccountIDResponse")]
        string GetPkOrderMonthByAccountID(string AccountID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/WXGetParkDerate", ReplyAction="http://tempuri.org/WXService/WXGetParkDerateResponse")]
        string WXGetParkDerate(string sellerID, string VID, string ProxyNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/WXGetIORecordByPlateNumber", ReplyAction="http://tempuri.org/WXService/WXGetIORecordByPlateNumberResponse")]
        string WXGetIORecordByPlateNumber(string PlateNumber, string VID, string SellerID, string ProxyNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/WXDiscountPlateNumber", ReplyAction="http://tempuri.org/WXService/WXDiscountPlateNumberResponse")]
        string WXDiscountPlateNumber(string IORecordID, string DerateID, string VID, string SellerID, decimal DerateMoney, string ProxyNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/WXGetSellerInfo", ReplyAction="http://tempuri.org/WXService/WXGetSellerInfoResponse")]
        string WXGetSellerInfo(string SellerNo, string PWD, string SellerID, string ProxyNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/WXEditSellerPwd", ReplyAction="http://tempuri.org/WXService/WXEditSellerPwdResponse")]
        bool WXEditSellerPwd(string SellerID, string Pwd, string ProxyNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/WXGetParkCarDerate", ReplyAction="http://tempuri.org/WXService/WXGetParkCarDerateResponse")]
        string WXGetParkCarDerate(string parms, int rows, int pageindex, string ProxyNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/WXGetParkCarDerateByVID", ReplyAction="http://tempuri.org/WXService/WXGetParkCarDerateByVIDResponse")]
        string WXGetParkCarDerateByVID(string parms, int rows, int pageindex, string ProxyNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/WXScanCodeInOut", ReplyAction="http://tempuri.org/WXService/WXScanCodeInOutResponse")]
        int WXScanCodeInOut(string PKID, string GateNo, string OpenId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/GetParkVisitor", ReplyAction="http://tempuri.org/WXService/GetParkVisitorResponse")]
        string GetParkVisitor(string AccountID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/AddParkVisitor", ReplyAction="http://tempuri.org/WXService/AddParkVisitorResponse")]
        string AddParkVisitor(string jsondata);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/WXRemoteGate", ReplyAction="http://tempuri.org/WXService/WXRemoteGateResponse")]
        int WXRemoteGate(string UserID, string PKID, string GateID, string Remark);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/WXReservePKBit", ReplyAction="http://tempuri.org/WXService/WXReservePKBitResponse")]
        string WXReservePKBit(string AccountID, string BitNo, string parking_id, string AreaID, string license_plate, System.DateTime start_time, System.DateTime end_time);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/WXReserveBitPay", ReplyAction="http://tempuri.org/WXService/WXReserveBitPayResponse")]
        bool WXReserveBitPay(string ReserveID, string OrderID, decimal Amount, string PKID, string OnlineOrderID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/WXGetReservePKBit", ReplyAction="http://tempuri.org/WXService/WXGetReservePKBitResponse")]
        string WXGetReservePKBit(string AccountID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/WXReservePKBitZH", ReplyAction="http://tempuri.org/WXService/WXReservePKBitZHResponse")]
        string WXReservePKBitZH(string ProxyNo, string AccountID, string BitNo, string parking_id, string AreaID, string license_plate, System.DateTime start_time, System.DateTime end_time);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/WXTestClientProxyConnectionByVID", ReplyAction="http://tempuri.org/WXService/WXTestClientProxyConnectionByVIDResponse")]
        bool WXTestClientProxyConnectionByVID(string VID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/WXTestClientProxyConnectionByPKID", ReplyAction="http://tempuri.org/WXService/WXTestClientProxyConnectionByPKIDResponse")]
        bool WXTestClientProxyConnectionByPKID(string PKID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WXService/SendNotify", ReplyAction="http://tempuri.org/WXService/SendNotifyResponse")]
        void SendNotify(string title, string message);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface WXServiceChannel : SmartSystem.WeiXinInerface.WXService.WXService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WXServiceClient : System.ServiceModel.ClientBase<SmartSystem.WeiXinInerface.WXService.WXService>, SmartSystem.WeiXinInerface.WXService.WXService {
        
        public WXServiceClient() {
        }
        
        public WXServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public WXServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WXServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WXServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public bool RegisterAccount(string strwxinfo) {
            return base.Channel.RegisterAccount(strwxinfo);
        }
        
        public bool WXBindingMobilePhone(string AccountID, string MobilePhone) {
            return base.Channel.WXBindingMobilePhone(AccountID, MobilePhone);
        }
        
        public bool WXUnsubscribe(string OpenID) {
            return base.Channel.WXUnsubscribe(OpenID);
        }
        
        public string QueryWXByOpenId(string OpenID) {
            return base.Channel.QueryWXByOpenId(OpenID);
        }
        
        public bool EditWXInfo(string wx_info) {
            return base.Channel.EditWXInfo(wx_info);
        }
        
        public string QueryAccountByAccountID(string AccountID) {
            return base.Channel.QueryAccountByAccountID(AccountID);
        }
        
        public string QueryAccountByMobilePhone(string MobilePhone) {
            return base.Channel.QueryAccountByMobilePhone(MobilePhone);
        }
        
        public bool EditAccountOption(string wx_account) {
            return base.Channel.EditAccountOption(wx_account);
        }
        
        public bool EidtWX_infoVisitDate(string OpenID) {
            return base.Channel.EidtWX_infoVisitDate(OpenID);
        }
        
        public bool EidtWX_infoLastPlateNumber(string OpenID, string LastPlateNumber) {
            return base.Channel.EidtWX_infoLastPlateNumber(OpenID, LastPlateNumber);
        }
        
        public string GetPkparkinfoList(double currX, double currY, int Leng) {
            return base.Channel.GetPkparkinfoList(currX, currY, Leng);
        }
        
        public string GetPkparkinByAccountIDList(string AccountID) {
            return base.Channel.GetPkparkinByAccountIDList(AccountID);
        }
        
        public int AddWX_CarInfo(string CarInfo) {
            return base.Channel.AddWX_CarInfo(CarInfo);
        }
        
        public bool DelWX_CarInfo(string RecordID) {
            return base.Channel.DelWX_CarInfo(RecordID);
        }
        
        public string GetCarInfoByAccountID(string AccountID) {
            return base.Channel.GetCarInfoByAccountID(AccountID);
        }
        
        public string GetCarInfoByPlateNo(string AccountID, string PlateNo) {
            return base.Channel.GetCarInfoByPlateNo(AccountID, PlateNo);
        }
        
        public string[] GetTempCarInfoIn(string AccountID) {
            return base.Channel.GetTempCarInfoIn(AccountID);
        }
        
        public string GetMonthCarInfoByAccountID(string AccountID) {
            return base.Channel.GetMonthCarInfoByAccountID(AccountID);
        }
        
        public string GetMonthCarInfoByPlateNumber(string PlateNumber) {
            return base.Channel.GetMonthCarInfoByPlateNumber(PlateNumber);
        }
        
        public string GetBaseParkinfoByPKID(string PKID) {
            return base.Channel.GetBaseParkinfoByPKID(PKID);
        }
        
        public string GetLockCarByAccountID(string AccountID) {
            return base.Channel.GetLockCarByAccountID(AccountID);
        }
        
        public int WXLockCarInfo(string AccountID, string PKID, string PlateNumber) {
            return base.Channel.WXLockCarInfo(AccountID, PKID, PlateNumber);
        }
        
        public int WXUlockCarInfo(string AccountID, string PKID, string PlateNumber) {
            return base.Channel.WXUlockCarInfo(AccountID, PKID, PlateNumber);
        }
        
        public int OrderWhetherEffective(string OrderNo) {
            return base.Channel.OrderWhetherEffective(OrderNo);
        }
        
        public int OrderWhetherEffectiveNew(string OrderNo, string ParkID, string IORecordID) {
            return base.Channel.OrderWhetherEffectiveNew(OrderNo, ParkID, IORecordID);
        }
        
        public string WXMonthlyRenewals(string CardID, string PKID, int MonthNum, decimal Amount, string AccountID, int PayWay, int OrderSource, string OnlineOrderID, System.DateTime PayDate) {
            return base.Channel.WXMonthlyRenewals(CardID, PKID, MonthNum, Amount, AccountID, PayWay, OrderSource, OnlineOrderID, PayDate);
        }
        
        public string WXTempParkingFee(string PlateNumber, string PKID, System.DateTime CalculatDate, string AccountID, int OrderSource) {
            return base.Channel.WXTempParkingFee(PlateNumber, PKID, CalculatDate, AccountID, OrderSource);
        }
        
        public string WXTempStopPayment(string OrderID, int PayWay, decimal Amount, string PKID, string OnlineOrderID, string AccountID, System.DateTime PayDate) {
            return base.Channel.WXTempStopPayment(OrderID, PayWay, Amount, PKID, OnlineOrderID, AccountID, PayDate);
        }
        
        public string WXScanCodeTempParkingFee(string BoxID, string AccountID, int OrderSource) {
            return base.Channel.WXScanCodeTempParkingFee(BoxID, AccountID, OrderSource);
        }
        
        public string WXScanCodeTempParkingFeeByGateID(string GateID, string AccountID, int OrderSource) {
            return base.Channel.WXScanCodeTempParkingFeeByGateID(GateID, AccountID, OrderSource);
        }
        
        public string GetPkOrderTempByAccountID(string AccountID) {
            return base.Channel.GetPkOrderTempByAccountID(AccountID);
        }
        
        public string GetPkOrderMonthByAccountID(string AccountID) {
            return base.Channel.GetPkOrderMonthByAccountID(AccountID);
        }
        
        public string WXGetParkDerate(string sellerID, string VID, string ProxyNo) {
            return base.Channel.WXGetParkDerate(sellerID, VID, ProxyNo);
        }
        
        public string WXGetIORecordByPlateNumber(string PlateNumber, string VID, string SellerID, string ProxyNo) {
            return base.Channel.WXGetIORecordByPlateNumber(PlateNumber, VID, SellerID, ProxyNo);
        }
        
        public string WXDiscountPlateNumber(string IORecordID, string DerateID, string VID, string SellerID, decimal DerateMoney, string ProxyNo) {
            return base.Channel.WXDiscountPlateNumber(IORecordID, DerateID, VID, SellerID, DerateMoney, ProxyNo);
        }
        
        public string WXGetSellerInfo(string SellerNo, string PWD, string SellerID, string ProxyNo) {
            return base.Channel.WXGetSellerInfo(SellerNo, PWD, SellerID, ProxyNo);
        }
        
        public bool WXEditSellerPwd(string SellerID, string Pwd, string ProxyNo) {
            return base.Channel.WXEditSellerPwd(SellerID, Pwd, ProxyNo);
        }
        
        public string WXGetParkCarDerate(string parms, int rows, int pageindex, string ProxyNo) {
            return base.Channel.WXGetParkCarDerate(parms, rows, pageindex, ProxyNo);
        }
        
        public string WXGetParkCarDerateByVID(string parms, int rows, int pageindex, string ProxyNo) {
            return base.Channel.WXGetParkCarDerateByVID(parms, rows, pageindex, ProxyNo);
        }
        
        public int WXScanCodeInOut(string PKID, string GateNo, string OpenId) {
            return base.Channel.WXScanCodeInOut(PKID, GateNo, OpenId);
        }
        
        public string GetParkVisitor(string AccountID) {
            return base.Channel.GetParkVisitor(AccountID);
        }
        
        public string AddParkVisitor(string jsondata) {
            return base.Channel.AddParkVisitor(jsondata);
        }
        
        public int WXRemoteGate(string UserID, string PKID, string GateID, string Remark) {
            return base.Channel.WXRemoteGate(UserID, PKID, GateID, Remark);
        }
        
        public string WXReservePKBit(string AccountID, string BitNo, string parking_id, string AreaID, string license_plate, System.DateTime start_time, System.DateTime end_time) {
            return base.Channel.WXReservePKBit(AccountID, BitNo, parking_id, AreaID, license_plate, start_time, end_time);
        }
        
        public bool WXReserveBitPay(string ReserveID, string OrderID, decimal Amount, string PKID, string OnlineOrderID) {
            return base.Channel.WXReserveBitPay(ReserveID, OrderID, Amount, PKID, OnlineOrderID);
        }
        
        public string WXGetReservePKBit(string AccountID) {
            return base.Channel.WXGetReservePKBit(AccountID);
        }
        
        public string WXReservePKBitZH(string ProxyNo, string AccountID, string BitNo, string parking_id, string AreaID, string license_plate, System.DateTime start_time, System.DateTime end_time) {
            return base.Channel.WXReservePKBitZH(ProxyNo, AccountID, BitNo, parking_id, AreaID, license_plate, start_time, end_time);
        }
        
        public bool WXTestClientProxyConnectionByVID(string VID) {
            return base.Channel.WXTestClientProxyConnectionByVID(VID);
        }
        
        public bool WXTestClientProxyConnectionByPKID(string PKID) {
            return base.Channel.WXTestClientProxyConnectionByPKID(PKID);
        }
        
        public void SendNotify(string title, string message) {
            base.Channel.SendNotify(title, message);
        }
    }
}