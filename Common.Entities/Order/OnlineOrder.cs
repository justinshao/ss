using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Enum;
using Common.Entities.WX;
using Newtonsoft.Json;

namespace Common.Entities.Order
{
    public class OnlineOrder
    {
        #region Model
        private decimal _orderid;
        private string _orderno;
        private string _lx;
        private string _zt;
        private string _pkid;
        private string _pkname;
        private string _inoutid;
        private string _plateno;
        private string _platenumber;
        private DateTime _entrancetime;
        private DateTime _exittime;
        private decimal _amount;
        private decimal _payamount;
        private PaymentChannel _paymentchannel;
        private int _paybank;
        private string _payaccount;
        private string _payer;
        private string _nickname;
        private PaymentChannel _payeechannel;
        private int _payeebank;
        private string _payeeaccount;
        private string _payeeuser;
        private string _paydetailid;
        private string _serialnumber;
        private string _prepayid;
        private int _monthnum;
        private string _accountid;
        private string _cardid;
        private int _syncresulttimes=0;
        private DateTime _lastsyncresulttime;
        private string _parkcardno;
        private decimal _balance;
        private decimal _discountAmount=0;
        private string _refundorderid;
        private string _remark;
        private OnlineOrderType _ordertype;
        private OnlineOrderStatus _status;
        private DateTime _ordertime;
        private DateTime _realpaytime;
        private DateTime _paytime;
        private string _companyid;
        private PayOrderSource _orderSource;
        private string _externalPKID;
        /// <summary>
        /// 主键编号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal OrderID
        {
            set { _orderid = value; }
            get { return _orderid; }
        }
        /// <summary>
        /// 订单来源
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PayOrderSource OrderSource
        {
            set { _orderSource = value; }
            get { return _orderSource; }
        }
        /// <summary>
        /// 车场编号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PKID
        {
            set { _pkid = value; }
            get { return _pkid; }
        }
        /// <summary>
        /// 外部车场编号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ExternalPKID
        {
            set { _externalPKID = value; }
            get { return _externalPKID; }
        }
        /// <summary>
        /// 单位编号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CompanyID {
            set { _companyid = value; }
            get { return _companyid; }
        }
        /// <summary>
        /// 车场名称
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PKName
        {
            set { _pkname = value; }
            get { return _pkname; }
        }
        /// <summary>
        /// 进出记录编号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string InOutID
        {
            set { _inoutid = value; }
            get { return _inoutid; }
        }
        /// <summary>
        /// 车牌号码
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PlateNo
        {
            set { _plateno = value; }
            get { return _plateno; }
        }
        public string PlateNumber
        {
            set { _platenumber = value; }
            get { return _platenumber; }
        }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo
        {
            set { _orderno = value; }
            get { return _orderno; }
        }
        /// <summary>
        /// 进场时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime EntranceTime
        {
            set { _entrancetime = value; }
            get { return _entrancetime; }
        }
        /// <summary>
        /// 出场时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime ExitTime
        {
            set { _exittime = value; }
            get { return _exittime; }
        }
        /// <summary>
        /// 付款金额
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal Amount
        {
            set { _amount = value; }
            get { return _amount; }
        }
        public decimal PayAmount
        {
            set { _payamount = value; }
            get { return _payamount; }
        }
        /// <summary>
        /// 付款渠道
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PaymentChannel PaymentChannel
        {
            set { _paymentchannel = value; }
            get { return _paymentchannel; }
        }
        /// <summary>
        /// 付款银行
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int PayBank
        {
            set { _paybank = value; }
            get { return _paybank; }
        }
        /// <summary>
        /// 付款帐号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PayAccount
        {
            set { _payaccount = value; }
            get { return _payaccount; }
        }
        /// <summary>
        /// 付款人
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Payer
        {
            set { _payer = value; }
            get { return _payer; }
        }
        public string NickName
        {
            set { _nickname = value; }
            get { return _nickname; }
        }
        /// <summary>
        /// 支付人昵称（仅供分页查询使用）
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PayerNickName { get; set; }
        /// <summary>
        /// 收款渠道
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PaymentChannel PayeeChannel
        {
            set { _payeechannel = value; }
            get { return _payeechannel; }
        }
        /// <summary>
        /// 收款银行
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int PayeeBank
        {
            set { _payeebank = value; }
            get { return _payeebank; }
        }
        /// <summary>
        /// 收款帐号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PayeeAccount
        {
            set { _payeeaccount = value; }
            get { return _payeeaccount; }
        }
        /// <summary>
        /// 收款人
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PayeeUser
        {
            set { _payeeuser = value; }
            get { return _payeeuser; }
        }
        /// <summary>
        /// 支付明细编号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PayDetailID
        {
            set { _paydetailid = value; }
            get { return _paydetailid; }
        }
        /// <summary>
        /// 流水号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SerialNumber
        {
            set { _serialnumber = value; }
            get { return _serialnumber; }
        }
        /// <summary>
        /// 微信预支付编号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PrepayId
        {
            set { _prepayid = value; }
            get { return _prepayid; }
        }
        /// <summary>
        /// 月卡续费月数
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int MonthNum
        {
            set { _monthnum = value; }
            get { return _monthnum; }
        }
        /// <summary>
        /// 账号编号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AccountID
        {
            set { _accountid = value; }
            get { return _accountid; }
        }
        /// <summary>
        /// 卡编号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CardId
        {
            set { _cardid = value; }
            get { return _cardid; }
        }
        /// <summary>
        /// 同步结果次数
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int SyncResultTimes
        {
            set { _syncresulttimes = value; }
            get { return _syncresulttimes; }
        }
        /// <summary>
        /// 最后同步结果时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime LastSyncResultTime
        {
            set { _lastsyncresulttime = value; }
            get { return _lastsyncresulttime; }
        }
        public string LastSyncResultTimeToString
        {
            get
            {
                return LastSyncResultTime.ToTimeString();
            }
        }
        /// <summary>
        /// 停车卡编号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ParkCardNo
        {
            set { _parkcardno = value; }
            get { return _parkcardno; }
        }
        /// <summary>
        /// 余额
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal Balance
        {
            set { _balance = value; }
            get { return _balance; }
        }
        /// <summary>
        /// 优惠金额
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal DiscountAmount {
            set { _discountAmount = value; }
            get { return _discountAmount; }
        }
        /// <summary>
        /// 退款编号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string RefundOrderId
        {
            set { _refundorderid = value; }
            get { return _refundorderid; }
        }
        /// <summary>
        /// 备注信息
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 订单类型
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public OnlineOrderType OrderType
        {
            set { _ordertype = value; }
            get { return _ordertype; }
        }
        public string lx
        {
            set { _lx = value; }
            get { return _lx; }
        }
        /// <summary>
        /// 状态
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public OnlineOrderStatus Status
        {
            set { _status = value; }
            get { return _status; }
        }
        public string zt
        {
            set { _zt = value; }
            get { return _zt; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime OrderTime
        {
            set { _ordertime = value; }
            get { return _ordertime; }
        }

        public string OrderTimeToString
        {
            get
            {
                return OrderTime.ToTimeString();
            }
        }
        /// <summary>
        /// 实际支付时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime RealPayTime
        {
            set { _realpaytime = value; }
            get { return _realpaytime; }
        }
        public string RealPayTimeToString
        {
            get
            {
                return RealPayTime.ToTimeString();
            }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime PayTime
        {
            set { _paytime = value; }
            get { return _paytime; }
        }
        public string PayTimeToString
        {
            get
            {
                return PayTime.ToTimeString();
            }
        }
        /// <summary>
        /// 车位预约开始时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime BookingStartTime { get; set; }
        /// <summary>
        /// 预约车位结束时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime BookingEndTime { get; set; }
        /// <summary>
        /// 车位预约区域
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BookingAreaID { get; set; }
        /// <summary>
        /// 预约车位号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BookingBitNo { get; set; }

        /// <summary>
        /// H5支付地址
        /// </summary>
        public string MWebUrl { get; set; }
        public string TagID { get; set; }

        #endregion Model
    }
}
