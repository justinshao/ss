using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Parking
{
    /// <summary>
    /// ParkOrder:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class ParkOrder
    {
        public ParkOrder()
        { }
        #region Model
        private int _id;
        private string _recordid;
        private string _orderno;
        private string _tagid;
        private OrderType _ordertype;
        private OrderPayWay _payway;
        private decimal _amount;
        private decimal _unpayamount;
        private decimal _payamount;
        private decimal _discountamount;
        private string _carderateid;
        private int _status;
        private OrderSource _ordersource;
        private DateTime _ordertime;
        private DateTime _paytime;
        private DateTime _olduseruldate;
        private DateTime _newusefuldate;
        private decimal _oldmoney;
        private decimal _newmoney;
        private string _pKID;
        private string _userid;
        private string _onlineuserid;
        private string _onlineorderno;
        private string _remark;
        private DateTime _lastupdatetime;
        private int _haveupdate;
        private DataStatus _datastatus;
        private DateTime _cashTime;
        private decimal _cashMoney;
        private DateTime _oldUserBegin;
        private DateTime _newUserBegin;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string FeeRuleID { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CarModelName { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string NetWeight { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ZZWeight { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Tare { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Goods { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Shipper { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Shippingspace { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DocumentsNo { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime NewUserBegin
        {
            get { return _newUserBegin; }
            set { _newUserBegin = value; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime OldUserBegin
        {
            get { return _oldUserBegin; }
            set { _oldUserBegin = value; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal CashMoney
        {
            get
            {
                return _cashMoney;
            }
            set
            {
                _cashMoney = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime CashTime
        {
            get { return _cashTime; }
            set { _cashTime = value; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CashTimeToString
        {
            get
            {
                return CashTime.ToString();
            }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string RecordID
        {
            set { _recordid = value; }
            get { return _recordid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OrderNo
        {
            set { _orderno = value; }
            get { return _orderno; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TagID
        {
            set { _tagid = value; }
            get { return _tagid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public OrderType OrderType
        {
            set { _ordertype = value; }
            get { return _ordertype; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public OrderPayWay PayWay
        {
            set { _payway = value; }
            get { return _payway; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal Amount
        {
            set { _amount = value; }
            get { return _amount; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal UnPayAmount
        {
            set { _unpayamount = value; }
            get { return _unpayamount; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal PayAmount
        {
            set { _payamount = value; }
            get { return _payamount; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal DiscountAmount
        {
            set { _discountamount = value; }
            get { return _discountamount; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CarderateID
        {
            set { _carderateid = value; }
            get { return _carderateid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public OrderSource OrderSource
        {
            set { _ordersource = value; }
            get { return _ordersource; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime OrderTime
        {
            set { _ordertime = value; }
            get { return _ordertime; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OrderTimeToString
        {
            get
            {
                return OrderTime.ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime PayTime
        {
            set { _paytime = value; }
            get { return _paytime; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PayTimeToString
        {
            get
            {
                return PayTime.ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime OldUserulDate
        {
            set { _olduseruldate = value; }
            get { return _olduseruldate; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OldUserulDateToString
        {
            get
            {
                return OldUserulDate.ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime NewUsefulDate
        {
            set { _newusefuldate = value; }
            get { return _newusefuldate; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string NewUsefulDateToString
        {
            get
            {
                return NewUsefulDate.ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal OldMoney
        {
            set { _oldmoney = value; }
            get { return _oldmoney; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal NewMoney
        {
            set { _newmoney = value; }
            get { return _newmoney; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PKID
        {
            set { _pKID = value; }
            get { return _pKID; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OnlineUserID
        {
            set { _onlineuserid = value; }
            get { return _onlineuserid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OnlineOrderNo
        {
            set { _onlineorderno = value; }
            get { return _onlineorderno; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime LastUpdateTime
        {
            set { _lastupdatetime = value; }
            get { return _lastupdatetime; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int HaveUpdate
        {
            set { _haveupdate = value; }
            get { return _haveupdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DataStatus DataStatus
        {
            set { _datastatus = value; }
            get { return _datastatus; }
        }
        #endregion Model

        #region WPF

        private ParkIORecord _iorecord;
        public ParkIORecord IORecord
        {
            get { return _iorecord; }
            set
            {

                if (_iorecord != value)
                {
                    _iorecord = value; 
                }
            }
        } 
        #endregion

        #region 扩展字段
        /// <summary>
        /// 手机号码
        /// </summary>
        public string MobilePhone
        {
            get;
            set;
        }
        /// <summary>
        /// 车场名称
        /// </summary>
        public string PKName
        {
            get;
            set;
        }
        /// <summary>
        /// 车主
        /// </summary>
        public string EmployeeName
        {
            get;
            set;
        }
        /// <summary>
        /// 卡片编号
        /// </summary>
        public string CardNo
        {
            get;
            set;
        }
        /// <summary>
        /// 订单类型名称
        /// </summary>
        public string OrderTypeName
        {
            get;
            set;
        }
        /// <summary>
        /// 操作人
        /// </summary>
        public string Operator
        {
            get;
            set;
        }
        /// <summary>
        /// 订单来源
        /// </summary>
        public string OrderSourceName
        {
            get;
            set;
        }
        /// <summary>
        /// 支付方式
        /// </summary>
        public string PayWayName
        {
            get;
            set;
        }
        /// <summary>
        /// 车牌号码
        /// </summary>
        public string PlateNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 进场时间
        /// </summary>
        public DateTime EntranceTime
        {
            get;
            set;
        }
        public string EntranceTimeToString
        {
            get
            {
                return EntranceTime.ToyyyyMMddHHmmss();
            }
        }
        /// <summary>
        /// 出场时间
        /// </summary>
        public DateTime ExitTime
        {
            get;
            set;
        }
        public string ExitTimeToString
        {
            get
            {
                return ExitTime.ToyyyyMMddHHmmss();
            }
        }
        /// <summary>
        /// 停车时长
        /// </summary>
        public string LongTime
        {
            get;
            set;
        }
        /// <summary>
        /// 月卡续期时长
        /// </summary>
        public string MonthLongTime
        {
            get;
            set;
        }
        /// <summary>
        /// 进场图片
        /// </summary>
        public string EntranceImage
        {
            get;
            set;
        }
        /// <summary>
        /// 出场图片
        /// </summary>
        public string ExitImage
        {
            get;
            set;
        }
        public int ReleaseType { set; get; }
        public string SellerName { set; get; }
        #endregion
    }
}
